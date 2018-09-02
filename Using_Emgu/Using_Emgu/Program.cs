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
using Emgu.CV.XFeatures2D;

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

			RUN_MODE MODE = RUN_MODE.CROP;

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
			My_Form.emgu_Image_Panel_Left.Emgu_Im_Box.Image = Panorama_Tools.Crop_To_Edges(ref My_Image);
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
							Pano_Image_Box.Image = Panorama_Tools.Crop_To_Edges(ref result);
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

		private void Crop_To_Edges_Caller() { Pano_Image_Box.Image = Panorama_Tools.Crop_To_Edges(); }

		public static void find_features(ref Image<Bgr, Byte> pano_image) {
			SIFT sift = new SIFT();
			VectorOfKeyPoint vectorOfKeyPoint = new VectorOfKeyPoint();
			Image<Bgr, Byte> _IOutputArray_Image = new Image<Bgr, Byte>(pano_image.Size);
			sift.DetectAndCompute(pano_image, null, vectorOfKeyPoint, _IOutputArray_Image, false);

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
