Prerequisites:
- MCUXpressoIDE_10.3.1_2233 (with installed SDK_2.5.0_FRDM-K22F)

This project is for debugging an unmodified GD77 full flash readout.

First you need a full flash readout from your GD77. The file is not included due to
licensing issues as it contains the AMBE software codec (Digital Voice Systems Inc.)
in unencrypted form. So you'll have to create that file sourself. Please use the
project "GD-77_flash_readout" (also in this repo) for that.

If you use the file on a different GD77 other than the one it got read from:
Make sure that you patched in your GD77s CPU-UID as described in the "GD-77_cpu-id_calc"
project.


Once you have that file just replace the dummy "../firmware/full_flash.dat" (contains only 0x00)
with your file and fire up the MCUXpresso project. This should give you an environment
where you can download the firmware via debug probe to your device, start the firmware
and debug it.


That's it, have fun!
