using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorlabs.MotionControl.DeviceManagerCLI;
using Thorlabs.MotionControl.KCube.DCServoCLI;

namespace Try_Kinesis
{
	class Program
	{
		//private static KCubeDCServo _kCubeDCServoMotor = null;
		private static KCubeDCServo _kCubeDCServoMotor = new KCubeDCServo();

		static void Main(string[] args)
		{
			Console.WriteLine("Ready to start!");
			Console.ReadKey();

			if (_kCubeDCServoMotor != null)
			{
				Console.WriteLine("Device already connected");
				return;
			}

			const string serialNumber = "27250312";

			// All of this operation has been placed inside a single "catch-all"
			// exception handler. This is to reduce the size of the example code.
			// Normally you would have a try...catch per API call and catch the
			// specific exceptions that could be thrown (details of which can be
			// found in the Kinesis .NET API document).
			try
			{
				// Instructs the DeviceManager to build and maintain the list of
				// devices connected.
				DeviceManagerCLI.BuildDeviceList();

				_kCubeDCServoMotor = KCubeDCServo.CreateKCubeDCServo(serialNumber);

				// Establish a connection with the device.
				_kCubeDCServoMotor.Connect(serialNumber);

				// Wait for the device settings to initialize. We ask the device to
				// throw an exception if this takes more than 5000ms (5s) to complete.
				_kCubeDCServoMotor.WaitForSettingsInitialized(5000);

				// Initialize the DeviceUnitConverter object required for real world
				// unit parameters.
				//_kCubeDCServoMotor.GetMotorConfiguration(serialNumber);
				_kCubeDCServoMotor.GetMotorConfiguration(serialNumber, DeviceConfiguration.DeviceSettingsUseOptionType.UseDeviceSettings);

				// This starts polling the device at intervals of 250ms (0.25s).
				_kCubeDCServoMotor.StartPolling(250);

				// We are now able to enable the device for commands.
				_kCubeDCServoMotor.EnableDevice();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Unable to connect to device\n" + ex);
			}

			Console.WriteLine("Holly shit it worked.... now to move!!");
			Console.ReadKey();

			_kCubeDCServoMotor.MoveTo(0, 0);

			_kCubeDCServoMotor.MoveTo(10, 10000);


			Console.WriteLine("ohhhh baby");
			Console.ReadKey();

		}
	}
}
