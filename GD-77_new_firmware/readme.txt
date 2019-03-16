Disclaimer: Do not use for anything right now! The current state is
            highly experimental only.


The MCUXpresso project will become a new GD-77 firmware, written from
scratch and beeing 100% open source using 100% free tools.

The updated flashmanager will get used as a tool to communicate with
the new GD-77 firmware.


The MCUXpresso project produces a full flash image replacement (starting
at 0x00000000 without a bootloader) that is intended for getting flashed
from within the IDE. Later on standalone firmware files for use with the
regular GD-77 updater can get created.

The new firmware is based on the KSDK 2.5.0 FreeRTOS HID mouse device example
and got patched in a way to resemble the way the original GD-77 firmware was
created from the KSDK 1.X MQX HID mouse device example as much as possible.


Currently it starts up the FreeRTOS context and acts as a USB HID device listening
for some communication to directly sent back. It sticks to the same communication
design that the original GD-77 firmware is using. The updated flashmanager is able to
communicate with the firmware (loop of [send, receive, check] when "Read" is used
with "Test USB" checked) as a test client.
