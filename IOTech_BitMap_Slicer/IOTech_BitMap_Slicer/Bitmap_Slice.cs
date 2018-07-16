using g3;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOTech_BitMap_Slicer
{
	class Bitmap_Slice
	{
		public Bitmap bitmap { get; }
		public byte[] bitmap_byte_array;

		private byte byte_map_value = 100;

		public static int SCALE_FACTOR;
		public static int PEN_FINE_WIDTH { get; set; }
		public static int PEN_ROBUST_WIDTH { get; set; }
		public static System.Drawing.Color bitmap_color { get; set; }
		public static System.Drawing.Pen Pen_Fine;
		public static System.Drawing.Pen Pen_Robust;
		private System.Drawing.Graphics graphics;

		private bool[] bool_array;
		private int current_position;
		public int bitmap_width { get; set; }
		public int bitmap_height { get; set; }

		public static int recursive_count = 0;
		
		public Bitmap_Slice(double width, double height)
		{
			bitmap = new Bitmap(get_int_dimension(width), get_int_dimension(height));
			graphics = Graphics.FromImage(bitmap);
			bitmap_width = bitmap.Width;
			bitmap_height = bitmap.Height;
			bool_array = new bool[bitmap_height * bitmap_width];
			//bitmap_byte_array = new byte[bitmap_height * bitmap_width];
		}

		public Bitmap_Slice(Bitmap bitmap)
		{
			this.bitmap = bitmap;
			bitmap_width = bitmap.Width;
			bitmap_height = bitmap.Height;
			bool_array = new bool[bitmap_height * bitmap_width];
			//bitmap_byte_array = Array1DFromBitmap(bitmap);
		}

		public void flood_fill_recursive()
		{
			recursive_count++;

			int current_x = current_position % bitmap_width;
			int current_y = current_position / bitmap_width;

			bool_array[current_x + current_y * bitmap_width] = true;

			if (!check_color_equal(bitmap.GetPixel(current_x, current_y), bitmap_color))
			{
				bitmap.SetPixel(current_x, current_y, bitmap_color);
			}
			//if (bitmap_byte_array[current_x + current_y * bitmap_width] == 0)
			//{
			//	//bitmap_byte_array[current_x + current_y * bitmap_width] = byte_map_value;
			//}
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
				while(!check_color_equal(bitmap.GetPixel(i, height), bitmap_color)) { i--; }
				i = height;
				while(!check_color_equal(bitmap.GetPixel(width, i), bitmap_color)) { i--; }
				i = width;
				while(!check_color_equal(bitmap.GetPixel(width, i), bitmap_color)) { i++; }
				i = width;
				while(!check_color_equal(bitmap.GetPixel(i, height), bitmap_color)) { i++; }
			}
			catch (Exception)
			{
				throw new System.ArgumentException("Starting coordiantes for flodd fill are not bound");
			}
			current_position = width + height * bitmap_width;
		}

		private bool check_color_equal(Color c1, Color c2)
		{
			return (c1.A == c2.A && c1.R == c2.R && c1.G == c2.G && c1.B == c2.B) ? true : false;
		}

		public void DrawLineInt(Vector2d origin_vec, Vector2d dest_vec)
		{
			graphics.DrawLine(Pen_Fine, (float)origin_vec.x, (float)(bitmap_height - origin_vec.y),
							(float)dest_vec.x, (float)(bitmap_height - dest_vec.y));
			//DrawLine_byte_array(origin_vec, dest_vec);
		}

		public void DrawLine_byte_array(Vector2d origin_vec, Vector2d dest_vec)
		{ 
			int x0 = (int) Math.Floor(origin_vec.x);
			int y0 = (int) Math.Floor(origin_vec.y);
			int x1 = (int) Math.Ceiling(dest_vec.x);
			int y1 = (int) Math.Ceiling(dest_vec.y);

			int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
			int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
			int err = (dx > dy ? dx : -dy) / 2, e2;

			while (true)
			{
				bitmap_byte_array[x0 + y0 * bitmap_width] = byte_map_value;
				if (x0 == x1 && y0 == y1) break;
				e2 = err;
				if (e2 > -dx) { err -= dy; x0 += sx; }
				if (e2 < dy) { err += dx; y0 += sy; }
			}
		}

		public Bitmap get_bitmap_from_byte()
		{
			return BitmapFromArray1D(bitmap_byte_array, bitmap_width, bitmap_height);
		}

		private Int32 get_int_dimension(double in_double)
		{
			return (Int32)Math.Ceiling(in_double + PEN_ROBUST_WIDTH * 2) * SCALE_FACTOR;
		}

		public static byte[] Array1DFromBitmap(Bitmap bmp)
		{
			if (bmp == null) throw new NullReferenceException("Bitmap is null");

			System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
			IntPtr ptr = data.Scan0;

			//declare an array to hold the bytes of the bitmap
			int numBytes = data.Stride * bmp.Height;
			byte[] bytes = new byte[numBytes];

			//copy the RGB values into the array
			System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, numBytes);

			bmp.UnlockBits(data);

			return bytes;
		}

		public static Bitmap BitmapFromArray1D(byte[] bytes, int width, int height)
		{
			Bitmap grayBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
			System.Drawing.Rectangle grayRect = new System.Drawing.Rectangle(0, 0, grayBmp.Width, grayBmp.Height);
			BitmapData grayData = grayBmp.LockBits(grayRect, ImageLockMode.ReadWrite, grayBmp.PixelFormat);
			IntPtr grayPtr = grayData.Scan0;

			int grayBytes = grayData.Stride * grayBmp.Height;
			ColorPalette pal = grayBmp.Palette;

			for (int g = 0; g < 256; g++)
			{
				pal.Entries[g] = System.Drawing.Color.FromArgb(g, g, g);
			}

			grayBmp.Palette = pal;

			System.Runtime.InteropServices.Marshal.Copy(bytes, 0, grayPtr, grayBytes);

			grayBmp.UnlockBits(grayData);
			return grayBmp;
		}

		internal void Draw_rectangle(int width, int height)
		{
			graphics.DrawLine(Pen_Robust, 0, 0, 0, height);
			graphics.DrawLine(Pen_Robust, 0, height, width, height);
			graphics.DrawLine(Pen_Robust, width, height, width, 0);
			graphics.DrawLine(Pen_Robust, width, 0, 0, 0);
		}
	}
}
