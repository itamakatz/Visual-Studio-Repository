using g3;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOTech_BitMap_Slicer
{
	class Bitmap_Slice
	{
		public Bitmap bitmap { get; }
		public byte[] bitmap_byte_array;
		private GCHandle handle;
		private int stride;
		private int pixel_in_byte_size;
		private IntPtr pointer;
		private int im_size_in_bytes;
		private BitmapData bitmap_data;

		public bool Scaled { get; set; }

		public static int SCALE_FACTOR;
		public static int PEN_WIDTH { get; set; }
		public static Pen Pen;
		private static byte[] byte_color;
		private static Color bitmap_color;
		public static Color Bitmap_Color
		{
			get
			{
				return bitmap_color;
			}
			set
			{
				bitmap_color = value;
				byte_color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R };
			}
		}
		public static PixelFormat PIXEL_FORMAT;

		public Graphics graphics;

		private bool[] bool_array;
		private int current_position;
		private int bitmap_width;
		private int bitmap_height;

		public static int recursive_count = 0;
		public static int recursive_count_in_if = 0;


		public Bitmap_Slice(double width, double height)
		{
			Scaled = true;

			bitmap_width = get_int_dimension(width);
			bitmap_height = get_int_dimension(height);

			bitmap = new Bitmap(bitmap_width, bitmap_height, PIXEL_FORMAT);
			graphics = Graphics.FromImage(bitmap);

			bool_array = new bool[bitmap_width * bitmap_height];
		}

		public Bitmap_Slice(Bitmap source_bitmap)
		{
			Scaled = false;

			bitmap_width = source_bitmap.Width;
			bitmap_height = source_bitmap.Height;

			bitmap = source_bitmap;
			graphics = Graphics.FromImage(bitmap);

			bool_array = new bool[bitmap_width * bitmap_height];
		}

		public void flood_fill_recursive()
		{
			recursive_count++;

			int current_x = current_position % bitmap_width;
			int current_y = current_position / bitmap_width;

			bool_array[current_x + current_y * bitmap_width] = true;

			int current_position_stride = current_x * pixel_in_byte_size + current_y * stride;

			if (!check_byte_color_equal(current_position_stride))
			{
				recursive_count_in_if++;

				set_byte_color(current_position_stride);
			}
			else
			{
				return;
			}

			if (current_x > 0)
			{
				current_position = (current_x - 1) + current_y * bitmap_width;
				if (!bool_array[current_position])
				{
					flood_fill_recursive();
				}
			}
			if (current_y > 0)
			{
				current_position = current_x + (current_y - 1) * bitmap_width;
				if (!bool_array[current_position])
				{
					flood_fill_recursive();
				}
			}
			if (current_x < bitmap_width - 1)
			{
				current_position = (current_x + 1) + current_y * bitmap_width;
				if (!bool_array[current_position])
				{
					flood_fill_recursive();
				}
			}
			if (current_y < bitmap_height - 1)
			{
				current_position = current_x + (current_y + 1) * bitmap_width;
				if (!bool_array[current_position])
				{
					flood_fill_recursive();
				}
			}
		}

		public void set_starting_point(int width, int height)
		{
			try
			{
				int i = width;
				while(!check_color_equal(bitmap.GetPixel(width, i), Bitmap_Color)) { i--; }
				i = width;
				while(!check_color_equal(bitmap.GetPixel(width, i), Bitmap_Color)) { i++; }
				i = width;
				while (!check_color_equal(bitmap.GetPixel(i, height), Bitmap_Color)) { i--; }
				i = height;
				while (!check_color_equal(bitmap.GetPixel(i, height), Bitmap_Color)) { i++; }
			}
			catch (Exception)
			{
				throw new System.ArgumentException("Starting coordiantes for flood fill are not bound");
			}
			current_position = width + height * bitmap_width;
		}

		private bool check_color_equal(Color c1, Color c2)
		{
			return (c1.A == c2.A && c1.R == c2.R && c1.G == c2.G && c1.B == c2.B) ? true : false;
		}

		private bool check_byte_color_equal(int byte_array_index)
		{
			if (PIXEL_FORMAT == PixelFormat.Format24bppRgb)
			{
				return (byte_color[0] == bitmap_byte_array[byte_array_index] &&
						byte_color[1] == bitmap_byte_array[byte_array_index + 1] &&
						byte_color[2] == bitmap_byte_array[byte_array_index + 2]) ? true : false;
			}
			else
			{
				Util.exit_messege(new string[] { "cant compare non 24 RGB Format" });
				return false;
			}
		}

		private void set_byte_color(int byte_array_index)
		{
			bitmap_byte_array[byte_array_index] = byte_color[0];
			bitmap_byte_array[byte_array_index + 1] = byte_color[1];
			bitmap_byte_array[byte_array_index + 2] = byte_color[2];
		}

		public void DrawLineInt(Vector2d origin_vec, Vector2d dest_vec)
		{
			graphics.DrawLine(Pen, (float)origin_vec.x, (float)(bitmap_height - origin_vec.y),
							(float)dest_vec.x, (float)(bitmap_height - dest_vec.y));
		}

		public void Switch_to_byte_manipulation()
		{
			graphics.Dispose();
			Rectangle rect = new Rectangle(0, 0, bitmap_width, bitmap_height);

			pixel_in_byte_size = Image.GetPixelFormatSize(PIXEL_FORMAT) / 8;
			stride = bitmap_width * pixel_in_byte_size;
			int padding = (stride % 4);
			stride += padding == 0 ? 0 : 4 - padding;//pad out to multiple of 4

			bitmap_data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PIXEL_FORMAT);
			pointer = bitmap_data.Scan0;
			im_size_in_bytes = bitmap_height * Math.Abs(bitmap_data.Stride);
			bitmap_byte_array = new byte[im_size_in_bytes];

			Marshal.Copy(pointer, bitmap_byte_array, 0, im_size_in_bytes);
		}

		public void Switch_to_bitmap_manipulation()
		{
			Marshal.Copy(bitmap_byte_array, 0, pointer, im_size_in_bytes);
			bitmap.UnlockBits(bitmap_data);
		}

		internal void Draw_rectangle(int width, int height)
		{
			if (Scaled)
			{
				height = height * SCALE_FACTOR - 1;
				width = width * SCALE_FACTOR - 1;
			}
			else
			{
				height = height - 1;
				width = width - 1;
			}

			graphics.DrawLine(Pen, 0, 0, 0, height);
			graphics.DrawLine(Pen, 0, height, width, height);
			graphics.DrawLine(Pen, width, height, width, 0);
			graphics.DrawLine(Pen, width, 0, 0, 0);
		}

		private Int32 get_int_dimension(double in_double)
		{
			if (Scaled)
			{
				return (Int32)Math.Ceiling(in_double + PEN_WIDTH * 2) * SCALE_FACTOR;
			}
			else
			{
				return (Int32)Math.Ceiling(in_double + PEN_WIDTH * 2);
			}
		}

		public static void Run(double[] a, double[] b, int N)
		{
			Parallel.For(0, N, i => { a[i] += b[i]; });
		}

	}
}


