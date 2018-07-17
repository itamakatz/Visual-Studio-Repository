//#define PAINT_BITMAP_BORDERS
//#define RUN_VISUAL
#define SHOW_STARTING_POINT
#define SHOW_LOCATION_OF_FLOOD_STARTING_POINT

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

namespace IOTech_BitMap_Slicer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		// ***** Variables that can be changed ***** //

		private string MODEL_IN_PATH = @"Desktop\CorvinCastle.stl";
		private string MODEL_OUT_PATH = @"Desktop\CorvinCastle_new.stl";

		private string BITMAP_DIR_PREFIX = @"Desktop\BITMAP_slice";
		private const string BITMAP_PATH_SUFIX = @".Bmp";
		ImageFormat IMAGE_FORMAT_EXTENSION = ImageFormat.Bmp;

		private string TMP_DIR = @"Desktop\TMP Directory";

		private const int SCALE_FACTOR = 20;
		private const double NUM_OF_SLICES = 4;
		private const Axis SLICING_AXIS = Axis.Y;

		private const int EXIT_CODE = 10;

		private const int PEN_FINE_WIDTH = 2;

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
		 */
		// maybe should varify the image is infact Format32bppRgb
		// all the formats below work perfectly
		//PixelFormat IMAGE_FORMAT = System.Drawing.Imaging.PixelFormat.Format32bppRgb;
		PixelFormat PIXEL_FORMAT = PixelFormat.Format24bppRgb; // best format I found
		//PixelFormat IMAGE_FORMAT = System.Drawing.Imaging.PixelFormat.Format16bppRgb555; // dangerous whern comparing colors

		//private const Int32 stackSize = 2147483647;  // max Int32 = 2147483647 
		private const Int32 stackSize = 1000000000;  // max Int32 = 2147483647 

		// ***** Initialization of other variables ***** //

		private string USER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

		private Vector2d Bitmap_dimensions = new Vector2d();
		private Vector2d Mesh_min_dimensions = new Vector2d();

		private Bitmap_Slice bitmap_slice;



		internal Stopwatch watch = new Stopwatch();

		private static int Slice_Count = 1;

		private enum Axis { X, Y, Z };

		public MainWindow()
		{
			// initialization and declaration of variables
			InitializeComponent();

			Check_directories();

			DMesh3 Imported_STL_mesh = null;
			Model3D HelixToolkit_Model3D = null;
			Vector3d SLICING_DIRECTION_UNIT;

			Bitmap_Slice.SCALE_FACTOR = SCALE_FACTOR;
			Bitmap_Slice.Bitmap_Color = bitmap_color;
			Bitmap_Slice.PEN_WIDTH = (int) PEN_FINE_WIDTH;
			Bitmap_Slice.Pen = new System.Drawing.Pen(bitmap_color, PEN_FINE_WIDTH);
			Bitmap_Slice.PIXEL_FORMAT = PIXEL_FORMAT;

			Util.EXIT_CODE = EXIT_CODE;

			Trace.WriteLine("wasssup broooo????");

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
				Util.exit_messege(new string[] { "DMesh3Builder faild to open stl model" ,
					"USER_PATH is - " + USER_PATH ,
					"MODEL_IN_PATH is - " + MODEL_IN_PATH });
			}

			Vector3d STL_mesh_Diagonal = Imported_STL_mesh.CachedBounds.Diagonal;

			IEnumerable<double> Slice_Enumerator = Range_Enumerator(Imported_STL_mesh.CachedBounds.Min[(int)SLICING_AXIS],
									Imported_STL_mesh.CachedBounds.Max[(int)SLICING_AXIS],
									STL_mesh_Diagonal[(int)SLICING_AXIS] / NUM_OF_SLICES);

			switch (SLICING_AXIS)
			{
				case Axis.X:

					Bitmap_dimensions.x = STL_mesh_Diagonal.y;
					Mesh_min_dimensions.x = Imported_STL_mesh.CachedBounds.Min.y;

					Bitmap_dimensions.y = STL_mesh_Diagonal.z;
					Mesh_min_dimensions.y = Imported_STL_mesh.CachedBounds.Min.z;

					SLICING_DIRECTION_UNIT = Vector3d.AxisX;

					break;
				case Axis.Y:

					Bitmap_dimensions.x = STL_mesh_Diagonal.x;
					Mesh_min_dimensions.x = Imported_STL_mesh.CachedBounds.Min.x;

					Bitmap_dimensions.y = STL_mesh_Diagonal.z;
					Mesh_min_dimensions.y = Imported_STL_mesh.CachedBounds.Min.z;

					SLICING_DIRECTION_UNIT = Vector3d.AxisY;

					break;
				case Axis.Z:

					Bitmap_dimensions.x = STL_mesh_Diagonal.x;
					Mesh_min_dimensions.x = Imported_STL_mesh.CachedBounds.Min.x;

					Bitmap_dimensions.y = STL_mesh_Diagonal.y;
					Mesh_min_dimensions.y = Imported_STL_mesh.CachedBounds.Min.y;

					SLICING_DIRECTION_UNIT = Vector3d.AxisZ;

					break;
				default:
					break;
			}

			// Cut mesh model and save as STL file
			MeshPlaneCut plane_cut_cross_section = null;

			//MeshPlaneCut second_cross_section;
			foreach (double slice_step in Slice_Enumerator)
			{
				plane_cut_cross_section = new MeshPlaneCut(new DMesh3(Imported_STL_mesh), SLICING_DIRECTION_UNIT * slice_step, SLICING_DIRECTION_UNIT);
				plane_cut_cross_section.Cut();
				Create_Bitmap(plane_cut_cross_section);
			}

			watch.Reset();
			watch.Start();

			flood_fill_bitmap();

			watch.Stop();


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
			Trace.WriteLine(Bitmap_Slice.recursive_count);
			Util.Print_elapsed(watch.Elapsed);
		}

		private void Create_Bitmap(MeshPlaneCut cross_section)
		{
			List<EdgeLoop> cutLoops = cross_section.CutLoops;
			List<EdgeSpan> cutSpans = cross_section.CutSpans;

			int loop_count = 1;

			// how come we do not need Bitmap_dimensions.x * SCALE_FACTOR I simply dont understand. After all we do multiply in
			//							loop_vertices.Add((vec3d.xz - Mesh_min_dimensions) * SCALE_FACTOR + PEN_WIDTH);
			bitmap_slice = new Bitmap_Slice(Bitmap_dimensions.x, Bitmap_dimensions.y);

#if PAINT_BITMAP_BORDERS
			bitmap_slice.Draw_rectangle(bitmap_slice.bitmap.Width, bitmap_slice.bitmap.Height);
#endif
			foreach (EdgeLoop edgeLoop in cutLoops)
			{

				DCurve3 cutLoop_Curve = edgeLoop.ToCurve();
				List<Vector2d> loop_vertices = new List<Vector2d>();

				foreach (Vector3d vec3d in cutLoop_Curve.Vertices)
				{
					loop_vertices.Add((vec3d.xz - Mesh_min_dimensions) * SCALE_FACTOR + PEN_FINE_WIDTH);
				}

				for (int i = 0; i < cutLoop_Curve.VertexCount; i++)
				{
					bitmap_slice.DrawLineInt(loop_vertices[i], loop_vertices[(i + 1) % cutLoop_Curve.VertexCount]);
				}
			}

			try
			{
				//bitmap_slice.get_bitmap_from_byte().Save(BITMAP_DIR_PREFIX + @"\" + "slice_" + (Slice_Count) + "_loop_" + (loop_count++) + BITMAP_PATH_SUFIX, image_format);
				bitmap_slice.bitmap.Save(BITMAP_DIR_PREFIX + @"\" + "slice_" + (Slice_Count) + "_loop_" + (loop_count++) + BITMAP_PATH_SUFIX, IMAGE_FORMAT_EXTENSION);
			}
			catch (Exception e)
			{
				Util.exit_messege(new string[] { "Error saving bitmap" }, e);
			}

			Slice_Count++;
		}

		private void flood_fill_bitmap()
		{
			bitmap_slice = new Bitmap_Slice(new Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + "2" + "_loop_" + "1" + BITMAP_PATH_SUFIX));
			//Bitmap bitmap_starting_point = new Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + "2" + "_loop_" + "1" + BITMAP_PATH_SUFIX);

			int begin_x = bitmap_slice.bitmap.Width / 2;
			int begin_y = bitmap_slice.bitmap.Height / 2;
			int mark_length = 3;

#if SHOW_LOCATION_OF_FLOOD_STARTING_POINT

			bitmap_slice.graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Yellow, PEN_FINE_WIDTH), begin_x - mark_length, begin_y - mark_length, begin_x + mark_length, begin_y + mark_length);
			bitmap_slice.graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Yellow, PEN_FINE_WIDTH), begin_x + mark_length, begin_y - mark_length, begin_x - mark_length, begin_y + mark_length);

			bitmap_slice.bitmap.Save(BITMAP_DIR_PREFIX + @"\" + "slice_" + "2" + "_loop_" + "1_2" + BITMAP_PATH_SUFIX, IMAGE_FORMAT_EXTENSION);

			bitmap_slice = new Bitmap_Slice(new Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + "2" + "_loop_" + "1" + BITMAP_PATH_SUFIX));
