#ifndef __PORT_IO_DEF_H__
#define __PORT_IO_DEF_H__

#include "fsl_common.h"
#include "fsl_port.h"
#include "fsl_gpio.h"
#include "pin_mux.h"

static gpio_pin_config_t pin_config_input =
{
	kGPIO_DigitalInput,
	0
};

static gpio_pin_config_t pin_config_output =
{
	kGPIO_DigitalOutput,
	0
};

// LEDs
#define Port_LEDgreen	PORTB
#define GPIO_LEDgreen	GPIOB
#define Pin_LEDgreen	18
#define Port_LEDred		PORTC
#define GPIO_LEDred		GPIOC
#define Pin_LEDred		14

// Buttons
#define Port_PTT		PORTA
#define GPIO_PTT		GPIOA
#define Pin_PTT			1
#define Port_SK1		PORTB
#define GPIO_SK1		GPIOB
#define Pin_SK1			1
#define Port_SK2		PORTB
#define GPIO_SK2		GPIOB
#define Pin_SK2			9
#define Port_Orange		PORTA
#define GPIO_Orange		GPIOA
#define Pin_Orange		2

// Keyboard column lines
#define Port_Key_Col0   PORTC
#define GPIO_Key_Col0 	GPIOC
#define Pin_Key_Col0	0
#define Port_Key_Col1   PORTC
#define GPIO_Key_Col1 	GPIOC
#define Pin_Key_Col1 	1
#define Port_Key_Col2   PORTC
#define GPIO_Key_Col2 	GPIOC
#define Pin_Key_Col2 	2
#define Port_Key_Col3   PORTC
#define GPIO_Key_Col3 	GPIOC
#define Pin_Key_Col3 	3

// Keyboard row lines
#define Port_Key_Row0   PORTB
#define GPIO_Key_Row0 	GPIOB
#define Pin_Key_Row0	19
#define Port_Key_Row1   PORTB
#define GPIO_Key_Row1 	GPIOB
#define Pin_Key_Row1	20
#define Port_Key_Row2   PORTB
#define GPIO_Key_Row2 	GPIOB
#define Pin_Key_Row2	21
#define Port_Key_Row3   PORTB
#define GPIO_Key_Row3 	GPIOB
#define Pin_Key_Row3	22
#define Port_Key_Row4   PORTB
#define GPIO_Key_Row4 	GPIOB
#define Pin_Key_Row4	23

// Display
#define Port_Display_Light	PORTC
#define GPIO_Display_Light	GPIOC
#define Pin_Display_Light	4
#define Port_Display_CS		PORTC
#define GPIO_Display_CS		GPIOC
#define Pin_Display_CS		8
#define Port_Display_RST	PORTC
#define GPIO_Display_RST	GPIOC
#define Pin_Display_RST		9
#define Port_Display_RS		PORTC
#define GPIO_Display_RS		GPIOC
#define Pin_Display_RS		10
#define Port_Display_SCK	PORTC
#define GPIO_Display_SCK 	GPIOC
#define Pin_Display_SCK		11
#define Port_Display_SDA    PORTC
#define GPIO_Display_SDA 	GPIOC
#define Pin_Display_SDA		12

// Power On/Off logic
#define Port_Keep_Power_On  PORTE
#define GPIO_Keep_Power_On  GPIOE
#define Pin_Keep_Power_On	26
#define Port_Power_Switch   PORTA
#define GPIO_Power_Switch 	GPIOA
#define Pin_Power_Switch	13

// I2C to AT24C512 EEPROM & AT1846S
// OUT/ON E24 - I2C SCL to AT24C512 EEPROM & AT1846S
// OUT/ON E25 - I2C SDA to AT24C512 EEPROM & AT1846S
#define Port_I2C_SCL       PORTE
#define GPIO_I2C_SCL       GPIOE
#define Pin_I2C_SCL		   24
#define Port_I2C_SDA       PORTE
#define GPIO_I2C_SDA       GPIOE
#define Pin_I2C_SDA		   25

// SPI to W25Q80BV 1M flash
// OUT/ON  A19 - SPI /CS to W25Q80BV 1M flash
// OUT/OFF E5  - SPI CLK to W25Q80BV 1M flash
// OUT/ON  E4  - SPI DI to W25Q80BV 1M flash
// IN      E6  - SPI DO to W25Q80BV 1M flash
#define Port_SPI_CS_FLASH  PORTA
#define GPIO_SPI_CS_FLASH  GPIOA
#define Pin_SPI_CS_FLASH   19
#define Port_SPI_CLK_FLASH PORTE
#define GPIO_SPI_CLK_FLASH GPIOE
#define Pin_SPI_CLK_FLASH  5
#define Port_SPI_DI_FLASH  PORTE
#define GPIO_SPI_DI_FLASH  GPIOE
#define Pin_SPI_DI_FLASH   4
#define Port_SPI_DO_FLASH  PORTE
#define GPIO_SPI_DO_FLASH  GPIOE
#define Pin_SPI_DO_FLASH   6

