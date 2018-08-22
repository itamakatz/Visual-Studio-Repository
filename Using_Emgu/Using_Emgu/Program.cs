using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;

namespace Using_Emgu {
	class Program {

		static Form Compare_Form = new Form();
		static OpenFileDialog Open_File = new OpenFileDialog();
		static SaveFileDialog Save_File = new SaveFileDialog();
		static Image<Bgr, Byte> My_Image;

		static ImageBox Image_Box_UL = new ImageBox();
		static ImageBox Image_Box_UR = new ImageBox();
		static ImageBox Image_Box_DL = new ImageBox();
		static ImageBox Image_Box_DR = new ImageBox();

		static Form Pano_Form = new Form();
		static Image<Bgr, Byte> Pano_Image;
		static ImageBox Pano_Image_Box = new ImageBox();
		//static PictureBox Pano_Image_Box = new PictureBox();

		[STAThread]
		static void Main(string[] args) {

			Init_Controls();

			//Create_Pano();

			Compare_Images();

			Compare_Form.ShowDialog();
			//Pano_Form.ShowDialog();
		}

		static void Compare_Images() {
			My_Image = new Image<Bgr, byte>(@"C:\Users\admin\Desktop\COM_Integration\Panorama Stiching\Image Sets\pano_calibrate_3.jpg");

			Image_Box_UL.Image = My_Image;
			Image_Box_UR.Image = My_Image.Canny(100, 200);
			//Image_Box_DL.Image = My_Image.Canny(255, 255 / 4);
			Image_Box_DL.Image = My_Image.Canny(255, 200);
			Image_Box_DR.Image = My_Image.Laplace(1);
			//Image_Box_UR.Image = My_Image.SmoothGaussian(21).ToBitmap();
			//My_Image.Canny(5, 5);
			//Image_Box_UR.Image = My_Image.SmoothGaussian(21).ToBitmap();
		}

		static void Create_Pano() {
			using (Stitcher stitcher = new Stitcher(Stitcher.Mode.Panorama, false)) {

				using (VectorOfMat vm = new VectorOfMat()) {

					Open_File.Multiselect = true;
					Open_File.ShowDialog();

					foreach (string fileName in Open_File.FileNames) {
						vm.Push(new Mat(fileName));
					}

					Mat result = new Mat();
					stitcher.Stitch(vm, result);
					Pano_Image_Box.Image = result;
				}
			}
		}

		static void Init_Controls() {

			// Pano_Form //

			Pano_Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Pano_Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Pano_Form.Controls.Add(Pano_Image_Box);

			Pano_Image_Box.SizeMode = PictureBoxSizeMode.Zoom;
			Pano_Image_Box.Size = Pano_Form.Size;
			Pano_Image_Box.MouseClick += new MouseEventHandler(Pano_MouseMove);

			// Compare_Form //

			Compare_Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Compare_Form.Width = Screen.PrimaryScreen.Bounds.Width;

			Compare_Form.SizeChanged += PictureBox_Resize;

			PictureBox_Resize(null, null);

			Image_Box_UR.DoubleClick += PictureBox_DoubleClick;
			Image_Box_UL.DoubleClick += PictureBox_DoubleClick;
			Image_Box_DL.DoubleClick += PictureBox_DoubleClick;
			Image_Box_DR.DoubleClick += PictureBox_DoubleClick;

			Compare_Form.Controls.Add(Image_Box_UL);
			Compare_Form.Controls.Add(Image_Box_UR);
			Compare_Form.Controls.Add(Image_Box_DL);
			Compare_Form.Controls.Add(Image_Box_DR);

			Image_Box_UL.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_UR.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_DL.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_DR.SizeMode = PictureBoxSizeMode.Zoom;
		}

		static void PictureBox_Resize(object sender, EventArgs e) {
			if (Image_Box_UL == null) { return; }

			Image_Box_UL.Height = Compare_Form.Height / 2 - 15;
			Image_Box_UR.Height = Compare_Form.Height / 2 - 15;
			Image_Box_DL.Height = Compare_Form.Height / 2 - 15;
			Image_Box_DR.Height = Compare_Form.Height / 2 - 15;

			Image_Box_UL.Width = Compare_Form.Width / 2 - 10;
			Image_Box_UR.Width = Compare_Form.Width / 2 - 10;
			Image_Box_DL.Width = Compare_Form.Width / 2 - 10;
			Image_Box_DR.Width = Compare_Form.Width / 2 - 10;

			Image_Box_UL.Location = new Point(0, 0);
			Image_Box_UR.Location = new Point(Compare_Form.Width / 2 - 5, 0);
			Image_Box_DL.Location = new Point(0, Compare_Form.Height / 2 - 10);
			Image_Box_DR.Location = new Point(Compare_Form.Width / 2 - 5, Compare_Form.Height / 2 - 10);
		}

		static void PictureBox_DoubleClick(object sender, EventArgs e) {

			if (Image_Box_UL.SizeMode == PictureBoxSizeMode.Zoom) {
				Image_Box_UL.SizeMode = PictureBoxSizeMode.CenterImage;
				Image_Box_UR.SizeMode = PictureBoxSizeMode.CenterImage;
				Image_Box_DL.SizeMode = PictureBoxSizeMode.CenterImage;
				Image_Box_DR.SizeMode = PictureBoxSizeMode.CenterImage;
			} else {
				Image_Box_UL.SizeMode = PictureBoxSizeMode.Zoom;
				Image_Box_UR.SizeMode = PictureBoxSizeMode.Zoom;
				Image_Box_DL.SizeMode = PictureBoxSizeMode.Zoom;
				Image_Box_DR.SizeMode = PictureBoxSizeMode.Zoom;
			}
		}

		//DataGridView myNewGrid = new DataGridView();

		//Image_Box_Left.MouseMove += new MouseEventHandler(pb_MouseMove);
		//Image_Box_Right.MouseMove += new MouseEventHandler(pb_MouseMove);

		//static void pb_MouseMove(object sender, MouseEventArgs e) {
		//	if (e.Button == MouseButtons.Left && Image_Box_Left.SizeMode == PictureBoxSizeMode.CenterImage) {
		//		Image_Box_Left.Left = e.X;
		//		Image_Box_Left.Top = e.Y;
		//	}
		//}

		static void Pano_MouseMove(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Create_Pano();
			}
		}
	}
}
