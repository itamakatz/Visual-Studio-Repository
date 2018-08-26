using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Using_Emgu {

	class Create_Form_and_ImageBox {

		Form Form = new Form();

		public My_ImageBox Image_Box_Left = new My_ImageBox();
		public My_ImageBox Image_Box_Right = new My_ImageBox();
		public FlowLayoutPanel Layout_Panel = new FlowLayoutPanel();

		public Panel Panel_Divider = new Panel();
		public Panel Panel_Image_Left = new Panel();
		public Panel Panel_Image_Right = new Panel();

		public Label Label_Left = new Label();
		public Label Label_Right = new Label();

		public TextBox Output_TextBox = new TextBox();

		Point Last_Mouse_Location;
		int Im_Zoom_Height;
		int MIDDLE_PANEL_WIDTH = 10;

		public Create_Form_and_ImageBox() {

			Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Form.WindowState = FormWindowState.Maximized;

			Picture_Box_Resize_To_Form(this, null);

			Image_Box_Left.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_Right.SizeMode = PictureBoxSizeMode.Zoom;

			Image_Box_Left.MouseDoubleClick += new MouseEventHandler(Picture_Box_MouseDoubleClick);
			Image_Box_Left.MouseDown += new MouseEventHandler(Picture_Box_MouseDown);
			Image_Box_Left.MouseMove += new MouseEventHandler(Picture_Box_MouseMoove);
			Image_Box_Left.MouseWheel += new MouseEventHandler(Picture_Box_MouseWheel);

			Image_Box_Right.MouseDoubleClick += new MouseEventHandler(Picture_Box_MouseDoubleClick);
			Image_Box_Right.MouseDown += new MouseEventHandler(Picture_Box_MouseDown);
			Image_Box_Right.MouseMove += new MouseEventHandler(Picture_Box_MouseMoove);
			Image_Box_Right.MouseWheel += new MouseEventHandler(Picture_Box_MouseWheel);

			Panel_Image_Left.MouseDoubleClick += new MouseEventHandler(Label_MouseDoubleClick);
			Panel_Image_Right.MouseDoubleClick += new MouseEventHandler(Label_MouseDoubleClick);

			Panel_Image_Left.Controls.Add(Image_Box_Left);
			Panel_Image_Right.Controls.Add(Image_Box_Right);
			Form.Controls.Add(Panel_Image_Left);
			Form.Controls.Add(Panel_Image_Right);

			Form.Controls.Add(Panel_Divider);
			Form.Controls.Add(Output_TextBox);

			Output_TextBox.BorderStyle = BorderStyle.Fixed3D;
			Output_TextBox.Font = new Font("Arial", 10, FontStyle.Bold);
			Output_TextBox.Dock = DockStyle.Bottom;
			Output_TextBox.Height = 50;
			Output_TextBox.Multiline = true;
			Output_TextBox.WordWrap = true;
			Output_TextBox.ScrollBars = ScrollBars.Both;
			Output_TextBox.TextAlign = HorizontalAlignment.Left;
			Output_TextBox.ReadOnly = true;
			Output_TextBox.BringToFront();
		}

		public void Form_ShowDialog() {
			Form.ShowDialog();
		}

		public void Set_Left_Label(string label_str) {
			Label_Left.Text = label_str;
			Label_Left.TextAlign = ContentAlignment.MiddleCenter;
			Label_Left.Font = new Font(Label_Left.Font, FontStyle.Bold);
			Panel_Image_Left.Controls.Add(Label_Left);
			Label_Left.Dock = DockStyle.Top;
			Label_Left.BringToFront();
		}

		public void Set_Right_Label(string label_str) {
			Label_Right.Text = label_str;
			Label_Right.TextAlign = ContentAlignment.MiddleCenter;
			Label_Right.Font = new Font(Label_Right.Font, FontStyle.Bold);
			Panel_Image_Right.Controls.Add(Label_Right);
			Label_Right.Dock = DockStyle.Top;
			Label_Right.BringToFront();
		}

		public void Append_Output_TextBox(string label_str) {
			Output_TextBox.AppendText("\r\n" + label_str);
		}

		public void Append_Output_TextBox_Same_Line(string label_str) {
			Output_TextBox.AppendText(label_str);
		}

		void Picture_Box_Resize_To_Form(object sender, MouseEventArgs e) {
			if (sender == null) { return; }

			Panel_Divider.Height = Form.Height;
			Panel_Divider.Width = MIDDLE_PANEL_WIDTH;
			Panel_Divider.BorderStyle = BorderStyle.Fixed3D;
			Panel_Divider.Location = new Point((Form.Width / 2) - (MIDDLE_PANEL_WIDTH / 2), 0);

			Panel_Image_Left.Height = Form.Height;
			Panel_Image_Left.Width = Form.Width / 2;

			Panel_Image_Right.Size = Panel_Image_Left.Size;
			Image_Box_Left.Size = Panel_Image_Left.Size;
			Image_Box_Right.Size = Panel_Image_Right.Size;

			Panel_Image_Left.Location = new Point(0, 0);
			Panel_Image_Right.Location = new Point((Form.Width / 2) + (MIDDLE_PANEL_WIDTH / 2), 0);
			Image_Box_Left.Location = new Point(0, 0);
			Image_Box_Right.Location = new Point(0, 0);

			Im_Zoom_Height = Form.Height;
		}

		void Label_MouseDoubleClick(object sender, MouseEventArgs e) {
			Picture_Box_MouseDoubleClick(sender, e);
		}

		void Picture_Box_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (sender == null) { return; }

			Picture_Box_Resize_To_Form(this, null);

			Image_Box_Left.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_Right.SizeMode = PictureBoxSizeMode.Zoom;
		}

		private void Picture_Box_MouseDown(object sender, MouseEventArgs e) {
			Image_Box_Left.Size = new Size(Im_Zoom_Height * Image_Box_Left.Image.Size.Width / Image_Box_Left.Image.Size.Height, Im_Zoom_Height);
			Image_Box_Right.Size = Image_Box_Left.Size;

			MouseEventArgs mouse = e as MouseEventArgs;
			if (mouse.Button == MouseButtons.Left) { Last_Mouse_Location = mouse.Location; }
		}

		private void Picture_Box_MouseMoove(object sender, MouseEventArgs mouse) {

			if (mouse.Button == MouseButtons.Left) {
				Point mousePosNow = mouse.Location;

				int deltaX = mousePosNow.X - Last_Mouse_Location.X;
				int deltaY = mousePosNow.Y - Last_Mouse_Location.Y;

				int newX_Left = Image_Box_Left.Location.X + deltaX;
				int newY_Left = Image_Box_Left.Location.Y + deltaY;

				Image_Box_Left.Location = new Point(newX_Left, newY_Left);

				int newX_Right = Image_Box_Right.Location.X + deltaX;
				int newY_Right = Image_Box_Right.Location.Y + deltaY;

				Image_Box_Right.Location = new Point(newX_Right, newY_Right);
			}
		}

		private void Picture_Box_MouseWheel(object sender, MouseEventArgs e) {
			if (Im_Zoom_Height + e.Delta > 0) {
				Im_Zoom_Height += e.Delta;
				Image_Box_Left.Size = new Size(Im_Zoom_Height * Image_Box_Left.Image.Size.Width / Image_Box_Left.Image.Size.Height, Im_Zoom_Height);
				Image_Box_Right.Size = Image_Box_Left.Size;
			}
		}
	}
}