/*
 * Copyright (c) 2015 - 2016, Freescale Semiconductor, Inc.
 * Copyright 2016 - 2017 NXP
 * All rights reserved.
 *
 * SPDX-License-Identifier: BSD-3-Clause
 */

#include "usb_device_config.h"
#include "usb.h"
#include "usb_device.h"

#include "usb_device_class.h"
#include "usb_device_hid.h"
#include "usb_device_ch9.h"
#include "usb_device_descriptor.h"
#include "mouse.h"

#include "fsl_device_registers.h"
#include "clock_config.h"
#include "board.h"
#include "fsl_debug_console.h"

#include <stdio.h>
#include <stdlib.h>
#if (defined(FSL_FEATURE_SOC_SYSMPU_COUNT) && (FSL_FEATURE_SOC_SYSMPU_COUNT > 0U))
#include "fsl_sysmpu.h"
#endif /* FSL_FEATURE_SOC_SYSMPU_COUNT */

#if defined(USB_DEVICE_CONFIG_EHCI) && (USB_DEVICE_CONFIG_EHCI > 0U)
#include "usb_phy.h"
#endif

#include "port_io_def.h"

#include "UC1701.h"

/*******************************************************************************
 * Definitions
 ******************************************************************************/

/*******************************************************************************
 * Prototypes
 ******************************************************************************/
void BOARD_InitHardware(void);
void USB_DeviceClockInit(void);
void USB_DeviceIsrEnable(void);
#if USB_DEVICE_CONFIG_USE_TASK
void USB_DeviceTaskFn(void *deviceHandle);
#endif

static usb_status_t USB_DeviceHidMouseCallback(class_handle_t handle, uint32_t event, void *param);
static usb_status_t USB_DeviceCallback(usb_device_handle handle, uint32_t event, void *param);
static void USB_DeviceApplicationInit(void);

/*******************************************************************************
 * Variables
 ******************************************************************************/

USB_DMA_NONINIT_DATA_ALIGN(USB_DATA_ALIGN_SIZE) static uint8_t s_MouseBuffer[USB_HID_MOUSE_REPORT_LENGTH];
usb_hid_mouse_struct_t g_UsbDeviceHidMouse;

extern usb_device_class_struct_t g_UsbDeviceHidMouseConfig;

#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
usb_device_dcd_charging_time_t g_UsbDeviceDcdTimingConfig;
#endif

/* Set class configurations */
usb_device_class_config_struct_t g_UsbDeviceHidConfig[1] = {{
    USB_DeviceHidMouseCallback, /* HID mouse class callback pointer */
    (class_handle_t)NULL,       /* The HID class handle, This field is set by USB_DeviceClassInit */
    &g_UsbDeviceHidMouseConfig, /* The HID mouse configuration, including class code, subcode, and protocol, class type,
                           transfer type, endpoint address, max packet size, etc.*/
}};

/* Set class configuration list */
usb_device_class_config_list_struct_t g_UsbDeviceHidConfigList = {
    g_UsbDeviceHidConfig, /* Class configurations */
    USB_DeviceCallback,   /* Device callback pointer */
    1U,                   /* Class count */
};

/*******************************************************************************
 * Code
 ******************************************************************************/

void USB0_IRQHandler(void)
{
    USB_DeviceKhciIsrFunction(g_UsbDeviceHidMouse.deviceHandle);
    /* Add for ARM errata 838869, affects Cortex-M4, Cortex-M4F Store immediate overlapping
    exception return operation might vector to incorrect interrupt */
    __DSB();
}
void USB_DeviceClockInit(void)
{
    SystemCoreClockUpdate();
    CLOCK_EnableUsbfs0Clock(kCLOCK_UsbSrcIrc48M, 48000000U);
}
void USB_DeviceIsrEnable(void)
{
    uint8_t irqNumber;

    uint8_t usbDeviceKhciIrq[] = USB_IRQS;
    irqNumber = usbDeviceKhciIrq[CONTROLLER_ID - kUSB_ControllerKhci0];

/* Install isr, set priority, and enable IRQ. */
    NVIC_SetPriority((IRQn_Type)irqNumber, USB_DEVICE_INTERRUPT_PRIORITY);
    EnableIRQ((IRQn_Type)irqNumber);
}
#if USB_DEVICE_CONFIG_USE_TASK
void USB_DeviceTaskFn(void *deviceHandle)
{
    USB_DeviceKhciTaskFunction(deviceHandle);
}
#endif

