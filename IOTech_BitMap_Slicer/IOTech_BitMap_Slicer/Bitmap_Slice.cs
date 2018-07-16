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
		private int pixelFormatSize;
		private IntPtr pointer;

		private byte BYTE_MAP_VALUE = 100;

		public static int SCALE_FACTOR;
		public static int PEN_WIDTH { get; set; }
		public static System.Drawing.Color bitmap_color { get; set; }
		public static System.Drawing.Pen Pen;
		public static PixelFormat IMAGE_FORMAT;

		private System.Drawing.Graphics graphics;

		private bool[] bool_array;
		private int current_position;
		public int bitmap_width { get; set; }
		public int bitmap_height { get; set; }

		public static int recursive_count = 0;
		
		public Bitmap_Slice(double width, double height)
		{
			bitmap_width = get_int_dimension(width);
			bitmap_height = get_int_dimension(height);

			Bitmap tmp_bitmap = new Bitmap(bitmap_width, bitmap_height, IMAGE_FORMAT);
			
			pixelFormatSize = Image.GetPixelFormatSize(IMAGE_FORMAT) / 8;
			stride = bitmap_width * pixelFormatSize;
			bitmap_byte_array = new byte[stride * bitmap_height];
			handle = GCHandle.Alloc(bitmap_byte_array, GCHandleType.Pinned);
			pointer = Marshal.UnsafeAddrOfPinnedArrayElement(bitmap_byte_array, 0);
			bitmap = new Bitmap(bitmap_width, bitmap_height, stride, IMAGE_FORMAT, pointer);

			graphics = Graphics.FromImage(bitmap);
			graphics.DrawImageUnscaledAndClipped(tmp_bitmap, new Rectangle(0, 0, bitmap_width, bitmap_height));
			//graphics.Dispose();

			bool_array = new bool[bitmap_width * bitmap_height];
		}

		public Bitmap_Slice(Bitmap source_bitmap)
		{
			bitmap_width = source_bitmap.Width;
			bitmap_height = source_bitmap.Height;

			pixelFormatSize = Image.GetPixelFormatSize(IMAGE_FORMAT) / 8;
			stride = bitmap_width * pixelFormatSize;
			bitmap_byte_array = new byte[stride * bitmap_height];
			handle = GCHandle.Alloc(bitmap_byte_array, GCHandleType.Pinned);
			pointer = Marshal.UnsafeAddrOfPinnedArrayElement(bitmap_byte_array, 0);
			bitmap = new Bitmap(bitmap_width, bitmap_height, stride, IMAGE_FORMAT, pointer);

			graphics = Graphics.FromImage(bitmap);
			graphics.DrawImageUnscaledAndClipped(source_bitmap, new Rectangle(0, 0, bitmap_width, bitmap_height));
			graphics.Dispose();

			bool_array = new bool[bitmap_width * bitmap_height];
			bitmap_byte_array = bitmap_to_byte_array(source_bitmap);
		}

		//public Bitmap_Slice(Bitmap bitmap)
		//{
		//	this.bitmap = bitmap;
		//	bitmap_width = bitmap.Width;
		//	bitmap_height = bitmap.Height;
		//	bool_array = new bool[bitmap_width * bitmap_height];
		//	bitmap_byte_array = bitmap_to_byte_array(bitmap);
		//}

		public void flood_fill_recursive()
		{
			recursive_count++;

			int current_x = current_position % bitmap_width;
			int current_y = current_position / bitmap_width;

			bool_array[current_x + current_y * bitmap_width] = true;

			//if (!check_color_equal(bitmap.GetPixel(current_x, current_y), bitmap_color))
			//{
			//	bitmap.SetPixel(current_x, current_y, bitmap_color);
			//	bitmap_byte_array[current_x + current_y * bitmap_width] = byte_map_value;
			//}
			if (bitmap_byte_array[current_x + current_y * bitmap_width] != BYTE_MAP_VALUE)
			{
				bitmap_byte_array[current_x + current_y * bitmap_width] = BYTE_MAP_VALUE;
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
				while(!check_color_equal(bitmap.GetPixel(width, i), bitmap_color)) { i--; }
				i = width;
				while(!check_color_equal(bitmap.GetPixel(width, i), bitmap_color)) { i++; }
				i = width;
				while (!check_color_equal(bitmap.GetPixel(i, height), bitmap_color)) { i--; }
				i = height;
				while (!check_color_equal(bitmap.GetPixel(i, height), bitmap_color)) { i++; }
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
			graphics.DrawLine(Pen, (float)origin_vec.x, (float)(bitmap_height - origin_vec.y),
							(float)dest_vec.x, (float)(bitmap_height - dest_vec.y));
			//DrawLine_byte_array(origin_vec, dest_vec);
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

		public void save_byte_array_to_bitmap_image(string path, System.Drawing.Imaging.ImageFormat image_format)
		{
			Bitmap bmp;
			using (MemoryStream ms = new MemoryStream(bitmap_byte_array))
			{
				bmp = new Bitmap(ms);
				bmp.Save(path, image_format);
			}
		}

		public static byte[] bitmap_to_byte_array(Bitmap bmp)
		{
			byte[] byteArray = new byte[0];
			using (MemoryStream stream = new MemoryStream())
			{
				bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
				//bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
				stream.Close();

				byteArray = stream.ToArray();
			}
			return byteArray;
		}

		internal void Draw_rectangle(int width, int height)
		{
			graphics.DrawLine(Pen, 0, 0, 0, height);
			graphics.DrawLine(Pen, 0, height, width, height);
			graphics.DrawLine(Pen, width, height, width, 0);
			graphics.DrawLine(Pen, width, 0, 0, 0);
		}

		private Int32 get_int_dimension(double in_double)
		{
			return (Int32)Math.Ceiling(in_double + PEN_WIDTH * 2) * SCALE_FACTOR;
		}

		private void LockUnlockBitsExample()
		{

			// Create a new bitmap.
			Bitmap bitmap = new Bitmap("c:\\fakePhoto.jpg");

			// Lock the bitmap's bits.  
			Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			System.Drawing.Imaging.BitmapData bitmap_data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

			// Get the address of the first line.
			IntPtr ptr = bitmap_data.Scan0;

			// Declare an array to hold the bytes of the bitmap.
			int psize = Math.Abs(bitmap_data.Stride) * bitmap.Height;
			byte[] rgbValues = new byte[psize];

			// Copy the RGB values into the array.
			System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, psize);

			// Set every third value to 255. A 24bpp bitmap will look red.  
			for (int counter = 2; counter < rgbValues.Length; counter += 3)
				rgbValues[counter] = 255;

			// Copy the RGB values back to the bitmap
			System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, psize);

			// Unlock the bits.
			bitmap.UnlockBits(bitmap_data);

			//// Draw the modified image.
			////e.Graphics.DrawImage(bmp, 0, 150);
			//var pixelFormatSize = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
			//var stride = bitmap.Width * pixelFormatSize;
			//int padding = (stride % 4);
			//stride += padding == 0 ? 0 : 4 - padding;//pad out to multiple of 4
			//var byteArray = new SharedPinnedByteArray(stride * bitmap.Height);
			//bitmap = new Bitmap(bitmap.Width, bitmap.Height, stride, bitmap.PixelFormat, byteArray.bitPtr);
		}

		private void lala()
		{

			//creation routine
			pixelFormatSize = Image.GetPixelFormatSize(IMAGE_FORMAT) / 8;
			stride = bitmap.Width * pixelFormatSize;
			bitmap_byte_array = new byte[stride * bitmap.Height];
			handle = GCHandle.Alloc(bitmap_byte_array, GCHandleType.Pinned);
			pointer = Marshal.UnsafeAddrOfPinnedArrayElement(bitmap_byte_array, 0);
			Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, stride, IMAGE_FORMAT, pointer);

			Graphics g = Graphics.FromImage(bitmap2);
			//'source' is the source bitmap
			g.DrawImageUnscaledAndClipped(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
			g.Dispose();
		}

		public static void Run(double[] a, double[] b, int N)
		{
			Parallel.For(0, N, i => { a[i] += b[i]; });
		}

	}
}
