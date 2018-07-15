using g3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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
		private static System.Drawing.Pen Pen_Fine;
		private static System.Drawing.Pen Pen_Robust;
		private System.Drawing.Graphics graphics;

		public Bitmap_Slice(double width, double height)
		{

			this.bitmap = new Bitmap(get_int_dimension(width), get_int_dimension(height));
			Pen_Fine = new System.Drawing.Pen(bitmap_color, PEN_FINE_WIDTH);
			Pen_Robust = new System.Drawing.Pen(bitmap_color, PEN_ROBUST_WIDTH);
			graphics = Graphics.FromImage(bitmap);
		}

		public Bitmap_Slice(Bitmap bitmap)
		{
			this.bitmap = bitmap;
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

		private static int count_1 = 0;
		private static int count_2 = 0;
		private static int count_3 = 0;
		private static int count_4 = 0;

		public void flood_fill_recursive(ref Color objective_color, int begin_x, int begin_y)
		{
			Color curren_colort = bitmap.GetPixel(begin_x, begin_y);

			if (!check_color_equal(curren_colort, objective_color))
			{
				bitmap.SetPixel(begin_x, begin_y, objective_color);
			}
			else
			{
				return;
			}

			if (count_3 == 3273)
			{
				Trace.WriteLine("yo yo babyyy");
			}

			if (begin_x > 0)
			{
				count_1++;
				flood_fill_recursive(ref objective_color, begin_x - 1, begin_y);
			}
			if (begin_y > 0)
			{
				count_2++;
				flood_fill_recursive(ref objective_color, begin_x, begin_y - 1);
			}
			if (begin_x < bitmap.Size.Width - 1)
			{
				count_3++;
				flood_fill_recursive(ref objective_color, begin_x + 1, begin_y);
			}
			if (begin_y < bitmap.Size.Height - 1)
			{
				count_2++;
				flood_fill_recursive(ref objective_color, begin_x, begin_y + 1);
			}
		}

		private bool check_color_equal(Color c1, Color c2)
		{
			return (c1.A == c2.A && c1.R == c2.R && c1.G == c2.G && c1.B == c2.B) ? true : false;
		}
	}
}
