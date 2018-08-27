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
	class Program : Form {

		public Form Compare_Form = new Form();
		public OpenFileDialog Open_File = new OpenFileDialog();
		Image<Bgr, Byte> My_Image;

		public Form Pano_Form = new Form();
		Image<Bgr, Byte> Pano_Image;
		ImageBox Pano_Image_Box = new ImageBox();

		public Compare_Images_GUI My_Form = new Compare_Images_GUI();
		public Compare_Images_GUI My_Form_2 = new Compare_Images_GUI();

		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MultiFormContext(new Program(), new Program()));
		}

		public Program() {

			//Init_Pano();
			//Create_Pano();
			//Pano_Form.ShowDialog();

			Compare_Images();
			//My_Form.Form_ShowDialog();
			//My_Form_2.Form.Show();
			//My_Form.Form.ShowDialog();
			//Console.ReadKey();
			//Compare_Form.ShowDialog();
		}

		void Compare_Images() {
			My_Image = new Image<Bgr, byte>(@"C:\Users\admin\Desktop\COM_Integration\Panorama Stiching\Image Sets\pano_calibrate_3.jpg");

			//My_Form.Image_Box_Left.Image = My_Image;
			Image<Gray, byte> Canny_Image = My_Image.Canny(255, 200);

			My_Form.emgu_Image_Panel_Right.Emgu_Im_Box.Image = Canny_Image;
			My_Form.emgu_Image_Panel_Right.Im_Label.Text = "Canny_Image";

			My_Form.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Canny(255, 200).Canny(255, 200);
			My_Form.emgu_Image_Panel_Left.Im_Label.Text = "My_Image.Canny(255, 200).Canny(255, 200)";

			My_Form.emgu_Image_Panel_Left.Emgu_Im_Box.Image.MinMax(
				out double[] minValues, 
				out double[] maxValues, 
				out Point[] minLocations, 
				out Point[] maxLocations);

			//My_Form.Im_Box_Right.Image = Canny_Image;
			//My_Form.Label_Right.Text = "Canny_Image";

			//My_Form.Im_Box_Left.Image = My_Image.Canny(255, 200).Canny(255, 200);
			//My_Form.Label_Left.Text = "My_Image.Canny(255, 200).Canny(255, 200)";

			//My_Form.Im_Box_Left.Image.MinMax(out double[] minValues, out double[] maxValues, out Point[] minLocations, out Point[] maxLocations);

			My_Form.Append_Output_TextBox_Same_Line("minValues: ");
			foreach (var item in minValues) {
				My_Form.Append_Output_TextBox_Same_Line(item + ", ");
			}
			My_Form.Append_Output_TextBox_Same_Line("maxValues: ");
			foreach (var item in maxValues) {
				My_Form.Append_Output_TextBox_Same_Line(item + ", ");
			}
			My_Form.Append_Output_TextBox_Same_Line("minLocations: ");
			foreach (var item in minLocations) {
				My_Form.Append_Output_TextBox_Same_Line("X: "+ item.X + ", Y: "+ item.Y + ", ");
			}
			My_Form.Append_Output_TextBox_Same_Line("maxLocations: ");
			foreach (var item in maxLocations) {
				My_Form.Append_Output_TextBox_Same_Line("X: "+ item.X + ", Y: "+ item.Y + ", ");
			}


			Single[,] Edge_Detection_Kernel = new Single[,] { { -1, -1, -1 }, { -1,  8, -1 }, { -1, -1, -1 } };
			Single[,] Sharpen_Kernel = new Single[,] { { 0, -1, 0 }, { -1,  5, -1 }, { 0, -1, 0 } };
			//Single[,] Blur_Kernel = new Single[,] { { 1/9, 1/9, 1/9 }, { 1 / 9, 1 / 9, 1 / 9 }, { 1/9, 1/9, 1/9 } };
			Single[,] Blur_Kernel = new Single[,] { { 1,1,1 }, { 1,1,1 }, { 1,1,1 } };

			ConvolutionKernelF convolutionKernelF_Edge_Detection = new ConvolutionKernelF(Edge_Detection_Kernel);
			ConvolutionKernelF convolutionKernelF_Sharpening = new ConvolutionKernelF(Edge_Detection_Kernel);
			ConvolutionKernelF convolutionKernelF_Blur = new ConvolutionKernelF(Blur_Kernel);

			My_Form_2.emgu_Image_Panel_Right.Emgu_Im_Box.Image = My_Image.Convolution(convolutionKernelF_Edge_Detection);
			My_Form_2.emgu_Image_Panel_Right.Im_Label.Text = "convolutionKernelF_Edge_Detection";

			//My_Form_2.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Convolution(convolutionKernelF_Sharpening);
			//My_Form_2.emgu_Image_Panel_Left.Im_Label.Text = "convolutionKernelF_Sharpening";

			My_Form_2.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Convolution(convolutionKernelF_Blur);
			My_Form_2.emgu_Image_Panel_Left.Im_Label.Text = "convolutionKernelF_Blur";
		}

		void Create_Pano() {
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

		void Init_Pano() {
			Pano_Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Pano_Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Compare_Form.WindowState = FormWindowState.Maximized;
			Pano_Form.Controls.Add(Pano_Image_Box);

			Pano_Image_Box.SizeMode = PictureBoxSizeMode.Zoom;
			Pano_Image_Box.Size = Pano_Form.Size;
			Pano_Image_Box.MouseClick += new MouseEventHandler(Pano_MouseMove);
		}

		void Pano_MouseMove(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Create_Pano();
				Pano_Form.Refresh();
			}
		}
	}
}
