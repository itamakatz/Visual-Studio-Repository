using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
	internal class Program : Form {

		public enum RUN_MODE { COMPARE, PANO , CROP};

		RUN_MODE Mode;

		public OpenFileDialog Open_File = new OpenFileDialog();

		public Form Pano_Form = new Form();
		ImageBox Pano_Image_Box = new ImageBox();

		public Compare_Images_GUI My_Form = new Compare_Images_GUI();
		public Compare_Images_GUI My_Form_2 = new Compare_Images_GUI();

		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Program my_program;

			RUN_MODE MODE = RUN_MODE.PANO;

			switch (MODE) {

				case RUN_MODE.COMPARE:
					my_program = new Program(RUN_MODE.COMPARE);
					Application.Run(new MultiFormContext(my_program.My_Form.Form, my_program.My_Form_2.Form));
					break;

				case RUN_MODE.PANO:
					my_program = new Program(RUN_MODE.PANO);
					Application.Run(new MultiFormContext(my_program.Pano_Form));
					break;
				case RUN_MODE.CROP:
					my_program = new Program(RUN_MODE.CROP);
					Application.Run(new MultiFormContext(my_program.Pano_Form));
					break;
				default:
					break;
			}
		}

		public Program(RUN_MODE mode) {

			Mode = mode;

			switch (mode) {

				case RUN_MODE.COMPARE:
					Compare_Images();
					break;

				case RUN_MODE.PANO:
					Init_Pano();
					Create_Pano();
					break;

				case RUN_MODE.CROP:
					Init_Pano();
					Crop_To_Edges_Caller();
					break;

				default:
					break;
			}
		}

		void Compare_Images() {
			Image<Bgr, Byte> My_Image = new Image<Bgr, byte>(@"C:\Users\admin\Desktop\COM_Integration\Panorama Stiching\Image Sets\Set_7\Results\All_Images_pano.jpg");

			//My_Form.emgu_Image_Panel_Right.Emgu_Im_Box.Image = My_Image.Canny(255, 250 / 4);
			//My_Form.emgu_Image_Panel_Right.Im_Label.Text = "Canny(255, 250 / 4)";

			//My_Form.emgu_Image_Panel_Left.Emgu_Im_Box.Image = My_Image.Canny(255, 250 / 4).Canny(255, 250 / 4);
			//My_Form.emgu_Image_Panel_Left.Im_Label.Text = "Canny(255, 250 / 4).Canny(255, 250 / 4)";

			My_Form.emgu_Image_Panel_Right.Emgu_Im_Box.Image = ResizeImage(My_Image, My_Image.Width / 4, My_Image.Height / 4);
			My_Form.emgu_Image_Panel_Right.Im_Label.Text = "ResizeImage";
			My_Form.emgu_Image_Panel_Left.Emgu_Im_Box.Image = Crop_To_Edges(ref My_Image);
			My_Form.emgu_Image_Panel_Left.Im_Label.Text = "Crop_To_Edges(ref My_Image)";

			Single[,] Edge_Detection_Kernel =	new Single[,] { { -1, -1, -1 }, { -1,  8, -1 }, { -1, -1, -1 } };
			Single[,] Sharpen_Kernel =			new Single[,] { { 0, -1, 0 }, { -1,  5, -1 }, { 0, -1, 0 } };
			Single[,] Blur_Kernel =				new Single[,] { { 1, 1, 1 }, { 1,1,1 }, { 1,1,1 } };

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

		/// <summary>
		/// Resize the image to the specified width and height.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="width">The width to resize to.</param>
		/// <param name="height">The height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Image<Bgr, Byte> ResizeImage(Image<Bgr, Byte> image, int width, int height) {
		//public static Bitmap ResizeImage(Image image, int width, int height) {
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.Bitmap.HorizontalResolution, image.Bitmap.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage)) {
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.None;
				graphics.PixelOffsetMode = PixelOffsetMode.None;

				using (var wrapMode = new ImageAttributes()) {
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image.Bitmap, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return new Image<Bgr, byte>(destImage);
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
							Pano_Image_Box.Image = Crop_To_Edges(ref result);
							Console.WriteLine(String.Format("Stitched in {0} milliseconds.", watch.ElapsedMilliseconds));
						} else if (stitchStatus == Stitcher.Status.ErrNeedMoreImgs) {
							MessageBox.Show("Stiching Error: Need more images..");
							Pano_Image_Box.Image = null;
						} else if (stitchStatus == Stitcher.Status.ErrHomographyEstFail) {
							MessageBox.Show("Stiching Error: Homography estimation failed.");
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

		private void Crop_To_Edges_Caller() { Pano_Image_Box.Image = Crop_To_Edges(); }

		private Image<Bgr, Byte> Crop_To_Edges() {

			if (Open_File.ShowDialog() != DialogResult.OK) {
				MessageBox.Show(String.Format("User Error: No Images Were Selected"));
				Pano_Image_Box.Image = null;
				return null;
			};

			Image<Bgr, Byte> pano_image = new Image<Bgr, Byte>(Open_File.FileNames[0]);
			return Crop_To_Edges(ref pano_image);
		}

		private Image<Bgr, Byte> Crop_To_Edges(ref Mat pano_image) {
			Image<Bgr, Byte> crom_image = pano_image.ToImage<Bgr, Byte>();
			return Crop_To_Edges(ref crom_image);
		}

		private Image<Bgr, Byte> Crop_To_Edges(ref Image<Bgr, Byte> pano_image) {

			Image <Gray, Byte> pano_gray = new Image<Gray, Byte>(pano_image.Bitmap);

			Bitmap bitmap_gray = pano_gray.Bitmap;

			// Locking bits of the image for better performance //

			int height = pano_gray.Height;
			int width = pano_gray.Width;

			PixelFormat pixel_format = pano_gray.Bitmap.PixelFormat;

			Rectangle rect = new Rectangle(0, 0, width, height);

			var bitmap_data = bitmap_gray.LockBits(rect, ImageLockMode.ReadOnly, pixel_format);
			int single_pixel_num_of_byte = Image.GetPixelFormatSize(pixel_format) / 8;
			int stride = width * single_pixel_num_of_byte;
			int padding = (stride % 4);
			stride += padding == 0 ? 0 : 4 - padding; // pad out to multiple of 4 - CRITICAL
			int im_num_of_bytes = height * Math.Abs(stride);

			IntPtr pointer = bitmap_data.Scan0;
			byte[] bitmap_byte_array = new byte[im_num_of_bytes];

			Marshal.Copy(pointer, bitmap_byte_array, 0, im_num_of_bytes);

			/* Important Note: Do not forget y_min and y_max  are in fact flipped and each represent the opposite direction 
				after applaying the formula real_y = height - y - 1. This concept is CRUCIAL to understand the boundry conditions
				of the program.
				An exapmle of the is in the line of code:
				return pano_image.Copy(new Rectangle(x_min, height - y_max - 1, new_width, new_height));
				which sets the y coordinate at the begining of the Rectangle to: height - y_max - 1 and NOT y_min
				Given the above, I will name the bool variables according to the real representation*/

			int x_min = 0;
			int y_min = 0;

			int x_max = width - 1;
			int y_max = height - 1;

			int BLACK_VALUE_THRESHOLD = 10;
			double BLACK_COUNT_THRESHOLD_AVERAGE = 0.02;

			bool BL_Corner_Black   = true;
			bool BR_Corner_Black   = true;
			bool TL_Corner_Black   = true;
			bool TR_Corner_Black   = true;

			while (BL_Corner_Black || BR_Corner_Black || TL_Corner_Black || TR_Corner_Black) {

				var index = 0;
				int value = 0;

				index = Byte_Index(x_min, y_min);
				value = bitmap_byte_array[index];
				if (BL_Corner_Black && bitmap_byte_array[Byte_Index(x_min, y_min)] < BLACK_VALUE_THRESHOLD) {
					if (Check_Right_Left(x_min) > Check_Top_Bottom(y_min)) { x_min++; } else { y_min++; }
				} else { BL_Corner_Black = false; }

				index = Byte_Index(x_max, y_min);
				value = bitmap_byte_array[index];
				if (BR_Corner_Black && bitmap_byte_array[Byte_Index(x_max, y_min)] < BLACK_VALUE_THRESHOLD) {
					if (Check_Right_Left(x_max) > Check_Top_Bottom(y_min)) { x_max--; } else { y_min++; }
				} else { BR_Corner_Black = false; }

				index = Byte_Index(x_min, y_max);
				value = bitmap_byte_array[index];
				if (TL_Corner_Black && bitmap_byte_array[Byte_Index(x_min, y_max)] < BLACK_VALUE_THRESHOLD) {
					if (Check_Right_Left(x_min) > Check_Top_Bottom(y_max)) { x_min++; } else { y_max--; }
				} else { TL_Corner_Black = false; }

				index = Byte_Index(x_max, y_max);
				value = bitmap_byte_array[index];
				if (TR_Corner_Black && bitmap_byte_array[Byte_Index(x_max, y_max)] < BLACK_VALUE_THRESHOLD) {
					if (Check_Right_Left(x_max) > Check_Top_Bottom(y_max)) { x_max--; } else { y_max--; }
				} else { TR_Corner_Black = false; }
			}

			bool Right_Edge_Black   = true;
			bool Left_Edge_Black    = true;
			bool Bottom_Edge_Black	= true;
			bool Top_Edge_Black		= true;

			while (true) {

				Left_Edge_Black		=	Check_Right_Left(x_min) > BLACK_COUNT_THRESHOLD_AVERAGE;
				Bottom_Edge_Black	=	Check_Top_Bottom(y_min) > BLACK_COUNT_THRESHOLD_AVERAGE;

				Right_Edge_Black	=	Check_Right_Left(x_max) > BLACK_COUNT_THRESHOLD_AVERAGE; 
				Top_Edge_Black		=	Check_Top_Bottom(y_max) > BLACK_COUNT_THRESHOLD_AVERAGE;

				if (!(Right_Edge_Black || Left_Edge_Black || Bottom_Edge_Black || Top_Edge_Black)) { break; }

				if (Left_Edge_Black)	{ x_min++; }
				if (Right_Edge_Black)	{ x_max--; }
				if (Bottom_Edge_Black)	{ y_min++; }
				if (Top_Edge_Black)		{ y_max--; }
			}

			Marshal.Copy(bitmap_byte_array, 0, pointer, im_num_of_bytes);
			bitmap_gray.UnlockBits(bitmap_data);

			int new_width = x_max - x_min;
			int new_height = y_max - y_min;

			return pano_image.Copy(new Rectangle(x_min, height - y_max - 1, new_width, new_height));


			// Defining a few methods to perform the crop //

			int Byte_Index(int x, int y) { return x * single_pixel_num_of_byte + (height - y - 1) * stride; }

			float Check_Right_Left(int fixed_x) {
				int zeroCount = 0;
				for (int j = y_min; j <= y_max; j++) {
					if (bitmap_byte_array[Byte_Index(fixed_x, j)] < BLACK_VALUE_THRESHOLD) { zeroCount++; }
				}
				return (zeroCount / (float) (y_max - y_min));
			}

			float Check_Top_Bottom(int fixed_y) {
				int zeroCount = 0;
				for (int i = x_min; i <= x_max; i++) {
					if (bitmap_byte_array[Byte_Index(i, fixed_y)] < BLACK_VALUE_THRESHOLD) { zeroCount++; }
				}
				return (zeroCount / (float) (x_max - x_min));
			}
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

				switch (Mode) {
					case RUN_MODE.PANO:
						Create_Pano();
						Pano_Form.Refresh();
						break;
					case RUN_MODE.CROP:
						Crop_To_Edges_Caller();
						Pano_Form.Refresh();
						break;
					default:
						break;
				}
			}
		}
	}
}
