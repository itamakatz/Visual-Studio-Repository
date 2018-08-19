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

namespace Get_Panorama_Images
{
	public delegate void Append_Status_Delegate(string str);

	public partial class Get_Panorama_Images : Form
	{
		My_Camera my_camera;
		Motors[] Motors;

		const string X_Serial_number = "27000961";
		const string Y_Serial_number = "27000963";
		const string Z_Serial_number = "27251998";

		public Get_Panorama_Images()
		{
			InitializeComponent();

			my_camera = new My_Camera(Update_Status_TextBox, DisplayWindow.Handle);

			Motors = new Motors[] { new Motors(Update_Status_TextBox, X_Serial_number),
									new Motors(Update_Status_TextBox, Y_Serial_number),
									new Motors(Update_Status_TextBox, Z_Serial_number)};
		}

		private void Button_Live_Video_Click(object sender, EventArgs e) { my_camera.Video_Live(); }

		private void Button_Stop_Video_Click(object sender, EventArgs e) { my_camera.Video_Stop(); }

		private void Button_Freeze_Video_Click(object sender, EventArgs e) { my_camera.Video_Freeze(); }

		private void Auto_Gain_Balance_CheckedChanged(object sender, EventArgs e)
		{ my_camera.Auto_Gain_Toggle(CB_Auto_Gain_Balance.Checked); }

		private void Button_Exit_Prog_Click(object sender, EventArgs e)
		{
			// Close the Camera
			Status_TextBox.AppendText("Exiting Program\n");
			my_camera.Exit_Cam();
			Close();
		}

		private void Save_Image_Click(object sender, EventArgs e) { my_camera.Save_Image(); }

		private void Right_Click(object sender, EventArgs e)
		{
			Motors[0].Move_Relative(true);
		}

		private void Left_Click(object sender, EventArgs e)
		{
			Motors[0].Move_Relative(false);
		}

		private void Forward_Click(object sender, EventArgs e)
		{
			Motors[1].Move_Relative(true);
		}

		private void Backwards_Click(object sender, EventArgs e)
		{
			Motors[1].Move_Relative(false);
		}

		private void Up_Click(object sender, EventArgs e)
		{
			Motors[2].Move_Relative(true);
		}

		private void Down_Click(object sender, EventArgs e)
		{
			Motors[2].Move_Relative(false);
		}

		private void Home_Motors_Click(object sender, EventArgs e) { foreach (Motors Motor in Motors) { Motor.Home_Axis(); } }

		public void Update_Status_TextBox(string msg) { Status_TextBox.AppendText(msg); }
	}
}
