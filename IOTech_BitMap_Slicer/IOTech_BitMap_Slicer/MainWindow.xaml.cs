//#define PAINT_BITMAP_BORDERS
//#define RUN_VISUAL
#define SHOW_LOCATION_OF_FLOOD_STARTING_POINT
//#define DEBUG_FILLING_POINT
//#define DEBUG_XOR

using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Drawing;
using HelixToolkit.Wpf;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace IOTech_BitMap_Slicer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		// ***** Variables that can be changed ***** //

		internal static string MODEL_IN_PATH = @"Desktop\BitMap_Slicer\CorvinCastle.stl";
		internal static string MODEL_OUT_PATH = @"Desktop\BitMap_Slicer\CorvinCastle_new.stl";

		internal static string BITMAP_DIR_PREFIX = @"Desktop\BitMap_Slicer\BITMAP_slice";
		private const string BITMAP_PATH_SUFIX = @".Bmp";
		private static ImageFormat IMAGE_FORMAT_EXTENSION = ImageFormat.Bmp;

		private const int SCALE_FACTOR = 15;
		private const int NUM_OF_SLICES = 3;
		private const Axis SLICING_AXIS = Axis.X;

		private const int EXIT_CODE = 10;

		private const int PEN_FINE_WIDTH = 1;

		private static Color bitmap_color = Color.Magenta;
		//private static Color bitmap_color = Color.White;

		/* CAN NOT BE OF THE FORMAT TYPE : (see https://stackoverflow.com/questions/11368412/lowering-bitmap-quality-produces-outofmemoryexception)
		 * Undefined
		 * DontCare
		 * Format16bppArgb1555
		 * Format16bppGrayScale
		 * Format1bppIndexed
		 * Format4bppIndexed
		 * Format8bppIndexed
		 * 
		 * Formats that work well:
		 * Format32bppRgb - works but contains unnecessary alpha channel
		 * Format24bppRgb - best option. only has RGB color (though in reversed order namely BGR)
		 * Format16bppRgb555 - works and uses even less bytes thought for the comparison of the colors the implementation is complicated
		 */
		// maybe should varify the image is infact Format32bppRgb?
		private static PixelFormat PIXEL_FORMAT = PixelFormat.Format24bppRgb; // best format I found

		//private const int stackSize = 2147483647;  // max Int32 = 2147483647 
		private const int stackSize = 1000000000;  // max Int32 = 2147483647 

		// ***** Initialization of other variables ***** //

		internal static string USER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

		private static Tuple<double, double> Bitmap_dimensions;
		private static Tuple<double, double> Mesh_min_dimensions;

		internal static int slice_count = 1;
		internal static int loop_count = 1;

		private enum Axis { X, Y, Z };

		public MainWindow()
		{

			// initialization and declaration of variables

			Stopwatch watch_all_program = new Stopwatch();

			watch_all_program.Reset();
			watch_all_program.Start();

			InitializeComponent(); // default initialization of WPF

			Util.Check_directories();

			DMesh3 Imported_STL_mesh = null;
			Model3D HelixToolkit_Model3D = null;
			Vector3d SLICING_DIRECTION_UNIT;

			Bitmap_Slice.SCALE_FACTOR = SCALE_FACTOR;
			Bitmap_Slice.Bitmap_Color = bitmap_color;
			Bitmap_Slice.PEN_WIDTH = (int) PEN_FINE_WIDTH;
			Bitmap_Slice.Pen = new Pen(bitmap_color, PEN_FINE_WIDTH);
			Bitmap_Slice.PIXEL_FORMAT = PIXEL_FORMAT;

			Util.EXIT_CODE = EXIT_CODE;

			System.IO.File.WriteAllText(@"C:\Users\admin\Desktop\BitMap_Slicer\debug.txt", string.Empty);

			// Open STL file and create DMesh3 objects
			DMesh3Builder DMesh3_builder = new DMesh3Builder();
			StandardMeshReader reader = new StandardMeshReader() { MeshBuilder = DMesh3_builder };
			IOReadResult result = reader.Read(MODEL_IN_PATH, ReadOptions.Defaults);
			if (result.code == IOCode.Ok)
			{
				Imported_STL_mesh = DMesh3_builder.Meshes[0];
			}
			else
			{
				Util.exit_messege(new string[] { "DMesh3Builder faild to open stl model" , "USER_PATH is - " + USER_PATH , "MODEL_IN_PATH is - " + MODEL_IN_PATH });
			}

			Vector3d STL_mesh_Diagonal = Imported_STL_mesh.CachedBounds.Diagonal;

			IEnumerable<double> Slice_Enumerator = Util.Range_Enumerator(
									Imported_STL_mesh.CachedBounds.Min[(int)SLICING_AXIS],
									Imported_STL_mesh.CachedBounds.Max[(int)SLICING_AXIS],
									NUM_OF_SLICES);

			switch (SLICING_AXIS)
			{
				case Axis.X:

					Bitmap_dimensions = Tuple.Create(STL_mesh_Diagonal.y, STL_mesh_Diagonal.z);
					Mesh_min_dimensions = Tuple.Create(Imported_STL_mesh.CachedBounds.Min.y, Imported_STL_mesh.CachedBounds.Min.z);

					SLICING_DIRECTION_UNIT = Vector3d.AxisX;

					break;
				case Axis.Y:

					Bitmap_dimensions = Tuple.Create(STL_mesh_Diagonal.x, STL_mesh_Diagonal.z);
					Mesh_min_dimensions = Tuple.Create(Imported_STL_mesh.CachedBounds.Min.x, Imported_STL_mesh.CachedBounds.Min.z);

					SLICING_DIRECTION_UNIT = Vector3d.AxisY;

					break;
				case Axis.Z:

					Bitmap_dimensions = Tuple.Create(STL_mesh_Diagonal.x, STL_mesh_Diagonal.y);
					Mesh_min_dimensions = Tuple.Create(Imported_STL_mesh.CachedBounds.Min.x, Imported_STL_mesh.CachedBounds.Min.y);

					SLICING_DIRECTION_UNIT = Vector3d.AxisZ;

					break;
				default:
					break;
			}

			// Cut mesh model and save as STL file
			MeshPlaneCut plane_cut_cross_section = null;
			foreach (double slice_step in Slice_Enumerator)
			{
				plane_cut_cross_section = new MeshPlaneCut(new DMesh3(Imported_STL_mesh), SLICING_DIRECTION_UNIT * slice_step, SLICING_DIRECTION_UNIT);
				plane_cut_cross_section.Cut();
				Create_Bitmap(plane_cut_cross_section);
			}

#if RUN_VISUAL

			//second_cross_section = new MeshPlaneCut(main_cross_section.Mesh, plane_origine - 0.01F, SLICING_NORMAL * -1);
			//second_cross_section.Cut();

			StandardMeshWriter.WriteFile(MODEL_OUT_PATH, new List<WriteMesh>() { new WriteMesh(plane_cut_cross_section.Mesh) }, WriteOptions.Defaults);

			// Using HelixToolkit show 3D object on GUI
			ModelVisual3D device3D = new ModelVisual3D();
			try
			{
				ModelImporter import = new ModelImporter();
				HelixToolkit_Model3D = import.Load(MODEL_OUT_PATH);
			}
			catch (Exception e)
			{
				// MessageBox.Show("Exception Error : " + e.StackTrace);
				Util.exit_messege(new string[] { "Exception loading HelixToolkit Model3D" }, e);
			}

#if SHOW_BOUNDS
			ModelVisual3D model_visual = new ModelVisual3D();
			
			MeshGeometry3D mymesh = new MeshGeometry3D();

			var dim_x = HelixToolkit_Model3D.Bounds.SizeX;
			var dim_y = HelixToolkit_Model3D.Bounds.SizeY;
			var dim_z = HelixToolkit_Model3D.Bounds.SizeZ;

			//mymesh.Positions.Add(new Point3D(0, 0, 0));
			//mymesh.Positions.Add(new Point3D(0, dim_y, 0));
			//mymesh.Positions.Add(new Point3D(dim_x, 0, 0));
			//mymesh.Positions.Add(new Point3D(dim_x, dim_y, 0));
			mymesh.Positions.Add(new Point3D(0, 0, dim_z));
			mymesh.Positions.Add(new Point3D(0, dim_y, dim_z));
			mymesh.Positions.Add(new Point3D(dim_x, 0, dim_z));
			mymesh.Positions.Add(new Point3D(dim_x, dim_y, dim_z));

			Model3DGroup my_Model3DGroup2 = new Model3DGroup();
			Model3D my_new_Model3D = new GeometryModel3D(mymesh, new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 128, 0))));
			model_visual.Content = my_new_Model3D;

			model_visual.Children.Add(device3D);
			my_3d_view.Children.Add(model_visual);

