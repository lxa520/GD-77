Prerequisites:
- MCUXpressoIDE_10.3.1_2233 (with installed SDK_2.5.0_FRDM-K22F)
  Important: If you are using J-Link please upgrade to J-Link >= 6.43,
  otherwise the MK22FN512xxx12 won't work.

This project is for injecting code in the USB communication using a split and patched
GD77 full flash readout. This example is the firmware patch that adds the functionality
for the full flash readout.

First you need a full flash readout from your GD77. The file is not included due to
licensing issues as it contains the AMBE software codec (Digital Voice Systems Inc.)
in unencrypted form. So you'll have to create that file sourself. Please use the
project "GD-77_flash_readout" (also in this repo) for that.

If you use the file on a different GD77 other than the one it got read from:
Make sure that you patched in your GD77s CPU-UID as described in the "GD-77_cpu-id_calc"
project.


Once you have that file split it into "full_flash_header.dat" and
"full_flash_footer.dat" containing the following parts of the original 
full flash readout:

  FW V3.1.1:
    header runs from  0x00000000 to 0x0007aaff
    footer runs from  0x0007b000 to 0x0007ffff
    => free for own code 0x0007ab00 to 0x0007afff
  FW V3.1.8
    header runs from  0x00000000 to 0x0007ae9f
    footer runs from  0x0007b000 to 0x0007ffff
    => free for own code 0x0007aea0 to 0x0007afff

Then replace the following bytes in "fullflash_header.dat:
  FW V3.1.1:
    replace 0x00039572 to 0x00039589 with
	41 F0 C5 BA 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF
  FW V3.1.8
    replace 0x0003988a to 0x000398a1 with
	41 F0 09 BB 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF 00 BF
	
This will leave you with a patched header file that contains the bootloader and the
firmware. The firmware is cut off just before area of unused bytes at its end. The
firmware is patched to hook in the USB communication and jump to the unused area.

The additionally USB communication is compiled/assembled into the unused area.

The footer file then adds the rest of the data.


Using the free area that is still part of the firmware file makes it possible to inject the
new functionality into the originally firmware file, reencrypt it and upload it to the device
using the original tools. This has for now to be done manually but can possibly get automated
later on.

The free unused area behind the V3.1.8 version is much smaller than the V3.1.1 version. So for
experiments using the V3.1.1 version gives more room.

Later on it will be better to free up some other unused areas within the firmware and/or extend
the firmware size to include additional unused blocks at its end. But for now the approach
described here is working and leads to good results where programming code injects using an
open source IDE and assembler or C becomes possible.


That's it, have fun!
