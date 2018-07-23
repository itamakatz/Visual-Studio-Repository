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
		[NonSerialized]
		private Graphics graphics;
		private BitmapData bitmap_data;

		private int stride;
		private int single_pixel_num_of_byte;
		private IntPtr pointer;
		private int im_num_of_bytes;

		private readonly int bitmap_width;
		public readonly int bitmap_height;

		public bool Scaled { get; set; }
		private bool On_byte_Manupulation { get; set; }

		public static int SCALE_FACTOR;
		public static PixelFormat PIXEL_FORMAT;

		public static int PEN_WIDTH = 1;
		public static Pen Pen;
		private static byte[] byte_color;
		private static Color bitmap_color;

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

		public static Color Bitmap_Color
		{
			get { return bitmap_color; }
			set
			{
				bitmap_color = value;
				byte_color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R }; // BGR not RGB
			}
		}

		public void update_bool_array()
		{
			for (int x = 0; x < bitmap_width; x++)
			{
				for (int y = 0; y < bitmap_height; y++)
				{
					int index = Byte_Index(x, y);
					if (bitmap_byte_array[index] != 0 || bitmap_byte_array[index + 1] != 0 || bitmap_byte_array[index + 2] != 0)
						bool_array[Bool_Index(x, y)] = true;
				}
			}
		}

		private void update_byte_array()
		{
			for (int x = 0; x < bitmap_width; x++)
			{
				for (int y = 0; y < bitmap_height; y++)
				{
					if (bool_array[Bool_Index(x, y)])
						Set_RGB(x, y);
				}
			}
		}

		private void Switch_to_byte_manipulation()
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

		private void Switch_to_bitmap_manipulation()
		{
			if (On_byte_Manupulation)
			{
				update_byte_array();
				Marshal.Copy(bitmap_byte_array, 0, pointer, im_num_of_bytes);
				bitmap.UnlockBits(bitmap_data);
				graphics = Graphics.FromImage(bitmap);
				On_byte_Manupulation = false;
			}
		}

		public void Save_Bitmap(string path, ImageFormat IMAGE_FORMAT_EXTENSION)
		{
			// color point
			//byte_color = new byte[] { Color.Yellow.B, Color.Yellow.G, Color.Yellow.R };
			//Set_RGB(120, 178);
			//bool_array[Bool_Index(121, 178)] = false;
			//byte_color = new byte[] { bitmap_color.B, bitmap_color.G, bitmap_color.R };
			Switch_to_bitmap_manipulation();
			var bitmap2 = new Bitmap(bitmap);
			bitmap2.Save(path, IMAGE_FORMAT_EXTENSION);
		}

		private void Flood_fill_recursive(int x, int y)
		{
			if (bool_array[Bool_Index(x, y)]) { return; }

			bool_array[Bool_Index(x, y)] = true;

			if (x > 0) { Flood_fill_recursive(x - 1, y); }
			if (y > 0) { Flood_fill_recursive(x, y - 1); }
			if (x < (bitmap_width - 1)) { Flood_fill_recursive(x + 1, y); }
			if (y < (bitmap_height - 1)) { Flood_fill_recursive(x, y + 1); }
		}

		public void Flood_Fill(List<Vector2d> loop_vertices)
		{
			Switch_to_byte_manipulation();

			for (int i = 0; i < loop_vertices.Count; i++)
			{
				Vector2d origin_vec = loop_vertices[i];
				Vector2d dest_vec = loop_vertices[(i + 1) % loop_vertices.Count];

				Queue<Tuple<int, int>> starting_points = get_starting_queue(origin_vec, dest_vec);

				Tuple<int, int> starting_point = find_starting_point(starting_points.Dequeue());
				if (starting_point != null) { Flood_fill_recursive(starting_point.Item1, starting_point.Item2); return; }

				Tuple<int, int> find_starting_point(Tuple<int, int> check_pair)
				{
					int check_x = check_pair.Item1;
					int check_y = check_pair.Item2;

					if (check_x < 0 || check_y < 0 || check_x >= bitmap_width || check_y >= bitmap_height)
					{
						if (starting_points.Count > 0) { return find_starting_point(starting_points.Dequeue()); }
						else
						{
							//Util.Print_Messege(new string[] { "Starting coordiantes for flood fill are not bound" });
							return null;
						}
					}

					if (bool_array[Bool_Index(check_x, check_y)])
					{
						if (starting_points.Count > 0) { return find_starting_point(starting_points.Dequeue()); }
						else
						{
							//Util.Print_Messege(new string[] { "Starting coordiantes for flood fill are not bound" });
							return null;
						}
					}

					bool x_right = false, x_left_ = false, y_up___ = false, y_down_ = false;


					for (int j = check_x; 0 <= j; j--)
					{
						if (bool_array[Bool_Index(j, check_y)] && j < bitmap_width - 1 && !bool_array[Bool_Index(j + 1, check_y)]) { x_left_ = !x_left_; }
					}
					for (int j = check_x; j < bitmap_width; j++)
					{
						if (bool_array[Bool_Index(j, check_y)] && 0 < j && !bool_array[Bool_Index(j - 1, check_y)]) { x_right = !x_right; }
					}

					for (int j = check_y; 0 <= j; j--)
					{
						if (bool_array[Bool_Index(check_x, j)] && j < bitmap_height - 1 && !bool_array[Bool_Index(check_x, j + 1)]) { y_down_ = !y_down_; }
					}
					for (int j = check_y; j < bitmap_height; j++)
					{
						if (bool_array[Bool_Index(check_x, j)] && 0 < j && !bool_array[Bool_Index(check_x, j - 1)]) { y_up___ = !y_up___; }
					}

					if (x_right && x_left_ && y_up___ && y_down_) { return check_pair; }
					else if (starting_points.Count > 0) { return find_starting_point(starting_points.Dequeue()); }

					//Util.Print_Messege(new string[] { "Starting coordiantes for flood fill are not bound" });
					return null;
				}
			}
		}

		public Queue<Tuple<int, int>> get_starting_queue(Vector2d origin_vec, Vector2d dest_vec)
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
					if (start_x1 + i >= 0 && start_y1 + j >= 0 && start_x1 + i < bitmap_width && start_y1 + j < bitmap_height)
					{
						starting_points.Enqueue(new Tuple<int, int>(start_x1 + i, start_y1 + j));
					}

					if (start_x2 + i >= 0 && start_y2 + j >= 0 && start_x2 + i < bitmap_width && start_y2 + j < bitmap_height)
					{
						starting_points.Enqueue(new Tuple<int, int>(start_x2 + i, start_y2 + j));
					}
				}
			}

			return starting_points;
		}

		private void Set_RGB(int x, int y, bool set_black = false)
		{
			int byte_array_index = x * single_pixel_num_of_byte + (bitmap_height - y - 1) * stride;

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

		private int Get_Int_Dimension(double in_double)
		{
			if (Scaled) { return (int) Math.Ceiling(in_double + PEN_WIDTH * 2) * SCALE_FACTOR; }
			else		{ return (int) Math.Ceiling(in_double + PEN_WIDTH * 2); }
		}

		public void Byte_XOR(ref Bitmap_Slice XOR)
		{
			this.Switch_to_byte_manipulation();
			XOR.Switch_to_byte_manipulation();

			if (this.bitmap_width != XOR.bitmap_width ||
				this.bitmap_height != XOR.bitmap_height ||
				this.im_num_of_bytes != XOR.im_num_of_bytes)
			{ Util.exit_messege(new string[] { "XOR failed. Dimensions are not equal" }); }

			for (int x = 0; x < bitmap_width; x++)
			{
				for (int y = 0; y < bitmap_height; y++)
				{
					if (this.bool_array[Bool_Index(x, y)] != XOR.bool_array[Bool_Index(x, y)])
					{
						this.bool_array[Bool_Index(x, y)] = true;
						//this.Set_RGB(x, y);
					}
					else if (this.bool_array[Bool_Index(x, y)])
					{
						this.bool_array[Bool_Index(x, y)] = false;
						//this.Set_RGB(x, y, true); // true - set the color to black
					}
				}
			}
		}

		// implemented for debugging. not used in actual algorithms due to heavy decrease in speed
		public int Bool_Index(int x, int y) { return x + (bitmap_height - y - 1) * bitmap_width; }
		public int Byte_Index(int x, int y) { return x * single_pixel_num_of_byte + (bitmap_height - y - 1) * stride; }
	}
}