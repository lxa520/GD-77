Reading the GD-77 internal flash with this hack requires some steps in a quite hacky way, but for now it works and
has only to be done once. Later on probably a modified firmware with a clean way to read out the GD-77 internal
flash will get created.

The patched firmware is located in the folder "patched_firmware".

1. Upload the patched "GD-77_V3.1.1_with_flashreadpatch.sgl" firmware with the GD-77 updater.
   It is patched to answer the codeplug read command in a modified way that enables reading the internal flash in
   16 byte chunks. It will crash the GD-77 after about 400 continuous reads so the community CPS modified for the
   readout will read the data in batches of 256 x 16 byte, disconnecting between them to let the system cool down.
   This avoids the crash and allows for a full readout in one pass. Don't forget to restart the GD-77 after the
   firmware upload.
2. Use the modified community CPS "cps_full_flash_readout_step_1" and select "read codeplug". This will read the
   full internal flash into a file "full_flash.dat". It takes about ten minutes and finishes with a message
   box "ready".
3. As the approach in 2) does not read the first 16 bytes at 0x00000000 correctly now use the modified
   CPS "cps_full_flash_readout_step_2" and select "read codeplug" again. It reads the 15 bytes from internal
   flash 0x00000001-0x0000000f and writes them to the resulting file "full_flash_1.dat" at 0x00000000.
4. Now use a hex editor and replace the bytes 0x00000001-0x0000000f in "full_flash.dat" with according bytes
   from "full_flash_1.dat" (remember that they start at 0x00000000).
5. Now upload the patched "GD-77_V3.1.1_with_startbytesreadpatch.sgl" firmware with the GD-77 updater.
   It is patched to answer the codeplug read in a modified way that every time returns the first four bytes
   of the internal flash. Don't forget to restart the GD-77 after the firmware upload.
6. As the approach in 3) still does not read the first byte at 0x00000000 correctly now use the modified
   CPS "cps_full_flash_readout_step_2" and select "read codeplug" again. It reads the first four bytes from
   internal flash and writes them to the resulting file "full_flash_1.dat" at 0x00000000.
7. Now use a hex editor and replace the first byte in "full_flash.dat" with according byte from "full_flash_1.dat".
8. You should now have a full readout of the flash of your GD-77. But the read out file still contains the patch.
   Therefore use the hex editor to replace 0x00004000-0x0007b000 with the decoded original version 3.1.1 firmware.
   This will leave you with a full flash readout that is exactly the same as your GD-77 internal flash.

Don't forget to upload the unmodified original firmware to your GD-77 again afterwards to get it into a clean working
state again.

That's it, have fun!
