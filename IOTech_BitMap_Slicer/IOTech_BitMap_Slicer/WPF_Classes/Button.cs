﻿
namespace IOTech_BitMap_Slicer.WPF_Classes
{
	public class Button : ObservableObject
	{
		private string _name;

		public string Bind_Name
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_name))
					return "Unknown";

				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged("Bind_Name");
			}
		}
	}
}