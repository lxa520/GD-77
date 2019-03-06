@echo off
copy ..\GD-77_firmware_stripper_workdir\*.raw . /y
firmware_decrypt GD-77_V2.5.6_1137_075801 datafile 0x0423 decrypt
firmware_decrypt GD-77_V2.6.1_6901_075801 datafile 0x1A6F decrypt
firmware_decrypt GD-77_V2.6.3_2333_075801 datafile 0x1620 decrypt
firmware_decrypt GD-77_V2.6.6_0F10_075801 datafile 0x222F decrypt
firmware_decrypt GD-77_V2.6.7_60F3_075801 datafile 0x4ABB decrypt
firmware_decrypt GD-77_V2.6.8_2780_077001 datafile 0x4BE7 decrypt
firmware_decrypt GD-77_V2.6.9_02DA_077001 datafile 0x28EA decrypt
firmware_decrypt GD-77_V3.0.4_121E_078001 datafile 0x5060 decrypt
firmware_decrypt GD-77_V3.0.6_0BF2_078001 datafile 0x35B5 decrypt
firmware_decrypt GD-77_V3.1.0_5D9C_077001 datafile 0x7C0B decrypt
firmware_decrypt GD-77_V3.1.1_73F3_077001 datafile 0x0000 decrypt
firmware_decrypt GD-77_V3.1.2_4D78_077001 datafile 0x328B decrypt
firmware_decrypt GD-77_V3.1.3_61F7_077001 datafile 0x01B1 decrypt
firmware_decrypt GD-77_V3.1.5_frequency_offset_problem_fix_5EFE_077001 datafile 0x2C63 decrypt
firmware_decrypt GD-77_V3.1.6_5CE3_077001 datafile 0x52BF decrypt
firmware_decrypt GD-77_V3.1.8_0CD1_077001 datafile 0x0807 decrypt
firmware_decrypt GD-77_V3.2.1_1823_077801 datafile 0x55E6 decrypt
pause