#endif

			// ORIGINAL view port
			device3D.Content = HelixToolkit_Model3D;

			//Set GUI properties
			my_3d_view.RotateGesture = new MouseGesture(MouseAction.LeftClick);
			my_3d_view.Children.Add(device3D);
#endif
			watch_all_program.Stop();

			Util.Print_elapsed(watch_all_program.Elapsed);
		}

		private static void Create_Bitmap(MeshPlaneCut cross_section)
		{
			List<EdgeLoop> cutLoops = cross_section.CutLoops;
			List<EdgeSpan> cutSpans = cross_section.CutSpans;

			loop_count = 1;

			// how come we do not need Bitmap_dimensions.x * SCALE_FACTOR I simply dont understand. After all we do multiply in
			//							loop_vertices.Add((vec3d.xz - Mesh_min_dimensions) * SCALE_FACTOR + PEN_WIDTH);
			Bitmap_Slice main_bitmap = new Bitmap_Slice(Bitmap_dimensions.Item1, Bitmap_dimensions.Item2);
			Bitmap_Slice temp_bitmap = new Bitmap_Slice(Bitmap_dimensions.Item1, Bitmap_dimensions.Item2);

#if PAINT_BITMAP_BORDERS

			temp_bitmap.Draw_rectangle(bitmap_slice.bitmap.Width, bitmap_slice.bitmap.Height);

#endif

			foreach (EdgeLoop edgeLoop in cutLoops)
			{

				DCurve3 cutLoop_Curve = edgeLoop.ToCurve();
				List<Vector2d> loop_vertices = new List<Vector2d>();

				Tuple<double, double> create_Vector2d = new Tuple<double, double>(0,0);

				double x, y, z;

				foreach (Vector3d vec3d in cutLoop_Curve.Vertices)
				{
					switch (SLICING_AXIS)
					{
						case Axis.X:
							y = (vec3d.y - Mesh_min_dimensions.Item1) * SCALE_FACTOR + PEN_FINE_WIDTH;
							//z = temp_bitmap.bitmap_height - (vec3d.z - Mesh_min_dimensions.Item2) * SCALE_FACTOR + PEN_FINE_WIDTH;
							z = (vec3d.z - Mesh_min_dimensions.Item2) * SCALE_FACTOR + PEN_FINE_WIDTH;
							loop_vertices.Add(new Vector2d(y, z));
							break;
						case Axis.Y:
							x = (vec3d.x - Mesh_min_dimensions.Item1) * SCALE_FACTOR + PEN_FINE_WIDTH;
							z = (vec3d.z - Mesh_min_dimensions.Item2) * SCALE_FACTOR + PEN_FINE_WIDTH;
							loop_vertices.Add(new Vector2d(x, z));
							break;
						case Axis.Z:
							x = (vec3d.x - Mesh_min_dimensions.Item1) * SCALE_FACTOR + PEN_FINE_WIDTH;
							y = (vec3d.y - Mesh_min_dimensions.Item2) * SCALE_FACTOR + PEN_FINE_WIDTH;
							loop_vertices.Add(new Vector2d(x, y));
							break;
						default:
							break;
					}
				}

				for (int i = 0; i < cutLoop_Curve.VertexCount; i++)
				{
					temp_bitmap.Draw_Line(loop_vertices[i], loop_vertices[(i + 1) % cutLoop_Curve.VertexCount]);
				}

#if DEBUG_FILLING_POINT
				Debug_Filling_Point(new Bitmap_Slice(temp_bitmap.Bitmap), loop_vertices[0], loop_vertices[1]);
#endif
				Thread thread = new Thread(() => temp_bitmap.Flood_Fill(loop_vertices[0], loop_vertices[1]), stackSize);
				thread.Start();
				thread.Join();


				main_bitmap.Byte_XOR(ref temp_bitmap);

#if DEBUG_XOR
				temp_bitmap.Save_Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + (slice_count) + "_loop_" + (loop_count) + "_b_temp_bitmap" + BITMAP_PATH_SUFIX, IMAGE_FORMAT_EXTENSION);
				main_bitmap.Save_Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + (slice_count) + "_loop_" + (loop_count++) + "_c_main_bitmap" + BITMAP_PATH_SUFIX, IMAGE_FORMAT_EXTENSION);
