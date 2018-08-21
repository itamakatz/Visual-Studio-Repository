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
using static Get_Panorama_Images.Util;

namespace Get_Panorama_Images
{
	class Motor
	{
		public event Append_Status_Delegate Append_Status_Event;

		private KCubeDCServo motor;

		public Axis Axis { get; }
		public string Serial_Number { get; }

		static int THREAD_SLEEP_TIME = 500;
		static int INITIALIZATION_TIME = 5000;
		static int IDENTIFICATION_TIME = 3000;
		private decimal HOMING_VELOCITY = 2.3m;

		public decimal Current_Position { get => (motor as KCubeDCServo).Position; }

		public decimal Relative_Move_Step
		{
			get { return motor.GetMoveRelativeDistance(); }
			set
			{
				motor.SetMoveRelativeDistance(value);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Setting Relative Move Step to = " + value);
			}
		}

		public bool Is_Homed { get; set; } = false;

		private bool _move_complete;
		private ulong _taskID;

		public Motor(Axis axis, Action<string> Update_Status_TextBox, string serial_number) {
			Axis = axis;
			Append_Status_Event += new Append_Status_Delegate(Update_Status_TextBox);
			Serial_Number = serial_number;
		}

		public void Init_Motor(decimal relative_Move_Step) {
			try { DeviceManagerCLI.BuildDeviceList(); }
			catch (Exception ex) {
				Show_Error("Motor " + Axis.Str() + " Axis Error: Exception raised by BuildDeviceList " + ex);
			}

			List<string> serialNumbers = DeviceManagerCLI.GetDeviceList(KCubeDCServo.DevicePrefix);

			if (!serialNumbers.Contains(Serial_Number)) {
				Show_Error("Motor " + Axis.Str() + " Axis Error: DeviceManagerCLI.GetDeviceList Failed");
			}

			motor = KCubeDCServo.CreateKCubeDCServo(Serial_Number);
			if (motor == null) {
				Show_Error("Motor " + Axis.Str() + " Axis Error: " + Serial_Number + " is not a KCubeDCServo");
			}

			try {
				motor.Connect(Serial_Number);
			} catch (Exception) {
				Show_Error("Motor " + Axis.Str() + " Axis Error: Failed to open motor" + Serial_Number);
			}

			if (!motor.IsSettingsInitialized()) {
				try { motor.WaitForSettingsInitialized(INITIALIZATION_TIME); }
				catch (Exception) { Show_Error("Motor " + Axis.Str() + " Axis Error: Settings failed to initialize", false); }
			}

			motor.StartPolling(250); // start the motor polling
			Thread.Sleep(THREAD_SLEEP_TIME);
			
			motor.EnableDevice(); // enable the channel otherwise any move is ignored 
			Thread.Sleep(THREAD_SLEEP_TIME);

			// call LoadMotorConfiguration on the motor to initialize the DeviceUnitConverter object required for real world unit parameters
			MotorConfiguration motorSettings = motor.LoadMotorConfiguration(Serial_Number);

			motor.SetHomingVelocity(HOMING_VELOCITY);
			Append_Status_Event("Motor " + Axis.Str() + " Axis: Setting Homing Velocity to = " + HOMING_VELOCITY);

			Relative_Move_Step = relative_Move_Step;

			//motor.SetMoveRelativeDistance(Relative_Move_Step);
			//Append_Status_Event("Motor " + Axis.Str() + " Axis: Setting Step Distance for Relative Move to = " + Relative_Move_Step);
		}

		private void Display_Info() {
			KCubeDCMotorSettings currentDeviceSettings = motor.MotorDeviceSettings as KCubeDCMotorSettings;
			DeviceInfo deviceInfo = motor.GetDeviceInfo();
			Append_Status_Event("Motor " + Axis.Str() + " Axis: Motor" + deviceInfo.SerialNumber + " = " + deviceInfo.Name);

			motor.RequestHomingParams();
			Append_Status_Event("Motor " + Axis.Str() + " Axis: Motor Homing Velocity is = " + motor.GetHomingParams().Velocity);
		}

		private void Stop_Motors(string serial_number, decimal position, decimal velocoty) {
			motor.StopPolling();
			Append_Status_Event("Motor " + Axis.Str() + " Axis: Motor Stoped Polling");

			var lala = motor.GetConnectionState();

			if ((motor != null) && motor.GetConnectionState() == ThorlabsConnectionManager.ConnectionStates.Connected) {
				motor.Disconnect(true);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Motor Disconnected!");
			}
		}

		public void Home_Axis() {
			if(motor == null) { Show_Error("Error: Motor Wasn't Initialized"); }

			Append_Status_Event("Motor " + Axis.Str() + " Axis: Homing Motor");
			_move_complete = false;
			_taskID = motor.Home(CommandCompleteFunction);

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Position = " + (motor as KCubeDCServo).Position);
			}

			Append_Status_Event("Motor " + Axis.Str() + " Axis: Motor Homed");
			Is_Homed = motor.Status.IsHomed;
		}

		public void Move_Absolute(decimal position) {
			if(motor == null) { Show_Error("Motor Error: Motor Wasn't Initialized"); }

			Append_Status_Event("Motor " + Axis.Str() + " Axis: Absolute Move to = " + position);

			_move_complete = false;
			_taskID = motor.MoveTo(position, CommandCompleteFunction);

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Position = " + (motor as KCubeDCServo).Position);
			}

			Append_Status_Event("Motor " + Axis.Str() + " Axis: Finished Absolute Move");
		}

		public void Move_Relative(bool forward) {
			if(motor == null) { Show_Error("Motor Error: Motor Wasn't Initialized"); }

			decimal relative_distance = Relative_Move_Step;
			_move_complete = false;

			if (forward) {
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Relative Move of = " + relative_distance);
				_taskID = motor.MoveRelative(MotorDirection.Forward, relative_distance, CommandCompleteFunction);
			} else {
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Relative Move of = " + (-1 * relative_distance));
				_taskID = motor.MoveRelative(MotorDirection.Backward, relative_distance, CommandCompleteFunction);
			}

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Position = " + (motor as KCubeDCServo).Position);
			}

			Append_Status_Event("Motor " + Axis.Str() + " Axis: Finished Relative Move");
		}

		public void Move_Relative(decimal move) {
			if(motor == null) { Show_Error("Motor Error: Motor Wasn't Initialized"); }

			Append_Status_Event("Motor " + Axis.Str() + " Axis: Relative Move of " + move);

			Relative_Move_Step = move;

			_move_complete = false;
			_taskID = motor.MoveRelative(CommandCompleteFunction);

			while (!_move_complete) {
				Thread.Sleep(THREAD_SLEEP_TIME);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Velocity = " + (motor.Status as KCubeDCStatus).Velocity);
				Append_Status_Event("Motor " + Axis.Str() + " Axis: Current Position = " + (motor as KCubeDCServo).Position);
			}

			motor.SetMoveRelativeDistance(Relative_Move_Step);

			Append_Status_Event("Motor " + Axis.Str() + " Axis: Finished Relative Move");
		}

		private void CommandCompleteFunction(ulong taskID) {
			if ((_taskID > 0) && (_taskID == taskID)) { _move_complete = true; }
		}

		public void Identify_Motor() {
			if(motor == null) { Show_Error("Motor Error: Motor Wasn't Initialized"); }
			Append_Status_Event("Motor " + Axis.Str() + " Axis: Identifing motor - " + Serial_Number);
			motor.IdentifyDevice();
			Thread.Sleep(IDENTIFICATION_TIME);
		}

		public void Exit_Motor() { if (motor != null) { motor.ShutDown(); } }
	}
}
