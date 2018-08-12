using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace IOTech_BitMap_Slicer
{
	public static class V
	{
		// ***** Variables that can be changed ***** //

		//public static string MODEL_IN_PATH = @"C:\Users\admin\Desktop\BitMap_Slicer" + "\\" + "CorvinCastle" + @".stl";
		public static string MODEL_IN_PATH = @"C:\Users\admin\Desktop\BitMap_Slicer\Test Part Crown" + "\\" + "test-part_crown_conical" + @".stl";
		//public static string MODEL_IN_PATH = @"C:\Users\admin\Desktop\BitMap_Slicer" + "\\" + "Intersecting_triangles" + @".stl";
		public static string MODEL_OUT_PATH = @"Desktop\BitMap_Slicer\CorvinCastle_new.stl";

		public static string BITMAP_DIR_PREFIX = @"Desktop\BitMap_Slicer\BITMAP_slice";
		public static string BITMAP_PATH_SUFIX = @".Bmp";
		public static ImageFormat IMAGE_FORMAT_EXTENSION = ImageFormat.Bmp;

		public const int SCALE_FACTOR = 20;
		public const int NUM_OF_SLICES = 100;
		public static Axis SLICING_AXIS = Axis.Z;

		public static bool REVERSED_BW = true;

		public const int PEN_FINE_WIDTH = 1;
		public static Color bitmap_color = Color.Blue;

		/* CAN NOT BE OF THE FORMAT TYPE : (see https://stackoverflow.com/questions/11368412/lowering-bitmap-quality-produces-outofmemoryexception)
		 * Undefined
		 * DontCare
		 * Format16bppArgb1555
		 * Format16bppGrayScale
		 * Format1bppIndexed
		 * Format4bppIndexed
		 * Format8bppIndexed
		 * 
		 * Formats that work well:
		 * Format32bppRgb - works but contains unnecessary alpha channel
		 * Format24bppRgb - best option. only has RGB color (though in reversed order namely BGR)
		 * Format16bppRgb555 - works and uses even less bytes thought for the comparison of the colors the implementation is complicated
		 */
		// maybe should varify the image is infact Format32bppRgb?
		public const PixelFormat PIXEL_FORMAT = PixelFormat.Format24bppRgb; // best format I found

		public const int STACK_SIZE = 1000000000;  // max Int32 = 2147483647 

		public const int EXIT_CODE = 10;

		// ***** Initialization and Declaration of other variables ***** //


		public static string USER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

		public static Pen Pen = new Pen(bitmap_color, PEN_FINE_WIDTH);

		public static Tuple<double, double> Mesh_min_dimensions;

		public static int slice_count = 1;
		public static int loop_count = 1;

		public enum Axis { X, Y, Z };

		public static int stride;
		public static int im_num_of_bytes;
		public static int single_pixel_num_of_byte = Image.GetPixelFormatSize(PIXEL_FORMAT) / 8;

		public static int Bitmap_Width;
		public static int Bitmap_Height;

		public static int Actual_Max_Height;
		public static int Actual_Min_Height;
		public static int Actual_Max_Width;
		public static int Actual_Min_Width;

	}
}