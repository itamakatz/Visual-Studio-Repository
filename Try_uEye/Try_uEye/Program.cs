using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using uEye;

namespace Try_uEye
{
	class Program
	{

		const string SAVE_PATH = @"C:\Users\admin\Desktop\COM_Integration\uEye\";
		const string SAVE_PATH_SUFFIX = @".PNG";

		private uEye.Camera camera;
		IntPtr displayHandle = IntPtr.Zero;
		private bool bLive = false;


		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.ReadKey();

			Program p = new Program();
			p.My_Init_Camera();
			//p.Old_Program();

			Console.WriteLine("Program Finished");
			Console.ReadKey();
		}

		private void My_Init_Camera()
		{

			uEye.Camera cam = new uEye.Camera();
			cam.Init();

			uEye.Configuration.BootBoost.Wait();

			displayHandle = Process.GetCurrentProcess().MainWindowHandle;
			
			cam.Memory.Allocate();

			int s32Wait = 500;

			cam.Acquisition.Capture(s32Wait);

			//cam.Acquisition.Capture();

			uEye.Defines.DeviceParameter la = uEye.Defines.DeviceParameter.DontWait;
			//uEye.Defines.DeviceParameter la = uEye.Defines.DeviceParameter.Wait;

			cam.Acquisition.Freeze(la);

			Int32 s32MemID;
			cam.Memory.GetActive(out s32MemID);

			cam.Display.Render(s32MemID, displayHandle, uEye.Defines.DisplayRenderMode.FitToWindow);

			//cam.Display.Render(mode, handle);

			cam.Image.Save(SAVE_PATH + "image" + SAVE_PATH_SUFFIX, ImageFormat.Png);

		}


		private void Old_Program()
		{

			uEye.Camera cam = new uEye.Camera();
			cam.Init();

			uEye.Configuration.BootBoost.Wait();

			cam.Memory.Allocate();

			//cam.Acquisition.Capture();

			int s32Wait = 500;

			cam.Acquisition.Capture(s32Wait);

			uEye.Defines.DeviceParameter la = uEye.Defines.DeviceParameter.DontWait;
			//uEye.Defines.DeviceParameter la = uEye.Defines.DeviceParameter.Wait;

			cam.Acquisition.Freeze(la);

			uEye.Defines.DisplayRenderMode mode = uEye.Defines.DisplayRenderMode.FitToWindow;

			cam.Display.Render(mode);

			cam.Image.Save(SAVE_PATH + "image" + SAVE_PATH_SUFFIX, ImageFormat.Png);

			//Console.WriteLine("Finished running code!\n");
			//Console.ReadKey();
		}


		//private void InitCamera()
		//{
		//	camera = new uEye.Camera();

		//	uEye.Defines.Status statusRet = 0;

		//	// Open Camera
		//	statusRet = camera.Init();
		//	if (statusRet != uEye.Defines.Status.Success)
		//	{
		//		Console.WriteLine("Camera initializing failed");
		//		Environment.Exit(-1);
		//	}

		//	// Allocate Memory
		//	statusRet = camera.Memory.Allocate();
		//	if (statusRet != uEye.Defines.Status.Success)
		//	{
		//		Console.WriteLine("Allocate Memory failed");
		//		Environment.Exit(-1);
		//	}

		//	// Start Live Video
		//	statusRet = camera.Acquisition.Capture();
		//	if (statusRet != uEye.Defines.Status.Success)
		//	{
		//		Console.WriteLine("Start Live Video failed");
		//	}
		//	else
		//	{
		//		bLive = true;
		//	}

		//	// Connect Event
		//	camera.EventFrame += onFrameEvent;

		//	//CB_Auto_Gain_Balance.Enabled = camera.AutoFeatures.Software.Gain.Supported;
		//	//CB_Auto_White_Balance.Enabled = camera.AutoFeatures.Software.WhiteBalance.Supported;
		//}

		//private void onFrameEvent(object sender, EventArgs e)
		//{
		//	uEye.Camera Camera = sender as uEye.Camera;

		//	Int32 s32MemID;
		//	Camera.Memory.GetActive(out s32MemID);

		//	Camera.Display.Render(s32MemID, displayHandle, uEye.Defines.DisplayRenderMode.FitToWindow);
		//}
	}
}
