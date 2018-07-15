#define PAINT_BITMAP_BORDERS
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Drawing;
using HelixToolkit.Wpf;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;
//using PictureBoxScroll;
//using FloodFill2;

namespace IOTech_BitMap_Slicer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// ****
		// should chane all paths to be relative using:
		// string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg");

		// ***** Variables that can be changed ***** //


		private string MODEL_IN_PATH = @"Desktop\CorvinCastle.stl";
		private string MODEL_OUT_PATH = @"Desktop\CorvinCastle_new.stl";

		private string SVG_DIR_PREFIX = @"Desktop\SVG_slice";
		private const string SVG_PATH_SUFIX = @".SVG";

		private string BITMAP_DIR_PREFIX = @"Desktop\BITMAP_slice";
		private const string BITMAP_PATH_SUFIX = @".Bmp";
		System.Drawing.Imaging.ImageFormat image_format = System.Drawing.Imaging.ImageFormat.Bmp;

		private string TMP_DIR = @"Desktop\TMP Directory";

		private const int SCALE_FACTOR = 10;
		private const double NUM_OF_SLICES = 4;
		private const Axis SLICING_AXIS = Axis.Y;
		
		private const float SVG_WIDTH = 0.1f;
		private const int EXIT_CODE = 10;

		private const int PEN_FINE_WIDTH = 3;
		private const int PEN_ROBUST_WIDTH = 3;

		private static System.Drawing.Color bitmap_color = System.Drawing.Color.Blue;

		//System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, PEN_WIDTH );
		private static System.Drawing.Pen Pen_Robust = new System.Drawing.Pen(bitmap_color, PEN_ROBUST_WIDTH);
		private static System.Drawing.Pen Pen_Fine = new System.Drawing.Pen(bitmap_color, PEN_FINE_WIDTH);



		// ***** Initialization of other variables ***** //

		private string USER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		 
		private Vector3d SLICING_DIRECTION_UNIT;
		private Vector2d Bitmap_dimensions = new Vector2d();
		private Vector2d Mesh_min_dimensions = new Vector2d();
		
		private IEnumerable<double> Slice_Enumerator;

		private DMesh3 Imported_STL_mesh;
		private Model3D HelixToolkit_Model3D = null;
		private Bitmap_Slice bitmap_slice;

		internal Stopwatch watch = new Stopwatch();

		private static int Slice_Count = 1;

		private enum Axis { X, Y, Z };

		public MainWindow()
		{
			InitializeComponent();

			Check_directories();

			Bitmap_Slice.SCALE_FACTOR = SCALE_FACTOR;
			Bitmap_Slice.bitmap_color = bitmap_color;
			Bitmap_Slice.PEN_FINE_WIDTH = (int)PEN_FINE_WIDTH;
			Bitmap_Slice.PEN_ROBUST_WIDTH = PEN_ROBUST_WIDTH;
			Bitmap_Slice.Pen_Fine = new System.Drawing.Pen(bitmap_color, PEN_FINE_WIDTH);
			Bitmap_Slice.Pen_Robust = new System.Drawing.Pen(bitmap_color, PEN_ROBUST_WIDTH);
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
				Trace.WriteLine("USER_PATH is - " + USER_PATH);
				Trace.WriteLine("MODEL_IN_PATH is - " + MODEL_IN_PATH);
				Trace.WriteLine("DMesh3Builder faild to open stl model");
				Environment.Exit(EXIT_CODE);
			}

			Vector3d STL_mesh_Diagonal = Imported_STL_mesh.CachedBounds.Diagonal;

			Slice_Enumerator = Range_Enumerator(Imported_STL_mesh.CachedBounds.Min[(int)SLICING_AXIS],
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

				//Create_SVG(plane_cut_cross_section);
			}

			watch.Reset();
			watch.Start();

			flood_fill_bitmap();

			watch.Stop();

			//flood_fill_bitmap();
			//my_flood_fill_botmap();
			//second_cross_section = new MeshPlaneCut(main_cross_section.Mesh, plane_origine - 0.01F, SLICING_NORMAL * -1);
			//second_cross_section.Cut();