#endif
				temp_bitmap = new Bitmap_Slice(Bitmap_dimensions.Item1, Bitmap_dimensions.Item2);
			}

			main_bitmap.Save_Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + (slice_count) + BITMAP_PATH_SUFIX, IMAGE_FORMAT_EXTENSION);
			slice_count++;
		}

#if DEBUG_FILLING_POINT
		private static void Debug_Filling_Point(Bitmap_Slice debug_bitmap, Vector2d _loop_vertex_1, Vector2d _loop_vertex_2)
		{
			debug_bitmap.update_bool_array();

			using (StreamWriter file = new StreamWriter(@"C:\Users\admin\Desktop\BitMap_Slicer\debug.txt", true))
			{
				int mark_length = 1;

				int near_point_x1;
				int near_point_x2;
				int near_point_y1;
				int near_point_y2;

				int on_loop_x1 = (int)Math.Floor(_loop_vertex_1.x);
				int on_loop_y1 = (int)Math.Floor(_loop_vertex_1.y);

				int on_loop_x2 = (int)Math.Floor(_loop_vertex_2.x);
				int on_loop_y2 = (int)Math.Floor(_loop_vertex_2.y);

				get_dear_indices(_loop_vertex_1, _loop_vertex_2);

				int bool_on_i_1 = debug_bitmap.Bool_Index(on_loop_x1, on_loop_y1);
				int bool_on_i_2 = debug_bitmap.Bool_Index(on_loop_x2, on_loop_y2);

				int bool_near_i_1 = debug_bitmap.Bool_Index(near_point_x1, near_point_y1);
				int bool_near_i_2 = debug_bitmap.Bool_Index(near_point_x2, near_point_y2);


				int byte_on_i_1 = debug_bitmap.Byte_Index(on_loop_x1, on_loop_y1);
				int byte_on_i_2 = debug_bitmap.Byte_Index(on_loop_x2, on_loop_y2);

				int byte_near_i_1 = debug_bitmap.Byte_Index(near_point_x1, near_point_y1);
				int byte_near_i_2 = debug_bitmap.Byte_Index(near_point_x2, near_point_y2);

				debug_bitmap.Draw_X(_loop_vertex_1, mark_length, Color.Yellow);
				debug_bitmap.Draw_X(_loop_vertex_2, mark_length, Color.Green);
				debug_bitmap.Draw_X(new Vector2d(near_point_x1, near_point_y1), mark_length, Color.Blue);
				debug_bitmap.Draw_X(new Vector2d(near_point_x2, near_point_y2), mark_length, Color.Red);

				debug_bitmap.Save_Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + (slice_count) + "_loop_" + (loop_count) + "_a_debug_bitmap_colored" + BITMAP_PATH_SUFIX, IMAGE_FORMAT_EXTENSION);

				file.WriteLine("On Loop Points:");
				file.WriteLine("(" + on_loop_x1 + ", " + on_loop_y1 + ") - Colored in Yellow");
				file.WriteLine("(" + on_loop_x2 + ", " + on_loop_y2 + ") - Colored in Green");

				file.WriteLine();

				file.WriteLine("Lookinh For Point Inside Loop (near_points):");
				file.WriteLine("(" + near_point_x1 + ", " + near_point_y1 + ") - Colored in Blue");
				file.WriteLine("(" + near_point_x2 + ", " + near_point_y2 + ") - Colored in Red");

				file.WriteLine();

				file.WriteLine("bool_array indices and values for all points:");
				file.WriteLine("Index: " + bool_on_i_1 + ", Value: " + debug_bitmap.bool_array[bool_on_i_1]);
				file.WriteLine("Index: " + bool_on_i_2 + ", Value: " + debug_bitmap.bool_array[bool_on_i_2]);
				file.WriteLine("Index: " + bool_near_i_1 + ", Value: " + debug_bitmap.bool_array[bool_near_i_1]);
				file.WriteLine("Index: " + bool_near_i_2 + ", Value: " + debug_bitmap.bool_array[bool_near_i_2]);

				file.WriteLine();

				file.WriteLine("bitmap_byte_array indices and values for all points:");
				file.WriteLine("Index: " + byte_on_i_1 + ", Value: " + debug_bitmap.bitmap_byte_array[byte_on_i_1]);
				file.WriteLine("Index: " + byte_on_i_2 + ", Value: " + debug_bitmap.bitmap_byte_array[byte_on_i_2]);
				file.WriteLine("Index: " + byte_near_i_1 + ", Value: " + debug_bitmap.bitmap_byte_array[byte_near_i_1]);
				file.WriteLine("Index: " + byte_near_i_2 + ", Value: " + debug_bitmap.bitmap_byte_array[byte_near_i_2]);

				file.WriteLine();
				file.WriteLine("************ End of loop " + loop_count + " ************");
				file.WriteLine();

				void get_dear_indices(Vector2d origin_vec, Vector2d dest_vec)
				{
					double mid_x = Math.Abs(origin_vec.x + dest_vec.x) / 2;
					double mid_y = Math.Abs(origin_vec.y + dest_vec.y) / 2;

					near_point_x1 = (int)Math.Floor(mid_x) - 1;
					near_point_x2 = (int)Math.Ceiling(mid_x) + 1;

					if (origin_vec.y > dest_vec.y && origin_vec.x > dest_vec.x)
					{
						near_point_y1 = (int)Math.Ceiling(mid_y) + 1;
						near_point_y2 = (int)Math.Floor(mid_y) - 1;
					}
					else
					{
						near_point_y1 = (int)Math.Floor(mid_y) - 1;
						near_point_y2 = (int)Math.Ceiling(mid_y) + 1;
					}
				}
			}
		}
#endif
	}
}