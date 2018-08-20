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
	class Motor
	{
		public event Append_Status_Delegate Append_Status_Event;

		private KCubeDCServo motor;

		static int THREAD_SLEEP_TIME = 500;
		static int INITIALIZATION_TIME = 5000;
		//static int INITIALIZATION_TIME = 5000;
		static int IDENTIFICATION_TIME = 3000;
		//static int IDENTIFICATION_TIME = 5000;

		private string Serial_Number { get; set; }
		private decimal Relative_Move_Step { get; set; } = 1m;
		private decimal Homing_Velocity { get; set; } = 2.3m;

		public bool Is_Homed { get; set; } = false;

		private bool _move_complete;
		private ulong _taskID;

		public Motor(Action<string> Update_Status_TextBox, string serial_number) {
			Append_Status_Event += new Append_Status_Delegate(Update_Status_TextBox);
			Serial_Number = serial_number;
		}

		public void Init_Motor() {
			try { DeviceManagerCLI.BuildDeviceList(); }
			catch (Exception ex) {
				Show_Error.ShowMessage("Exception raised by BuildDeviceList " + ex);
			}

			List<string> serialNumbers = DeviceManagerCLI.GetDeviceList(KCubeDCServo.DevicePrefix);

			if (!serialNumbers.Contains(Serial_Number)) {
				Show_Error.ShowMessage("DeviceManagerCLI.GetDeviceList Failed");
			}

			motor = KCubeDCServo.CreateKCubeDCServo(Serial_Number);
			if (motor == null) {
				Show_Error.ShowMessage(Serial_Number + " is not a KCubeDCServo");
			}

			try {
				motor.Connect(Serial_Number);
			} catch (Exception) {
				Show_Error.ShowMessage("Failed to open motor" + Serial_Number);
			}

			if (!motor.IsSettingsInitialized()) {
				try { motor.WaitForSettingsInitialized(INITIALIZATION_TIME); }
				catch (Exception) { Show_Error.ShowMessage("Settings failed to initialize", false); }
			}

			motor.StartPolling(250); // start the motor polling
			Thread.Sleep(THREAD_SLEEP_TIME);
			
			motor.EnableDevice(); // enable the channel otherwise any move is ignored 
			Thread.Sleep(THREAD_SLEEP_TIME);

			// call LoadMotorConfiguration on the motor to initialize the DeviceUnitConverter object required for real world unit parameters
			MotorConfiguration motorSettings = motor.LoadMotorConfiguration(Serial_Number);

			motor.SetHomingVelocity(Homing_Velocity);
			Append_Status_Event("Motor: Setting Homing Velocity to = " + Serial_Number);

			motor.SetMoveRelativeDistance(Relative_Move_Step);
			Append_Status_Event("Motor: Setting Step Distance for Relative Move to = " + Relative_Move_Step);
		}

		private void Display_Info() {
			KCubeDCMotorSettings currentDeviceSettings = motor.MotorDeviceSettings as KCubeDCMotorSettings;
			DeviceInfo deviceInfo = motor.GetDeviceInfo();
			Append_Status_Event("Motor: Motor" + deviceInfo.SerialNumber + " = " + deviceInfo.Name);

			motor.RequestHomingParams();
			Append_Status_Event("Motor: Motor Homing Velocity is = " + motor.GetHomingParams().Velocity);
		}

		private void Stop_Motors(string serial_number, decimal position, decimal velocoty) {
			motor.StopPolling();
			Append_Status_Event("Motor: Motor Stoped Polling");

			var lala = motor.GetConnectionState();

			if ((motor != null) && motor.GetConnectionState() == ThorlabsConnectionManager.ConnectionStates.Connected) {
				motor.Disconnect(true);
				Append_Status_Event("Motor: Motor Disconnected!");
			}
		}

		public void Home_Axis() {
			if(motor == null) { Show_Error.ShowMessage("Error: Motor Wasn't Initialized"); }

			Append_Status_Event("Motor: Homing motor");
			_move_complete = false;
			_taskID = motor.Home(CommandCompleteFunction);

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor: Motor Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor: Motor Homing = " + (motor as KCubeDCServo).Position);
			}

			Append_Status_Event("Motor: Motor Homed");
			Is_Homed = motor.Status.IsHomed;
		}

		public void Move_Absolute(decimal position) {
			if(motor == null) { Show_Error.ShowMessage("Error: Motor Wasn't Initialized"); }

			Append_Status_Event("Motor: Absolute Move to = " + position);

			_move_complete = false;
			_taskID = motor.MoveTo(position, CommandCompleteFunction);

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor: Motor Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor: Motor Homing = " + (motor as KCubeDCServo).Position);
			}

			Append_Status_Event("Motor: Finished Absolute Move");
		}

		public void Move_Relative(bool forward) {
			if(motor == null) { Show_Error.ShowMessage("Error: Motor Wasn't Initialized"); }

			decimal relative_distance = motor.GetMoveRelativeDistance();
			_move_complete = false;

			if (forward) {
				Append_Status_Event("Motor: Relative Move of = " + relative_distance);
				_taskID = motor.MoveRelative(MotorDirection.Forward, relative_distance, CommandCompleteFunction);
			} else {
				Append_Status_Event("Motor: Relative Move of = " + (-1 * relative_distance));
				_taskID = motor.MoveRelative(MotorDirection.Backward, relative_distance, CommandCompleteFunction);
			}

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor: Motor Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor: Motor Homing = " + (motor as KCubeDCServo).Position);
			}

			Append_Status_Event("Motor: Finished Relative Move");
		}

		public void Move_Relative(decimal move) {
			if(motor == null) { Show_Error.ShowMessage("Error: Motor Wasn't Initialized"); }

			Append_Status_Event("Motor: Relative Move of " + move);

			motor.SetMoveRelativeDistance(move);

			_move_complete = false;
			_taskID = motor.MoveRelative(CommandCompleteFunction);

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor: Device Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor: Device Homing = " + (motor as KCubeDCServo).Position);
			}

			motor.SetMoveRelativeDistance(Relative_Move_Step);

			Append_Status_Event("Motor: Finished Relative Move");
		}

		private void CommandCompleteFunction(ulong taskID) {
			if ((_taskID > 0) && (_taskID == taskID)) { _move_complete = true; }
		}

		public void Identify_Motor() {
			if(motor == null) { Show_Error.ShowMessage("Error: Motor Wasn't Initialized"); }
			Append_Status_Event("Motor: Identifing motor - " + Serial_Number);
			motor.IdentifyDevice();
			Thread.Sleep(IDENTIFICATION_TIME);
		}

		public void Exit_Motor() { if (motor != null) { motor.ShutDown(); } }
	}
}
