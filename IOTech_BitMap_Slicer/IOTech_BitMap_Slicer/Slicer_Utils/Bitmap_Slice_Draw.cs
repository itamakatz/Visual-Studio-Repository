using g3;
using System;
using System.Drawing;

namespace IOTech_BitMap_Slicer
{
	partial class Bitmap_Slice
	{
		public void Draw_Line(int x0, int y0, int x1, int y1, bool draw_color = false)
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
				if (draw_color)
				{
					Set_RGB(x0, y0);
					bool_array[Bool_Index(x0, y0)] = false;
				}
				else { bool_array[Bool_Index(x0, y0)] = true; }

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

		public void Draw_Line(Vector2d origin_vec, Vector2d dest_vec, bool draw_color = false)
		{
			// ****** Possibly change to Ceiling or Round ****** //
			int x0 = (int)Math.Floor(origin_vec.x);
			int y0 = (int)Math.Floor(origin_vec.y);
			int x1 = (int)Math.Floor(dest_vec.x);
			int y1 = (int)Math.Floor(dest_vec.y);
			Draw_Line(x0, y0, x1, y1, draw_color);
		}

		public void Draw_Line_Color(int x0, int y0, int x1, int y1, Color temp_color)
		{
			byte_color = new byte[] { temp_color.B, temp_color.G, temp_color.R };
			Draw_Line(x0, y0, x1, y1, true);
			byte_color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R };
		}

		public void Draw_Line_Color(Vector2d origin_vec, Vector2d dest_vec, Color color)
		{
			// ****** Possibly change to Ceiling or Round ****** //
			int x0 = (int)Math.Floor(origin_vec.x);
			int y0 = (int)Math.Floor(origin_vec.y);
			int x1 = (int)Math.Floor(dest_vec.x);
			int y1 = (int)Math.Floor(dest_vec.y);
			Draw_Line_Color(x0, y0, x1, y1, color);
		}


		internal void Draw_rectangle(int width, int height)
		{
			if (Scaled)
			{
				height = height * V.SCALE_FACTOR - 1;
				width = width * V.SCALE_FACTOR - 1;
			}
			else
			{
				height = height - 1;
				width = width - 1;
			}

			Switch_to_byte_manipulation();

			Draw_Line(0, 0, 0, height);
			Draw_Line(0, height, width, height);
			Draw_Line(width, height, width, 0);
			Draw_Line(width, 0, 0, 0);
		}

		internal void Draw_X(Vector2d point, int length, Color color)
		{
			Switch_to_byte_manipulation();

			Draw_Line_Color(new Vector2d(point.x - length, point.y - length), new Vector2d(point.x + length, point.y + length), color);
			Draw_Line_Color(new Vector2d(point.x + length, point.y - length), new Vector2d(point.x - length, point.y + length), color);
		}

		public void Draw_Line_With_Graphics(Vector2d origin_vec, Vector2d dest_vec)
		{
			graphics.DrawLine(V.Pen, (float)origin_vec.x, (float)(bitmap_height - origin_vec.y),
									(float)dest_vec.x, (float)(bitmap_height - dest_vec.y));
		}
	}
}
