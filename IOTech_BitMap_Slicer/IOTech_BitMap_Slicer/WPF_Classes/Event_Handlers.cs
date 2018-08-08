using g3;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Drawing;
using HelixToolkit.Wpf;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using System.Collections.Concurrent;
using IOTech_BitMap_Slicer.WPF_Classes;

namespace IOTech_BitMap_Slicer
{

	public partial class MainWindow : Window
	{
		private void button_1_Click(object sender, RoutedEventArgs e)
		{
			windows_bindings.SetBackground_invoke();
		}

		private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			double input_val = e.NewValue;
			windows_bindings.Set_Button_String(input_val.ToString());
		}
	}
}