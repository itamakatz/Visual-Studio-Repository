using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;

namespace Using_Emgu {
	class Program {

		static Form form = new Form();
		static OpenFileDialog Openfile = new OpenFileDialog();
		static Image<Bgr, Byte> My_Image;

		static PictureBox pictureBox_UL = new PictureBox();
		static PictureBox pictureBox_UR = new PictureBox();

		static PictureBox pictureBox_DL = new PictureBox();
		static PictureBox pictureBox_DR = new PictureBox();

		[STAThread]
		static void Main(string[] args) {

			Init_Controls();

			Open_Image();

			form.ShowDialog();
		}

		static void Open_Image() {
			Openfile.ShowDialog();
			My_Image = new Image<Bgr, byte>(Openfile.FileName);

			pictureBox_UL.Image = My_Image.ToBitmap();

			pictureBox_UR.Image = My_Image.SmoothGaussian(21).ToBitmap();
			My_Image.Canny(5, 5);
			pictureBox_UR.Image = My_Image.SmoothGaussian(21).ToBitmap();
		}

		static void Init_Controls() {

			form.Width = 800;
			form.Height = 800;

			form.Controls.Add(pictureBox_UL);
			form.Controls.Add(pictureBox_UR);
			form.Controls.Add(pictureBox_DL);
			form.Controls.Add(pictureBox_DR);

			form.SizeChanged += PictureBox_Resize;

			pictureBox_UR.DoubleClick += PictureBox_DoubleClick;
			pictureBox_UL.DoubleClick += PictureBox_DoubleClick;

			pictureBox_UL.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox_UR.SizeMode = PictureBoxSizeMode.Zoom;
		}

		static void PictureBox_Resize(object sender, EventArgs e) {
			if(pictureBox_UL == null) { return; }

			int offset = 0;

			if (form.Width % 2 != 0) { offset = 1; }

			pictureBox_UL.Height = form.Height / 2 - 1;
			pictureBox_UR.Height = form.Height / 2 - 1;
			pictureBox_DL.Height = form.Height / 2 - 1;
			pictureBox_DR.Height = form.Height / 2 - 1;

			pictureBox_UL.Width = form.Width / 2 - 1;
			pictureBox_UR.Width = form.Width / 2 - 1;
			pictureBox_DL.Width = form.Width / 2 - 1;
			pictureBox_DR.Width = form.Width / 2 - 1;

			pictureBox_UL.Location = new Point(form.Width / 2, 0);
			pictureBox_UR.Location = new Point(form.Width / 2, 0);
			pictureBox_DL.Location = new Point(form.Width / 2, 0);
			pictureBox_DR.Location = new Point(form.Width / 2, 0);

			pictureBox_UR.Location = new Point((int) Math.Ceiling((double) form.Width / 2) + 1, 0);


			pictureBox_UL.Width = (int) Math.Floor((double) form.Width / 2);

			if (form.Width % 2 == 0) {
				pictureBox_UR.Width = (int) Math.Ceiling((double) form.Width / 2) + 1;
			} else {
				pictureBox_UR.Width = (int) Math.Ceiling((double) form.Width / 2);
				pictureBox_UR.Location = new Point((int) Math.Ceiling((double) form.Width / 2), 0);
			}
		}

		static void PictureBox_DoubleClick(object sender, EventArgs e) {

			if (pictureBox_UL.SizeMode == PictureBoxSizeMode.Zoom) {
				pictureBox_UL.SizeMode = PictureBoxSizeMode.CenterImage;
				pictureBox_UR.SizeMode = PictureBoxSizeMode.CenterImage;
			} else {
				pictureBox_UL.SizeMode = PictureBoxSizeMode.Zoom;
				pictureBox_UR.SizeMode = PictureBoxSizeMode.Zoom;
			}
		}

		//DataGridView myNewGrid = new DataGridView();

		//pictureBox_Left.MouseMove += new MouseEventHandler(pb_MouseMove);
		//pictureBox_Right.MouseMove += new MouseEventHandler(pb_MouseMove);

		//static void pb_MouseMove(object sender, MouseEventArgs e) {
		//	if (e.Button == MouseButtons.Left && pictureBox_Left.SizeMode == PictureBoxSizeMode.CenterImage) {
		//		pictureBox_Left.Left = e.X;
		//		pictureBox_Left.Top = e.Y;
		//	}
		//}
	}
}
