using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		static FlowLayoutPanel layoutPanel = new FlowLayoutPanel();
		


		static Form Pano_Form = new Form();
		static Image<Bgr, Byte> Pano_Image;
		static ImageBox Pano_Image_Box = new ImageBox();
		//static PictureBox Pano_Image_Box = new PictureBox();

		static Create_Form_and_ImageBox My_Form = new Create_Form_and_ImageBox();

		[STAThread]
		static void Main(string[] args) {

			Init_Controls();

			//Create_Pano();
			//Pano_Form.ShowDialog();

			Compare_Images();
			My_Form.Form_ShowDialog();
			Compare_Form.ShowDialog();
		}

		static void Compare_Images() {
			My_Image = new Image<Bgr, byte>(@"C:\Users\admin\Desktop\COM_Integration\Panorama Stiching\Image Sets\pano_calibrate_3.jpg");

			//My_Form.Image_Box_Left.Image = My_Image;
			Image<Gray, byte> Canny_Image = My_Image.Canny(255, 200);

			My_Form.Image_Box_Right.Image = Canny_Image;

			My_Form.Image_Box_Left.Image = My_Image.Canny(255, 200).Canny(255, 200);

			My_Form.Image_Box_Left.Image.MinMax(out double[] minValues, out double[] maxValues, out Point[] minLocations, out Point[] maxLocations);

			// ======

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
			//using (Stitcher stitcher = new Stitcher(Stitcher.Mode.Panorama, false)) {

			//	using (VectorOfMat vm = new VectorOfMat()) {

			//		Open_File.Multiselect = true;
			//		Open_File.ShowDialog();

			//		foreach (string fileName in Open_File.FileNames) {
			//			vm.Push(new Mat(fileName));
			//		}

			//		Mat result = new Mat();
			//		stitcher.Stitch(vm, result);
			//		Pano_Image_Box.Image = result;
			//	}
			//}
			//only use GPU if you have build the native binary from code and enabled "NON_FREE"
			using (Stitcher stitcher = new Stitcher(false)) {
				using (AKAZEFeaturesFinder finder = new AKAZEFeaturesFinder()) {

					stitcher.SetFeaturesFinder(finder);

					using (VectorOfMat vm = new VectorOfMat()) {

						Open_File.Multiselect = true;
						Open_File.ShowDialog();

						foreach (string fileName in Open_File.FileNames) {
							vm.Push(new Mat(fileName));
						}

						Mat result = new Mat();

						Stopwatch watch = Stopwatch.StartNew();

						Console.WriteLine("Stitching");
						Stitcher.Status stitchStatus = stitcher.Stitch(vm, result);
						watch.Stop();

						if (stitchStatus == Stitcher.Status.Ok) {
							Pano_Image_Box.Image = result;
							Console.WriteLine(String.Format("Stitched in {0} milliseconds.", watch.ElapsedMilliseconds));
						} else {
							MessageBox.Show(String.Format("Stiching Error: {0}", stitchStatus));
							Pano_Image_Box.Image = null;
						}
					}
				}
			}
		}

		static void Init_Controls() {

			// Pano_Form //

			Pano_Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Pano_Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Compare_Form.WindowState = FormWindowState.Maximized;
			Pano_Form.Controls.Add(Pano_Image_Box);

			Pano_Image_Box.SizeMode = PictureBoxSizeMode.Zoom;
			Pano_Image_Box.Size = Pano_Form.Size;
			Pano_Image_Box.MouseClick += new MouseEventHandler(Pano_MouseMove);
			// Compare_Form //

			Compare_Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Compare_Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Compare_Form.WindowState = FormWindowState.Maximized;

			Compare_Form.SizeChanged += PictureBox_Resize;

			layoutPanel.WrapContents = true;
			layoutPanel.FlowDirection = FlowDirection.LeftToRight;
			layoutPanel.Dock = DockStyle.Fill;

			PictureBox_Resize(null, null);

			Image_Box_UR.DoubleClick += PictureBox_DoubleClick;
			Image_Box_UL.DoubleClick += PictureBox_DoubleClick;
			Image_Box_DL.DoubleClick += PictureBox_DoubleClick;
			Image_Box_DR.DoubleClick += PictureBox_DoubleClick;

			Image_Box_UL.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_UR.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_DL.SizeMode = PictureBoxSizeMode.Zoom;
			Image_Box_DR.SizeMode = PictureBoxSizeMode.Zoom;

			layoutPanel.Controls.Add(Image_Box_UL);
			layoutPanel.Controls.Add(Image_Box_UR);
			layoutPanel.Controls.Add(Image_Box_DL);
			layoutPanel.Controls.Add(Image_Box_DR);

			Compare_Form.Controls.Add(layoutPanel);
		}

		static void PictureBox_Resize(object sender, EventArgs e) {
			if (Image_Box_UL == null) { return; }

			layoutPanel.Size = Compare_Form.Size;

			Image_Box_UL.Height = Compare_Form.Height - 50;
			Image_Box_UL.Width = Compare_Form.Width / 4 - 10;

			Image_Box_UR.Size = Image_Box_UL.Size;
			Image_Box_DL.Size = Image_Box_UL.Size;
			Image_Box_DR.Size = Image_Box_UL.Size;
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

		static void Pano_MouseMove(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Create_Pano();
				Pano_Form.Refresh();
			}
		}
	}
}
