INCLUDE "../linkerscripts/memory.ld"

ResetISR = 0x00000004;  
ENTRY(ResetISR)

SECTIONS
{
    .text :
    {
       . = 0x00000000;
       *(.full_flash)
        KEEP(*(.full_flash))
       *(.text*)
        KEEP(*(.text*))
    } > PROGRAM_FLASH
}
