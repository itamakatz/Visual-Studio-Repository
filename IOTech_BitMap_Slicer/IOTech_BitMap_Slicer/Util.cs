using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTech_BitMap_Slicer
{
	class Util
	{

		public static int EXIT_CODE { get; set; }

		public void Clear_dir(String path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			foreach (FileInfo file in di.EnumerateFiles())
			{
				file.Delete();
			}
			foreach (DirectoryInfo dir in di.EnumerateDirectories())
			{
				dir.Delete(true);
			}
		}

		public static void exit_messege(string[] messages)
		{
			// possibly use:
			// MessageBox.Show("Exception Error : " + e.StackTrace);
			Trace.WriteLine("\n");

			foreach (string msg in messages)
			{
				Trace.WriteLine(msg);
			}

			Trace.WriteLine("\n");

			Environment.Exit(EXIT_CODE);
		}

		public static void exit_messege(string[] messages, Exception e)
		{
			// possibly use:
			// MessageBox.Show("Exception Error : " + e.StackTrace);
			Trace.WriteLine("\n");

			foreach (string msg in messages)
			{
				Trace.WriteLine(msg);
			}

			Trace.WriteLine("\n");

			Trace.WriteLine(e.StackTrace);
			Trace.WriteLine(e.Message);

			throw e;

			//Environment.Exit(EXIT_CODE);
		}

		public static void Print_elapsed(TimeSpan time_stamp)
		{
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			time_stamp.Hours, time_stamp.Minutes, time_stamp.Seconds,
			time_stamp.Milliseconds / 10);
			Trace.WriteLine("RunTime: " + elapsedTime);
		}
	}
}
