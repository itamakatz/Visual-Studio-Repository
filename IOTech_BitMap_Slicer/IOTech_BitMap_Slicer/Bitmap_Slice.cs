using g3;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOTech_BitMap_Slicer
{
	class Bitmap_Slice
	{
		public Bitmap bitmap { get; }
		public static int SCALE_FACTOR;
		public static int PEN_FINE_WIDTH { get; set; }
		public static int PEN_ROBUST_WIDTH { get; set; }
		public static System.Drawing.Color bitmap_color { get; set; }
		public static System.Drawing.Pen Pen_Fine;
		public static System.Drawing.Pen Pen_Robust;
		private System.Drawing.Graphics graphics;

		private bool[] bool_array;
		private int current_position;
		private int bitmap_width;

		public static int recursive_count = 0;
		
		public Bitmap_Slice(double width, double height)
		{
			bitmap = new Bitmap(get_int_dimension(width), get_int_dimension(height));
			graphics = Graphics.FromImage(bitmap);
			bitmap_width = bitmap.Width;
			bool_array = new bool[bitmap.Height * bitmap_width];
		}

		public Bitmap_Slice(Bitmap bitmap)
		{
			this.bitmap = bitmap;
			bitmap_width = bitmap.Width;
			bool_array = new bool[bitmap.Height * bitmap_width];
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
			if (current_x < bitmap.Size.Width - 1)
			{
				current_position = (current_x + 1) + current_y * bitmap_width;
				if (!bool_array[current_position])
				{
					flood_fill_recursive();
				}
			}
			if (current_y < bitmap.Size.Height - 1)
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
			current_position = width + height * bitmap_width;
		}

		private bool check_color_equal(Color c1, Color c2)
		{
			return (c1.A == c2.A && c1.R == c2.R && c1.G == c2.G && c1.B == c2.B) ? true : false;
		}

		public void DrawLineInt(Vector2d origin_vec, Vector2d dest_vec)
		{
			graphics.DrawLine(Pen_Fine, (float)origin_vec.x, (float)(bitmap.Size.Height - origin_vec.y),
							(float)dest_vec.x, (float)(bitmap.Size.Height - dest_vec.y));
		}

		private Int32 get_int_dimension(double in_double)
		{
			return (Int32)Math.Ceiling(in_double + PEN_ROBUST_WIDTH * 2) * SCALE_FACTOR;
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
