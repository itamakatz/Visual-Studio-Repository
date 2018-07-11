// using System;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Drawing;
// using System.Data;
// using System.Text;
// using System.Windows.Forms;

// namespace FloodFill2
// {
// 	public partial class TextBoxSlider : UserControl
// 	{
// 		public TextBoxSlider()
// 		{
// 			InitializeComponent();
// 			UpdateText();
// 		}

// 		[Category("Behavior"), DefaultValue(0f)]
// 		public float Value
// 		{
// 			get { return FromSliderValue(trackBar.Value); }
// 			set { trackBar.Value = ToSliderValue(value); }
// 		}

// 		private int decimalPlaces;

// 		[Category("Behavior"), DefaultValue(0f)]
// 		public int DecimalPlaces
// 		{
// 			get { return decimalPlaces; }
// 			set { decimalPlaces = value; }
// 		}

// 		[Category("Behavior"), DefaultValue(100f)]
// 		public float Maximum
// 		{
// 			get { return FromSliderValue(trackBar.Maximum); }
// 			set { trackBar.Maximum = ToSliderValue(value); }
// 		}

// 		[Category("Behavior"), DefaultValue(0f)]
// 		public float Minimum
// 		{
// 			get { return FromSliderValue(trackBar.Minimum); }
// 			set { trackBar.Minimum = ToSliderValue(value); }
// 		}

// 		[Category("Behavior"), DefaultValue(1f)]
// 		public float SmallChange
// 		{
// 			get { return FromSliderValue(trackBar.SmallChange); }
// 			set { trackBar.SmallChange = ToSliderValue(value); }
// 		}

// 		[Category("Behavior"), DefaultValue(5f)]
// 		public float LargeChange
// 		{
// 			get { return FromSliderValue(trackBar.LargeChange); }
// 			set { trackBar.LargeChange = ToSliderValue(value); }
// 		}

// 		[Category("Appearance"), DefaultValue(1f)]
// 		public float TickFrequency
// 		{
// 			get { return FromSliderValue(trackBar.TickFrequency); }
// 			set { trackBar.TickFrequency = ToSliderValue(value); }
// 		}

// 		[Category("Appearance"), DefaultValue(TickStyle.BottomRight)]
// 		public TickStyle TickStyle
// 		{
// 			get { return trackBar.TickStyle; }
// 			set { trackBar.TickStyle = value; }
// 		}

// 		public override string Text
// 		{
// 			get
// 			{
// 				return textBox.Text;
// 			}
// 			set
// 			{
// 				textBox.Text = value;
// 			}
// 		}

// 		int ToSliderValue(float val)
// 		{
// 			float fval=(float)(val * Math.Pow(10, decimalPlaces));
// 			if(fval>(float)int.MaxValue)
// 				throw new ArgumentOutOfRangeException();
// 			return (int) fval;
// 		}

// 		float FromSliderValue(int val)
// 		{
// 			return (float)(((float)val) * Math.Pow(10, -decimalPlaces));
// 		}

// 		private void textBox_TextChanged(object sender, EventArgs e)
// 		{
// 			float fVal;
// 			if (float.TryParse(textBox.Text, out fVal))
// 			{
// 				if (fVal > Maximum || fVal < Minimum)
// 					SetTextBoxValidState(false);
// 				else
// 				{
// 					Value = fVal;
// 					SetTextBoxValidState(true);
// 				}
// 			}
// 			else
// 				SetTextBoxValidState(false);
// 			OnTextChanged(e);

// 		}

// 		bool textBoxValid = true;
// 		private void SetTextBoxValidState(bool valid)
// 		{
// 			textBoxValid = valid;
// 			if (valid)
// 				textBox.BackColor = Color.White;
// 			else
// 				textBox.BackColor = Color.FromArgb(255, 220, 220);
// 		}

// 		public event EventHandler ValueChanged;

// 		private void trackBar_ValueChanged(object sender, EventArgs e)
// 		{
// 			UpdateText();
// 			if (ValueChanged != null)
// 				ValueChanged(this, e);
// 		}

// 		void UpdateText()
// 		{
// 			//Use roundtrip formatting specifier so that the string value
// 			//will be the same when converted roundtrip.
// 			Text = Value.ToString("r");
// 		}

// 		private void textBox_Leave(object sender, EventArgs e)
// 		{
// 			if (!textBoxValid)
// 				UpdateText();
// 		}

// 		public event EventHandler TrackBarScroll;

// 		private void trackBar_Scroll(object sender, EventArgs e)
// 		{
// 			if (TrackBarScroll != null)
// 				TrackBarScroll(this, e);
// 		}
		
		
// 	}
// }
