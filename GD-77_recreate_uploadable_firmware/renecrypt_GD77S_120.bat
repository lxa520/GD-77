@echo off
copy ..\GD-77_firmware_decrypt_encrypt_workdir\GD-77S_V1.2.0_1A0C2_050001.dec . /y
firmware_decrypt GD-77S_V1.2.0_1A0C2_050001 datafile 0x2a8e encrypt
copy /b header_GD77S_120.bin+GD-77S_V1.2.0_1A0C2_050001.enc GD-77S_V1.2.0_1A0C2_050001.sgl /y
pause
