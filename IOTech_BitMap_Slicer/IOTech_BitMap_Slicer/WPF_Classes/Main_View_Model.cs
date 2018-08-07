using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IOTech_BitMap_Slicer.WPF_Classes
{
	public class Main_View_Model : ObservableObject
	{
		//public WPF_Button button { get; set; }
		//public WPF_Button button { get; private set; }
		private Brush _background_color;

		public WPF_Button Button { get; set; }

		public Brush Background_Color
		{
			get
			{
				if (_background_color == null)
					return _background_color;

				return _background_color;
			}
			set
			{
				_background_color = value;
				OnPropertyChanged("Background_Color");
			}
		}
		
		//public BackgroundViewModel Background { get; private set; }

		public Main_View_Model()
		{
			Button = new WPF_Button();
			//Background = new BackgroundViewModel();
		}

		bool background_state = false;
		public void SetBackground_invoke()
		{
			if (background_state) { Background_Color = Brushes.Yellow; }
			else { Background_Color = Brushes.Blue; }

			background_state = !background_state;
		}
	}
}