void init_GD77()
{
    CLOCK_EnableClock(kCLOCK_PortA);
    CLOCK_EnableClock(kCLOCK_PortB);
    CLOCK_EnableClock(kCLOCK_PortC);
    CLOCK_EnableClock(kCLOCK_PortD);
    CLOCK_EnableClock(kCLOCK_PortE);

    // LEDs
    PORT_SetPinMux(Port_LEDgreen, Pin_LEDgreen, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_LEDred, Pin_LEDred, kPORT_MuxAsGpio);

    // Buttons
    PORT_SetPinMux(Port_PTT, Pin_PTT, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SK1, Pin_SK1, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SK2, Pin_SK2, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Orange, Pin_Orange, kPORT_MuxAsGpio);

    // Keyboard column lines
    PORT_SetPinMux(Port_Key_Col0, Pin_Key_Col0, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Key_Col1, Pin_Key_Col1, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Key_Col2, Pin_Key_Col2, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Key_Col3, Pin_Key_Col3, kPORT_MuxAsGpio);

    // Keyboard row lines
    PORT_SetPinMux(Port_Key_Row0, Pin_Key_Row0, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Key_Row1, Pin_Key_Row1, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Key_Row2, Pin_Key_Row2, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Key_Row3, Pin_Key_Row3, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Key_Row4, Pin_Key_Row4, kPORT_MuxAsGpio);

    // Power On/Off logic
    PORT_SetPinMux(Port_Keep_Power_On, Pin_Keep_Power_On, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_Power_Switch, Pin_Power_Switch, kPORT_MuxAsGpio);

    // I2C to AT24C512 EEPROM & AT1846S
    PORT_SetPinMux(Port_I2C_SCL, Pin_I2C_SCL, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_I2C_SDA, Pin_I2C_SDA, kPORT_MuxAsGpio);

    // SPI to W25Q80BV 1M flash
    PORT_SetPinMux(Port_SPI_CS_FLASH, Pin_SPI_CS_FLASH, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_CLK_FLASH, Pin_SPI_CLK_FLASH, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DI_FLASH, Pin_SPI_DI_FLASH, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DO_FLASH, Pin_SPI_DO_FLASH, kPORT_MuxAsGpio);

    // SPI to C6000 (C_SPI)
    PORT_SetPinMux(Port_SPI_CS_C6000_C, Pin_SPI_CS_C6000_C, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_CLK_C6000_C, Pin_SPI_CLK_C6000_C, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DI_C6000_C, Pin_SPI_DI_C6000_C, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DO_C6000_C, Pin_SPI_DO_C6000_C, kPORT_MuxAsGpio);

    // SPI to C6000 (V_SPI)
    PORT_SetPinMux(Port_SPI_CS_C6000_V, Pin_SPI_CS_C6000_V, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_CLK_C6000_V, Pin_SPI_CLK_C6000_V, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DI_C6000_V, Pin_SPI_DI_C6000_V, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DO_C6000_V, Pin_SPI_DO_C6000_V, kPORT_MuxAsGpio);

    // SPI to C6000 (U_SPI)
    PORT_SetPinMux(Port_SPI_CS_C6000_U, Pin_SPI_CS_C6000_U, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_CLK_C6000_U, Pin_SPI_CLK_C6000_U, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DI_C6000_U, Pin_SPI_DI_C6000_U, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_SPI_DO_C6000_U, Pin_SPI_DO_C6000_U, kPORT_MuxAsGpio);

    // C6000 interrupts
    PORT_SetPinMux(Port_INT_C6000_RF_RX, Pin_INT_C6000_RF_RX, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_INT_C6000_RF_TX, Pin_INT_C6000_RF_TX, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_INT_C6000_SYS, Pin_INT_C6000_SYS, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_INT_C6000_TS, Pin_INT_C6000_TS, kPORT_MuxAsGpio);

    // Connections with C6000
    PORT_SetPinMux(Port_INT_C6000_RESET, Pin_INT_C6000_RESET, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_INT_C6000_PWD, Pin_INT_C6000_PWD, kPORT_MuxAsGpio);

    // Yet unknown
    PORT_SetPinMux(Port_UNKOWN_A17, Pin_UNKOWN_A17, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_UNKOWN_B0, Pin_UNKOWN_B0, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_UNKOWN_C5, Pin_UNKOWN_C5, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_UNKOWN_C6, Pin_UNKOWN_C6, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_UNKOWN_C13, Pin_UNKOWN_C13, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_UNKOWN_C15, Pin_UNKOWN_C15, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_UNKOWN_E2, Pin_UNKOWN_E2, kPORT_MuxAsGpio);
    PORT_SetPinMux(Port_UNKOWN_E3, Pin_UNKOWN_E3, kPORT_MuxAsGpio);

    // LEDs
    GPIO_PinInit(GPIO_LEDgreen, Pin_LEDgreen, &pin_config_output);
    GPIO_PinInit(GPIO_LEDred, Pin_LEDred, &pin_config_output);

    // Buttons
    GPIO_PinInit(GPIO_PTT, Pin_PTT, &pin_config_input);
    GPIO_PinInit(GPIO_SK1, Pin_SK1, &pin_config_input);
    GPIO_PinInit(GPIO_SK2, Pin_SK2, &pin_config_input);
    GPIO_PinInit(GPIO_Orange, Pin_Orange, &pin_config_input);

    // Keyboard column lines
    GPIO_PinInit(GPIO_Key_Col0, Pin_Key_Col0, &pin_config_input);
    GPIO_PinInit(GPIO_Key_Col1, Pin_Key_Col1, &pin_config_input);
    GPIO_PinInit(GPIO_Key_Col2, Pin_Key_Col2, &pin_config_input);
    GPIO_PinInit(GPIO_Key_Col3, Pin_Key_Col3, &pin_config_input);

    // Keyboard row lines
    GPIO_PinInit(GPIO_Key_Row0, Pin_Key_Row0, &pin_config_input);
    GPIO_PinInit(GPIO_Key_Row1, Pin_Key_Row1, &pin_config_input);
    GPIO_PinInit(GPIO_Key_Row2, Pin_Key_Row2, &pin_config_input);
    GPIO_PinInit(GPIO_Key_Row3, Pin_Key_Row3, &pin_config_input);
    GPIO_PinInit(GPIO_Key_Row4, Pin_Key_Row4, &pin_config_input);

    // Power On/Off logic
    GPIO_PinInit(GPIO_Keep_Power_On, Pin_Keep_Power_On, &pin_config_output);
    GPIO_PinInit(GPIO_Power_Switch, Pin_Power_Switch, &pin_config_input);
	GPIO_PinWrite(GPIO_Keep_Power_On, Pin_Keep_Power_On, 1);

    // I2C to AT24C512 EEPROM & AT1846S
    GPIO_PinInit(GPIO_I2C_SCL, Pin_I2C_SCL, &pin_config_output);
    GPIO_PinInit(GPIO_I2C_SDA, Pin_I2C_SDA, &pin_config_output);
    GPIO_PinWrite(GPIO_I2C_SCL, Pin_I2C_SCL, 1);
    GPIO_PinWrite(GPIO_I2C_SDA, Pin_I2C_SDA, 1);

    // SPI to W25Q80BV 1M flash
    GPIO_PinInit(GPIO_SPI_CS_FLASH, Pin_SPI_CS_FLASH, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_CLK_FLASH, Pin_SPI_CLK_FLASH, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DI_FLASH,Pin_SPI_DI_FLASH, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DO_FLASH, Pin_SPI_DO_FLASH, &pin_config_input);
    GPIO_PinWrite(GPIO_SPI_CS_FLASH, Pin_SPI_CS_FLASH, 1);
    GPIO_PinWrite(GPIO_SPI_CLK_FLASH, Pin_SPI_CLK_FLASH, 0);
    GPIO_PinWrite(GPIO_SPI_DI_FLASH,Pin_SPI_DI_FLASH, 1);

    // SPI to C6000 (C_SPI)
    GPIO_PinInit(GPIO_SPI_CS_C6000_C, Pin_SPI_CS_C6000_C, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_CLK_C6000_C, Pin_SPI_CLK_C6000_C, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DI_C6000_C,Pin_SPI_DI_C6000_C, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DO_C6000_C, Pin_SPI_DO_C6000_C, &pin_config_input);
    GPIO_PinWrite(GPIO_SPI_CS_C6000_C, Pin_SPI_CS_C6000_C, 1);
    GPIO_PinWrite(GPIO_SPI_CLK_C6000_C, Pin_SPI_CLK_C6000_C, 0);
    GPIO_PinWrite(GPIO_SPI_DI_C6000_C,Pin_SPI_DI_C6000_C, 1);

    // SPI to C6000 (V_SPI)
    GPIO_PinInit(GPIO_SPI_CS_C6000_V, Pin_SPI_CS_C6000_V, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_CLK_C6000_V, Pin_SPI_CLK_C6000_V, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DI_C6000_V,Pin_SPI_DI_C6000_V, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DO_C6000_V, Pin_SPI_DO_C6000_V, &pin_config_input);
    GPIO_PinWrite(GPIO_SPI_CS_C6000_V, Pin_SPI_CS_C6000_V, 1);
    GPIO_PinWrite(GPIO_SPI_CLK_C6000_V, Pin_SPI_CLK_C6000_V, 0);
    GPIO_PinWrite(GPIO_SPI_DI_C6000_V,Pin_SPI_DI_C6000_V, 1);

    // SPI to C6000 (U_SPI)
    GPIO_PinInit(GPIO_SPI_CS_C6000_U, Pin_SPI_CS_C6000_U, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_CLK_C6000_U, Pin_SPI_CLK_C6000_U, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DI_C6000_U,Pin_SPI_DI_C6000_U, &pin_config_output);
    GPIO_PinInit(GPIO_SPI_DO_C6000_U, Pin_SPI_DO_C6000_U, &pin_config_input);
    GPIO_PinWrite(GPIO_SPI_CS_C6000_U, Pin_SPI_CS_C6000_U, 1);
    GPIO_PinWrite(GPIO_SPI_CLK_C6000_U, Pin_SPI_CLK_C6000_U, 0);
    GPIO_PinWrite(GPIO_SPI_DI_C6000_U,Pin_SPI_DI_C6000_U, 1);

    // C6000 interrupts
    GPIO_PinInit(GPIO_INT_C6000_RF_RX, Pin_INT_C6000_RF_RX, &pin_config_input);
    GPIO_PinInit(GPIO_INT_C6000_RF_TX, Pin_INT_C6000_RF_TX, &pin_config_input);
    GPIO_PinInit(GPIO_INT_C6000_SYS, Pin_INT_C6000_SYS, &pin_config_input);
    GPIO_PinInit(GPIO_INT_C6000_TS, Pin_INT_C6000_TS, &pin_config_input);

    // Yet unknown
    GPIO_PinInit(GPIO_UNKOWN_B0, Pin_UNKOWN_B0, &pin_config_output);
    GPIO_PinInit(GPIO_UNKOWN_C5, Pin_UNKOWN_C5, &pin_config_output);
    GPIO_PinInit(GPIO_UNKOWN_C6, Pin_UNKOWN_C6, &pin_config_output);
    GPIO_PinWrite(GPIO_UNKOWN_B0, Pin_UNKOWN_B0, 0);
    GPIO_PinWrite(GPIO_UNKOWN_C5, Pin_UNKOWN_C5, 1);
    GPIO_PinWrite(GPIO_UNKOWN_C6, Pin_UNKOWN_C6, 0);

    // More unknown pin initialization before SPI init of C6000
    GPIO_PinWrite(GPIO_UNKOWN_C5, Pin_UNKOWN_C5, 0);
    GPIO_PinWrite(GPIO_UNKOWN_C6, Pin_UNKOWN_C6, 1);

    // Connections with C6000
    GPIO_PinInit(GPIO_INT_C6000_RESET, Pin_INT_C6000_RESET, &pin_config_output);
    GPIO_PinInit(GPIO_INT_C6000_PWD, Pin_INT_C6000_PWD, &pin_config_output);
    GPIO_PinWrite(GPIO_INT_C6000_RESET, Pin_INT_C6000_RESET, 1);
    GPIO_PinWrite(GPIO_INT_C6000_PWD, Pin_INT_C6000_PWD, 1);

    // More C6000 pin initialization before SPI init of C6000
    GPIO_PinWrite(GPIO_INT_C6000_PWD, Pin_INT_C6000_PWD, 0);

    // Yet unknown
    GPIO_PinInit(GPIO_UNKOWN_A17, Pin_UNKOWN_A17, &pin_config_output);
    GPIO_PinInit(GPIO_UNKOWN_C13, Pin_UNKOWN_C13, &pin_config_output);
    GPIO_PinInit(GPIO_UNKOWN_C15, Pin_UNKOWN_C15, &pin_config_output);
    GPIO_PinInit(GPIO_UNKOWN_E2, Pin_UNKOWN_E2, &pin_config_output);
    GPIO_PinInit(GPIO_UNKOWN_E3, Pin_UNKOWN_E3, &pin_config_output);
    GPIO_PinWrite(GPIO_UNKOWN_A17, Pin_UNKOWN_A17, 0);
    GPIO_PinWrite(GPIO_UNKOWN_C13, Pin_UNKOWN_C13, 0);
    GPIO_PinWrite(GPIO_UNKOWN_C15, Pin_UNKOWN_C15, 0);
    GPIO_PinWrite(GPIO_UNKOWN_E2, Pin_UNKOWN_E2, 0);
    GPIO_PinWrite(GPIO_UNKOWN_E3, Pin_UNKOWN_E3, 0);

    UC1701_begin();
}

