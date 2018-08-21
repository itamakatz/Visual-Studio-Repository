using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Get_Panorama_Images.Util;

namespace Get_Panorama_Images
{
	class My_Camera
	{
		public event Append_Status_Delegate Append_Status_Event;

		private uEye.Camera cam;
		private IntPtr Display_Handle { get; set; } = IntPtr.Zero;

		const string SAVE_PATH = @"C:\Users\admin\Desktop\COM_Integration\uEye\";
		const string SAVE_PATH_SUFFIX = @".PNG";
		ImageFormat IMAGE_FORMAT = ImageFormat.Png;

		int MAX_GAIN = 50;
		int SAVE_QUALITY = 100;
		int EDGE_ENHANCEMENT = 9;

		int IMAGE_WIDTH = 2592;
		int IMAGE_HIGHT = 2048;
		int BITS_PER_PIXEL;
		uint MAX_RESOLUTION_FORMAT_ID = 36;

		int Freeze_Memory_ID;
		int Live_Memory_ID;

		public My_Camera(Action<string> Update_Status_TextBox, IntPtr display_handle) {
			Append_Status_Event += new Append_Status_Delegate(Update_Status_TextBox);
			Display_Handle = display_handle;
		}

		public void Init_Camera() {
			cam = new uEye.Camera();

			// Open Camera
			if (cam.Init() != uEye.Defines.Status.Success) {
				Show_Error("Camera initializing failed");
			}

			Append_Status_Event("Camera: Camera Initialized");

			Set_Settings();
			Append_Status_Event("Camera: Finished Configuration of Settings ");

			cam.PixelFormat.GetBitsPerPixel(out BITS_PER_PIXEL);
			
			// Allocate Memory
			if (cam.Memory.Allocate(IMAGE_WIDTH, IMAGE_HIGHT, BITS_PER_PIXEL, false, out Freeze_Memory_ID) != uEye.Defines.Status.Success) {
			//if (cam.Memory.Allocate() != uEye.Defines.Status.Success) {
				Show_Error("Allocate Memory failed");
			}

			cam.Memory.Lock(Freeze_Memory_ID);

			if (cam.Memory.Allocate(IMAGE_WIDTH, IMAGE_HIGHT, BITS_PER_PIXEL, true, out Live_Memory_ID) != uEye.Defines.Status.Success) {
				//if (cam.Memory.Allocate() != uEye.Defines.Status.Success) {
				Show_Error("Allocate Memory failed");
			}

			Append_Status_Event("Camera: Memory for camera allocated");

			Video_Live();

			cam.EventFrame += Render_Event;

			Append_Status_Event("Camera: Set Edge EnhancEment to = " + EDGE_ENHANCEMENT);
		}

		private void Set_Settings() {
			cam.PixelFormat.Set(uEye.Defines.ColorMode.RGB12Unpacked);
			cam.Size.ImageFormat.Set(MAX_RESOLUTION_FORMAT_ID);

			cam.AutoFeatures.Software.Hysteresis.Set(10);
			cam.AutoFeatures.Software.PeakWhite.SetEnable(true);
			cam.AutoFeatures.Software.Shutter.SetEnable(true);
			cam.AutoFeatures.Software.Speed.Set(100);
			cam.AutoFeatures.Software.WhiteBalance.SetEnable(true);

			cam.EdgeEnhancement.Set(EDGE_ENHANCEMENT);
		}

		private void Render_Event(object sender, EventArgs e) {
			uEye.Camera Camera = sender as uEye.Camera;

			Int32 s32MemID;
			Camera.Memory.GetActive(out s32MemID);

			//Camera.Display.Render(s32MemID, Display_Handle, uEye.Defines.DisplayRenderMode.FitToWindow);
			Camera.Display.Render(s32MemID, Display_Handle, uEye.Defines.DisplayRenderMode.Normal);
		}

		public void Video_Live(bool print_update = true) {
			if(cam == null) { Show_Error("Error: Camera Wasn't Initialized"); }

			cam.Acquisition.HasStarted(out bool started);
			if (started) { Show_Error("Camera is Already Live", false); return; }

			if (cam.Acquisition.Capture() != uEye.Defines.Status.Success) { Show_Error("Start Live Video failed"); }

			if (print_update) { Append_Status_Event("Camera: Started Live Mode"); }
		}

		public void Video_Stop(){
			if(cam == null) { Show_Error("Error: Camera Wasn't Initialized"); }

			cam.Acquisition.Stop(uEye.Defines.DeviceParameter.Wait);
			Append_Status_Event("Camera: Stop Live Mode");
		}

		public void Video_Freeze() {
			if(cam == null) { Show_Error("Error: Camera Wasn't Initialized"); }

			cam.Acquisition.Stop(uEye.Defines.DeviceParameter.Wait);
			cam.Acquisition.Freeze(uEye.Defines.DeviceParameter.Wait);
			Append_Status_Event("Camera: Freezing image - Saving it to the memory");
		}

		public void Auto_Gain_Toggle(bool state) {
			if(cam == null) { Show_Error("Error: Camera Wasn't Initialized"); }

			if (state) {
				cam.AutoFeatures.Software.Gain.SetMax(MAX_GAIN);
				cam.AutoFeatures.Software.Gain.SetEnable(true);
				Append_Status_Event("Camera: Enabling Auto Gain");
			} else {
				cam.AutoFeatures.Software.Gain.SetMax(0);
				cam.AutoFeatures.Software.Gain.SetEnable(false);
				Append_Status_Event("Camera: Disabling Auto Gain");
			}
		}

		public void Save_Image() {
			if(cam == null) { Show_Error("Error: Camera Wasn't Initialized"); }

			int i = 0;
			while (File.Exists(SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX)) { i++; }

			cam.Acquisition.Stop(uEye.Defines.DeviceParameter.Wait);
			cam.Memory.Unlock(Freeze_Memory_ID);
			cam.Memory.SetActive(Freeze_Memory_ID);
			cam.Acquisition.Freeze(uEye.Defines.DeviceParameter.Wait);

			cam.Image.Save(SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX, IMAGE_FORMAT, SAVE_QUALITY);

			cam.Memory.Lock(Freeze_Memory_ID);

			cam.Memory.SetActive(Live_Memory_ID);

			Video_Live(false);

			Append_Status_Event("Camera: Image saved to = " + SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX);
		}

		public void Exit_Cam() { if (cam != null) { cam.Exit(); } }
	}
}
