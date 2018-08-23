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

		public ImageBox Image_Box_Right = new ImageBox();
		public My_ImageBox Image_Box_Left = new My_ImageBox();
		static FlowLayoutPanel Layout_Panel = new FlowLayoutPanel();

		public Create_Form_and_ImageBox() {

			Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Form.WindowState = FormWindowState.Maximized;

			Layout_Panel.WrapContents = true;
			Layout_Panel.FlowDirection = FlowDirection.LeftToRight;
			Layout_Panel.Dock = DockStyle.Fill;

			PictureBox_Resize(this, null);

			Image_Box_Right.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_Left.SizeMode = PictureBoxSizeMode.Zoom;

			Image_Box_Right.MouseDoubleClick += new MouseEventHandler(PictureBox_Zoom);
			Image_Box_Left.MouseDoubleClick += new MouseEventHandler(PictureBox_Zoom);

			Layout_Panel.Controls.Add(Image_Box_Right);
			Layout_Panel.Controls.Add(Image_Box_Left);

			Form.Controls.Add(Layout_Panel);
		}

		public void Form_ShowDialog() { Form.ShowDialog(); }

		void PictureBox_Resize(object sender, MouseEventArgs e) {
			if (sender == null) { return; }

			Layout_Panel.Size = Form.Size;

			Image_Box_Right.Height = Form.Height - 50;
			Image_Box_Right.Width = Form.Width / 2 - 10;

			Image_Box_Left.Size = Image_Box_Right.Size;
		}

		void PictureBox_Zoom(object sender, MouseEventArgs e) {
			if (sender == null) { return; }

			if (Image_Box_Right.SizeMode == PictureBoxSizeMode.Zoom) {
				Image_Box_Right.SizeMode = PictureBoxSizeMode.CenterImage;
				Image_Box_Left.SizeMode = PictureBoxSizeMode.CenterImage;
			} else {
				Image_Box_Right.SizeMode = PictureBoxSizeMode.Zoom;
				Image_Box_Left.SizeMode = PictureBoxSizeMode.Zoom;
			}
		}
	}
}
