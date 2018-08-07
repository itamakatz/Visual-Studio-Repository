using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IOTech_BitMap_Slicer
{
	class Util
	{
		public static int EXIT_CODE { get; set; }

		public void Clear_dir(String path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			foreach (FileInfo file in di.EnumerateFiles()) { file.Delete(); }
			foreach (DirectoryInfo dir in di.EnumerateDirectories()) { dir.Delete(true); }
		}

		public static void Print_Messege(string[] messages)
		{
			// possibly use - MessageBox.Show("Exception Error : " + e.StackTrace);
			Trace.WriteLine("\n");
			foreach (string msg in messages) { Trace.WriteLine(msg); }
			Trace.WriteLine("\n");
		}

		public static void Exit_messege(string[] messages)
		{
			Print_Messege(messages);
			Environment.Exit(EXIT_CODE);
		}

		public static void Exit_messege(string[] messages, Exception e)
		{
			Exit_messege(messages);
			Trace.WriteLine(e.StackTrace);
			Trace.WriteLine(e.Message);
			throw e;
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
				V.MODEL_IN_PATH = Path.Combine(V.USER_PATH, V.MODEL_IN_PATH);
				V.MODEL_OUT_PATH = Path.Combine(V.USER_PATH, V.MODEL_OUT_PATH);
				V.BITMAP_DIR_PREFIX = Path.Combine(V.USER_PATH, V.BITMAP_DIR_PREFIX);

				if (!File.Exists(V.BITMAP_DIR_PREFIX)) { Directory.CreateDirectory(V.BITMAP_DIR_PREFIX); }
			}
			catch (Exception e) { Exit_messege(new string[] { "Check_directories faild" }, e); }
		}

		public static IEnumerable<double> Range_Enumerator(double start, double end, int num_of_slices)
		{
			double increment = (end - start) / (num_of_slices + 2);
			for (double current = start + increment, i = 0; i < num_of_slices; current += increment, i++)
				yield return current;
		}
	}
}
