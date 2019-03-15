INCLUDE "../linkerscripts/memory.ld"

ResetISR = 0x00000004;  
ENTRY(ResetISR)

SECTIONS
{
    .text :
    {
       . = 0x00000000;
       *(.full_flash_header)
        KEEP(*(.full_flash_header))
    } > PROGRAM_FLASH

    .text :
    {
       . = 0x0007ab00;
       *(.usb_hook)
        KEEP(*(.usb_hook))
       *(.usb_hook_worker)
        KEEP(*(.usb_hook_worker))
       *(.text*)
        KEEP(*(.text*))
    } > PROGRAM_FLASH

    .text :
    {
       . = 0x0007b000;
       *(.full_flash_footer)
        KEEP(*(.full_flash_footer))
    } > PROGRAM_FLASH
}
