using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;     // DLL support
using MG17MotorLib;

namespace Using_My_DLL
{
	class Program
	{
		static void Main(string[] args)
		{
			MG17MotorLib.MG17Motor mG17Motor = new MG17Motor();
			//CMG17SystemCtrl cMG17SystemCtrl a;

			//MG17MotorLib.MG17Motor
			Console.WriteLine("Hello World!");
			Console.ReadKey();
		}
	}

	class hi
	{
		//[DllImport("TryMakeDLL.dll")]
		//public static extern void DisplayHelloFromDLL();

		//static void Main()
		//{
		//	Console.WriteLine("This is C# program");
		//	DisplayHelloFromDLL();
		//}
	}
}