uint8_t read_keyboard_col()
{
	uint8_t result=0;
	if (GPIO_PinRead(GPIO_Key_Row0, Pin_Key_Row0)==0)
	{
		result|=0x01;
	}
	if (GPIO_PinRead(GPIO_Key_Row1, Pin_Key_Row1)==0)
	{
		result|=0x02;
	}
	if (GPIO_PinRead(GPIO_Key_Row2, Pin_Key_Row2)==0)
	{
		result|=0x04;
	}
	if (GPIO_PinRead(GPIO_Key_Row3, Pin_Key_Row3)==0)
	{
		result|=0x08;
	}
	if (GPIO_PinRead(GPIO_Key_Row4, Pin_Key_Row4)==0)
	{
		result|=0x10;
	}
	return result;
}

uint32_t read_keyboard()
{
    GPIO_PinInit(GPIO_Key_Col3, Pin_Key_Col3, &pin_config_output);
	GPIO_PinWrite(GPIO_Key_Col3, Pin_Key_Col3, 0);
	uint32_t result=read_keyboard_col();
	GPIO_PinWrite(GPIO_Key_Col3, Pin_Key_Col3, 1);
    GPIO_PinInit(GPIO_Key_Col3, Pin_Key_Col3, &pin_config_input);

    GPIO_PinInit(GPIO_Key_Col2, Pin_Key_Col2, &pin_config_output);
	GPIO_PinWrite(GPIO_Key_Col2, Pin_Key_Col2, 0);
	result=(result<<5)|read_keyboard_col();
	GPIO_PinWrite(GPIO_Key_Col2, Pin_Key_Col2, 1);
    GPIO_PinInit(GPIO_Key_Col2, Pin_Key_Col2, &pin_config_input);

    GPIO_PinInit(GPIO_Key_Col1, Pin_Key_Col1, &pin_config_output);
	GPIO_PinWrite(GPIO_Key_Col1, Pin_Key_Col1, 0);
	result=(result<<5)|read_keyboard_col();
	GPIO_PinWrite(GPIO_Key_Col1, Pin_Key_Col1, 1);
    GPIO_PinInit(GPIO_Key_Col1, Pin_Key_Col1, &pin_config_input);

    GPIO_PinInit(GPIO_Key_Col0, Pin_Key_Col0, &pin_config_output);
	GPIO_PinWrite(GPIO_Key_Col0, Pin_Key_Col0, 0);
	result=(result<<5)|read_keyboard_col();
	GPIO_PinWrite(GPIO_Key_Col0, Pin_Key_Col0, 1);
    GPIO_PinInit(GPIO_Key_Col0, Pin_Key_Col0, &pin_config_input);

    return result;
}

