# GD-77
GD-77 and GD-77S reverse engineering

# Details
"Radioddity_GD-77" contains the original encrypted GD-77 firmware files collected by Roger Clark (https://github.com/rogerclarkmelbourne/Radioddity_GD-77.git) as they also are or have been available on the Radioddity web site.

"Radioddity_GD-77S" contain original encrypted GD-77S firmware files as they also are or have been available on the Radioddity web site.

"GD-77_firmware_stripper" contains the source for the firmware stripping tool. It removes the header and seperates the raw encrytped firmware data as it is transfered by the uploader.

"GD-77_firmware_stripper_workdir" contains the executable and the batch. Run it to receive the results.

"GD-77_firmware_decrypt_encrypt" contains the source for the firmware decrypt/encrypt tool. It uses a keyfile and a sequence entry point for XORing and applies an additional XOR/negate/rotate step and with that decrypts or encrypts a firmware file.

"GD-77_firmware_decrypt_encrypt_workdir" contains the executable and the batch. Run it to receive the results.
 
"GD-77_recreate_uploadable_firmware" contains the toolchain to recreate an uploadable firmware file. It takes a decrypted (and maybe patched) firmware file, encrypts it again and copyies the original header in front. The result is an uploadable firmware file.

"GD-77_flash_readout" contains patched firmware and tools to get a flush flash readout.

"GD-77_cpu-id_calc" contains tools to calculate the data necessary to patch "full_flash.dat" so that the bootloader and the main application survive their CPU-UID checks.