#if RUN_VISUAL
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
				MessageBox.Show("Exception Error : " + e.StackTrace);
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
			TimeSpan ts = watch.Elapsed;
			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			ts.Hours, ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
			Trace.WriteLine("RunTime: " + elapsedTime);
		}

		private void flood_fill_bitmap()
		{
			bitmap_slice = new Bitmap_Slice(new Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + "2" + "_loop_" + "1" + BITMAP_PATH_SUFIX));

			int begin_x = bitmap_slice.bitmap.Width / 2;
			int begin_y = bitmap_slice.bitmap.Height / 2;
			int mark_length = 20;

			var stackSize = 100000000;  // max Int32 = 2147483648 
			bitmap_slice.set_starting_point(begin_x, begin_y);

			Thread thread = new Thread(new ThreadStart(bitmap_slice.flood_fill_recursive), stackSize);
			thread.Start();
			thread.Join();
			bitmap_slice.bitmap.Save(BITMAP_DIR_PREFIX + @"\" + "flood_fill_Bitmap" + BITMAP_PATH_SUFIX, image_format);

#if SHOW_LOCATION_OF_FLOOD_STARTING_POINT
			Bitmap bitmap_flood = new Bitmap(BITMAP_DIR_PREFIX + @"\" + "slice_" + "2" + "_loop_" + "1" + BITMAP_PATH_SUFIX);

			using (var graphics = Graphics.FromImage(bitmap_flood))
			{
				graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Yellow, PEN_ROBUST_WIDTH), begin_x - mark_length, begin_y, begin_x + mark_length, begin_y);
				graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Yellow, PEN_ROBUST_WIDTH), begin_x, begin_y - mark_length, begin_x, begin_y + mark_length);
			}
			bitmap_flood.Save(BITMAP_DIR_PREFIX + @"\" + "slice_" + "2" + "_loop_" + "1_2" + BITMAP_PATH_SUFIX, image_format);
#endif
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
					loop_vertices.Add((vec3d.xz - Mesh_min_dimensions) * SCALE_FACTOR + PEN_ROBUST_WIDTH);
				}

				for (int i = 0; i < cutLoop_Curve.VertexCount; i++)
				{
					bitmap_slice.DrawLineInt(loop_vertices[i], loop_vertices[(i + 1) % cutLoop_Curve.VertexCount]);
				}
			}

			try
			{
				bitmap_slice.bitmap.Save(BITMAP_DIR_PREFIX + @"\" + "slice_" + (Slice_Count) + "_loop_" + (loop_count++) + BITMAP_PATH_SUFIX,
						image_format);
			}
			catch (Exception)
			{
				throw;
			}
			
			Slice_Count++;
		}

		public IEnumerable<double> Range_Enumerator(double start, double end, double increment)
		{
			for (double i = start + increment; i < end; i += increment)
				yield return i;
		}

		public static byte[] Array1DFromBitmap(Bitmap bmp)
		{
			if (bmp == null) throw new NullReferenceException("Bitmap is null");

			System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
			IntPtr ptr = data.Scan0;

			//declare an array to hold the bytes of the bitmap
			int numBytes = data.Stride * bmp.Height;
			byte[] bytes = new byte[numBytes];

			//copy the RGB values into the array
			System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, numBytes);

			bmp.UnlockBits(data);

			return bytes;
		}

		public static Bitmap BitmapFromArray1D(byte[] bytes, int width, int height)
		{
			Bitmap grayBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
			System.Drawing.Rectangle grayRect = new System.Drawing.Rectangle(0, 0, grayBmp.Width, grayBmp.Height);
			BitmapData grayData = grayBmp.LockBits(grayRect, ImageLockMode.ReadWrite, grayBmp.PixelFormat);
			IntPtr grayPtr = grayData.Scan0;

			int grayBytes = grayData.Stride * grayBmp.Height;
			ColorPalette pal = grayBmp.Palette;

			for (int g = 0; g < 256; g++)
			{
				pal.Entries[g] = System.Drawing.Color.FromArgb(g, g, g);
			}

			grayBmp.Palette = pal;

			System.Runtime.InteropServices.Marshal.Copy(bytes, 0, grayPtr, grayBytes);

			grayBmp.UnlockBits(grayData);
			return grayBmp;
		}

		void Check_directories()
		{
			try
			{
				// adjust all path's relative to specific pc
				MODEL_IN_PATH = System.IO.Path.Combine(USER_PATH, MODEL_IN_PATH);
				MODEL_OUT_PATH = System.IO.Path.Combine(USER_PATH, MODEL_OUT_PATH);
				SVG_DIR_PREFIX = System.IO.Path.Combine(USER_PATH, SVG_DIR_PREFIX);
				BITMAP_DIR_PREFIX = System.IO.Path.Combine(USER_PATH, BITMAP_DIR_PREFIX);
				TMP_DIR = System.IO.Path.Combine(USER_PATH, TMP_DIR);

				if (!File.Exists(SVG_DIR_PREFIX))
				{
					Directory.CreateDirectory(SVG_DIR_PREFIX);
				}

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
				Trace.WriteLine("Check_directories faild");
				Trace.WriteLine(e);
				Environment.Exit(EXIT_CODE);
			}
		}

		public void Clear_dir(String path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			foreach (FileInfo file in di.EnumerateFiles())
			{
				file.Delete();
			}
			foreach (DirectoryInfo dir in di.EnumerateDirectories())
			{
				dir.Delete(true);
			}
		}
	}
}