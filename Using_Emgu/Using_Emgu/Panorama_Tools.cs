using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Using_Emgu {
	static class Panorama_Tools {
		private static OpenFileDialog Open_File = new OpenFileDialog();
		public static Image<Bgr, Byte> Crop_To_Edges() {

			if (Open_File.ShowDialog() != DialogResult.OK) {
				MessageBox.Show(String.Format("User Error: No Images Were Selected"));
				return null;
			};

			Image<Bgr, Byte> pano_image = new Image<Bgr, Byte>(Open_File.FileNames[0]);
			return Crop_To_Edges(ref pano_image);
		}

		public static Image<Bgr, Byte> Crop_To_Edges(ref Mat pano_image) {
			Image<Bgr, Byte> crom_image = pano_image.ToImage<Bgr, Byte>();
			return Crop_To_Edges(ref crom_image);
		}

		public static Image<Bgr, Byte> Crop_To_Edges(ref Image<Bgr, Byte> pano_image) {

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
			bool Bottom_Edge_Black  = true;
			bool Top_Edge_Black     = true;

			while (true) {

				Left_Edge_Black = Check_Right_Left(x_min) > BLACK_COUNT_THRESHOLD_AVERAGE;
				Bottom_Edge_Black = Check_Top_Bottom(y_min) > BLACK_COUNT_THRESHOLD_AVERAGE;

				Right_Edge_Black = Check_Right_Left(x_max) > BLACK_COUNT_THRESHOLD_AVERAGE;
				Top_Edge_Black = Check_Top_Bottom(y_max) > BLACK_COUNT_THRESHOLD_AVERAGE;

				if (!(Right_Edge_Black || Left_Edge_Black || Bottom_Edge_Black || Top_Edge_Black)) { break; }

				if (Left_Edge_Black) { x_min++; }
				if (Right_Edge_Black) { x_max--; }
				if (Bottom_Edge_Black) { y_min++; }
				if (Top_Edge_Black) { y_max--; }
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

	}
}
