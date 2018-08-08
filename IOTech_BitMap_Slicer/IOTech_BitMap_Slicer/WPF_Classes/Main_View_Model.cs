
using System.Windows.Media;

namespace IOTech_BitMap_Slicer.WPF_Classes
{
	public class Main_View_Model : ObservableObject
	{

		public Button Button { get; set; }
		public Slider Slider { get; set; }

		int _stam;

		public int Get_Slider_Value
		{
			get
			{
				return Slider.Slider_Value;
			}
			set
			{
				_stam = value;
			}
		}

		private bool _background_color_invoke = false;
		private Brush _background_color;

		public Brush Background_Color
		{
			get
			{
				if (_background_color == null) { return _background_color; }
				return _background_color;
			}
			set
			{
				_background_color = value;
				OnPropertyChanged("Background_Color");
			}
		}

		public Main_View_Model()
		{
			Button = new Button();
			Slider = new Slider();
		}

		public void Set_Button_String(string str)
		{
			Button.Bind_Name = str;
		}

		public void SetBackground_invoke()
		{
			Background_Color = _background_color_invoke ? Brushes.Yellow : Brushes.Blue;

			_background_color_invoke = !_background_color_invoke;
		}
	}
}
