#ifndef __UC1701_H__
#define __UC1701_H__

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

// Display initialization (dimensions in pixels)...
void UC1701_begin();

// Erase everything on the display...
void UC1701_clear();
void UC1701_clearLine();  // ...or just the current line

// Place the cursor at the start of the current line...
void UC1701_home();

// Place the cursor at position (column, line)...
void UC1701_setCursor(unsigned char column, unsigned char line);

// Assign a user-defined glyph (5x8) to an ASCII character (0-31)...
void UC1701_createChar(unsigned char chr, const unsigned char *glyph);

// Print an ASCII string at the current cursor position (7-bit)...
void UC1701_print(unsigned char *text);

// Write an ASCII character at the current cursor position (7-bit)...
int UC1701_write(unsigned char chr);

// Draw a bitmap at the current cursor position...
void UC1701_drawBitmap(const unsigned char *data, unsigned char columns, unsigned char lines);

// Draw a chart element at the current cursor position...
void UC1701_drawColumn(unsigned char lines, unsigned char value);

// Send a command or data to the display...
void UC1701_Transfer_command(int data1);
void UC1701_Transfer_data(int data1);

#endif /* __UC1701_H__ */
