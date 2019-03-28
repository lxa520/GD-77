@echo off
copy ..\GD-77_firmware_decrypt_encrypt_workdir\BF-5R_V2.1.6_BB8A_078001.dec . /y
firmware_decrypt BF-5R_V2.1.6_BB8A_078001 datafile 0x7d54 encrypt
copy /b header_RD5R_216.bin+BF-5R_V2.1.6_BB8A_078001.enc BF-5R_V2.1.6_BB8A_078001.sgl /y
pause
