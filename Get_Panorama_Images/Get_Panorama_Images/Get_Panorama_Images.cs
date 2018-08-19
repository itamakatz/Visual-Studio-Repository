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

		public Get_Panorama_Images()
		{
			InitializeComponent();

			my_camera = new My_Camera(Update_Status_TextBox, DisplayWindow.Handle);
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

		private void Down_Click(object sender, EventArgs e)
		{

		}

		private void Up_Click(object sender, EventArgs e)
		{

		}

		private void Straight_Click(object sender, EventArgs e)
		{

		}

		private void Left_Click(object sender, EventArgs e)
		{

		}

		private void Back_Click(object sender, EventArgs e)
		{

		}

		private void Right_Click(object sender, EventArgs e)
		{

		}

		private void Output_Update_TextChanged(object sender, EventArgs e)
		{

		}

		public void Update_Status_TextBox(string msg) { Status_TextBox.AppendText(msg); }
	}
}
