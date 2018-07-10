using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections;
using System.IO;
using System.Xml;
using System.Threading;
using PictureBoxScroll;

namespace FloodFill2
{
	public partial class Form1 : Form
	{
		protected AbstractFloodFiller floodFiller;
		protected bool mouseDown = false;
		SolidBrush fillBrush;

		public Form1()
		{
			InitializeComponent();
			Init();
		}

		///<summary>Does all non-designer initialization</summary>
		void Init()
		{
			//populate combo boxes
			comboNamedColor.Items.Add("<Custom>");
			comboNamedColor.Items.AddRange(Enum.GetNames(typeof(System.Drawing.KnownColor)));
			comboNamedColor.SelectedIndex = 0;
			comboFillType.Items.Add("QueueLinear (Array method)");
			comboFillType.Items.Add("QueueLinear (LockBits method)");
			comboFillType.SelectedIndex = 0;

			//set tolerance slider inital values
			sliderPickerTolR.Value=30;
			sliderPickerTolG.Value=30;
			sliderPickerTolB.Value=30;

			//load default image if it is present
			if (File.Exists(@"poppy_sm.jpg"))
				OpenBitmap(@"poppy_sm.jpg");

			//raise events to set values
			SetFillColor(Color.Black);

			//add screen update handler
			floodFiller.UpdateScreen = new UpdateScreenDelegate(this.UpdateScreen);
		}

		/// <summary>
		/// Performs the floodfill operation, and handles updates afterwards. 
		/// Also handles the case where the user presses Stop Fill while a slow fill is being done.
		/// </summary>
		/// <param name="pt">The point to start the fill at.</param>
		void DoFill(Point pt)
		{
			btnStop.Enabled = true;
			
			//disable controls so they can't be used during slow fill
			//(Application.DoEvents() is called during slow fill)
			if (floodFiller.Slow)
			{
				panel.Enabled = false;
				groupBoxFillColor.Enabled = false;
				groupBoxTolerance.Enabled = false;
			}


			try
			{
				floodFiller.FloodFill(pt);
			}
			catch (FillOperationAbortedException)
			{
				//this occurs when the user clicks "Stop Fill".
			}
			catch (Exception e)
			{
				MessageBox.Show(String.Format("An exception of type '{0}' occurred when performing the fill operation.", e.GetType().Name));
			}

			//enable controls again
			panel.Enabled = true;
			groupBoxFillColor.Enabled = true;
			groupBoxTolerance.Enabled = true;

			//update image
			panel.Invalidate();

			//update the statusbar & button
			statusStrip.Items[0].Text = "Last fill took "+floodFiller.watch.ElapsedMilliseconds.ToString()+"ms";
			btnStop.Enabled = false;
		}

		#region Panel Events

		void panelMouseDown(object sender, System.Windows.Forms.MouseEventArgs ev)
		{
			mouseDown = true;
		}

		void panelMouseUp(object sender, System.Windows.Forms.MouseEventArgs ev)
		{
			//make sure :
			//1) we recieved a mousedown message before this
			//2) the coordinates are not above the top of the bitmap
			if (!mouseDown || ev.X - panel.AutoScrollPosition.X < 0 || ev.Y - panel.AutoScrollPosition.Y < 0) return;
			mouseDown = false;

			if (ev.Button == System.Windows.Forms.MouseButtons.Left && floodFiller.Bitmap!=null) //if the left button was pressed and bitmap isn't null
			{
				//get the actual point that was clicked on the bitmap
				Point pt = new Point(ev.X - panel.AutoScrollPosition.X, ev.Y - panel.AutoScrollPosition.Y);	
				pt.X %= panel.Image.Bitmap.Width;
				pt.Y %= panel.Image.Bitmap.Height;
				//do the fill
				DoFill(pt);
			}
		}

		void panelMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			statusStrip.Items[1].Text = "Position: " + (e.X - panel.AutoScrollPosition.X).ToString() + ", " + (e.Y - panel.AutoScrollPosition.Y).ToString();
		}