uint8_t LED_to_device = 0x00;
uint8_t Button_from_device = 0x00;
uint32_t Keyboard_from_device = 0x00000000;
bool LED_Touched = false;

void IO_task()
{
	while(1U)
	{
		taskENTER_CRITICAL();
		uint8_t LED_to_device_TMP = LED_to_device;
		bool LED_Touched_tmp = LED_Touched;
		LED_Touched = false;
		taskEXIT_CRITICAL();

		if (LED_Touched_tmp)
		{
			if ((LED_to_device_TMP & 0x01)!=0)
			{
				GPIO_PinWrite(GPIO_LEDgreen, Pin_LEDgreen, 1);
			}
			else
			{
				GPIO_PinWrite(GPIO_LEDgreen, Pin_LEDgreen, 0);
			}
			if ((LED_to_device_TMP & 0x02)!=0)
			{
				GPIO_PinWrite(GPIO_LEDred, Pin_LEDred, 1);
			}
			else
			{
				GPIO_PinWrite(GPIO_LEDred, Pin_LEDred, 0);
			}
			if ((LED_to_device_TMP & 0x04)!=0)
			{
				GPIO_PinWrite(GPIO_Display_Light, Pin_Display_Light, 1);
			}
			else
			{
				GPIO_PinWrite(GPIO_Display_Light, Pin_Display_Light, 0);
			}
		}

		uint8_t Button_from_device_TMP=0;
    	if (GPIO_PinRead(GPIO_PTT, Pin_PTT)==0)
    	{
    		Button_from_device_TMP|=0x01;
    	}
    	if (GPIO_PinRead(GPIO_SK1, Pin_SK1)==0)
    	{
    		Button_from_device_TMP|=0x02;
    	}
    	if (GPIO_PinRead(GPIO_SK2, Pin_SK2)==0)
    	{
    		Button_from_device_TMP|=0x04;
    	}
    	if (GPIO_PinRead(GPIO_Orange, Pin_Orange)==0)
    	{
    		Button_from_device_TMP|=0x08;
    	}
    	uint32_t Keyboard_from_device_TMP = read_keyboard();

		taskENTER_CRITICAL();
		Button_from_device=Button_from_device_TMP;
		Keyboard_from_device=Keyboard_from_device_TMP;
		taskEXIT_CRITICAL();

		vTaskDelay(portTICK_PERIOD_MS);
	}
}

uint8_t Device_CMD = 0;
uint8_t Device_buffer[128];
int Display_light_Timer = 0;
bool Display_light_Touched = false;
bool Show_SplashScreen = false;
int SplashScreen_Timer = 0;
bool Shutdown = false;
int Shutdown_Timer = 0;

void show_splashscreen()
{
	UC1701_clear();
	UC1701_setCursor(5*6,1);
	UC1701_print((unsigned char*)"Experimental");
	UC1701_setCursor(7*6,2);
	UC1701_print((unsigned char*)"firmware");
	UC1701_setCursor(10*6,4);
	UC1701_print((unsigned char*)"by");
	UC1701_setCursor(8*6,6);
	UC1701_print((unsigned char*)"DG4KLU");
	Display_light_Touched = true;
}

void show_running()
{
	UC1701_clear();
	UC1701_setCursor(7*6+3,4);
	UC1701_print((unsigned char*)"RUNNING");
	Display_light_Touched = true;
}

