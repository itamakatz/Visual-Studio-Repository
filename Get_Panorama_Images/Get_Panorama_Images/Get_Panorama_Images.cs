using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uEye;
using System.Runtime.CompilerServices;

namespace Get_Panorama_Images
{
	public delegate void Append_Status_Delegate(string str);

	public enum Axis { X, Y, Z};

	public partial class Get_Panorama_Images : Form
	{
		My_Camera Camera;
		Motor[] Motors;

		readonly string[] Serial_Numbers = new string[] { "27000963" , "27000961", "27251998" }; 

		public Get_Panorama_Images() {
			InitializeComponent();

			Camera = new My_Camera(Update_Status_TextBox, DisplayWindow.Handle);

			Motors = new Motor[] { new Motor(Update_Status_TextBox, Serial_Numbers[(int) Axis.X]),
									new Motor(Update_Status_TextBox, Serial_Numbers[(int) Axis.Y]),
									new Motor(Update_Status_TextBox, Serial_Numbers[(int) Axis.Z])};
		}

		public void Update_Status_TextBox(string msg) { Status_TextBox.AppendText(msg + "\n"); }

		private void Button_Live_Video_Click(object sender, EventArgs e) { Camera.Video_Live(); }

		private void Button_Stop_Video_Click(object sender, EventArgs e) { Camera.Video_Stop(); }

		private void Button_Freeze_Video_Click(object sender, EventArgs e) { Camera.Video_Freeze(); }

		private void Save_Image_Click(object sender, EventArgs e) { Camera.Save_Image(); }

		private void Right_X_Click(object sender, EventArgs e) { Motors[(int) Axis.X].Move_Relative(true); }
		private void Left_X_Click(object sender, EventArgs e) { Motors[(int) Axis.X].Move_Relative(false); }

		private void Forward_Y_Click(object sender, EventArgs e) { Motors[(int) Axis.Y].Move_Relative(true); }
		private void Backwards_Y_Click(object sender, EventArgs e) { Motors[(int) Axis.Y].Move_Relative(false); }

		private void Up_Z_Click(object sender, EventArgs e) { Motors[(int) Axis.Z].Move_Relative(true); }
		private void Down_Z_Click(object sender, EventArgs e) { Motors[(int) Axis.Z].Move_Relative(false); }

		private void Home_Motors_Click(object sender, EventArgs e) { foreach (Motor Motor in Motors) { Motor.Home_Axis(); } }

		private void Home_X_Click(object sender, EventArgs e) { Motors[(int) Axis.X].Home_Axis(); }
		private void Home_Y_Click(object sender, EventArgs e) { Motors[(int) Axis.Y].Home_Axis(); }
		private void Home_Z_Click(object sender, EventArgs e) { Motors[(int) Axis.Z].Home_Axis(); }

		private void Identify_X_Click(object sender, EventArgs e) { Motors[(int) Axis.X].Identify_Motor(); }
		private void Identify_Y_Click(object sender, EventArgs e) { Motors[(int) Axis.Y].Identify_Motor(); }
		private void Identify_Z_Click(object sender, EventArgs e) { Motors[(int) Axis.Z].Identify_Motor(); }

		private void Init_Motors_Click(object sender, EventArgs e) { foreach (Motor Motor in Motors) { Motor.Init_Motor(); } }
		private void Init_Camera_Click(object sender, EventArgs e) { Camera.Init_Camera(); }

		private void Run_Relative_Move_X_Click(object sender, EventArgs e) {
			decimal input = Parse_Move_Box(X_Fine_Relative_Move_Box);
			if (input != 0m) { Motors[(int) Axis.X].Move_Relative(input); }
		}

		private void Run_Relative_Move_Y_Click(object sender, EventArgs e) {
			decimal input = Parse_Move_Box(Y_Fine_Relative_Move_Box);
			if (input != 0m) { Motors[(int) Axis.Y].Move_Relative(input); }

		}

		private void Run_Relative_Move_Z_Click(object sender, EventArgs e) {
			decimal input = Parse_Move_Box(Z_Fine_Relative_Move_Box);
			if (input != 0m) { Motors[(int) Axis.Z].Move_Relative(input); }
		}

		private Decimal Parse_Move_Box(TextBox textBox)
		{
			try {
				return decimal.Parse(textBox.Text);

			} catch (FormatException) {
				Update_Status_TextBox("Format Error: Movement Value Must Be A Rational Number");
				X_Fine_Relative_Move_Box.ResetText();
				return 0m;
			}
		}

		private void Auto_Gain_Balance_CheckedChanged(object sender, EventArgs e) {
			Camera.Auto_Gain_Toggle(CB_Auto_Gain_Balance.Checked);
		}

		private void Button_Exit_Prog_Click(object sender, EventArgs e) {
			Status_TextBox.AppendText("Exiting Program\n");
			Stop_Motors_Click(sender, e);
			Stop_Camera_Click(sender, e);
			Close();
		}

		private void Stop_Motors_Click(object sender, EventArgs e) {
			Status_TextBox.AppendText("Closing Motors\n");
			foreach (Motor Motor in Motors) { Motor.Exit_Motor(); }
		}

		private void Stop_Camera_Click(object sender, EventArgs e) {
			Status_TextBox.AppendText("Closing Camera\n");
			Camera.Exit_Cam();
		}
	}

	public static class Show_Error{
		public static void ShowMessage(string message, bool exit = true,
			[CallerLineNumber] int lineNumber = 0,
			[CallerFilePath] string callingFilePath = null,
			[CallerMemberName] string caller = null) {

			MessageBox.Show(message + 
				"\nFile: " + Path.GetFileName(callingFilePath) + 
				"\nLine: " + lineNumber + 
				"\nCaller: (" + caller + ")");
			if (exit) { Environment.Exit(-1); }
		}
	}
}
