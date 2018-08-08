using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTech_BitMap_Slicer.WPF_Classes
{
	public class Slider : ObservableObject
	{
		public int Min_Value { get; set; } = 0;
		public int Mid_Value { get; set; } = 10;
		public int Max_Value { get; set; } = 20;
		public int Slider_Value { get; set; } = 10;
	}
}