//public void DrawLine_byte_array(Vector2d origin_vec, Vector2d dest_vec)
//{ 
//	int x0 = (int) Math.Floor(origin_vec.x);
//	int y0 = (int) Math.Floor(origin_vec.y);
//	int x1 = (int) Math.Ceiling(dest_vec.x);
//	int y1 = (int) Math.Ceiling(dest_vec.y);

//	int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
//	int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
//	int err = (dx > dy ? dx : -dy) / 2, e2;

//	while (true)
//	{
//		bitmap_byte_array[x0 + y0 * bitmap_width] = BYTE_MAP_VALUE;
//		if (x0 == x1 && y0 == y1) break;
//		e2 = err;
//		if (e2 > -dx) { err -= dy; x0 += sx; }
//		if (e2 < dy) { err += dx; y0 += sy; }
//	}
//}

//public Bitmap Bitmap_XOR(ref Bitmap bitmap_xor)
//{
//	foreach (var item in bitmap_xor.GetPixel)
//	{

//	}
//}

//public void save_byte_array_to_bitmap_image(string path, System.Drawing.Imaging.ImageFormat image_format)
//{
//	bitmap.Save(path, image_format);

//	//Bitmap bmp;
//	//using (MemoryStream ms = new MemoryStream(bitmap_byte_array))
//	//{
//	//	bmp = new Bitmap(ms);
//	//	handle.Free();
//	//	bmp.Save(path, image_format);
//	//}
//}