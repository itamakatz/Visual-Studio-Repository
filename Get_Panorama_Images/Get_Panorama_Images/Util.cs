using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Get_Panorama_Images {

	public enum Axis { X, Y, Z };

	public static class Util {

		public static void Show_Error(string message, bool exit = true,
			[CallerLineNumber] int lineNumber = 0,
			[CallerFilePath] string callingFilePath = null,
			[CallerMemberName] string caller = null) {

			MessageBox.Show(message +
				"\nFile: " + Path.GetFileName(callingFilePath) +
				"\nLine: " + lineNumber +
				"\nCaller: (" + caller + ")");
			if (exit) { Environment.Exit(-1); }
		}

		public static int Num(this Axis axis) {
			switch (axis) {
				case Axis.X:
					return 0;
				case Axis.Y:
					return 1;
				case Axis.Z:
					return 2;
				default:
					return -1;
			}
		}

		public static string Str(this Axis axis) {
			switch (axis) {
				case Axis.X:
					return "X";
				case Axis.Y:
					return "Y";
				case Axis.Z:
					return "Z";
				default:
					return null;
			}
		}
	}
}
