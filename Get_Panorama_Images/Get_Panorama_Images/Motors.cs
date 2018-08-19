using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorlabs.MotionControl.DeviceManagerCLI;
using Thorlabs.MotionControl.GenericMotorCLI;
using Thorlabs.MotionControl.GenericMotorCLI.ControlParameters;
using Thorlabs.MotionControl.GenericMotorCLI.AdvancedMotor;
using Thorlabs.MotionControl.GenericMotorCLI.KCubeMotor;
using Thorlabs.MotionControl.GenericMotorCLI.Settings;
using Thorlabs.MotionControl.KCube.DCServoCLI;
using System.Threading;
using System.Windows.Forms;

namespace Get_Panorama_Images
{
	class Motors
	{
		public event Append_Status_Delegate Append_Status_Event;

		private KCubeDCServo device;

		static int THREAD_SLEEP_TIME = 500;
		static int INITIALIZATION_TIME = 5000;
		//static int INITIALIZATION_TIME = 5000;
		static int IDENTIFICATION_TIME = 3000;
		//static int IDENTIFICATION_TIME = 5000;

		private string Serial_Number { get; set; }
		private decimal Relative_Step { get; set; } = 1m;
		private decimal Homing_Velocity { get; set; } = 2.3m;

		public bool Is_Homed { get; set; } = false;

		private bool _taskComplete;
		private ulong _taskID;

		public Motors(Action<string> Update_Status_TextBox, string serial_number)
		{
			Append_Status_Event += new Append_Status_Delegate(Update_Status_TextBox);
			Serial_Number = serial_number;
			Init_Motors();
		}

		void Init_Motors()
		{
			try { DeviceManagerCLI.BuildDeviceList(); }
			catch (Exception ex)
			{
				MessageBox.Show("Exception raised by BuildDeviceList " + ex);
				Environment.Exit(-1);
			}
			
			List<string> serialNumbers = DeviceManagerCLI.GetDeviceList(KCubeDCServo.DevicePrefix);
			if (!serialNumbers.Contains(Serial_Number))
			{
				Environment.Exit(-1);
			}

			device = KCubeDCServo.CreateKCubeDCServo(Serial_Number);
			if (device == null)
			{
				MessageBox.Show(Serial_Number + " is not a KCubeDCServo");
				Environment.Exit(-1);
			}

			try
			{
				//Append_Status_Event("Opening device " + Serial_Number);
				device.Connect(Serial_Number);
			}
			catch (Exception)
			{
				// connection failed
				MessageBox.Show("Failed to open device" + Serial_Number);
				Environment.Exit(-1);
			}

			if (!device.IsSettingsInitialized())
			{
				try { device.WaitForSettingsInitialized(INITIALIZATION_TIME); }
				catch (Exception) { MessageBox.Show("Settings failed to initialize"); }
			}

			device.StartPolling(250); // start the device polling
			Thread.Sleep(THREAD_SLEEP_TIME);
			
			device.EnableDevice(); // enable the channel otherwise any move is ignored 
			Thread.Sleep(THREAD_SLEEP_TIME);

			// call LoadMotorConfiguration on the device to initialize the DeviceUnitConverter object required for real world unit parameters
			MotorConfiguration motorSettings = device.LoadMotorConfiguration(Serial_Number);

			device.SetHomingVelocity(Homing_Velocity);
			Append_Status_Event("Setting Homing Velocity to = " + Serial_Number);

			device.SetMoveRelativeDistance(Relative_Step);
			Append_Status_Event("Setting Step Distance for Relative Move to = " + Relative_Step);

			Identify();
		}

		void Identify()
		{
			Append_Status_Event("IdentifyDevice - " + Serial_Number);
			device.IdentifyDevice();
			Thread.Sleep(IDENTIFICATION_TIME);
		}

		void Display_Info()
		{
			KCubeDCMotorSettings currentDeviceSettings = device.MotorDeviceSettings as KCubeDCMotorSettings;
			DeviceInfo deviceInfo = device.GetDeviceInfo();
			//Console.WriteLine("\nDevice {0} = {1}", deviceInfo.SerialNumber, deviceInfo.Name);
			Append_Status_Event("Device" + deviceInfo.SerialNumber + " = " + deviceInfo.Name);

			device.RequestHomingParams();
			Append_Status_Event("Device Homing Velocity is = " + device.GetHomingParams().Velocity);
		}

		void Stop_Motors(string serial_number, decimal position, decimal velocoty)
		{
			device.StopPolling();
			Append_Status_Event("Motor Stoped Polling");

			var lala = device.GetConnectionState();

			if ((device != null) && device.GetConnectionState() == ThorlabsConnectionManager.ConnectionStates.Connected)
			{
				device.Disconnect(true);
				Append_Status_Event("Motor Disconnected!");
			}
		}

		public void Home_Axis()
		{
			Append_Status_Event("Homing device");
			_taskComplete = false;
			_taskID = device.Home(CommandCompleteFunction);

			while (!_taskComplete)
			{
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Device Velocity = " + (device.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Device Homing = " + (device as KCubeDCServo).Position);
			}

			Append_Status_Event("Device Homed");
			Is_Homed = device.Status.IsHomed;
		}

		public void Move_Absolute(decimal position)
		{
			Append_Status_Event("Absolute Move to = " + position);

			_taskComplete = false;
			_taskID = device.MoveTo(position, CommandCompleteFunction);

			while (!_taskComplete)
			{
				Thread.Sleep(THREAD_SLEEP_TIME);

				Append_Status_Event("Device Velocity = " + (device.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Device Homing = " + (device as KCubeDCServo).Position);

			}

			Append_Status_Event("Finished Absolute Move");
		}

		public void Move_Relative(bool forward)
		{
			Decimal relative_distance = device.GetMoveRelativeDistance();
			_taskComplete = false;

			if (forward)
			{
				Append_Status_Event("Moving Relative Device by = " + relative_distance);
				_taskID = device.MoveRelative(MotorDirection.Forward, relative_distance, CommandCompleteFunction);
			}
			else
			{
				Append_Status_Event("Moving Relative Device by = " + (-1 * relative_distance));
				_taskID = device.MoveRelative(MotorDirection.Backward, relative_distance, CommandCompleteFunction);
			}

			while (!_taskComplete)
			{
				Thread.Sleep(THREAD_SLEEP_TIME);

				Append_Status_Event("Device Velocity = " + (device.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Device Homing = " + (device as KCubeDCServo).Position);
			}

			Append_Status_Event("Finished Relative Move");
		}

		public void CommandCompleteFunction(ulong taskID)
		{
			if ((_taskID > 0) && (_taskID == taskID))
			{
				_taskComplete = true;
			}
		}
	}
}