		#endregion

		#region Misc Events
		private void openBtn_Click(object sender, EventArgs e)
		{
			OpenBitmap();
		}

		private void saveBtn_Click(object sender, EventArgs e)
		{
			SaveBitmap();
		}

		private void sliderPickerColor_ValueChanged(object sender, EventArgs e)
		{
			SetFillColor(Color.FromArgb((int)sliderPickerR.Value,(int)sliderPickerG.Value,(int)sliderPickerB.Value));
		}

		private void btnSlow_CheckedChanged(object sender, EventArgs e)
		{
			floodFiller.Slow = btnSlow.Checked;
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			throw new FillOperationAbortedException();//stop the operation
		}
		#endregion

		#region Combo Events

		///<summary>Custom-draws the color combo.  A modified version of Steve McMahon's
		/// owner-drawn combo box in his HLS-To-RGB example, available at www.vbaccelerator.com.</summary>
		void comboNamedColorDrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			int itemIndex = e.Index;

			e.DrawBackground();

			if (itemIndex > 0) //known color
			{
				//get color name
				string knownColorName = comboNamedColor.Items[itemIndex].ToString();

				Rectangle textRectangle = e.Bounds;

				//compute the rectangle to draw the color swatch in
				Rectangle colorRectangle = e.Bounds;
				colorRectangle.X += 2;
				colorRectangle.Y += 2;
				colorRectangle.Width = 16;
				colorRectangle.Height -= 4;

				//Get color to draw, and draw the color swatch.
				KnownColor k = (KnownColor)Enum.Parse(typeof(System.Drawing.KnownColor), knownColorName);
				Color color = Color.FromKnownColor(k);
				using(SolidBrush colorBrush = new SolidBrush(color))
					e.Graphics.FillRectangle(colorBrush, colorRectangle);
				e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, colorRectangle);

				//adjust text rectangle to exclude color swatch area
				textRectangle.X += 20;
				textRectangle.Width -= 20;

