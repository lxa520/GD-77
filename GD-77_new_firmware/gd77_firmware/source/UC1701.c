/*
 * UC7101 - Interface with UC7101 (or compatible) LCDs.
 *
 * Copyright (c) 2014 Rustem Iskuzhin
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

#include "UC1701.h"
#include "UC1701_charset.h"

#include "port_io_def.h"

#include "FreeRTOS.h"
#include "semphr.h"
#include "event_groups.h"

// The size of the display, in pixels...
unsigned char UC1701_width;
unsigned char UC1701_height;

// Current cursor position...
unsigned char UC1701_column;
unsigned char UC1701_line;

// User-defined glyphs (below the ASCII space character)...
const unsigned char *UC1701_custom[' '];

void UC1701_begin()
{
	UC1701_width = 128;
	UC1701_height = 64;

	UC1701_column = 0;
	UC1701_line = 0;

    // All pins are outputs
    PORT_SetPinMux(Port_Display_Light, Pin_Display_Light, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Display_CS, Pin_Display_CS, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Display_RST, Pin_Display_RST, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Display_RS, Pin_Display_RS, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Display_SCK, Pin_Display_SCK, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Display_SDA, Pin_Display_SDA, kPORT_MuxAsGpio);

    GPIO_PinInit(GPIO_Display_Light, Pin_Display_Light, &pin_config_output);
    GPIO_PinInit(GPIO_Display_CS, Pin_Display_CS, &pin_config_output);
    GPIO_PinInit(GPIO_Display_RST, Pin_Display_RST, &pin_config_output);
    GPIO_PinInit(GPIO_Display_RS, Pin_Display_RS, &pin_config_output);
    GPIO_PinInit(GPIO_Display_SCK, Pin_Display_SCK, &pin_config_output);
    GPIO_PinInit(GPIO_Display_SDA, Pin_Display_SDA, &pin_config_output);

    // Init pins
	GPIO_PinWrite(GPIO_Display_Light, Pin_Display_Light, 0);
	GPIO_PinWrite(GPIO_Display_CS, Pin_Display_CS, 1);
	GPIO_PinWrite(GPIO_Display_RST, Pin_Display_RST, 1);
	GPIO_PinWrite(GPIO_Display_RS, Pin_Display_RS, 1);
	GPIO_PinWrite(GPIO_Display_SCK, Pin_Display_SCK, 1);
	GPIO_PinWrite(GPIO_Display_SDA, Pin_Display_SDA, 1);

	// Reset LCD
	GPIO_PinWrite(GPIO_Display_RST, Pin_Display_RST, 0);
    vTaskDelay(portTICK_PERIOD_MS * 100);
	GPIO_PinWrite(GPIO_Display_RST, Pin_Display_RST, 1);

    // Set the LCD parameters...
    UC1701_Transfer_command(0xE2); // System Reset
    UC1701_Transfer_command(0x2F); // Voltage Follower On
    UC1701_Transfer_command(0x81); // Set Electronic Volume = 15
    UC1701_Transfer_command(0x0E); //
    UC1701_Transfer_command(0xA2); // Set Bias = 1/9
    UC1701_Transfer_command(0xA1); // Set SEG Direction
    UC1701_Transfer_command(0xC0); // Set COM Direction
    UC1701_Transfer_command(0xA4); // Normal display
    UC1701_clear();
    UC1701_Transfer_command(0xAF); // Set Display Enable
}

void UC1701_clear()
{
	for  (unsigned short j = 0; j < 8; j++)
	{
		UC1701_setCursor(0, j);
		for (unsigned short i = 0; i < 132 ; i++) {
			UC1701_Transfer_data(0x00);
		}
	}

	UC1701_setCursor(0, 0);
}

void UC1701_setCursor(unsigned char column, unsigned char line)
{
       int i, j;
       column = column+4;
       UC1701_column = column;
       UC1701_line = line;

       i=(column&0xF0)>>4;
       j=column&0x0F;
       UC1701_Transfer_command(0xb0+line);
       UC1701_Transfer_command(0x10+i);
       UC1701_Transfer_command(j);
}

void UC1701_print(unsigned char *text)
{
	int i=0;
	while (text[i]!=0)
	{
		UC1701_write(text[i]);
		i++;
	}
}

int UC1701_write(unsigned char chr)
{
    // ASCII 7-bit only...
    if (chr >= 0x80) {
        return 0;
    }

    if (chr == '\r') {
    	UC1701_setCursor(0, UC1701_line);
        return 1;
    } else if (chr == '\n') {
    	UC1701_setCursor(UC1701_column, UC1701_line + 1);
        return 1;
    }

    const unsigned char *glyph;
    unsigned char pgm_buffer[5];

    if (chr >= ' ') {
        // Regular ASCII characters are kept in flash to save RAM...
        memcpy(pgm_buffer, &UC1701_charset[chr - ' '], sizeof(pgm_buffer));
        glyph = pgm_buffer;
    } else {
        // Custom glyphs, on the other hand, are stored in RAM...
        if (UC1701_custom[chr]) {
            glyph = UC1701_custom[chr];
        } else {
            // Default to a space character if unset...
            memcpy(pgm_buffer, &UC1701_charset[0], sizeof(pgm_buffer));
            glyph = pgm_buffer;
        }
    }

    // Output one column at a time...
    for (unsigned char i = 0; i < 5; i++) {
    	UC1701_Transfer_data(glyph[i]);
    }

    // One column between characters...
    UC1701_Transfer_data( 0x00);

    // Update the cursor position...
    UC1701_column = (UC1701_column + 6) % UC1701_width;

    if (UC1701_column == 0) {
    	UC1701_line = (UC1701_line + 1) % (UC1701_height/9 + 1);
    }

    return 1;
}

void UC1701_createChar(unsigned char chr, const unsigned char *glyph)
{
    // ASCII 0-31 only...
    if (chr >= ' ') {
        return;
    }
    
    UC1701_custom[chr] = glyph;
}

void UC1701_clearLine()
{
	UC1701_setCursor(0, UC1701_line);

    for (unsigned char i = 4; i < 132; i++) {
    	UC1701_Transfer_data( 0x00);
    }

    UC1701_setCursor(0, UC1701_line);
}

void UC1701_home()
{
	UC1701_setCursor(0, UC1701_line);
}

void UC1701_drawBitmap(const unsigned char *data, unsigned char columns, unsigned char lines)
{
    unsigned char scolumn = UC1701_column;
    unsigned char sline = UC1701_line;

    // The bitmap will be clipped at the right/bottom edge of the display...
    unsigned char mx = (scolumn + columns > UC1701_width) ? (UC1701_width - scolumn) : columns;
    unsigned char my = (sline + lines > UC1701_height/8) ? (UC1701_height/8 - sline) : lines;

    for (unsigned char y = 0; y < my; y++) {
    	UC1701_setCursor(scolumn, sline + y);

        for (unsigned char x = 0; x < mx; x++) {
        	UC1701_Transfer_data(data[y * columns + x]);
        }
    }

    // Leave the cursor in a consistent position...
    UC1701_setCursor(scolumn + columns, sline);
}

void UC1701_drawColumn(unsigned char lines, unsigned char value)
{
    unsigned char scolumn = UC1701_column;
    unsigned char sline = UC1701_line;

    // Keep "value" within range...
    if (value > lines*8) {
        value = lines*8;
    }

    // Find the line where "value" resides...
    unsigned char mark = (lines*8 - 1 - value)/8;
    
    // Clear the lines above the mark...
    for (unsigned char line = 0; line < mark; line++) {
    	UC1701_setCursor(scolumn, sline + line);
    	UC1701_Transfer_data( 0x00);
    }

    // Compute the byte to draw at the "mark" line...
    unsigned char b = 0xff;
    for (unsigned char i = 0; i < lines*8 - mark*8 - value; i++) {
        b <<= 1;
    }

    UC1701_setCursor(scolumn, sline + mark);
    UC1701_Transfer_data(b);

    // Fill the lines below the mark...
    for (unsigned char line = mark + 1; line < lines; line++) {
    	UC1701_setCursor(scolumn, sline + line);
    	UC1701_Transfer_data(0xff);
    }
  
    // Leave the cursor in a consistent position...
    UC1701_setCursor(scolumn + 1, sline);
}

void UC1701_Transfer_command(int data1)
{
   char i;
   GPIO_PinWrite(GPIO_Display_CS, Pin_Display_CS, 0);
   GPIO_PinWrite(GPIO_Display_RS, Pin_Display_RS, 0);
   for (i=0; i<8; i++)
   {
	 GPIO_PinWrite(GPIO_Display_SCK, Pin_Display_SCK, 0);
	 if (data1&0x80)
		 GPIO_PinWrite(GPIO_Display_SDA, Pin_Display_SDA, 1);
	 else
		 GPIO_PinWrite(GPIO_Display_SDA, Pin_Display_SDA, 0);
	 GPIO_PinWrite(GPIO_Display_SCK, Pin_Display_SCK, 1);
	 data1=data1<<1;
   }
}

void UC1701_Transfer_data(int data1)
{
   char i;
   GPIO_PinWrite(GPIO_Display_CS, Pin_Display_CS, 0);
   GPIO_PinWrite(GPIO_Display_RS, Pin_Display_RS, 1);
   for (i=0; i<8; i++)
   {
	 GPIO_PinWrite(GPIO_Display_SCK, Pin_Display_SCK, 0);
	 if (data1&0x80)
		 GPIO_PinWrite(GPIO_Display_SDA, Pin_Display_SDA, 1);
	 else
		 GPIO_PinWrite(GPIO_Display_SDA, Pin_Display_SDA, 0);
	 GPIO_PinWrite(GPIO_Display_SCK, Pin_Display_SCK, 1);
	 data1=data1<<1;
   }
}
