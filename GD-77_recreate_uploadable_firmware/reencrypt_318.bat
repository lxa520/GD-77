@echo off
copy ..\GD-77_firmware_decrypt_encrypt_workdir\GD-77_V3.1.8_0CD1_077001.dec . /y
firmware_decrypt GD-77_V3.1.8_0CD1_077001 datafile 0x0807 encrypt
copy /b header_318.bin+GD-77_V3.1.8_0CD1_077001.enc GD-77_V3.1.8_0CD1_077001.sgl /y
pause
