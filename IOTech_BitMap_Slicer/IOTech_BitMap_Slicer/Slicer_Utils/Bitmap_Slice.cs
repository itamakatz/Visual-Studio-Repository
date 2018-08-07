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
	partial class Bitmap_Slice
	{
		private Bitmap bitmap;
		public byte[] bitmap_byte_array;
		public bool[] bool_array;

		private Graphics graphics;
		private BitmapData bitmap_data;

		private IntPtr pointer;

		private bool On_byte_Manupulation { get; set; }

		private static byte[] byte_color;
		private static Color bitmap_color = V.bitmap_color;

		public static byte[] Byte_Color
		{
			get
			{
				if (byte_color != null) { return byte_color; }
				else
				{
					byte_color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R }; // BGR not RGB
					return byte_color;
				}
			}
			set { byte_color = value; }
		}

		public static Color Bitmap_Color
		{
			get { return bitmap_color; }
			set
			{
				bitmap_color = value;
				Byte_Color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R }; // BGR not RGB
			}
		}

		public Bitmap_Slice()
		{
			On_byte_Manupulation = false;

			bitmap = new Bitmap(V.Bitmap_Width, V.Bitmap_Height, V.PIXEL_FORMAT);
			graphics = Graphics.FromImage(bitmap);

			bool_array = new bool[V.Bitmap_Width * V.Bitmap_Height];

			Switch_to_byte_manipulation();
		}

		public Bitmap_Slice(Bitmap source_bitmap)
		{
			On_byte_Manupulation = false;

			bitmap = source_bitmap;
			graphics = Graphics.FromImage(bitmap);

			bool_array = new bool[V.Bitmap_Width * V.Bitmap_Height];

			Switch_to_byte_manipulation();
		}

		public Bitmap Bitmap
		{
			get
			{
				Switch_to_bitmap_manipulation();
				Bitmap return_bitmap = new Bitmap(bitmap);
				Switch_to_byte_manipulation();
				return return_bitmap;
			}
		}

		public void Update_bool_array()
		{
			for (int x = 0; x < V.Bitmap_Width; x++)
			{
				for (int y = 0; y < V.Bitmap_Height; y++)
				{
					int index = Byte_Index(x, y);
					if (bitmap_byte_array[index] != 0 || bitmap_byte_array[index + 1] != 0 || bitmap_byte_array[index + 2] != 0)
						{ bool_array[Bool_Index(x, y)] = true; }
				}
			}
		}

		private void Update_byte_array()
		{
			for (int x = 0; x < V.Bitmap_Width; x++)
			{
				for (int y = 0; y < V.Bitmap_Height; y++)
				{
					if (bool_array[Bool_Index(x, y)]) { Set_RGB(x, y); }
				}
			}
		}

		private void Switch_to_byte_manipulation()
		{
			if (!On_byte_Manupulation)
			{ 
				graphics.Dispose();
				Rectangle rect = new Rectangle(0, 0, V.Bitmap_Width, V.Bitmap_Height);

				V.single_pixel_num_of_byte = Image.GetPixelFormatSize(V.PIXEL_FORMAT) / 8;
				V.stride = V.Bitmap_Width * V.single_pixel_num_of_byte;
				int padding = (V.stride % 4);
				V.stride += padding == 0 ? 0 : 4 - padding; //pad out to multiple of 4 - CRITICAL

				bitmap_data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, V.PIXEL_FORMAT);
				pointer = bitmap_data.Scan0;
				V.im_num_of_bytes = V.Bitmap_Height * Math.Abs(bitmap_data.Stride);
				bitmap_byte_array = new byte[V.im_num_of_bytes];

				Marshal.Copy(pointer, bitmap_byte_array, 0, V.im_num_of_bytes);
				On_byte_Manupulation = true;
			}
		}

		private void Switch_to_bitmap_manipulation()
		{
			if (On_byte_Manupulation)
			{
				Update_byte_array();
				Marshal.Copy(bitmap_byte_array, 0, pointer, V.im_num_of_bytes);
				bitmap.UnlockBits(bitmap_data);
				graphics = Graphics.FromImage(bitmap);
				On_byte_Manupulation = false;
			}
		}

		public void Save_Bitmap(string path)
		{
			Switch_to_bitmap_manipulation();
			var bitmap2 = new Bitmap(bitmap);
			bitmap2.Save(path, V.IMAGE_FORMAT_EXTENSION);
		}

		private void Flood_fill_recursive(int x, int y)
		{
			if (bool_array[Bool_Index(x, y)]) { return; }

			bool_array[Bool_Index(x, y)] = true;

			if (x > 0) { Flood_fill_recursive(x - 1, y); }
			if (y > 0) { Flood_fill_recursive(x, y - 1); }
			if (x < (V.Bitmap_Width - 1)) { Flood_fill_recursive(x + 1, y); }
			if (y < (V.Bitmap_Height - 1)) { Flood_fill_recursive(x, y + 1); }
		}

		public void Flood_Fill(List<Vector2d> loop_vertices)
		{
			Switch_to_byte_manipulation();

			for (int i = 0; i < loop_vertices.Count; i++)
			{
				Vector2d origin_vec = loop_vertices[i];
				Vector2d dest_vec = loop_vertices[(i + 1) % loop_vertices.Count];

				Queue<Tuple<int, int>> starting_points = Get_starting_queue(origin_vec, dest_vec);

				Tuple<int, int> starting_point = find_starting_point(starting_points.Dequeue());
				if (starting_point != null) { Flood_fill_recursive(starting_point.Item1, starting_point.Item2); return; }

				Tuple<int, int> find_starting_point(Tuple<int, int> check_pair)
				{
					int check_x = check_pair.Item1;
					int check_y = check_pair.Item2;

					if (check_x < 0 || check_y < 0 || check_x >= V.Bitmap_Width || check_y >= V.Bitmap_Height)
					{
						if (starting_points.Count > 0) { return find_starting_point(starting_points.Dequeue()); }
						else { return null; }
					}

					if (bool_array[Bool_Index(check_x, check_y)])
					{
						if (starting_points.Count > 0) { return find_starting_point(starting_points.Dequeue()); }
						else { return null; }
					}

					bool x_right = false, x_left_ = false, y_up___ = false, y_down_ = false;


					for (int j = check_x; 0 <= j; j--)
					{
						if (bool_array[Bool_Index(j, check_y)] && j < V.Bitmap_Width - 1 && !bool_array[Bool_Index(j + 1, check_y)]) { x_left_ = !x_left_; }
					}
					for (int j = check_x; j < V.Bitmap_Width; j++)
					{
						if (bool_array[Bool_Index(j, check_y)] && 0 < j && !bool_array[Bool_Index(j - 1, check_y)]) { x_right = !x_right; }
					}

					for (int j = check_y; 0 <= j; j--)
					{
						if (bool_array[Bool_Index(check_x, j)] && j < V.Bitmap_Height - 1 && !bool_array[Bool_Index(check_x, j + 1)]) { y_down_ = !y_down_; }
					}
					for (int j = check_y; j < V.Bitmap_Height; j++)
					{
						if (bool_array[Bool_Index(check_x, j)] && 0 < j && !bool_array[Bool_Index(check_x, j - 1)]) { y_up___ = !y_up___; }
					}

					if (x_right && x_left_ && y_up___ && y_down_) { return check_pair; }
					else if (starting_points.Count > 0) { return find_starting_point(starting_points.Dequeue()); }

					return null;
				}
			}
		}

		public Queue<Tuple<int, int>> Get_starting_queue(Vector2d origin_vec, Vector2d dest_vec)
		{
			// possibly use ConcurrentQueue for parallelizm
			Queue<Tuple<int, int>> starting_points = new Queue<Tuple<int, int>>();

			double mid_x = Math.Abs(origin_vec.x + dest_vec.x) / 2;
			double mid_y = Math.Abs(origin_vec.y + dest_vec.y) / 2;

			int start_x1 = (int)Math.Floor(mid_x);
			int start_x2 = (int)Math.Ceiling(mid_x);

			//int start_x1 = (int)Math.Floor(mid_x) - 1;
			//int start_x2 = (int)Math.Ceiling(mid_x) + 1;

			int start_y1;
			int start_y2;

			if (origin_vec.y > dest_vec.y && origin_vec.x > dest_vec.x)
			{
				start_y1 = (int)Math.Ceiling(mid_y) + 1;
				start_y2 = (int)Math.Floor(mid_y) - 1;
			}
			else
			{
				start_y1 = (int)Math.Floor(mid_y) - 1;
				start_y2 = (int)Math.Ceiling(mid_y) + 1;
			}

			int range = 3;
			for (int i = -range; i <= range; i++)
			{
				for (int j = -range; j <= range; j++)
				{
					if (start_x1 + i >= 0 && start_y1 + j >= 0 && start_x1 + i < V.Bitmap_Width && start_y1 + j < V.Bitmap_Height)
					{
						starting_points.Enqueue(new Tuple<int, int>(start_x1 + i, start_y1 + j));
					}

					if (start_x2 + i >= 0 && start_y2 + j >= 0 && start_x2 + i < V.Bitmap_Width && start_y2 + j < V.Bitmap_Height)
					{
						starting_points.Enqueue(new Tuple<int, int>(start_x2 + i, start_y2 + j));
					}
				}
			}

			return starting_points;
		}

		private void Set_RGB(int x, int y, bool set_black = false)
		{
			int byte_array_index = x * V.single_pixel_num_of_byte + (V.Bitmap_Height - y - 1) * V.stride;

			if (!set_black)
			{
				bitmap_byte_array[byte_array_index] = Byte_Color[0];
				bitmap_byte_array[byte_array_index + 1] = Byte_Color[1];
				bitmap_byte_array[byte_array_index + 2] = Byte_Color[2];
				return;
			}

			bitmap_byte_array[byte_array_index] = 0;
			bitmap_byte_array[byte_array_index + 1] = 0;
			bitmap_byte_array[byte_array_index + 2] = 0;
		}


		public void Byte_XOR(ref Bitmap_Slice XOR)
		{
			this.Switch_to_byte_manipulation();
			XOR.Switch_to_byte_manipulation();

			for (int x = 0; x < V.Bitmap_Width; x++)
			{
				for (int y = 0; y < V.Bitmap_Height; y++)
				{
					if (this.bool_array[Bool_Index(x, y)] != XOR.bool_array[Bool_Index(x, y)])
					{
						this.bool_array[Bool_Index(x, y)] = true;
					}
					else if (this.bool_array[Bool_Index(x, y)])
					{
						this.bool_array[Bool_Index(x, y)] = false;
					}
				}
			}
		}

		// implemented for debugging. not used in actual algorithms due to heavy decrease in speed
		public static int Bool_Index(int x, int y) { return x + (V.Bitmap_Height - y - 1) * V.Bitmap_Width; }
		public static int Byte_Index(int x, int y) { return x * V.single_pixel_num_of_byte + (V.Bitmap_Height - y - 1) * V.stride; }
	}
}