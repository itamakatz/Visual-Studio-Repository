using System;
using System.Collections.Generic;
using System.Diagnostics;
using Thorlabs.MotionControl.DeviceManagerCLI;
using Thorlabs.MotionControl.KCube.DCServoCLI;

namespace Try_Kinesis
{
	class Program
	{
		//private static KCubeDCServo _kCubeDCServoMotor = null;
		//private static KCubeDCServo _kCubeDCServoMotor;

		private const string serialNumber = "27000961";

		static KCubeDCServo _kCubeDCServoMotor;

		const int DEFAULTVEL = 10;
		const int DEFAULTACC = 10;
		const int TPOLLING = 250;
		const int TIMEOUTSETTINGS = 7000;
		const int TIMEOUTMOVE = 100000;

		//void run()
		//{
		//	KCubeDCServo.CreateKCubeDCServo(serialNumber);
		//}

		static void Main(string[] args)
		{

			
			Program program = new Program();

			//program .run();

			//DeviceManagerCLI.BuildDeviceList();

			//KCubeDCServo _kCubeDCServoMotor;
			//_kCubeDCServoMotor = KCubeDCServo.CreateKCubeDCServo(serialNumber);
			//KCubeDCServo.CreateKCubeDCServo(serialNumber);

			//if (_kCubeDCServoMotor != null)
			//{
			//	Console.WriteLine("Device already connected");
			//	return;
			//}

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
				List<string> device_list = DeviceManagerCLI.GetDeviceList();

				_kCubeDCServoMotor = KCubeDCServo.CreateKCubeDCServo(serialNumber);

				_kCubeDCServoMotor.ClearDeviceExceptions();

				if (!_kCubeDCServoMotor.IsSettingsInitialized())
				{
					//Establish a connection with the device.
					_kCubeDCServoMotor.Connect(serialNumber);
				}

				if (!_kCubeDCServoMotor.IsSettingsInitialized())
				{
					//Establish a connection with the device.
					WriteLine("Unable to initialise device" + serialNumber);
				}

				_kCubeDCServoMotor.StartPolling(TPOLLING);

				var motorSettings = _kCubeDCServoMotor.GetMotorConfiguration(serialNumber, DeviceConfiguration.DeviceSettingsUseOptionType.UseDeviceSettings);

				Thorlabs.MotionControl.GenericMotorCLI.Settings.MotorDeviceSettings currentDeviceSettingsNET = _kCubeDCServoMotor.MotorDeviceSettings;
				var deviceInfoNET = _kCubeDCServoMotor.GetDeviceInfo();
				var MotDi = Thorlabs.MotionControl.GenericMotorCLI.Settings.RotationSettings.RotationDirections;
				//.RotationDirection.Forward;
				currentDeviceSettingsNET.Rotation.RotationDirection;
				//= MotDi;
				//.Rotation.RotationDirection = MotDir; 

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
				WriteLine("Unable to connect to device\n" + ex);
			}

			//Console.WriteLine("Holly shit it worked.... now to move!!");
			//Console.ReadKey();
			try
			{
				_kCubeDCServoMotor.IdentifyDevice();
				Decimal max_vel = _kCubeDCServoMotor.GetVelocityParams().MaxVelocity;
				//WriteLine(max_vel.ToString());
				WriteLine(max_vel + "");
			}
			catch (Exception ex)
			{
				WriteLine("Unable to connect to device\n" + ex);
			}
			//_kCubeDCServoMotor.MoveTo(0, 0);
			//_kCubeDCServoMotor.SetJogStepSize(10);
			//_kCubeDCServoMotor.MoveJog(Thorlabs.MotionControl.GenericMotorCLI.MotorDirection.Forward, 0);

			//_kCubeDCServoMotor.MoveTo(50, 0);

			WriteLine("end");
			Console.ReadKey();

			_kCubeDCServoMotor.DisableDevice();
			_kCubeDCServoMotor.StopPolling();
			_kCubeDCServoMotor.Disconnect(true);
		}

		static void WriteLine(string str)
		{
			Trace.WriteLine(str);
			Console.WriteLine(str);
		}
	}
}
