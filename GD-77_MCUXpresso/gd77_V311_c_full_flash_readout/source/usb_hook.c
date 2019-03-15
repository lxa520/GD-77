// Wrapper macros to create a stringified version of the value
//
#define QUAUX(X) #X
#define QU(X) QUAUX(X)

// Definition of locations
//
#define USB_SEND_DATA_ADDRESS           0x00023280
#define USB_READ_DATA_ADDRESS           0x0003942a
#define USB_RESET_BUFFER_ADDRESS        0x00023294
#define USB_HOOK_BACK_ADDRESS           0x0003958a
#define USB_HOOK_CONTINUE_ADDRESS       0x0003976c
#define MEM_BUFFER                      0x1fff5f40

// Macros to implement calls/jumps with inline assembler
//
#define CALL_USB_SEND_DATA asm volatile("bl " QU(USB_SEND_DATA_ADDRESS));
#define CALL_USB_READ_DATA asm volatile("bl " QU(USB_READ_DATA_ADDRESS));
#define CALL_USB_RESET_BUFFER asm volatile("bl " QU(USB_RESET_BUFFER_ADDRESS));

// Forward declarations
//
void usb_hook_worker(char* sp);


// Implementation of usb_hook
//
// The hook checks for M<0x02> and continues the original "M<0x02>"
// codepath if found.
// Otherwise it additionally checks for M<0xFF> and continues
// the original "no M<0x02>" codepath if not found or enters
// the new communication loop.
//

// Basic hook code, performs the original actions and hooks in the new function.
void usb_hook() __attribute__ ((naked));
void usb_hook()
{
  asm volatile (
		    "LDRB R0,[SP, #0xc]\n"
		  	"CMP R0, #0x4d\n"
	  	  	"BNE usb_hook_continue\n"
	  	  	"LDRB R0,[SP, #0xd]\n"
	  	  	"LDRB R0,[SP, #0xd]\n"
	  	  	"CMP R0, #0x02\n"
	  	  	"BNE usb_hook_continue\n"
	  	  	"BL " QU(USB_RESET_BUFFER_ADDRESS) "\n"
	  	  	"B " QU(USB_HOOK_BACK_ADDRESS) "\n"
	  	  	"usb_hook_continue:"
		  );

  register char* sp asm ("sp");
  usb_hook_worker(sp);

  asm volatile("b " QU(USB_HOOK_CONTINUE_ADDRESS));
}

// New communication loop.
void usb_hook_worker(char* sp)
{
  // direct access to some registers
  register int r0 asm ("r0");
  register int r1 asm ("r1");
  register int r2 asm ("r2");
  register int lr asm ("lr");

  // save the return link
  int tmp_lr=lr;

  // check for "M<0xff>"
  if (sp[0x0d] == 0xff) // new "M<0xff>" codepath
  {
    // send ACK for start command
    sp[0x0c]=0x41;
    r0=(int)sp+0x0c;
    r1=1;
    CALL_USB_SEND_DATA

    // read data loop
    while (1)
    {
      // reset mem buffer
      CALL_USB_RESET_BUFFER

      // read 5 byte data (cmd+adr)
	  r0=(int)sp+0x0c;
	  r1=5;
	  r2=200;
      CALL_USB_READ_DATA

      // first byte (cmd) != 1 => exit com
      if (sp[0x0c] != 0x01)
      {
        break;
      }

      // copy 16 bytes from adr to mem buffer
      char* address=(char*)((sp[0x0d]<<0)+(sp[0x0e]<<8)+(sp[0x0f]<<16)+(sp[0x10]<<24));
      char* buffer=(char*)MEM_BUFFER;
      for (int idx=0; idx<16; idx++)
      {
        buffer[idx]=address[idx];
      }

      // send 16 bytes data from mem buffer
  	  r0=MEM_BUFFER;
	  r1=16;
      CALL_USB_SEND_DATA
    }

    // send ACK for exit command
    sp[0x0c]=0x41;
    r0=(int)sp+0x0c;
    r1=1;
    CALL_USB_SEND_DATA

    // reset mem buffer
    CALL_USB_RESET_BUFFER
  }

  // restore the return link
  lr=tmp_lr;
}
