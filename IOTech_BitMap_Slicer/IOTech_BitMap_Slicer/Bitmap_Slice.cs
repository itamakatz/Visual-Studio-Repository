using g3;
using System;
using System.Collections;
using System.Collections.Generic;
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
		private Graphics graphics;
		private BitmapData bitmap_data;

		private int stride;
		private int single_pixel_num_of_byte;
		private IntPtr pointer;
		private int im_num_of_bytes;

		private readonly int bitmap_width;
		private readonly int bitmap_height;

		public bool Scaled { get; set; }
		private bool On_byte_Manupulation { get; set; }

		public static int SCALE_FACTOR;
		public static PixelFormat PIXEL_FORMAT;

		public static int PEN_WIDTH = 1;
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

		public Bitmap_Slice(double width, double height)
		{
			Scaled = true;
			On_byte_Manupulation = false;

			bitmap_width = Get_Int_Dimension(width);
			bitmap_height = Get_Int_Dimension(height);

			bitmap = new Bitmap(bitmap_width, bitmap_height, PIXEL_FORMAT);
			graphics = Graphics.FromImage(bitmap);

			bool_array = new bool[bitmap_width * bitmap_height];

			Switch_to_byte_manipulation();
		}

		public Bitmap_Slice(Bitmap source_bitmap)
		{
			Scaled = false;
			On_byte_Manupulation = false;

			bitmap_width = source_bitmap.Width;
			bitmap_height = source_bitmap.Height;

			bitmap = source_bitmap;
			graphics = Graphics.FromImage(bitmap);

			bool_array = new bool[bitmap_width * bitmap_height];

			Switch_to_byte_manipulation();

			//update_bool_array();

		}

		private void update_bool_array()
		{
			for (int bool_arr_i = 0, x = 0, y = 0; bool_arr_i < bool_array.Length; bool_arr_i++)
			{
				x = bool_arr_i % bitmap_width;
				y = bool_arr_i / bitmap_width;

				for (int i = 0; i < single_pixel_num_of_byte; i++)
				{
					if (bitmap_byte_array [x * single_pixel_num_of_byte + y * stride] != 0)
					{
						bool_array[bool_arr_i] = true;
						break;
					}
				}
			}
		}

		public void Switch_to_byte_manipulation()
		{
			if (!On_byte_Manupulation)
			{ 
				graphics.Dispose();
				Rectangle rect = new Rectangle(0, 0, bitmap_width, bitmap_height);

				single_pixel_num_of_byte = Image.GetPixelFormatSize(PIXEL_FORMAT) / 8;
				stride = bitmap_width * single_pixel_num_of_byte;
				int padding = (stride % 4);
				stride += padding == 0 ? 0 : 4 - padding; //pad out to multiple of 4 - CRITICAL

				bitmap_data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PIXEL_FORMAT);
				pointer = bitmap_data.Scan0;
				im_num_of_bytes = bitmap_height * Math.Abs(bitmap_data.Stride);
				bitmap_byte_array = new byte[im_num_of_bytes];

				Marshal.Copy(pointer, bitmap_byte_array, 0, im_num_of_bytes);
				On_byte_Manupulation = true;
			}
		}

		public void Switch_to_bitmap_manipulation()
		{
			if (On_byte_Manupulation)
			{
				Marshal.Copy(bitmap_byte_array, 0, pointer, im_num_of_bytes);
				bitmap.UnlockBits(bitmap_data);
				graphics = Graphics.FromImage(bitmap);
				On_byte_Manupulation = false;
			}
		}

		public void Save_Bitmap(string path, ImageFormat IMAGE_FORMAT_EXTENSION)
		{
			Switch_to_bitmap_manipulation();
			bitmap.Save(path, IMAGE_FORMAT_EXTENSION);
		}

		public void Flood_fill_recursive(int current_x, int current_y)
		{
			bool_array[current_x + current_y * bitmap_width] = true;

			if (RGB_Equal(current_x * single_pixel_num_of_byte + current_y * stride)) return;

			Set_RGB(current_x * single_pixel_num_of_byte + current_y * stride);

			if (current_x > 0 && !bool_array[(current_x - 1) + current_y * bitmap_width])
				Flood_fill_recursive(current_x - 1, current_y);

			if (current_y > 0 && !bool_array[current_x + (current_y - 1) * bitmap_width])
				Flood_fill_recursive(current_x, current_y - 1);

			if (current_x < (bitmap_width - 1) && !bool_array[(current_x + 1) + current_y * bitmap_width])
				Flood_fill_recursive(current_x + 1, current_y);

			if (current_y < (bitmap_height - 1) && !bool_array[current_x + (current_y + 1) * bitmap_width])
				Flood_fill_recursive(current_x, current_y + 1);
		}

		public void Flood_Fill(Vector2d origin_vec, Vector2d dest_vec)
		{
			Switch_to_byte_manipulation();

			double mid_x = Math.Abs(origin_vec.x + dest_vec.x) / 2;
			double mid_y = Math.Abs(origin_vec.y + dest_vec.y) / 2;

			int start_x1 = (int) Math.Floor(mid_x) - 1;
			int start_x2 = (int) Math.Ceiling(mid_x) + 1;

			int start_y1;
			int start_y2;


			if (origin_vec.y > dest_vec.y && origin_vec.x > dest_vec.x)
			{
				start_y1 = (int) Math.Ceiling(mid_y) + 1;
				start_y2 = (int) Math.Floor(mid_y) - 1;
			}
			else
			{
				start_y1 = (int) Math.Floor(mid_y) - 1;
				start_y2 = (int) Math.Ceiling(mid_y) + 1;
			}

			try
			{
				int i = start_x1;
				while (!bool_array[i + start_y1 * bitmap_width] && !RGB_Equal(i * single_pixel_num_of_byte + start_y1 * stride)) { i--; }
				i = start_x1;
				while (!bool_array[i + start_y1 * bitmap_width] && !RGB_Equal(i * single_pixel_num_of_byte + start_y1 * stride)) { i++; }
				i = start_y1;
				while (!bool_array[start_x1 + i * bitmap_width] && !RGB_Equal(start_x1 * single_pixel_num_of_byte + i * stride)) { i--; }
				i = start_y1;
				while (!bool_array[start_x1 + i * bitmap_width] && !RGB_Equal(start_x1 * single_pixel_num_of_byte + i * stride)) { i++; }

			}
			catch (Exception)
			{
				try
				{
					int i = start_x2;
					while (!bool_array[i + start_y2 * bitmap_width] && !RGB_Equal(i * single_pixel_num_of_byte + start_y2 * stride)) { i--; }
					i = start_x2;
					while (!bool_array[i + start_y2 * bitmap_width] && !RGB_Equal(i * single_pixel_num_of_byte + start_y2 * stride)) { i++; }
					i = start_y2;
					while (!bool_array[start_x2 + i * bitmap_width] && !RGB_Equal(start_x2 * single_pixel_num_of_byte + i * stride)) { i--; }
					i = start_y2;
					while (!bool_array[start_x2 + i * bitmap_width] && !RGB_Equal(start_x2 * single_pixel_num_of_byte + i * stride)) { i++; }

				}
				catch (Exception)
				{
					throw new System.ArgumentException("Starting coordiantes for flood fill are not bound");
				}

				Flood_fill_recursive(start_x2, start_y2);
				return;
			}

			Flood_fill_recursive(start_x1, start_y1);
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

		private void Set_RGB(int byte_array_index, bool set_black = false)
		{
			if (!set_black)
			{
				bitmap_byte_array[byte_array_index] = byte_color[0];
				bitmap_byte_array[byte_array_index + 1] = byte_color[1];
				bitmap_byte_array[byte_array_index + 2] = byte_color[2];
				return;
			}

			bitmap_byte_array[byte_array_index] = 0;
			bitmap_byte_array[byte_array_index + 1] = 0;
			bitmap_byte_array[byte_array_index + 2] = 0;
		}

		public void Draw_Line_On_Bitmap(Vector2d origin_vec, Vector2d dest_vec)
		{
			graphics.DrawLine(Pen, (float)origin_vec.x, (float)(bitmap_height - origin_vec.y),
							(float)dest_vec.x, (float)(bitmap_height - dest_vec.y));
		}



		public void Draw_Line(Vector2d origin_vec, Vector2d dest_vec)
		{
			// ****** Possibly change to Ceiling or Round ****** //
			int x0 = (int) Math.Floor(origin_vec.x);
			int y0 = (int) Math.Floor(origin_vec.y);
			int x1 = (int) Math.Floor(dest_vec.x);
			int y1 = (int) Math.Floor(dest_vec.y);
			Draw_Line(x0, y0, x1, y1);
		}

		public static int Draw_Line_pixel_count = 0;

		public void Draw_Line(int x0, int y0, int x1, int y1)
		{
			int w = x1 - x0;
			int h = y1 - y0;
			int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
			if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
			if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
			if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
			int longest = Math.Abs(w);
			int shortest = Math.Abs(h);
			if (!(longest > shortest))
			{
				longest = Math.Abs(h);
				shortest = Math.Abs(w);
				if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
				dx2 = 0;
			}
			int numerator = longest >> 1;
			for (int i = 0; i <= longest; i++)
			{
				Set_RGB(x0 * single_pixel_num_of_byte + (bitmap_height - y0) * stride);
				bool_array[x0 + (bitmap_height - y0) * bitmap_width] = true;
				Draw_Line_pixel_count++;
				numerator += shortest;
				if (!(numerator < longest))
				{
					numerator -= longest;
					x0 += dx1;
					y0 += dy1;
				}
				else
				{
					x0 += dx2;
					y0 += dy2;
				}
			}
		}

		public void Draw_Line(int x0, int y0, int x1, int y1, Color temp_color)
		{
			byte_color = new byte[] { temp_color.B, temp_color.G, temp_color.R };
			Draw_Line(x0, y0, x1, y1);
			byte_color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R };
		}

		public void Draw_Line(Vector2d origin_vec, Vector2d dest_vec, Color temp_color)
		{
			// ****** Possibly change to Ceiling or Round ****** //
			int x0 = (int) Math.Floor(origin_vec.x);
			int y0 = (int) Math.Floor(origin_vec.y);
			int x1 = (int) Math.Floor(dest_vec.x);
			int y1 = (int) Math.Floor(dest_vec.y);
			Draw_Line(x0, y0, x1, y1, temp_color);
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

			// might already be in that state, but it is checked there
			Switch_to_byte_manipulation();

			Draw_Line(0, 0, 0, height);
			Draw_Line(0, height, width, height);
			Draw_Line(width, height, width, 0);
			Draw_Line(width, 0, 0, 0);
		}

		internal void Draw_X(Vector2d point, int length, Color color)
		{
			// might already be in that state, but it is checked there
			Switch_to_byte_manipulation();

			Draw_Line(new Vector2d (point.x - length, point.y - length), new Vector2d(point.x + length, point.y + length), color);
			Draw_Line(new Vector2d (point.x + length, point.y - length), new Vector2d(point.x - length, point.y + length), color);
		}

		private int Get_Int_Dimension(double in_double)
		{
			if (Scaled)
			{
				return (int) Math.Ceiling(in_double + PEN_WIDTH * 2) * SCALE_FACTOR;
			}
			else
			{
				return (int) Math.Ceiling(in_double + PEN_WIDTH * 2);
			}
		}

		public void Byte_XOR(ref Bitmap_Slice XOR)
		{
			this.Switch_to_byte_manipulation();
			XOR.Switch_to_byte_manipulation();

			if (this.bitmap_width != XOR.bitmap_width ||
				this.bitmap_height != XOR.bitmap_height ||
				this.im_num_of_bytes != XOR.im_num_of_bytes)
			{
				Util.exit_messege(new string[] { "XOR failed. Dimensions are not equal" });
			}

			int current_x;
			int current_y;

			for (int bool_arr_i = 0; bool_arr_i < this.bool_array.Length; bool_arr_i++)
			{
				current_x = bool_arr_i % this.bitmap_width;
				current_y = bool_arr_i / this.bitmap_width;
				if (this.bool_array[bool_arr_i] != XOR.bool_array[bool_arr_i])
				{
					this.bool_array[bool_arr_i] = true;
					this.Set_RGB(current_x * this.single_pixel_num_of_byte + current_y * this.stride);
				}
				else if (!this.bool_array[bool_arr_i])
				{
					this.bool_array[bool_arr_i] = false;
					this.Set_RGB(current_x * this.single_pixel_num_of_byte + current_y * this.stride, true);
				}
			}
		}

		public static void Run(double[] a, double[] b, int N)
		{
			Parallel.For(0, N, i => { a[i] += b[i]; });
		}

	}
}