// SPI to C6000 (C_SPI)
// OUT/ON  A16 - SPI /C_CS to C6000
// OUT/OFF A14 - SPI C_CLK to C6000
// OUT/ON  A12 - SPI C_DI to C6000
// IN      A15 - SPI C_DO to C6000
#define Port_SPI_CS_C6000_C  PORTA
#define GPIO_SPI_CS_C6000_C  GPIOA
#define Pin_SPI_CS_C6000_C   16
#define Port_SPI_CLK_C6000_C PORTA
#define GPIO_SPI_CLK_C6000_C GPIOA
#define Pin_SPI_CLK_C6000_C  14
#define Port_SPI_DI_C6000_C  PORTA
#define GPIO_SPI_DI_C6000_C  GPIOA
#define Pin_SPI_DI_C6000_C   12
#define Port_SPI_DO_C6000_C  PORTA
#define GPIO_SPI_DO_C6000_C  GPIOA
#define Pin_SPI_DO_C6000_C   15

// SPI to C6000 (V_SPI)
// OUT/ON  B10 - SPI /V_CS to C6000
// OUT/OFF B11 - SPI V_CLK to C6000
// OUT/ON  B16 - SPI V_DI to C6000
// IN      B17 - SPI V_DO to C6000
#define Port_SPI_CS_C6000_V  PORTB
#define GPIO_SPI_CS_C6000_V  GPIOB
#define Pin_SPI_CS_C6000_V   10
#define Port_SPI_CLK_C6000_V PORTB
#define GPIO_SPI_CLK_C6000_V GPIOB
#define Pin_SPI_CLK_C6000_V  11
#define Port_SPI_DI_C6000_V  PORTB
#define GPIO_SPI_DI_C6000_V  GPIOB
#define Pin_SPI_DI_C6000_V   16
#define Port_SPI_DO_C6000_V  PORTB
#define GPIO_SPI_DO_C6000_V  GPIOB
#define Pin_SPI_DO_C6000_V   17

// SPI to C6000 (U_SPI)
// OUT/ON  D0 - SPI /U_CS to C6000
// OUT/OFF D1 - SPI U_CLK to C6000
// OUT/ON  D2 - SPI U_DI to C6000
// IN      D3 - SPI U_DO to C6000
#define Port_SPI_CS_C6000_U  PORTD
#define GPIO_SPI_CS_C6000_U  GPIOD
#define Pin_SPI_CS_C6000_U   0
#define Port_SPI_CLK_C6000_U PORTD
#define GPIO_SPI_CLK_C6000_U GPIOD
#define Pin_SPI_CLK_C6000_U  1
#define Port_SPI_DI_C6000_U  PORTD
#define GPIO_SPI_DI_C6000_U  GPIOD
#define Pin_SPI_DI_C6000_U   2
#define Port_SPI_DO_C6000_U  PORTD
#define GPIO_SPI_DO_C6000_U  GPIOD
#define Pin_SPI_DO_C6000_U   3

// C6000 interrupts
// IN      C7  - C6000 RF_RX_INTER
// IN      C16 - C6000 RF_TX_INTER
// IN      C17 - C6000 SYS_INTER
// IN      C18 - C6000 TIME_SLOT_INTER
#define Port_INT_C6000_RF_RX PORTC
#define GPIO_INT_C6000_RF_RX GPIOC
#define Pin_INT_C6000_RF_RX  7
#define Port_INT_C6000_RF_TX PORTC
#define GPIO_INT_C6000_RF_TX GPIOC
#define Pin_INT_C6000_RF_TX  16
#define Port_INT_C6000_SYS   PORTC
#define GPIO_INT_C6000_SYS   GPIOC
#define Pin_INT_C6000_SYS    17
#define Port_INT_C6000_TS    PORTC
#define GPIO_INT_C6000_TS    GPIOC
#define Pin_INT_C6000_TS     18

// Connections with C6000
// OUT/ON  E0 - C6000 RESETn
// OUT/ON  E1 - C6000 PWD
#define Port_INT_C6000_RESET PORTE
#define GPIO_INT_C6000_RESET GPIOE
#define Pin_INT_C6000_RESET  0
#define Port_INT_C6000_PWD   PORTE
#define GPIO_INT_C6000_PWD   GPIOE
#define Pin_INT_C6000_PWD    1

// Yet unknown
// OUT/OFF A17 - ???
// OUT/OFF B0  - ???
// OUT/ON  C5  - ???
// OUT/OFF C6  - ???
// OUT/OFF C13 - ???
// OUT/OFF C15 - ???
// OUT/OFF E2  - ???
// OUT/OFF E3  - ???
#define Port_UNKOWN_A17 PORTA
#define GPIO_UNKOWN_A17 GPIOA
#define Pin_UNKOWN_A17  17
#define Port_UNKOWN_B0  PORTB
#define GPIO_UNKOWN_B0  GPIOB
#define Pin_UNKOWN_B0   0
#define Port_UNKOWN_C5  PORTC
#define GPIO_UNKOWN_C5  GPIOC
#define Pin_UNKOWN_C5   5
#define Port_UNKOWN_C6  PORTC
#define GPIO_UNKOWN_C6  GPIOC
#define Pin_UNKOWN_C6   6
#define Port_UNKOWN_C13 PORTC
#define GPIO_UNKOWN_C13 GPIOC
#define Pin_UNKOWN_C13  13
#define Port_UNKOWN_C15 PORTC
#define GPIO_UNKOWN_C15 GPIOC
#define Pin_UNKOWN_C15  15
#define Port_UNKOWN_E2  PORTE
#define GPIO_UNKOWN_E2  GPIOE
#define Pin_UNKOWN_E2   2
#define Port_UNKOWN_E3  PORTE
#define GPIO_UNKOWN_E3  GPIOE
#define Pin_UNKOWN_E3   3

#endif /* __PORT_IO_DEF_H__ */
