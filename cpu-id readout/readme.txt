1. Take the 16 bytes of the CPU Unique Identification register (starting at 0x40048054).
2. Create a file "cpu-uid.dat" containing those 16 bytes twice.
   Attention: When using JLink mem32 to get the data convert the endianness of the each 32 bit word before
   writing it to the file.
3. Run "CPU_UID-calc". This will convert the data as needed by the bootloader and the main application and
   writes the result to "cpu-uid.out".
4. Copy the result (32 bytes) from "cpu-uid.out" into "full_flash.dat" to 0x0007f800.

This will patch "full_flash.dat" so that the bootloader and the main application survive their CPU-UID checks (the main application uses the data at various places).
