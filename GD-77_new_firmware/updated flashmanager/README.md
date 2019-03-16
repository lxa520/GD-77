# radioddity_gd-77_flash_manager

Tool to read and write some parts of the 1Mb SPI flash memory chip on the Radioddity GD-77 DMR radio

WARNING.
You use this software at your own risk. 
Writeing arbitary portions of the flash is likely to cause the GD-77 to execute a factory reset and could potentially "brick" your GD-77 entirely

In normal mode address 0x000000 to 0x01FFFF contain the codeplug (128k) and it seems possible to read and write small address ranges e.g. 32 bytes.

In DMR ID mode (Boot with SK2 and Green button and # pressed), the memory address 0x030000 to 0x04ffff seem to contain the DMR ID database.
This seems to be updateable in small sections e.g. 32 bytes

Note. The code only allows a minimum of 32 bytes to be read or written and the start address must be on a 32 byte boundard

The GD-77 firmware (3.0.6) seems to allow transfers of 8, 16 or 32 bytes, but does not seem to allow any larger transfers.
Also the CPS never seems to transfer over a 32 byte boundard, but I've not tested to see if this is a real limit in the firmware (3.0.6) or not.

I have not implemented 8 or 16 byte transfers, as its easy to read a 32 byte section, modify the bytes you want to change and then write that section

Note. At the moment it unclear how the firmware manages the page erases, in the flash memory, because with flash memory a page (typically 1k, 2k, 4k, 8k etc) must be erased before data can be written

I hope that the firmware caches all writes within a single page and then just does a read of the whole page, then an erase then writes a modified page.
As if it doesnt do that, the flash will get worn out after only uploading less than 200 codeplugs !!

We should be able to determine how and when the flash is erased and written soon, by attaching a logic analyser to the SPI chip inside the GD-77

Note.
Other areas in the 1Mb flash chip seem to return 0x00, as the address written to using the CPS command protocol is not the real address.
i.e the codeplug is not actually stored in address 0x00000 of the flash, but the firmware maps 0x00000 in the CPS protcol to some other address in the flash chip

Its unclear why Radioddity decided that the DMR ID addresses can only be accessed after starting the GD-77 in the special DMR ID mode.

As the address of the codeplug and the DMR ID are different.

I suspect that the DMR ID feature is an afterthought, as it is not part of the original firmware, and it was not easy for them to integrate the new DMR ID feature
So they kept it separate to avoid mapping conflicts.

One interesting thing to note, is that following a Factor Reset on firmware 3.0.6 the codeplug contains lots of data which is probably a pinyin (Chinese) character set, even though the GD-77 does appear to be switchable in Chinese any more.