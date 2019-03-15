.syntax unified

.equ USB_SEND_DATA,        0x00023280
.equ USB_READ_DATA,        0x0003942a
.equ USB_RESET_BUFFER,     0x00023294

.equ USB_HOOK_BACK,        0x0003958a
.equ USB_HOOK_CONTINUE,    0x0003976c

.equ MEM_BUFFER,           0x1fff5f40

.section .text

// The hook checks for M<0x02> and continues the original "M<0x02>"
// codepath if found.
// Otherwise it additionally checks for M<0xFF> and continues
// the original "no M<0x02>" codepath if not found or enters
// the new communication loop.

usb_hook:
  LDRB R0,[SP, #0xc]                    // check for "M"
  CMP R0, #0x4d
  BNE usb_hook_continue2
  LDRB R0,[SP, #0xd]                    // check for 0x02
  CMP R0, #0x02
  BNE usb_hook_continue1
  BL USB_RESET_BUFFER                   // original "M<0x02>" codepath
  B USB_HOOK_BACK

usb_hook_continue1:
  LDRB R0,[SP, #0xd]                    // additional check for 0xff
  CMP R0, #0xff
  BNE usb_hook_continue2                // original "no M<0x02>" codepath

// start of new "M<0xff>" codepath
  MOVS R0, #0x41                        // send ACK for start command
  STRB R0, [SP, #0xc]
  MOVS R1, #1
  ADD R0, SP, #0xc
  BL USB_SEND_DATA

usb_loop:                               // start of read data loop
  BL USB_RESET_BUFFER                   // reset mem buffer

  MOVS R2,#200                          // read 5 byte data (cmd+adr)
  MOVS R1, #5
  ADD R0, SP, #0xc
  BL USB_READ_DATA

  LDRB R0,[SP, #0xc]                    // first byte (cmd) != 1 => exit com
  CMP R0, #1
  BNE usb_finished

  LDR R0, [SP, #0xd]                    // copy 16 bytes from adr to mem buffer
  LDR R1, =MEM_BUFFER
  MOVS R2, #0
LOOP:
  LDR R3, [R0, R2]
  STR R3, [R1, R2]
  ADD R2, R2, #4
  CMP R2, #16
  BNE LOOP

  MOVS R1, #16                          // send 16 bytes data from mem buffer
  LDR R0, =MEM_BUFFER
  BL USB_SEND_DATA

  B usb_loop                            // end of read data loop

usb_finished:                           // exit com
  MOVS R0, #0x41                        // send ACK for exit command
  STRB R0, [SP, #0xc]
  MOVS R1, #1
  ADD R0, SP, #0xc
  BL USB_SEND_DATA

  BL USB_RESET_BUFFER                   // reset mem buffer
// end of new "M<0xff>" codepath

usb_hook_continue2:
  B USB_HOOK_CONTINUE

.end
