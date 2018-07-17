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
		private bool[] bool_array;
		public Graphics graphics;
		private BitmapData bitmap_data;

		private int stride;
		private int pixel_in_byte_size;
		private IntPtr pointer;
		private int im_size_in_bytes;

		private readonly int bitmap_width;
		private readonly int bitmap_height;

		public bool Scaled { get; set; }

		public static int SCALE_FACTOR;
		public static PixelFormat PIXEL_FORMAT;

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
				// why the hell is it ordered BGR and not RGB god knows even though in the docs it says RGB
				byte_color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R };
			}
		}
		
		private Func<int, int, int> XY_2_byte_arr;

		public Bitmap_Slice(double width, double height)
		{
			Scaled = true;

			bitmap_width = Get_Int_Dimension(width);
			bitmap_height = Get_Int_Dimension(height);

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

		public void Switch_to_byte_manipulation()
		{
			graphics.Dispose();
			Rectangle rect = new Rectangle(0, 0, bitmap_width, bitmap_height);

			pixel_in_byte_size = Image.GetPixelFormatSize(PIXEL_FORMAT) / 8;
			stride = bitmap_width * pixel_in_byte_size;
			int padding = (stride % 4);
			stride += padding == 0 ? 0 : 4 - padding; //pad out to multiple of 4 - CRITICAL

			bitmap_data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PIXEL_FORMAT);
			pointer = bitmap_data.Scan0;
			im_size_in_bytes = bitmap_height * Math.Abs(bitmap_data.Stride);
			bitmap_byte_array = new byte[im_size_in_bytes];

			Marshal.Copy(pointer, bitmap_byte_array, 0, im_size_in_bytes);

			XY_2_byte_arr = (x, y) => x * pixel_in_byte_size + y * stride;
		}

		public void Switch_to_bitmap_manipulation()
		{
			Marshal.Copy(bitmap_byte_array, 0, pointer, im_size_in_bytes);
			bitmap.UnlockBits(bitmap_data);
			graphics = Graphics.FromImage(bitmap);
			XY_2_byte_arr = null;
		}

		public void Flood_fill_recursive(int current_x, int current_y)
		{
			bool_array[current_x + current_y * bitmap_width] = true;

			if (RGB_Equal(current_x * pixel_in_byte_size + current_y * stride)) return;

			Set_RGB(current_x * pixel_in_byte_size + current_y * stride);

			if (current_x > 0 && !bool_array[(current_x - 1) + current_y * bitmap_width])
				Flood_fill_recursive(current_x - 1, current_y);

			if (current_y > 0 && !bool_array[current_x + (current_y - 1) * bitmap_width])
				Flood_fill_recursive(current_x, current_y - 1);

			if (current_x < (bitmap_width - 1) && !bool_array[(current_x + 1) + current_y * bitmap_width])
				Flood_fill_recursive(current_x + 1, current_y);

			if (current_y < (bitmap_height - 1) && !bool_array[current_x + (current_y + 1) * bitmap_width])
				Flood_fill_recursive(current_x, current_y + 1);
		}

		public void Flood_Fill(int starting_x, int starting_y)
		{
			try
			{
				int i = starting_x;
				while (!RGB_Equal(XY_2_byte_arr(starting_x, i))) { i--; }
				i = starting_x;
				while (!RGB_Equal(XY_2_byte_arr(starting_x, i))) { i++; }
				i = starting_x;
				while (!RGB_Equal(XY_2_byte_arr(i, starting_y))) { i--; }
				i = starting_y;
				while (!RGB_Equal(XY_2_byte_arr(i, starting_y))) { i++; }
			}
			catch (Exception)
			{
				throw new System.ArgumentException("Starting coordiantes for flood fill are not bound");
			}

			Flood_fill_recursive(starting_x, starting_y);
		}

		private bool RGB_Equal(int byte_array_index)
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

		private void Set_RGB(int byte_array_index)
		{
			bitmap_byte_array[byte_array_index] = byte_color[0];
			bitmap_byte_array[byte_array_index + 1] = byte_color[1];
			bitmap_byte_array[byte_array_index + 2] = byte_color[2];
		}

		public void Draw_Line_On_Bitmap(Vector2d origin_vec, Vector2d dest_vec)
		{
			graphics.DrawLine(Pen, (float)origin_vec.x, (float)(bitmap_height - origin_vec.y),
							(float)dest_vec.x, (float)(bitmap_height - dest_vec.y));
		}

		//public void Draw_Line_On_Byte_Array(Vector2d origin_vec, Vector2d dest_vec)
		//{
		//	int x0 = (int)Math.Floor(origin_vec.x);
		//	int y0 = (int)Math.Floor(origin_vec.y);
		//	int x1 = (int)Math.Ceiling(dest_vec.x);
		//	int y1 = (int)Math.Ceiling(dest_vec.y);

		//	int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
		//	int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
		//	int err = (dx > dy ? dx : -dy) / 2, e2;

		//	while (true)
		//	{
		//		Set_RGB(XY_2_byte_arr(x0, y0));
		//		if (x0 == x1 && y0 == y1) break;
		//		e2 = err;
		//		if (e2 > -dx) { err -= dy; x0 += sx; }
		//		if (e2 < dy) { err += dx; y0 += sy; }
		//	}
		//}

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

		private int Get_Int_Dimension(double in_double)
		{
			if (Scaled)
			{
				return (int)Math.Ceiling(in_double + PEN_WIDTH * 2) * SCALE_FACTOR;
			}
			else
			{
				return (int)Math.Ceiling(in_double + PEN_WIDTH * 2);
			}
		}

		//public Bitmap Bitmap_XOR(ref Bitmap bitmap_xor)
		//{
		//	foreach (var item in bitmap_xor.GetPixel)
		//	{

		//	}
		//}

		public static void Run(double[] a, double[] b, int N)
		{
			Parallel.For(0, N, i => { a[i] += b[i]; });
		}

	}
}