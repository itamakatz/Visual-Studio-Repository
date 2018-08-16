using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using uEye;
using System.IO;
using System.Timers;

namespace Try_uEye
{
	class Program
	{

		const string SAVE_PATH = @"C:\Users\admin\Desktop\COM_Integration\uEye\";
		const string SAVE_PATH_SUFFIX = @".PNG";

		IntPtr displayHandle = IntPtr.Zero;

		int capture_wait_time = 10;
		int timer_elapse = 50;

		private static System.Timers.Timer capture_timer;

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.ReadKey();

			Program p = new Program();
			p.My_Init_Camera();

			Console.WriteLine("Program Finished");
			Console.ReadKey();
		}

		private void My_Init_Camera()
		{

			Camera cam = new Camera();
			cam.Init();

			displayHandle = Process.GetCurrentProcess().MainWindowHandle;

			cam.Memory.Allocate();

			cam.Acquisition.Capture(capture_wait_time);

			Int32 s32MemID;
			cam.Memory.GetActive(out s32MemID);
			cam.EventFrame += Render_Event;

			capture_timer = new System.Timers.Timer(timer_elapse);

			capture_timer.Elapsed += (sender, e) => Capture_Event(sender, e, ref cam);
			capture_timer.AutoReset = true;
			capture_timer.Enabled = true;

			Console.ReadKey();

			cam.Display.Render(s32MemID, displayHandle, uEye.Defines.DisplayRenderMode.FitToWindow);

			save_iamge(ref cam);

			capture_timer.Stop();
			capture_timer.Dispose();
		}

		private void Render_Event(object sender, EventArgs e)
		{
			Camera Camera = sender as Camera;

			Int32 s32MemID;
			Camera.Memory.GetActive(out s32MemID);

			Camera.Display.Render(s32MemID, displayHandle, uEye.Defines.DisplayRenderMode.FitToWindow);
		}

		private void Capture_Event(object sender, EventArgs e, ref Camera cam)
		{
			cam.Acquisition.Capture(capture_wait_time);
		}

		private static void save_iamge(ref Camera cam)
		{
			int i = 0;
			while (File.Exists(SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX)) { i++; }

			cam.Image.Save(SAVE_PATH + "uEye_image_" + i + SAVE_PATH_SUFFIX, ImageFormat.Png);
		}
	}
}