#endif
			try
			{
				bitmap_slice.set_starting_point(begin_x, begin_y);
				bitmap_slice.Switch_to_byte_manipulation();

				Thread thread = new Thread(new ThreadStart(bitmap_slice.flood_fill_recursive), stackSize);
				thread.Start();
				thread.Join();

				//bitmap_slice.Dispose_GC();
				//bitmap_slice.bitmap.Save(BITMAP_DIR_PREFIX + @"\" + "flood_fill_Bitmap" + BITMAP_PATH_SUFIX, image_format);
				bitmap_slice.Switch_to_bitmap_manipulation();
				bitmap_slice.bitmap.Save(BITMAP_DIR_PREFIX + @"\" + "flood_fill_Bitmap2" + BITMAP_PATH_SUFIX, IMAGE_FORMAT_EXTENSION);
			}
			catch (ArgumentException e)
			{
				Util.exit_messege(new string[] { "Starting point of flood fill out of bounds.", "begin_x: " + begin_x, "begin_y: " + begin_y }, e);
			}
			catch (Exception e)
			{
				Util.exit_messege(new string[] { "Faild to flood fill bitmap" }, e);
			}
		}

		private IEnumerable<double> Range_Enumerator(double start, double end, double increment)
		{
			for (double i = start + increment; i < end; i += increment)
				yield return i;
		}

		void Check_directories()
		{
			try
			{
				// ****
				// maybe switch to this relative path
				//string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg");

				// adjust all path's relative to specific pc
				MODEL_IN_PATH = System.IO.Path.Combine(USER_PATH, MODEL_IN_PATH);
				MODEL_OUT_PATH = System.IO.Path.Combine(USER_PATH, MODEL_OUT_PATH);
				BITMAP_DIR_PREFIX = System.IO.Path.Combine(USER_PATH, BITMAP_DIR_PREFIX);
				TMP_DIR = System.IO.Path.Combine(USER_PATH, TMP_DIR);

				if (!File.Exists(BITMAP_DIR_PREFIX))
				{
					Directory.CreateDirectory(BITMAP_DIR_PREFIX);
				}

				if (!File.Exists(TMP_DIR))
				{
					Directory.CreateDirectory(TMP_DIR);
				}
			}
			catch (Exception e)
			{
				Util.exit_messege(new string[] { "Check_directories faild" }, e);
			}
		}
	}
}