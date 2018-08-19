using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Get_Panorama_Images
{
	class My_Camera
	{
		public event Append_Status_Delegate Append_Status_Event;

		private uEye.Camera cam;
		private IntPtr Display_Handle { get; set; } = IntPtr.Zero;

		const string SAVE_PATH = @"C:\Users\admin\Desktop\COM_Integration\uEye\";
		const string SAVE_PATH_SUFFIX = @".PNG";
		int CAPTURE_TIME = 10;
		int MAX_GAIN = 50;
		int SAVE_QUALITY = 100;
		int EDGE_ENHANCEMENT = 9;

		public My_Camera(Action<string> Update_Status_TextBox, IntPtr display_handle)
		{
			Append_Status_Event += new Append_Status_Delegate(Update_Status_TextBox);
			Display_Handle = display_handle;
			InitCamera();
		}

		private void InitCamera()
		{
			cam = new uEye.Camera();

			// Open Camera
			if (cam.Init() != uEye.Defines.Status.Success)
			{
				MessageBox.Show("Camera initializing failed");
				Environment.Exit(-1);
			}

			Append_Status_Event("Camera Initialized\n");

			// Allocate Memory
			if (cam.Memory.Allocate() != uEye.Defines.Status.Success)
			{
				MessageBox.Show("Allocate Memory failed");
				Environment.Exit(-1);
			}

			Append_Status_Event("Memory for camera allocated\n");

			// Start Live Video
			if (cam.Acquisition.Capture(CAPTURE_TIME) != uEye.Defines.Status.Success)
			{ MessageBox.Show("Start Live Video failed"); }

			Append_Status_Event("Started Live Mode\n");

			// Connect Event
			cam.EventFrame += Render_Event;

			cam.EdgeEnhancement.Set(EDGE_ENHANCEMENT);
			Append_Status_Event("Set Edge EnhancEment to = " + EDGE_ENHANCEMENT + "\n");

		}

		private void Render_Event(object sender, EventArgs e)
		{
			uEye.Camera Camera = sender as uEye.Camera;

			Int32 s32MemID;
			Camera.Memory.GetActive(out s32MemID);

			Camera.Display.Render(s32MemID, Display_Handle, uEye.Defines.DisplayRenderMode.FitToWindow);
		}

		public void Video_Live()
		{
			// Open Camera and Start Live Video
			cam.Acquisition.Capture(CAPTURE_TIME);
			Append_Status_Event("Started Live Mode\n");
		}

		public void Video_Stop()
		{
			// Stop Live Video
			cam.Acquisition.Stop(uEye.Defines.DeviceParameter.Wait);
			Append_Status_Event("Stop Live Mode\n");
		}

		public void Video_Freeze()
		{
			cam.Acquisition.Stop(uEye.Defines.DeviceParameter.Wait);
			cam.Acquisition.Freeze(uEye.Defines.DeviceParameter.Wait);
			Append_Status_Event("Freezing image - Saving it to the memory\n");
		}

		public void Auto_Gain_Toggle(bool state)
		{
			if (state)
			{
				cam.AutoFeatures.Software.Gain.SetMax(MAX_GAIN);
				cam.AutoFeatures.Software.Gain.SetEnable(true);
				Append_Status_Event("Enabling Auto Gain\n");
			}
			else
			{
				cam.AutoFeatures.Software.Gain.SetMax(0);
				cam.AutoFeatures.Software.Gain.SetEnable(false);
				Append_Status_Event("Disabling Auto Gain\n");
			}
		}

		public void Save_Image()
		{
			int i = 0;
			while (File.Exists(SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX)) { i++; }

			cam.Acquisition.Stop(uEye.Defines.DeviceParameter.Wait);

			cam.Acquisition.Freeze(uEye.Defines.DeviceParameter.Wait);

			cam.Image.Save(SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX, ImageFormat.Png, SAVE_QUALITY);

			Append_Status_Event("Image saved to = " + SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX + "\n");

			cam.Acquisition.Capture(CAPTURE_TIME);
		}

		public void Exit_Cam()
		{

		}


	}
}
