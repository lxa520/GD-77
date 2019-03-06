# radioddity_gd-77_cps2.0.5
Work to enhance and improve the Radioddity GD-77 CPS, based on reverse engineering the official exe


## Bugs fixed

* Supports more than 256 Scan lists, also fixes another bug in scanlists.
* Fixed uncommanded transmit if Dual Capacity Direct Mode was enabled on a channel which is set to Analog mode

## Enhancements
* CPS is always in Expert mode. Popups to enter password to switch between Basic and Expert mode have been removed
* App can now be installed in program folders as configuration information is now stored in AppData Local, and default data location is My Documents
* Previous saved or opened path is stored, so that opening the normal codeplug is easier.
* App supports being started with arguments which mean file associations can be setup in Windows to allow double clicking on codeplug file to launch and open the CPS
* Warning popups added on Read and Write to GD-77 to prevent codeplug in the GD-77 or the CPS being accidentally overwritten
* Added “crossband” support, including warning message that transmitter power is lower than normal when Tx and Rx band are not the same
* Enabled extended frequency range, 130Mhz to 520Mhz
* Added feature to compact Zone data in codeplug, so that adding a new Zone always happens at the end of the list
* Added feature to move Zones up or down in the list, including using control key operation
* Added language translation files supplied by various contributors, for German, Spanish and Polish
* Removed unused DLL files from the installation
* Added Internet download and import of Contacts from Ham-digital.org “last heard” database, based on region ID code.
* Language XML is cached rather than being loaded every time a screen (form) is opened.
* Message to indicate the codeplug file has been removed, and instead the callsign is displayed in the root tree node to indicate the file has been loaded
* Codeplug Read and Write , completion popups have been removed and replaced by a message on the progress dialog.
* Icon has been added to top left of Windows title bar rather than using the default “Dot Net” icon.

## Know bugs – part of the GD-77 firmware not the CPS

* Scan lists using channels above number 256 cause scan list name to be incorrectly displayed on the GD-77 screen
