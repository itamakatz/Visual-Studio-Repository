// Example_MNA601.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <stdlib.h>
#include <windows.h>
#include <conio.h>
#include <tchar.h>

#include "..\Includes\Thorlabs.MotionControl.ModularRack.NanoTrak.h"

//namespace MNA_Console
//{
	/// <summary> Main entry-point for this application. </summary>
	/// <param name="argc"> The argc. </param>
	/// <param name="argv"> The argv. </param>
	/// <returns> . </returns>
	int _tmain(int argc, _TCHAR* argv[])
	{
 
		if(argc < 1)
		{
			printf("Usage = Example_MNA601 [module_serial_no] [h position: optional (0 - 65535)] [v position: optional (0 - 65535)]\r\n");
			char c = _getch();
			return 1;
		}

		int serialNo = 52809118;
		if(argc > 1)
		{
			serialNo = _wtoi(argv[1]);
		}

		// get parameters from command line
		NT_HVComponent position;
		position.horizontalComponent = 0x8000;
		position.verticalComponent = 0x8000;
		if(argc > 3)
		{
			position.horizontalComponent = _wtoi(argv[2]);
			position.verticalComponent = _wtoi(argv[3]);
		}

		// identify and access device
		char testSerialNo[16];
		sprintf_s(testSerialNo, "%d", serialNo);

		try
        {
			// Build list of connected device
            if (TLI_BuildDeviceList() == 0)
            {
				// get device list size 
                short n = TLI_GetDeviceListSize();
				// get MST serial numbers
                char serialNos[100];
				// params define buffer size and module type 52 = NanoTrak Module
				TLI_GetDeviceListByTypeExt(serialNos, 100, 52);

				// output list of matching devices
				char *next_token1 = NULL;
				char *p = strtok_s(serialNos, ",", &next_token1);
				bool matched = false;
				while(p != NULL)
                {
					TLI_DeviceInfo deviceInfo;
					// get device info from device
                    TLI_GetDeviceInfo(p, &deviceInfo);
					// get strings from device info structure
					char desc[65];
					strncpy_s(desc, deviceInfo.description, 64);
					desc[64] = '\0';
					char serialNo[9];
					strncpy_s(serialNo, deviceInfo.serialNo, 8);
					serialNo[8] = '\0';
					// output
					printf("Found Device %s=%s : %s\r\n", p, serialNo, desc);
					p = strtok_s(NULL, ",", &next_token1);
					if(strncmp(testSerialNo, serialNo, 8) == 0)
					{
						printf("requested device %s found\r\n", serialNo);
						matched = true;
					}
				}

				if(!matched)
				{
					printf("NanoTraks %s not found\r\n", testSerialNo);
					char c = _getch();
					return 1;
				}

				// open device
				if(MMR_Open(testSerialNo) == 0)
		        {
					// start the device polling at 200ms intervals
			        NT_StartPolling(testSerialNo, 200);

					// NOTE The following uses Sleep functions to simulate timing
					// In reality, the program should read the status to check that commands have been completed
			        Sleep(1000);
					// Home device
					printf("Device %s homing\r\n", testSerialNo);
			        NT_SetCircleHomePosition(testSerialNo, &position);
			        NT_HomeCircle(testSerialNo);
			        Sleep(1000);

					// move to position (channel 1)
					NT_HVComponent pos;
			        NT_GetCirclePosition(testSerialNo, &pos);
					printf("Device %s position = (%d, %d)\r\n", testSerialNo, pos.horizontalComponent, pos.verticalComponent);
			        Sleep(1000);

					// stop polling
			        NT_StopPolling(testSerialNo);
					// close device
			        MMR_Close(testSerialNo);
	            }
            }
        }
        catch (char * e)
        {
			printf("Error %s\r\n", e);
        }

		char c = _getch();
		return 0;
    }
//}