void show_poweroff()
{
	UC1701_clear();
	UC1701_setCursor(4*6+3,2);
	UC1701_print((unsigned char*)"Power off ...");
	UC1701_setCursor(5*6,4);
	UC1701_print((unsigned char*)"73 de DG4KLU");
	Display_light_Touched = true;
}

void Display_task()
{
	Show_SplashScreen = true;

	while(1U)
	{
    	if ((GPIO_PinRead(GPIO_Power_Switch, Pin_Power_Switch)!=0) && (!Shutdown))
    	{
    		Show_SplashScreen=false;
    		SplashScreen_Timer=0;
    		show_poweroff();
    		Shutdown=true;
			Shutdown_Timer = 2000;
    	}
    	else if ((GPIO_PinRead(GPIO_Power_Switch, Pin_Power_Switch)==0) && (Shutdown))
    	{
			show_running();
			Shutdown=false;
			Shutdown_Timer = 0;
    	}

    	if (Shutdown)
    	{
    		if (Shutdown_Timer>0)
    		{
    			Shutdown_Timer--;
    			if (Shutdown_Timer==0)
    			{
    				GPIO_PinWrite(GPIO_Keep_Power_On, Pin_Keep_Power_On, 0);
    			}
    		}
    	}

		if (Show_SplashScreen)
		{
			show_splashscreen();
			SplashScreen_Timer = 4000;
			Show_SplashScreen = false;
		}

		if (SplashScreen_Timer>0)
		{
			SplashScreen_Timer--;
			if (SplashScreen_Timer==0)
			{
				show_running();
			}
		}

		taskENTER_CRITICAL();
		uint8_t Device_CMD_tmp=Device_CMD;
		Device_CMD=0;
		uint8_t Device_buffer_tmp[128];
		memcpy(Device_buffer_tmp, Device_buffer, sizeof(Device_buffer));
		taskEXIT_CRITICAL();

		if (Device_CMD_tmp==1)
		{
			UC1701_clear();
		}
		else if (Device_CMD_tmp==2)
		{
		    UC1701_setCursor(Device_buffer_tmp[0],Device_buffer_tmp[1]);
		}
		else if (Device_CMD_tmp==3)
		{
			UC1701_write(Device_buffer_tmp[0]);
			Display_light_Touched = true;
		}
		else if (Device_CMD_tmp==4)
		{
		    UC1701_print(Device_buffer_tmp);
			Display_light_Touched = true;
		}

		if (Display_light_Touched)
		{
			if (Display_light_Timer==0)
			{
				GPIO_PinWrite(GPIO_Display_Light, Pin_Display_Light, 1);
			}
			Display_light_Timer = 4000;
			Display_light_Touched = false;
		}

		if (Display_light_Timer>0)
		{
			Display_light_Timer--;
			if (Display_light_Timer==0)
			{
				GPIO_PinWrite(GPIO_Display_Light, Pin_Display_Light, 0);
			}
		}

		vTaskDelay(portTICK_PERIOD_MS);
	}
}

static int state = 0;
static int state_cmd = 0;

/* The hid class callback */
static usb_status_t USB_DeviceHidMouseCallback(class_handle_t handle, uint32_t event, void *param)
{
    usb_status_t error = kStatus_USB_Error;
    usb_device_endpoint_callback_message_struct_t *message = (usb_device_endpoint_callback_message_struct_t *)param;

    switch (event)
    {
        case kUSB_DeviceHidEventSendResponse:
            /* Resport sent */
            if (g_UsbDeviceHidMouse.attach)
            {
                if ((NULL != message) && (message->length == USB_UNINITIALIZED_VAL_32))
                {
                    return error;
                }
            }
            break;
        case kUSB_DeviceHidEventGetReport:
            break;
        case kUSB_DeviceHidEventSetReport:
        	switch (state)
        	{
        		case 0:
					if ((g_UsbDeviceHidMouse.buffer[4]=='C') && (g_UsbDeviceHidMouse.buffer[5]=='M') && (g_UsbDeviceHidMouse.buffer[6]=='D'))
					{
						state_cmd=g_UsbDeviceHidMouse.buffer[7];
						state=1;
    		        	g_UsbDeviceHidMouse.buffer[0]=0x03;
    		        	g_UsbDeviceHidMouse.buffer[1]=0;
    		        	g_UsbDeviceHidMouse.buffer[2]=1;
    		        	g_UsbDeviceHidMouse.buffer[3]=0;
						g_UsbDeviceHidMouse.buffer[4]='A';
						USB_DeviceHidSend(g_UsbDeviceHidMouse.hidHandle, USB_HID_MOUSE_ENDPOINT_IN, g_UsbDeviceHidMouse.buffer, USB_HID_MOUSE_REPORT_LENGTH);
					}
					break;
        		case 1:
        			switch (state_cmd)
        			{
        				case 1:
        		            for (int i=0; i<32; i++)
        		            {
        		            	g_UsbDeviceHidMouse.buffer[i+4]=255-g_UsbDeviceHidMouse.buffer[i+4];
        		            }
        		        	g_UsbDeviceHidMouse.buffer[0]=0x03;
        		            error = USB_DeviceHidSend(g_UsbDeviceHidMouse.hidHandle, USB_HID_MOUSE_ENDPOINT_IN, g_UsbDeviceHidMouse.buffer, USB_HID_MOUSE_REPORT_LENGTH);
        					break;
        				case 2:
							LED_to_device=g_UsbDeviceHidMouse.buffer[4];
							LED_Touched = true;
							g_UsbDeviceHidMouse.buffer[4]=Button_from_device;
    		        		g_UsbDeviceHidMouse.buffer[5]=(Keyboard_from_device & 0x000000ff)>>0;
    		        		g_UsbDeviceHidMouse.buffer[6]=(Keyboard_from_device & 0x0000ff00)>>8;
    		        		g_UsbDeviceHidMouse.buffer[7]=(Keyboard_from_device & 0x00ff0000)>>16;
    		        		g_UsbDeviceHidMouse.buffer[8]=(Keyboard_from_device & 0xff000000)>>24;
        		        	g_UsbDeviceHidMouse.buffer[0]=0x03;
        		            error = USB_DeviceHidSend(g_UsbDeviceHidMouse.hidHandle, USB_HID_MOUSE_ENDPOINT_IN, g_UsbDeviceHidMouse.buffer, USB_HID_MOUSE_REPORT_LENGTH);
        					break;
        				case 3:
        					Device_CMD = g_UsbDeviceHidMouse.buffer[4];
        					for (int i=0;i<30;i++)
        					{
            					Device_buffer[i] = g_UsbDeviceHidMouse.buffer[5 + i];
        					}
        		        	g_UsbDeviceHidMouse.buffer[0]=0x03;
        		        	g_UsbDeviceHidMouse.buffer[1]=0;
        		        	g_UsbDeviceHidMouse.buffer[2]=1;
        		        	g_UsbDeviceHidMouse.buffer[3]=0;
    						g_UsbDeviceHidMouse.buffer[4]='A';
    						USB_DeviceHidSend(g_UsbDeviceHidMouse.hidHandle, USB_HID_MOUSE_ENDPOINT_IN, g_UsbDeviceHidMouse.buffer, USB_HID_MOUSE_REPORT_LENGTH);
        					break;
        			}
					state=0;
					state_cmd=0;
					break;
        	}
            break;
        case kUSB_DeviceHidEventRequestReportBuffer:
            if (g_UsbDeviceHidMouse.attach)
            {
            	usb_device_hid_report_struct_t *g_output_report = (usb_device_hid_report_struct_t *)param;
            	if (g_output_report->reportLength <= USB_HID_MOUSE_REPORT_LENGTH)
            	{
                	g_output_report->reportBuffer = g_UsbDeviceHidMouse.buffer;
            	}
            	error = kStatus_USB_Success;
            }
            else
            {
            	error = kStatus_USB_InvalidRequest;
            }
            break;
        case kUSB_DeviceHidEventGetIdle:
        case kUSB_DeviceHidEventGetProtocol:
        case kUSB_DeviceHidEventSetIdle:
        case kUSB_DeviceHidEventSetProtocol:
            break;
        default:
            break;
    }

    return error;
}