				//draw text
				Brush textBrush;
				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					textBrush = SystemBrushes.HighlightText;
				else
					textBrush = SystemBrushes.ControlText;
				e.Graphics.DrawString(knownColorName, e.Font, textBrush, textRectangle);
			}
			else if (itemIndex == 0) //custom color
			{
				//draw text
				Rectangle textRectangle = e.Bounds;
				Brush textBrush;
				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					textBrush = SystemBrushes.HighlightText;
				else
					textBrush = SystemBrushes.ControlText;
				e.Graphics.DrawString(comboNamedColor.Items[itemIndex].ToString(), e.Font, textBrush, textRectangle);
			}

			//if item is focused, draw focus rect
			if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
				e.DrawFocusRectangle();
		}


		void comboNamedColorSelectedIndexChanged(object sender, System.EventArgs e)
		{
			int itemIndex = comboNamedColor.SelectedIndex;
			if (itemIndex > 0)
			{
				string knownColorName = comboNamedColor.Items[itemIndex].ToString();
				KnownColor k = (KnownColor)Enum.Parse(typeof(System.Drawing.KnownColor), knownColorName);
				Color color = Color.FromKnownColor(k);
				
				SetFillColor(color);
			}
		}

		private void SetFillColor(Color color)
		{
			sliderPickerR.Value = (int)color.R;
			sliderPickerG.Value = (int)color.G;
			sliderPickerB.Value = (int)color.B;

			//Update floodFiller.FillColor and color preview swatch
			floodFiller.FillColor = color;
			this.labelColorPreview.BackColor = floodFiller.FillColor;

			//cache a brush for use in updating the screen duringa "slow" fill
			if(fillBrush!=null)
				fillBrush.Dispose();
			fillBrush=new SolidBrush(floodFiller.FillColor);

			//If the color is a known color, select that color in the combo. Otherwise, select the "<Custom>" item.
			
			if(color.IsKnownColor)
			{
				comboNamedColor.SelectedIndex=comboNamedColor.Items.IndexOf(color.Name);
				return;
			}

			KnownColor[] Values = (KnownColor[])Enum.GetValues(typeof(System.Drawing.KnownColor));
			Array.Reverse(Values);
			int i = Values.Length;
			foreach (KnownColor val in Values)
			{
				Color kc =Color.FromKnownColor(val);
				if (kc.R == color.R && kc.G==color.G && kc.B==color.B)
				{
					comboNamedColor.SelectedIndex = i;
					return;
				}
				i--;
			}
			comboNamedColor.SelectedIndex = 0;
		}

		void comboFillTypeSelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (comboFillType.SelectedIndex == -1) comboFillType.SelectedIndex = 0;
			if (comboFillType.SelectedIndex == 0)
			{
				if (floodFiller as QueueLinearFloodFiller == null)
					floodFiller = new QueueLinearFloodFiller(floodFiller);
			}
			else
			{
				if (floodFiller as UnsafeQueueLinearFloodFiller == null)
					floodFiller = new UnsafeQueueLinearFloodFiller(floodFiller);
			}
		}
		#endregion

		#region Tolerance Slider Events
		private void sliderPickerTolR_ValueChanged(object sender, EventArgs e)
		{
			floodFiller.Tolerance[0] = (byte)sliderPickerTolR.Value;
		}

		private void sliderPickerTolG_ValueChanged(object sender, EventArgs e)
		{
			floodFiller.Tolerance[1] = (byte)sliderPickerTolG.Value;
		}

		private void sliderPickerTolB_ValueChanged(object sender, EventArgs e)
		{
			floodFiller.Tolerance[2] = (byte)sliderPickerTolB.Value;
		}
		#endregion


		#region Open/Save

		/// <summary>
		/// Lets the user choose a bitmap file to open, and then loads the bitmap file.
		/// </summary>
		void OpenBitmap()
		{
			OpenFileDialog dlg = new OpenFileDialog();

			//get available image format decoders, and create a filter string from them
			ImageCodecInfo[] decoders = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
			string allfiltersname = "All Supported Bitmap Files (";
			string allfilters = "";
			ArrayList filters = new ArrayList();
			for (int i = 0; i <= decoders.GetUpperBound(0); i++)
			{
				allfilters += (i == 0 ? "" : ";") + decoders[i].FilenameExtension;
				filters.Add(decoders[i].CodecName + " (" + decoders[i].FilenameExtension + ")|" + decoders[i].FilenameExtension);
			}
			dlg.Filter = allfiltersname + allfilters + "|" + allfilters + "|" +
					String.Join("|", ((string[])(filters.ToArray(typeof(string)))));


			dlg.Title = "Open a Bitmap";
			dlg.ShowDialog((IWin32Window)this);//

			if (dlg.FileName != "") OpenBitmap(dlg.FileName);
		}

		/// <summary>
		/// Loads the bitmap file specified by <see cref="filename"/>.
		/// </summary>
		/// <param name="filename">The path of the file to open.</param>
		void OpenBitmap(string filename)
		{
			if (filename != "")
			{
				Bitmap b = null;
				try { b = new Bitmap(filename); }
				catch { MessageBox.Show("The bitmap could not be loaded, because there was an error.", "Error Loading Bitmap!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
				if (b != null)
				{

					if (b.PixelFormat == PixelFormat.Format32bppArgb || b.PixelFormat == PixelFormat.Format24bppRgb || b.PixelFormat == PixelFormat.Format8bppIndexed)
					{
						//TODO: Right now only 32bpp is supported. We may also want to allow for other pixel formats.
						floodFiller.Bitmap = new EditableBitmap(b,PixelFormat.Format32bppArgb);
						panel.Image=floodFiller.Bitmap;
						panel.AutoScrollMinSize = new Size(panel.Image.Bitmap.Width, panel.Image.Bitmap.Height);
					}
					else
					{
						MessageBox.Show("The bitmap you selected is the wrong pixel format!  This utility only works with 8-bit indexed/grayscale, 24bpp RGB, and 32bpp ARGB pixel formats.", "Wrong Pixel Format!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					b.Dispose();
				}

			}
		}

		///<summary>Shows Save As dialog, and saves image to a file.</summary>
		void SaveBitmap()
		{
			if (floodFiller.Bitmap == null) return;
			ImageCodecInfo[] encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
			//ImageCodecInfo encoder;

			SaveFileDialog dlg = new SaveFileDialog();

			string allfilters = "";
			ArrayList filters = new ArrayList();
			for (int i = 0; i <= encoders.GetUpperBound(0); i++)
			{
				//if(!((encoders[i].Flags & ImageCodecFlags.SupportBitmap)!=0)) continue;
				allfilters += (i == 0 ? "" : ";") + encoders[i].FilenameExtension;
				filters.Add(encoders[i].FormatDescription + " (" + encoders[i].FilenameExtension + ")|" + encoders[i].FilenameExtension);
			}
			dlg.Filter = /*"All Supported Bitmap Files (" +*/ String.Join("|", ((string[])(filters.ToArray(typeof(string)))));


			dlg.DefaultExt = "*.bmp";
			dlg.OverwritePrompt = true;
			dlg.Title = "Save a Bitmap";
			dlg.ShowDialog((IWin32Window)this);
			if (dlg.FileName == "") return; //empty filename means cancel.

			//TODO: Support encoders and encoder params
			SaveBitmap(dlg.FileName);
		}


		/// <summary>
		/// Saves the currently loaded bitmap file to the file name specified by <see cref="filename"/>.
		/// </summary>
		/// <param name="filename">The path of the file to save the bitmap to.</param>
		void SaveBitmap(string filename)
		{
			floodFiller.Bitmap.Bitmap.Save(filename);
		}

		#endregion

		/// <summary>
		/// Updates the flood filler's FillColor property from the values of the color picker sliders,
		/// and then updates the color swatch and color combo box based on the newly updated FillColor.
		/// </summary>
		/*void ChangeFill()
		{
			//Update floodFiller.FillColor and color preview swatch
			floodFiller.FillColor = Color.FromArgb((byte)sliderPickerR.Value, (byte)sliderPickerG.Value, (byte)sliderPickerB.Value);
			this.labelColorPreview.BackColor = floodFiller.FillColor;

			//cache a brush for use in updating the screen duringa "slow" fill
			if(fillBrush!=null)
				fillBrush.Dispose();
			fillBrush=new SolidBrush(floodFiller.FillColor);

			//If the color is a known color, select that color in the combo. Otherwise, select the "<Custom>" item.
			Color color = floodFiller.FillColor;
			
			if(comboNamedColor.SelectedIndex>0)
			{
				Color namedColor=Color.FromKnownColor((KnownColor)Enum.Parse(typeof(KnownColor),comboNamedColor.SelectedItem.ToString()));
				if(color.R==namedColor.R && color.G==namedColor.G && color.B==namedColor.B)
					return;
			}

			KnownColor[] Values = (KnownColor[])Enum.GetValues(typeof(System.Drawing.KnownColor));
			Array.Reverse(Values);
			int i = Values.Length;
			foreach (KnownColor val in Values)
			{
				Color kc =Color.FromKnownColor(val);
				if (kc.R == color.R && kc.G==color.G && kc.B==color.B)
				{
					comboNamedColor.SelectedIndex = i;
					return;
				}
				i--;
			}
			comboNamedColor.SelectedIndex = 0;
		}*/

		/// <summary>
		/// Updates the specified point on the screen.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void UpdateScreen(ref int x, ref int y)
		{
			panel.SetPixel(fillBrush, x+panel.AutoScrollPosition.X,y+panel.AutoScrollPosition.Y);
			Application.DoEvents();
		}

	}

	
	[global::System.Serializable]
	public class FillOperationAbortedException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//	http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//	http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public FillOperationAbortedException() { }
		public FillOperationAbortedException(string message) : base(message) { }
		public FillOperationAbortedException(string message, Exception inner) : base(message, inner) { }
		protected FillOperationAbortedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}