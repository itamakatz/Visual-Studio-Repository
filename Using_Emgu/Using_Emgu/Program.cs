using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

		public OpenFileDialog Open_File = new OpenFileDialog();

		public Form Pano_Form = new Form();
		ImageBox Pano_Image_Box = new ImageBox();

		public Compare_Images_GUI My_Form = new Compare_Images_GUI();
		public Compare_Images_GUI My_Form_2 = new Compare_Images_GUI();

		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new MultiFormContext(new Program(), new Program()));

			new Program();
		}

		public Program() {

			Init_Pano();
			//Create_Pano();
			Crop_To_Edges();
			Pano_Form.ShowDialog();

			//Compare_Images();
			//My_Form.Form_ShowDialog();
			//My_Form_2.Form.Show();
			//My_Form.Form.ShowDialog();
			//Console.ReadKey();
			//Compare_Form.ShowDialog();
		}

		void Compare_Images() {
			Image<Bgr, Byte> My_Image = new Image<Bgr, byte>(@"C:\Users\admin\Desktop\COM_Integration\Panorama Stiching\Image Sets\Set_7\Results\All_Images_pano.jpg");

			My_Form.emgu_Image_Panel_Right.Emgu_Im_Box.Image = My_Image.Canny(255, 250 / 4);
			My_Form.emgu_Image_Panel_Right.Im_Label.Text = "Canny(255, 250 / 4)";

			My_Form.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Canny(255, 250 / 4).Canny(255, 250 / 4);
			My_Form.emgu_Image_Panel_Left.Im_Label.Text = "Canny(255, 250 / 4).Canny(255, 250 / 4)";

			My_Form.emgu_Image_Panel_Left.Emgu_Im_Box.Image.MinMax(
				out double[] minValues, 
				out double[] maxValues, 
				out Point[] minLocations, 
				out Point[] maxLocations);

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
			Single[,] Blur_Kernel = new Single[,] { { 1,1,1 }, { 1,1,1 }, { 1,1,1 } };

			ConvolutionKernelF convolutionKernelF_Edge_Detection = new ConvolutionKernelF(Edge_Detection_Kernel);
			ConvolutionKernelF convolutionKernelF_Sharpening = new ConvolutionKernelF(Edge_Detection_Kernel);
			ConvolutionKernelF convolutionKernelF_Blur = new ConvolutionKernelF(Blur_Kernel);

			My_Form_2.emgu_Image_Panel_Right.Emgu_Im_Box.Image = My_Image.Convolution(convolutionKernelF_Edge_Detection);
			My_Form_2.emgu_Image_Panel_Right.Im_Label.Text = "convolutionKernelF_Edge_Detection";

			//My_Form_2.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Convolution(convolutionKernelF_Sharpening);
			//My_Form_2.emgu_Image_Panel_Left.Im_Label.Text = "convolutionKernelF_Sharpening";

			//My_Form_2.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Convolution(convolutionKernelF_Blur);
			//My_Form_2.emgu_Image_Panel_Left.Im_Label.Text = "convolutionKernelF_Blur";

			My_Form_2.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Sobel(1,0,3);
			My_Form_2.emgu_Image_Panel_Left.Im_Label.Text = "My_Image.Sobel(1,0,3)";
		}

		void Create_Pano() {
			using (Stitcher stitcher = new Stitcher(false)) {
				using (AKAZEFeaturesFinder finder = new AKAZEFeaturesFinder()) {

					stitcher.SetFeaturesFinder(finder);
					stitcher.CompositingResol = -1; // Use -1 for original resolution

					using (VectorOfMat vm = new VectorOfMat()) {

						Open_File.Multiselect = true;
						if (Open_File.ShowDialog() != DialogResult.OK) {
							MessageBox.Show(String.Format("User Error: No Images Were Selected"));
							Pano_Image_Box.Image = null;
							return;
						};

						//Console.WriteLine("Please Enter Parameter for RegistrationResol");
						//double parameter = double.Parse(Console.ReadLine());
						//Console.WriteLine("You Entered: " + parameter);


						//stitcher.PanoConfidenceThresh = parameter;
						//stitcher.RegistrationResol = parameter;
						//stitcher.SeamEstimationResol =
 						//stitcher.WaveCorrection = true;
 						//stitcher.WaveCorrectionKind = Stitcher.WaveCorrectionType.Vert;
						//stitcher.WorkScale =


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
						} else if (stitchStatus == Stitcher.Status.ErrNeedMoreImgs) {
							MessageBox.Show("Stiching Error: Need more images..");
							Pano_Image_Box.Image = null;
						} else if (stitchStatus == Stitcher.Status.ErrHomographyEstFail) {
							MessageBox.Show("Stiching Error: Homography estimateion failed.");
							Pano_Image_Box.Image = null;
						} else if (stitchStatus == Stitcher.Status.ErrCameraParamsAdjustFail) {
							MessageBox.Show("Stiching Error: Camera parameters adjustment failed.");
							Pano_Image_Box.Image = null;
						} else {
							MessageBox.Show("Unknown Error...");
						}
					}
				}
			}
		}

		private void Crop_To_Edges() {

			if (Open_File.ShowDialog() != DialogResult.OK) {
				MessageBox.Show(String.Format("User Error: No Images Were Selected"));
				Pano_Image_Box.Image = null;
				return;
			};

			Mat pano_mat = new Mat(Open_File.FileNames[0]);

			Stopwatch watch = Stopwatch.StartNew();

			Bitmap bitmap = pano_mat.Bitmap;
			bool[] bool_array = new bool[bitmap.Width * bitmap.Height];

			PixelFormat pixel_format = bitmap.PixelFormat;

			Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

			var bitmap_data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, pixel_format);
			int single_pixel_num_of_byte = Image.GetPixelFormatSize(pixel_format) / 8;
			int stride = bitmap.Width * single_pixel_num_of_byte;
			int padding = (stride % 4);
			stride += padding == 0 ? 0 : 4 - padding; // pad out to multiple of 4 - CRITICAL
			int im_num_of_bytes = bitmap.Height * Math.Abs(stride);

			IntPtr pointer = bitmap_data.Scan0;
			byte[] bitmap_byte_array = new byte[im_num_of_bytes];

			Marshal.Copy(pointer, bitmap_byte_array, 0, im_num_of_bytes);

			int min = 0;
			int max = (bitmap.Width / 2);
			//int max = bitmap.Width - 1;
			while (min <= max) {
				int mid = (min + max) / 2;
				int intersection = 0;
				int index = Byte_Index(mid, 0);
				bool current = bitmap_byte_array[index] == 0 && bitmap_byte_array[index + 1] == 0 && bitmap_byte_array[index + 2] == 0;

				for (int y = 0; y < bitmap.Height; y++) {
					index = Byte_Index(mid, y);
					if (current != (bitmap_byte_array[index] == 0 && 
									bitmap_byte_array[index + 1] == 0 &&
									bitmap_byte_array[index + 2] == 0)) {
						intersection++;
						current = !current;
					}
				}

				for (int y = 0; y < bitmap.Height; y++) {
					index = Byte_Index(mid, y);
					bitmap_byte_array[index] = 0;
					bitmap_byte_array[index + 1] = 0;
					bitmap_byte_array[index + 2] = 255;
				}

				if (intersection > 2) { min = mid + 1; } 
				else if (intersection == 2) { max = mid - 1; } 
				else { MessageBox.Show("Error: intersection < 2 ..."); }
			}

			//for (int y = 0; y < bitmap.Height; y++) {
			//	int index = Byte_Index(max, y);
			//	bitmap_byte_array[index]		= 0;
			//	bitmap_byte_array[index + 1]	= 0;
			//	bitmap_byte_array[index + 2]	= 255;
			//}

			//for (int x = 0; x < bitmap.Width; x++) {
			//	for (int y = 0; y < bitmap.Height; y++) {
			//		int index = Byte_Index(x, y);
			//		if (bitmap_byte_array[index] == 0 && bitmap_byte_array[index + 1] == 0 && bitmap_byte_array[index + 2] == 0) {
			//			bool_array[Bool_Index(x, y)] = true;
			//		}
			//	}
			//}

			//watch.Stop();
			//var elapsed_time = watch.Elapsed;
			//Thread thread = new Thread(() => find_crop_bounds(0,0), 1000000000);
			//thread.Start();
			//thread.Join();

			//List<Tuple<int,int>> index_array = new List<Tuple<int,int>>();

			void find_crop_bounds(int x, int y) {

				if (!bool_array[Bool_Index(x, y)]) { return; }

				bool_array[Bool_Index(x, y)] = false;
				bool is_bound = true;

				if (Bool_Index(x + 1, y) < bool_array.Length)	{ if (!bool_array[Bool_Index(x + 1, y)]) { is_bound = false; }; }
				if (Bool_Index(x - 1, y) > 0)					{ if (!bool_array[Bool_Index(x - 1, y)]) { is_bound = false; }; }
				if (Bool_Index(x, y + 1) < bool_array.Length)	{ if (!bool_array[Bool_Index(x, y + 1)]) { is_bound = false; }; }
				if (Bool_Index(x, y - 1) < 0)					{ if (!bool_array[Bool_Index(x, y - 1)]) { is_bound = false; }; }

				if (is_bound) { Tuple.Create(x, y); }

				if (Bool_Index(x + 1, y) < bool_array.Length)	{ find_crop_bounds(x + 1, y); }
				if (Bool_Index(x - 1, y) > 0)					{ find_crop_bounds(x - 1, y); }
				if (Bool_Index(x, y + 1) < bool_array.Length)	{ find_crop_bounds(x, y + 1); }
				if (Bool_Index(x, y - 1) < 0)					{ find_crop_bounds(x, y - 1); }
			}

			Marshal.Copy(bitmap_byte_array, 0, pointer, im_num_of_bytes);
			bitmap.UnlockBits(bitmap_data);

			int Byte_Index(int x, int y) { return x * single_pixel_num_of_byte + (bitmap.Height - y - 1) * stride; }
			int Bool_Index(int x, int y) { return x + (bitmap.Height - y - 1) * bitmap.Width; }

			Pano_Image_Box.Image = pano_mat;

			//return null;
		}

		void Init_Pano() {
			Pano_Form.Height = Screen.PrimaryScreen.Bounds.Height;
			Pano_Form.Width = Screen.PrimaryScreen.Bounds.Width;
			Pano_Form.WindowState = FormWindowState.Maximized;
			Pano_Form.Controls.Add(Pano_Image_Box);

			Pano_Image_Box.SizeMode = PictureBoxSizeMode.Zoom;
			Pano_Image_Box.Size = Pano_Form.Size;
			Pano_Image_Box.MouseDoubleClick += new MouseEventHandler(Pano_MouseDoubleClick);
		}

		void Pano_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Create_Pano();
				Pano_Form.Refresh();
			}
		}
	}
}
