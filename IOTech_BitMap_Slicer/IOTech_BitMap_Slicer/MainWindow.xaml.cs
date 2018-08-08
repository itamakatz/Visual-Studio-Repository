//#define RUN_VISUAL
//#define SHOW_LOCATION_OF_FLOOD_STARTING_POINT
//#define DEBUG_FILLING_POINT

using g3;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Drawing;
using HelixToolkit.Wpf;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using System.Collections.Concurrent;
using IOTech_BitMap_Slicer.WPF_Classes;

namespace IOTech_BitMap_Slicer
{
	public partial class MainWindow : Window
	{
		private static ConcurrentQueue<Bitmap_Slice> Bitmap_Slice_Queue = new ConcurrentQueue<Bitmap_Slice>();
		public static ConcurrentQueue<int> Bitmap_Index_Queue = new ConcurrentQueue<int>();

		private WPF_Button button_1_binding = new WPF_Button() { Bind_Name = "hi" };
		private Main_View_Model windows_bindings = new Main_View_Model();

		public MainWindow()
		{

			// initialization and declaration of variables
			
			Stopwatch watch_all_program = new Stopwatch();

			watch_all_program.Reset();
			watch_all_program.Start();

			InitializeComponent(); // default initialization of WPF
			DataContext = windows_bindings;

			Util.Check_directories();

			DMesh3 Imported_STL_mesh = null;
			Model3D HelixToolkit_Model3D = null;
			Vector3d SLICING_DIRECTION_UNIT = new Vector3d();

			File.WriteAllText(@"C:\Users\admin\Desktop\BitMap_Slicer\debug.txt", string.Empty);

			// Open STL file and create DMesh3 objects
			DMesh3Builder DMesh3_builder = new DMesh3Builder();
			StandardMeshReader reader = new StandardMeshReader() { MeshBuilder = DMesh3_builder };
			IOReadResult result = reader.Read(V.MODEL_IN_PATH, ReadOptions.Defaults);

			if (result.code == IOCode.Ok) { Imported_STL_mesh = DMesh3_builder.Meshes[0]; }
			else { Util.Exit_messege(new string[] { "DMesh3Builder faild to open stl model", "USER_PATH is - " + V.USER_PATH, "MODEL_IN_PATH is - " + V.MODEL_IN_PATH }); }

			Vector3d STL_mesh_Diagonal = Imported_STL_mesh.CachedBounds.Diagonal;

			switch (V.SLICING_AXIS)
			{
				case V.Axis.X:
					V.Bitmap_Width = Util.Get_Int_Dimension(STL_mesh_Diagonal.y);
					V.Bitmap_Height = Util.Get_Int_Dimension(STL_mesh_Diagonal.z);
					V.Mesh_min_dimensions = Tuple.Create(Imported_STL_mesh.CachedBounds.Min.y, Imported_STL_mesh.CachedBounds.Min.z);
					SLICING_DIRECTION_UNIT = Vector3d.AxisX;
					break;

				case V.Axis.Y:
					V.Bitmap_Width = Util.Get_Int_Dimension(STL_mesh_Diagonal.x);
					V.Bitmap_Height = Util.Get_Int_Dimension(STL_mesh_Diagonal.z);
					V.Mesh_min_dimensions = Tuple.Create(Imported_STL_mesh.CachedBounds.Min.x, Imported_STL_mesh.CachedBounds.Min.z);
					SLICING_DIRECTION_UNIT = Vector3d.AxisY;
					break;

				case V.Axis.Z:
					V.Bitmap_Width = Util.Get_Int_Dimension(STL_mesh_Diagonal.x);
					V.Bitmap_Height = Util.Get_Int_Dimension(STL_mesh_Diagonal.y);
					V.Mesh_min_dimensions = Tuple.Create(Imported_STL_mesh.CachedBounds.Min.x, Imported_STL_mesh.CachedBounds.Min.y);
					SLICING_DIRECTION_UNIT = Vector3d.AxisZ;
					break;
			}

			V.stride = V.Bitmap_Width * V.single_pixel_num_of_byte;
			int padding = (V.stride % 4);
			V.stride += padding == 0 ? 0 : 4 - padding; // pad out to multiple of 4 - CRITICAL
			V.im_num_of_bytes = V.Bitmap_Height * Math.Abs(V.stride);

			IEnumerable<double> Slice_Enumerator = Util.Range_Enumerator(
						Imported_STL_mesh.CachedBounds.Min[(int)V.SLICING_AXIS],
						Imported_STL_mesh.CachedBounds.Max[(int)V.SLICING_AXIS],
						V.NUM_OF_SLICES);

			// Cut mesh model and save as STL file
			int enumerator_count = 0;
			MeshPlaneCut plane_cut_cross_section = null;
			List<Tuple<MeshPlaneCut,int>> all_bitmaps = new List<Tuple<MeshPlaneCut, int>>();
			foreach (double slice_step in Slice_Enumerator)
			{
				plane_cut_cross_section = new MeshPlaneCut(new DMesh3(Imported_STL_mesh), SLICING_DIRECTION_UNIT * slice_step, SLICING_DIRECTION_UNIT);
				plane_cut_cross_section.Cut();
				all_bitmaps.Add(Tuple.Create(plane_cut_cross_section, enumerator_count));

				if (enumerator_count++ % 5 == 0)
				{
					Parallel.ForEach(all_bitmaps, bitmap => Create_Bitmap(bitmap));
					all_bitmaps = new List<Tuple<MeshPlaneCut, int>>();
				}
			}

			Parallel.ForEach(all_bitmaps, bitmap => Create_Bitmap(bitmap));

#if RUN_VISUAL

			//second_cross_section = new MeshPlaneCut(main_cross_section.Mesh, plane_origine - 0.01F, SLICING_NORMAL * -1);
			//second_cross_section.Cut();

			StandardMeshWriter.WriteFile(V.MODEL_OUT_PATH, new List<WriteMesh>() { new WriteMesh(plane_cut_cross_section.Mesh) }, WriteOptions.Defaults);

			// Using HelixToolkit show 3D object on GUI
			ModelVisual3D device3D = new ModelVisual3D();
			try
			{
				ModelImporter import = new ModelImporter();
				HelixToolkit_Model3D = import.Load(V.MODEL_OUT_PATH);
			}
			catch (Exception e)
			{
				// MessageBox.Show("Exception Error : " + e.StackTrace);
				Util.Exit_messege(new string[] { "Exception loading HelixToolkit Model3D" }, e);
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

		//private static void Create_Bitmap(MeshPlaneCut mesh_slice)
		private static void Create_Bitmap(Tuple<MeshPlaneCut, int> cross_section_pair)
		{
			List<EdgeLoop> cutLoops = cross_section_pair.Item1.CutLoops;
			List<EdgeSpan> cutSpans = cross_section_pair.Item1.CutSpans;

			V.loop_count = 1;

			Bitmap_Slice[] XOR_Array = new Bitmap_Slice[cutLoops.Count];

			for (int i = 0; i < cutLoops.Count; i++)
			{
				DCurve3 cutLoop_Curve = cutLoops[i].ToCurve();
				List<Vector2d> loop_vertices = new List<Vector2d>();

				Tuple<double, double> create_Vector2d = new Tuple<double, double>(0,0);

				double x, y, z;

				foreach (Vector3d vec3d in cutLoop_Curve.Vertices)
				{
					switch (V.SLICING_AXIS)
					{
						case V.Axis.X:
							y = (vec3d.y - V.Mesh_min_dimensions.Item1) * V.SCALE_FACTOR + V.PEN_FINE_WIDTH;
							z = (vec3d.z - V.Mesh_min_dimensions.Item2) * V.SCALE_FACTOR + V.PEN_FINE_WIDTH;
							loop_vertices.Add(new Vector2d(y, z));
							break;
						case V.Axis.Y:
							x = (vec3d.x - V.Mesh_min_dimensions.Item1) * V.SCALE_FACTOR + V.PEN_FINE_WIDTH;
							z = (vec3d.z - V.Mesh_min_dimensions.Item2) * V.SCALE_FACTOR + V.PEN_FINE_WIDTH;
							loop_vertices.Add(new Vector2d(x, z));
							break;
						case V.Axis.Z:
							x = (vec3d.x - V.Mesh_min_dimensions.Item1) * V.SCALE_FACTOR + V.PEN_FINE_WIDTH;
							y = (vec3d.y - V.Mesh_min_dimensions.Item2) * V.SCALE_FACTOR + V.PEN_FINE_WIDTH;
							loop_vertices.Add(new Vector2d(x, y));
							break;
						default:
							break;
					}
				}

				XOR_Array[i] = new Bitmap_Slice();

				for (int j = 0; j < cutLoop_Curve.VertexCount; j++)
				{
					XOR_Array[i].Draw_Line(loop_vertices[j], loop_vertices[(j + 1) % cutLoop_Curve.VertexCount]);
				}

#if DEBUG_FILLING_POINT
				Debug_Filling_Point(new Bitmap_Slice(XOR_Array[i].Bitmap), loop_vertices[0], loop_vertices[1]);
#endif
				Thread thread = new Thread(() => XOR_Array[i].Flood_Fill(loop_vertices), V.STACK_SIZE);
				thread.Start();
				thread.Join();
			}

			Bitmap_Slice main_bitmap = Bitmap_Slice.Bitmap_XOR(ref XOR_Array);

			main_bitmap.Save_Bitmap(V.BITMAP_DIR_PREFIX + @"\" + "slice_" + (cross_section_pair.Item2) + V.BITMAP_PATH_SUFIX);
			V.slice_count++;
		}

#if DEBUG_FILLING_POINT
		private static void Debug_Filling_Point(Bitmap_Slice debug_bitmap, Vector2d _loop_vertex_1, Vector2d _loop_vertex_2)
		{
			debug_bitmap.Update_bool_array();

			// need to add mutex
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
				Queue<Tuple<int, int>> starting_points = debug_bitmap.Get_starting_queue(_loop_vertex_1, _loop_vertex_2);

				int bool_on_i_1 = Bitmap_Slice.Bool_Index(on_loop_x1, on_loop_y1);
				int bool_on_i_2 = Bitmap_Slice.Bool_Index(on_loop_x2, on_loop_y2);

				int bool_near_i_1 = Bitmap_Slice.Bool_Index(near_point_x1, near_point_y1);
				int bool_near_i_2 = Bitmap_Slice.Bool_Index(near_point_x2, near_point_y2);


				int byte_on_i_1 = Bitmap_Slice.Byte_Index(on_loop_x1, on_loop_y1);
				int byte_on_i_2 = Bitmap_Slice.Byte_Index(on_loop_x2, on_loop_y2);

				int byte_near_i_1 = Bitmap_Slice.Byte_Index(near_point_x1, near_point_y1);
				int byte_near_i_2 = Bitmap_Slice.Byte_Index(near_point_x2, near_point_y2);

				debug_bitmap.Draw_X(_loop_vertex_1, mark_length, Color.Yellow);
				debug_bitmap.Draw_X(_loop_vertex_2, mark_length, Color.Green);
				debug_bitmap.Draw_X(new Vector2d(near_point_x1, near_point_y1), mark_length, Color.Blue);
				debug_bitmap.Draw_X(new Vector2d(near_point_x2, near_point_y2), mark_length, Color.Red);

				debug_bitmap.Save_Bitmap(V.BITMAP_DIR_PREFIX + @"\" + "slice_" + (V.slice_count) + "_loop_" + (V.loop_count) + "_a_debug_bitmap_colored" + V.BITMAP_PATH_SUFIX);

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
				file.WriteLine("************ End of loop " + V.loop_count + " ************");
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