/* The device callback */
static usb_status_t USB_DeviceCallback(usb_device_handle handle, uint32_t event, void *param)
{
    usb_status_t error = kStatus_USB_Error;
    uint16_t *temp16 = (uint16_t *)param;
    uint8_t *temp8 = (uint8_t *)param;

    switch (event)
    {
        case kUSB_DeviceEventBusReset:
        {
            /* USB bus reset signal detected */
            g_UsbDeviceHidMouse.attach = 0U;
            g_UsbDeviceHidMouse.currentConfiguration = 0U;
            error = kStatus_USB_Success;
#if (defined(USB_DEVICE_CONFIG_EHCI) && (USB_DEVICE_CONFIG_EHCI > 0U)) || \
    (defined(USB_DEVICE_CONFIG_LPCIP3511HS) && (USB_DEVICE_CONFIG_LPCIP3511HS > 0U))
            /* Get USB speed to configure the device, including max packet size and interval of the endpoints. */
            if (kStatus_USB_Success == USB_DeviceClassGetSpeed(CONTROLLER_ID, &g_UsbDeviceHidMouse.speed))
            {
                USB_DeviceSetSpeed(handle, g_UsbDeviceHidMouse.speed);
            }
#endif
        }
        break;
#if (defined(USB_DEVICE_CONFIG_DETACH_ENABLE) && (USB_DEVICE_CONFIG_DETACH_ENABLE > 0U))
        case kUSB_DeviceEventAttach:
        {
#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
            g_UsbDeviceHidMouse.vReginInterruptDetected = 1;
            g_UsbDeviceHidMouse.vbusValid = 1;
#else
            usb_echo("USB device attached.\r\n");
            USB_DeviceRun(g_UsbDeviceHidMouse.deviceHandle);
#endif
        }
        break;
        case kUSB_DeviceEventDetach:
        {
#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
            g_UsbDeviceHidMouse.vReginInterruptDetected = 1;
            g_UsbDeviceHidMouse.vbusValid = 0;
            g_UsbDeviceHidMouse.attach = 0;
#else
            usb_echo("USB device detached.\r\n");
            g_UsbDeviceHidMouse.attach = 0;
            USB_DeviceStop(g_UsbDeviceHidMouse.deviceHandle);
#endif
        }
        break;
#endif
        case kUSB_DeviceEventSetConfiguration:
            if (0U ==(*temp8))
            {
                g_UsbDeviceHidMouse.attach = 0;
                g_UsbDeviceHidMouse.currentConfiguration = 0U;
            }
            else if (USB_HID_MOUSE_CONFIGURE_INDEX == (*temp8))
            {
                /* Set device configuration request */
                g_UsbDeviceHidMouse.attach = 1U;
                g_UsbDeviceHidMouse.currentConfiguration = *temp8;
            }
            else
            {
                error = kStatus_USB_InvalidRequest;
            }
            break;
        case kUSB_DeviceEventSetInterface:
            if (g_UsbDeviceHidMouse.attach)
            {
                /* Set device interface request */
                uint8_t interface = (uint8_t)((*temp16 & 0xFF00U) >> 0x08U);
                uint8_t alternateSetting = (uint8_t)(*temp16 & 0x00FFU);
                if (interface < USB_HID_MOUSE_INTERFACE_COUNT)
                {
                    g_UsbDeviceHidMouse.currentInterfaceAlternateSetting[interface] = alternateSetting;
                }
            }
            break;
        case kUSB_DeviceEventGetConfiguration:
            if (param)
            {
                /* Get current configuration request */
                *temp8 = g_UsbDeviceHidMouse.currentConfiguration;
                error = kStatus_USB_Success;
            }
            break;
        case kUSB_DeviceEventGetInterface:
            if (param)
            {
                /* Get current alternate setting of the interface request */
                uint8_t interface = (uint8_t)((*temp16 & 0xFF00U) >> 0x08U);
                if (interface < USB_HID_MOUSE_INTERFACE_COUNT)
                {
                    *temp16 = (*temp16 & 0xFF00U) | g_UsbDeviceHidMouse.currentInterfaceAlternateSetting[interface];
                    error = kStatus_USB_Success;
                }
                else
                {
                    error = kStatus_USB_InvalidRequest;
                }
            }
            break;
        case kUSB_DeviceEventGetDeviceDescriptor:
            if (param)
            {
                /* Get device descriptor request */
                error = USB_DeviceGetDeviceDescriptor(handle, (usb_device_get_device_descriptor_struct_t *)param);
            }
            break;
        case kUSB_DeviceEventGetConfigurationDescriptor:
            if (param)
            {
                /* Get device configuration descriptor request */
                error = USB_DeviceGetConfigurationDescriptor(handle,
                                                             (usb_device_get_configuration_descriptor_struct_t *)param);
            }
            break;
        case kUSB_DeviceEventGetStringDescriptor:
            if (param)
            {
                /* Get device string descriptor request */
                error = USB_DeviceGetStringDescriptor(handle, (usb_device_get_string_descriptor_struct_t *)param);
            }
            break;
        case kUSB_DeviceEventGetHidDescriptor:
            if (param)
            {
                /* Get hid descriptor request */
                error = USB_DeviceGetHidDescriptor(handle, (usb_device_get_hid_descriptor_struct_t *)param);
            }
            break;
        case kUSB_DeviceEventGetHidReportDescriptor:
            if (param)
            {
                /* Get hid report descriptor request */
                error =
                    USB_DeviceGetHidReportDescriptor(handle, (usb_device_get_hid_report_descriptor_struct_t *)param);
            }
            break;
        case kUSB_DeviceEventGetHidPhysicalDescriptor:
            if (param)
            {
                /* Get hid physical descriptor request */
                error = USB_DeviceGetHidPhysicalDescriptor(handle,
                                                           (usb_device_get_hid_physical_descriptor_struct_t *)param);
            }
            break;
#if (defined(USB_DEVICE_CONFIG_CV_TEST) && (USB_DEVICE_CONFIG_CV_TEST > 0U))
        case kUSB_DeviceEventGetDeviceQualifierDescriptor:
            if (param)
            {
                /* Get device descriptor request */
                error = USB_DeviceGetDeviceQualifierDescriptor(
                    handle, (usb_device_get_device_qualifier_descriptor_struct_t *)param);
            }
            break;
#endif
#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
        case kUSB_DeviceEventDcdTimeOut:
            if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusVBUSDetect)
            {
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusTimeOut;
            }
            break;
        case kUSB_DeviceEventDcdUnknownType:
            if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusVBUSDetect)
            {
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusUnknownType;
            }
            break;
        case kUSB_DeviceEventSDPDetected:
            if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusVBUSDetect)
            {
                g_UsbDeviceHidMouse.dcdPortType = kUSB_DeviceDCDPortTypeSDP;
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusDetectFinish;
            }
            break;
        case kUSB_DeviceEventChargingPortDetected:
            if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusVBUSDetect)
            {
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusChargingPortDetect;
            }
            break;
        case kUSB_DeviceEventChargingHostDetected:
            if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusVBUSDetect)
            {
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusDetectFinish;
                g_UsbDeviceHidMouse.dcdPortType = kUSB_DeviceDCDPortTypeCDP;
            }
            break;
        case kUSB_DeviceEventDedicatedChargerDetected:
            if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusVBUSDetect)
            {
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusDetectFinish;
                g_UsbDeviceHidMouse.dcdPortType = kUSB_DeviceDCDPortTypeDCP;
            }
            break;
