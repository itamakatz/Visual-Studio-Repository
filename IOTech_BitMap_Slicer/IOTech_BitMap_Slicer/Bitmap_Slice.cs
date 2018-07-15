using g3;
using System;
using System.Collections.Generic;
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

		public Bitmap_Slice(double width, double height)
		{
			bitmap = new Bitmap(get_int_dimension(width), get_int_dimension(height));
			Pen_Fine = new System.Drawing.Pen(bitmap_color, PEN_FINE_WIDTH);
			Pen_Robust = new System.Drawing.Pen(bitmap_color, PEN_ROBUST_WIDTH);
		}	

	public void DrawLineInt(Vector2d origin_vec, Vector2d dest_vec)
		{
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.DrawLine(Pen_Fine, (float)origin_vec.x, (float)(bitmap.Size.Height - origin_vec.y),
											(float)dest_vec.x, (float)(bitmap.Size.Height - dest_vec.y));
			}
		}

		private Int32 get_int_dimension(double in_double)
		{
			return (Int32)Math.Ceiling(in_double + PEN_ROBUST_WIDTH * 2) * SCALE_FACTOR;
		}

		internal void Draw_rectangle(int width, int height)
		{
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.DrawLine(Pen_Robust, 0, 0, 0, height);
				graphics.DrawLine(Pen_Robust, 0, height, width, height);
				graphics.DrawLine(Pen_Robust, width, height, width, 0);
				graphics.DrawLine(Pen_Robust, width, 0, 0, 0);
			}
		}
	}
}
