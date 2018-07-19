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

		public static void Check_directories()
		{
			try
			{
				// ****
				// maybe switch to this relative path
				//string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg");

				// adjust all path's relative to specific pc
				MainWindow.MODEL_IN_PATH = Path.Combine(MainWindow.USER_PATH, MainWindow.MODEL_IN_PATH);
				MainWindow.MODEL_OUT_PATH = Path.Combine(MainWindow.USER_PATH, MainWindow.MODEL_OUT_PATH);
				MainWindow.BITMAP_DIR_PREFIX = Path.Combine(MainWindow.USER_PATH, MainWindow.BITMAP_DIR_PREFIX);

				if (!File.Exists(MainWindow.BITMAP_DIR_PREFIX))
				{
					Directory.CreateDirectory(MainWindow.BITMAP_DIR_PREFIX);
				}
			}
			catch (Exception e)
			{
				Util.exit_messege(new string[] { "Check_directories faild" }, e);
			}
		}

		public static IEnumerable<double> Range_Enumerator(double start, double end, int num_of_slices)
		{
			//double increment = (end - start) / num_of_slices;
			//for (double i = start + increment; i < end; i += increment)
			//	yield return i;

			double increment = (end - start) / (num_of_slices + 2);
			for (double current = start + increment, i = 0; i < num_of_slices; current += increment, i++)
				yield return current;
		}

		public static void Run(double[] a, double[] b, int N)
		{
			Parallel.For(0, N, i => { a[i] += b[i]; });
		}
	}
}
