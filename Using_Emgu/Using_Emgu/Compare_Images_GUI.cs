using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Using_Emgu {

	class Compare_Images_GUI {

		public Form Form = new Form();
		public Emgu_Image_Panel emgu_Image_Panel_Right;
		public Emgu_Image_Panel emgu_Image_Panel_Left;

		public Panel Panel_Divider = new Panel();

		public TextBox Status_Update_TextBox = new TextBox();

		private Point Last_Mouse_Location;
		private int Im_Zoom_Height;
		readonly int MIDDLE_PANEL_WIDTH = 10;
		readonly int STATUS_TEXTBOX_HEIGHT = 50;

		public Compare_Images_GUI() {

			Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Form.WindowState = FormWindowState.Maximized;

			emgu_Image_Panel_Right = new Emgu_Image_Panel(new Size(Form.Height, Form.Width / 2), 
														new Action<object, MouseEventArgs>[] {
														Event_Picture_Box_MouseDoubleClick,
														Event_Picture_Box_MouseDown,
														Event_Picture_Box_MouseMoove,
														Event_Picture_Box_MouseWheel, }, 
														Event_Label_MouseDoubleClick) ;

			emgu_Image_Panel_Left = new Emgu_Image_Panel(new Size(Form.Height, Form.Width / 2),
														new Action<object, MouseEventArgs>[] {
														Event_Picture_Box_MouseDoubleClick,
														Event_Picture_Box_MouseDown,
														Event_Picture_Box_MouseMoove,
														Event_Picture_Box_MouseWheel, },
														Event_Label_MouseDoubleClick);

			Size_and_Location_Initialization();

			Attach_Events();

			Adding_Controls();

			Init_Status_Update_TextBox();

			Set_Z_Order();
		}

		// USER INPUT METHODS //

		public void Form_ShowDialog() { Form.ShowDialog();}

		public void Append_Output_TextBox(string label_str) {
			Status_Update_TextBox.AppendText("\r\n" + label_str);
		}

		public void Append_Output_TextBox_Same_Line(string label_str) {
			Status_Update_TextBox.AppendText(label_str);
		}

		// INITIALIZATION METHODS //

		private void Attach_Events() { Form.SizeChanged += Form_Size_Changed; }

		private void Adding_Controls() {

			Form.Controls.Add(emgu_Image_Panel_Left.Im_Panel);
			Form.Controls.Add(emgu_Image_Panel_Right.Im_Panel);

			Form.Controls.Add(Panel_Divider);
			Form.Controls.Add(Status_Update_TextBox);
		}

		private void Init_Status_Update_TextBox() {
			Status_Update_TextBox.BorderStyle = BorderStyle.Fixed3D;
			Status_Update_TextBox.Font = new Font("Arial", 10, FontStyle.Bold);
			Status_Update_TextBox.Dock = DockStyle.Bottom;
			Status_Update_TextBox.Height = STATUS_TEXTBOX_HEIGHT;
			Status_Update_TextBox.Multiline = true;
			Status_Update_TextBox.WordWrap = true;
			Status_Update_TextBox.ScrollBars = ScrollBars.Both;
			Status_Update_TextBox.TextAlign = HorizontalAlignment.Left;
			Status_Update_TextBox.ReadOnly = true;
		}

		private void Set_Z_Order() { Status_Update_TextBox.BringToFront(); }

		private void Size_and_Location_Initialization() {
			Form_Size_Changed(this, null);
			Event_Picture_Box_MouseDoubleClick(this, null);
		}

		// UPDATING METHODS //

		
		private void Form_Size_Changed(object sender, EventArgs e) {

			Panel_Divider.Height = Form.Height;
			Panel_Divider.Width = MIDDLE_PANEL_WIDTH;
			Panel_Divider.BorderStyle = BorderStyle.Fixed3D;
			Panel_Divider.Location = new Point((Form.Width / 2) - (MIDDLE_PANEL_WIDTH / 2), 0);

			emgu_Image_Panel_Left.Set_Panel_Size(new Size(Form.Width / 2, Form.Height));
			emgu_Image_Panel_Right.Set_Panel_Size(new Size(Form.Width / 2, Form.Height));

			emgu_Image_Panel_Left.Set_Panel_Location(new Point(0, 0));
			emgu_Image_Panel_Right.Set_Panel_Location(new Point(Form.Width / 2, 0));

			Form.Refresh();
		}

		private void Event_Picture_Box_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (sender == null) { return; }

			emgu_Image_Panel_Right.Reset_Coordinates();
			emgu_Image_Panel_Left.Reset_Coordinates();

			Im_Zoom_Height = Form.Height;

			Form.Refresh();
		}

		private void Event_Label_MouseDoubleClick(object sender, MouseEventArgs e) {
			Event_Picture_Box_MouseDoubleClick(sender, e);
			Form.Refresh();
		}

		private void Event_Picture_Box_MouseDown(object sender, MouseEventArgs mouse) {
			if (mouse.Button == MouseButtons.Left) { Last_Mouse_Location = mouse.Location; }
			Form.Refresh();
		}

		private void Event_Picture_Box_MouseMoove(object sender, MouseEventArgs mouse) {

			if (mouse.Button == MouseButtons.Left) {
				Point mousePosNow = mouse.Location;

				int deltaX = mousePosNow.X - Last_Mouse_Location.X;
				int deltaY = mousePosNow.Y - Last_Mouse_Location.Y;

				emgu_Image_Panel_Right.Update_Location(deltaX, deltaY);
				emgu_Image_Panel_Left.Update_Location(deltaX, deltaY);
				Form.Refresh();
			}
		}

		private void Event_Picture_Box_MouseWheel(object sender, MouseEventArgs e) {
			if (Im_Zoom_Height + e.Delta > 0) {
				Im_Zoom_Height += e.Delta;

				Size size = emgu_Image_Panel_Right.Emgu_Im_Box.Size;

				emgu_Image_Panel_Right.Update_Size(new Size(Im_Zoom_Height * size.Width /
												size.Height, Im_Zoom_Height));
				emgu_Image_Panel_Left.Update_Size(new Size(Im_Zoom_Height * size.Width /
												size.Height, Im_Zoom_Height));
				Form.Refresh();
			}
		}
	}
}