#endif
        default:
            break;
    }

    return error;
}

static void USB_DeviceApplicationInit(void)
{
    USB_DeviceClockInit();
#if (defined(FSL_FEATURE_SOC_SYSMPU_COUNT) && (FSL_FEATURE_SOC_SYSMPU_COUNT > 0U))
    SYSMPU_Enable(SYSMPU, 0);
#endif /* FSL_FEATURE_SOC_SYSMPU_COUNT */

    /* Set HID mouse to default state */
    g_UsbDeviceHidMouse.speed = USB_SPEED_FULL;
    g_UsbDeviceHidMouse.attach = 0U;
    g_UsbDeviceHidMouse.hidHandle = (class_handle_t)NULL;
    g_UsbDeviceHidMouse.deviceHandle = NULL;
    g_UsbDeviceHidMouse.buffer = s_MouseBuffer;

#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
    g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusDetached;

    g_UsbDeviceDcdTimingConfig.dcdSeqInitTime = USB_DEVICE_DCD_SEQ_INIT_TIME;
    g_UsbDeviceDcdTimingConfig.dcdDbncTime = USB_DEVICE_DCD_DBNC_MSEC;
    g_UsbDeviceDcdTimingConfig.dcdDpSrcOnTime = USB_DEVICE_DCD_VDPSRC_ON_MSEC;
    g_UsbDeviceDcdTimingConfig.dcdTimeWaitAfterPrD = USB_DEVICE_DCD_TIME_WAIT_AFTER_PRI_DETECTION;
    g_UsbDeviceDcdTimingConfig.dcdTimeDMSrcOn = USB_DEVICE_DCD_TIME_DM_SRC_ON;
#endif

    /* Initialize the usb stack and class drivers */
    if (kStatus_USB_Success !=
        USB_DeviceClassInit(CONTROLLER_ID, &g_UsbDeviceHidConfigList, &g_UsbDeviceHidMouse.deviceHandle))
    {
        usb_echo("USB device mouse failed\r\n");
        return;
    }
    else
    {
#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
        usb_echo("USB device DCD + HID mouse demo\r\n");
#else
        usb_echo("USB device HID mouse demo\r\n");
#endif
        /* Get the HID mouse class handle */
        g_UsbDeviceHidMouse.hidHandle = g_UsbDeviceHidConfigList.config->classHandle;
    }

    USB_DeviceIsrEnable();

#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
#else
    /* Start USB device HID mouse */
    USB_DeviceRun(g_UsbDeviceHidMouse.deviceHandle);
#endif
}

#if defined(USB_DEVICE_CONFIG_USE_TASK) && (USB_DEVICE_CONFIG_USE_TASK > 0)
void USB_DeviceTask(void *handle)
{
    while (1U)
    {
        USB_DeviceTaskFn(handle);
    }
}
#endif

void APP_task(void *handle)
{
    USB_DeviceApplicationInit();

    init_GD77();

    if (xTaskCreate(IO_task,                                  /* pointer to the task */
                    "IO task",                                /* task name for kernel awareness debugging */
                    5000L / sizeof(portSTACK_TYPE),            /* task stack size */
                    NULL,                      /* optional task startup argument */
                    5U,                                        /* initial priority */
                    &g_UsbDeviceHidMouse.IOTaskHandle /* optional task handle to create */
                    ) != pdPASS)
    {
        usb_echo("IO task create failed!\r\n");
        return;
    }

    if (xTaskCreate(Display_task,                                  /* pointer to the task */
                    "Display task",                                /* task name for kernel awareness debugging */
                    5000L / sizeof(portSTACK_TYPE),            /* task stack size */
                    NULL,                      /* optional task startup argument */
                    5U,                                        /* initial priority */
                    &g_UsbDeviceHidMouse.DisplayTaskHandle /* optional task handle to create */
                    ) != pdPASS)
    {
        usb_echo("Display task create failed!\r\n");
        return;
    }

#if USB_DEVICE_CONFIG_USE_TASK
    if (g_UsbDeviceHidMouse.deviceHandle)
    {
        if (xTaskCreate(USB_DeviceTask,                       /* pointer to the task */
                        "usb device task",                    /* task name for kernel awareness debugging */
                        5000L / sizeof(portSTACK_TYPE),       /* task stack size */
                        g_UsbDeviceHidMouse.deviceHandle,     /* optional task startup argument */
                        5U,                                   /* initial priority */
                        &g_UsbDeviceHidMouse.deviceTaskHandle /* optional task handle to create */
                        ) != pdPASS)
        {
            usb_echo("usb device task create failed!\r\n");
            return;
        }
    }
#endif

    while (1U)
    {
#if (defined(USB_DEVICE_CHARGER_DETECT_ENABLE) && (USB_DEVICE_CHARGER_DETECT_ENABLE > 0U)) && \
    ((defined(FSL_FEATURE_SOC_USBDCD_COUNT) && (FSL_FEATURE_SOC_USBDCD_COUNT > 0U)) ||        \
     (defined(FSL_FEATURE_SOC_USBHSDCD_COUNT) && (FSL_FEATURE_SOC_USBHSDCD_COUNT > 0U)))
        if (g_UsbDeviceHidMouse.vReginInterruptDetected)
        {
            g_UsbDeviceHidMouse.vReginInterruptDetected = 0;
            if (g_UsbDeviceHidMouse.vbusValid)
            {
                usb_echo("USB device attached.\r\n");
                USB_DeviceDcdInitModule(g_UsbDeviceHidMouse.deviceHandle, &g_UsbDeviceDcdTimingConfig);
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusVBUSDetect;
            }
            else
            {
                usb_echo("USB device detached.\r\n");
                USB_DeviceDcdDeinitModule(g_UsbDeviceHidMouse.deviceHandle);
                USB_DeviceStop(g_UsbDeviceHidMouse.deviceHandle);
                g_UsbDeviceHidMouse.dcdPortType = kUSB_DeviceDCDPortTypeNoPort;
                g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusDetached;
            }
        }

        if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusChargingPortDetect) /* This is only for BC1.1 */
        {
            USB_DeviceRun(g_UsbDeviceHidMouse.deviceHandle);
        }
        if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusTimeOut)
        {
            usb_echo("Timeout error.\r\n");
            g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusComplete;
        }
        if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusUnknownType)
        {
            usb_echo("Unknown port type.\r\n");
            g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusComplete;
        }
        if (g_UsbDeviceHidMouse.dcdDevStatus == kUSB_DeviceDCDDevStatusDetectFinish)
        {
            if (g_UsbDeviceHidMouse.dcdPortType == kUSB_DeviceDCDPortTypeSDP)
            {
                usb_echo("The device has been connected to a facility which is SDP(Standard Downstream Port).\r\n");
                USB_DeviceRun(
                    g_UsbDeviceHidMouse.deviceHandle); /* If the facility attached is SDP, start enumeration */
            }
            else if (g_UsbDeviceHidMouse.dcdPortType == kUSB_DeviceDCDPortTypeCDP)
            {
                usb_echo("The device has been connected to a facility which is CDP(Charging Downstream Port).\r\n");
                USB_DeviceRun(
                    g_UsbDeviceHidMouse.deviceHandle); /* If the facility attached is CDP, start enumeration */
            }
            else if (g_UsbDeviceHidMouse.dcdPortType == kUSB_DeviceDCDPortTypeDCP)
            {
                usb_echo("The device has been connected to a facility which is DCP(Dedicated Charging Port).\r\n");
            }
            g_UsbDeviceHidMouse.dcdDevStatus = kUSB_DeviceDCDDevStatusComplete;
        }
#endif
    }
}

#if defined(__CC_ARM) || (defined(__ARMCC_VERSION)) || defined(__GNUC__)
int main(void)
#else
void main(void)
#endif
{
    BOARD_InitPins();
    BOARD_BootClockHSRUN();
    BOARD_InitDebugConsole();

    if (xTaskCreate(APP_task,                                  /* pointer to the task */
                    "app task",                                /* task name for kernel awareness debugging */
                    5000L / sizeof(portSTACK_TYPE),            /* task stack size */
                    &g_UsbDeviceHidMouse,                      /* optional task startup argument */
                    4U,                                        /* initial priority */
                    &g_UsbDeviceHidMouse.applicationTaskHandle /* optional task handle to create */
                    ) != pdPASS)
    {
        usb_echo("app task create failed!\r\n");
#if (defined(__CC_ARM) || (defined(__ARMCC_VERSION)) || defined(__GNUC__))
        return 1U;
#else
        return;
#endif
    }

    vTaskStartScheduler();

#if (defined(__CC_ARM) || (defined(__ARMCC_VERSION)) || defined(__GNUC__))
    return 1U;
#endif
}
