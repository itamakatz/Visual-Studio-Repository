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
//using System.Drawing.Text;
using HelixToolkit.Wpf;
using System.IO;
using Svg;
using System.Drawing.Drawing2D;

namespace IOTech_BitMap_Slicer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		enum Axis { X, Y, Z };

		// ****
		// should chane all paths to be relative using:
		// string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg");

		private const bool RUN_VISUAL = false;

		private const string MODEL_IN_PATH = @"C:\Users\Itamar\Desktop\CorvinCastle.stl";
		private const string MODEL_OUT_PATH = @"C:\Users\Itamar\Desktop\CorvinCastle_new.stl";

		private const string SVG_PATH_PREFIX = @"C:\Users\Itamar\Desktop\SVG_slice";
		private const string SVG_PATH_SUFIX = @".SVG";

		private const string BITMAP_PATH_PREFIX = @"C:\Users\Itamar\Desktop\BITMAP_slice";
		private const string BITMAP_PATH_SUFIX = @".png";

		private const string PNG_PATH = @"C:\Users\Itamar\Desktop\bitmap.Png";
		private const string TMP_PATH = @"C:\Users\Itamar\Desktop\TMP Directory";
		private const float SVG_WIDTH = 0.1f;
		private const float SLICING_WIDTH = 0.1f / 2;
		private const Axis SLICING_AXIS = Axis.Y;
		private const double NUM_OF_SLICES = 4;
		private Vector3d SLICING_NORMAL;
		private Vector3d SLICING_ORIGIN;
		private IEnumerable<double> Slice_Enumerator;

		DMesh3 Imported_STL_mesh;
		SVGWriter my_SVGWriter;
		Model3D HelixToolkit_Model3D = null;
		static int SVG_Count = 1;

		public MainWindow()
		{
			InitializeComponent();

			// Open STL file and create DMesh3 objects
			DMesh3Builder DMesh3_builder = new DMesh3Builder();
			StandardMeshReader reader = new StandardMeshReader() { MeshBuilder = DMesh3_builder };
			IOReadResult result = reader.Read(MODEL_IN_PATH, ReadOptions.Defaults);
			if (result.code == IOCode.Ok)
			{
				Imported_STL_mesh = DMesh3_builder.Meshes[0];
			}

			Vector3d STL_mesh_Diagonal = Imported_STL_mesh.CachedBounds.Diagonal;

			Slice_Enumerator = Range_Enumerator(Imported_STL_mesh.CachedBounds.Min[(int)SLICING_AXIS],
									Imported_STL_mesh.CachedBounds.Max[(int)SLICING_AXIS],
									STL_mesh_Diagonal[(int)SLICING_AXIS] / NUM_OF_SLICES);

			switch (SLICING_AXIS)
			{
				case Axis.X:
					SLICING_ORIGIN = Vector3d.AxisX;
					SLICING_NORMAL = Vector3d.AxisX * SLICING_WIDTH;
					break;
				case Axis.Y:
					SLICING_ORIGIN = Vector3d.AxisY;
					SLICING_NORMAL = Vector3d.AxisY * SLICING_WIDTH;
					break;
				case Axis.Z:
					SLICING_ORIGIN = Vector3d.AxisZ;
					SLICING_NORMAL = Vector3d.AxisZ * SLICING_WIDTH;
					break;
				default:
					break;
			}
			// Cut mesh model and save as STL file

			MeshPlaneCut main_cross_section = null;
			//MeshPlaneCut second_cross_section;

			foreach (double slice_step in Slice_Enumerator)
			{
				main_cross_section = new MeshPlaneCut(new DMesh3(Imported_STL_mesh), SLICING_ORIGIN * slice_step, SLICING_NORMAL);
				main_cross_section.Cut();

				Create_SVG(main_cross_section);
			}


			//second_cross_section = new MeshPlaneCut(main_cross_section.Mesh, plane_origine - SLICING_WIDTH, SLICING_NORMAL * -1);
			//second_cross_section.Cut();
			if (RUN_VISUAL)
			{
				StandardMeshWriter.WriteFile(MODEL_OUT_PATH, new List<WriteMesh>() { new WriteMesh(main_cross_section.Mesh) }, WriteOptions.Defaults);

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

				device3D.Content = HelixToolkit_Model3D;

				// Set GUI properties
				my_3d_view.RotateGesture = new MouseGesture(MouseAction.LeftClick);
				my_3d_view.Children.Add(device3D);
			}
		}


		private void Create_SVG(MeshPlaneCut cross_section)
		{
			List<EdgeLoop> cutLoops = cross_section.CutLoops;
			List<EdgeSpan> cutSpans = cross_section.CutSpans;

			my_SVGWriter = new SVGWriter();

			my_SVGWriter.SetDefaultLineWidth(SVG_WIDTH);

			foreach (EdgeLoop edgeLoop in cutLoops)
			{
				DCurve3 cutLoop_Curve = edgeLoop.ToCurve();
				DGraph2 cutLoop_DGraph2 = new DGraph2();

				foreach (Vector3d vec3d in cutLoop_Curve.Vertices)
				{
					cutLoop_DGraph2.AppendVertex(vec3d.xz);
				}

				cutLoop_DGraph2.AllocateEdgeGroup();
				int current_group_id = cutLoop_DGraph2.MaxGroupID;

				for (int i = 0; i < cutLoop_Curve.VertexCount; i++)
				{
					cutLoop_DGraph2.AppendEdge(i, (i + 1) % cutLoop_Curve.VertexCount, current_group_id);
				}

				my_SVGWriter.AddGraph(cutLoop_DGraph2);
			}

			my_SVGWriter.Write(SVG_PATH_PREFIX + @"\" + (SVG_Count) + SVG_PATH_SUFIX);
			SVG_to_PNG(SVG_PATH_PREFIX + @"\" + (SVG_Count) + SVG_PATH_SUFIX, 
				BITMAP_PATH_PREFIX + @"\" + (SVG_Count));

			SVG_Count++;
		}

		public void DrawLineInt(Bitmap bmp)
		{
			System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

			int x1 = 100;
			int y1 = 100;
			int x2 = 500;
			int y2 = 100;
			// Draw line to screen.
			using (var graphics = Graphics.FromImage(bmp))
			{
				graphics.DrawLine(blackPen, x1, y1, x2, y2);
			}
		}

		public void SVG_to_PNG(String svg_path, String png_path)
		{
			GraphicsPath my_GraphicsPath = new GraphicsPath();

			//string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg");

			//SvgDocument sampleDoc = SvgDocument.Open<SvgDocument>(filePath, new Dictionary<string, string>
			//	{
			//		{"entity1", "fill:red" },
			//		{"entity2", "fill:yellow" }
			//	});

			//sampleDoc.Draw(1000, 1000).Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.png"));

			//string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
			//if (xml.StartsWith(_byteOrderMarkUtf8))
			//{
			//	xml = xml.Remove(0, _byteOrderMarkUtf8.Length);
			//}
			Bitmap bitmap2 = new Bitmap(800, 800);
			DrawLineInt(bitmap2);
			bitmap2.Save(png_path + "_whaaaa" + BITMAP_PATH_SUFIX, System.Drawing.Imaging.ImageFormat.Png);

			for (int i = 1; i < 10; i++)
			{
				Bitmap bitmap = new Bitmap(80 * i, 40 * i);
				SvgDocument svgDoc = SvgDocument.Open(svg_path);
				svgDoc.Draw(bitmap);
				bitmap.Save(png_path + "_" + i + BITMAP_PATH_SUFIX, System.Drawing.Imaging.ImageFormat.Png);
			}

			//Bitmap bitmap = new Bitmap(1000, 500);
			//SvgDocument svgDoc = SvgDocument.Open(svg_path);
			//svgDoc.Draw(bitmap);
			//bitmap.Save(png_path, System.Drawing.Imaging.ImageFormat.Png);
			//RenderSvg(svgDoc);

			//var byteArray = UTF8Encoding.Default.GetBytes(svg_path);
			//var byteArray = Encoding.ASCII.GetBytes(svg_path);

			//using (var s = new MemoryStream(UTF8Encoding.Default.GetBytes(svg_path)))
			//{
			//	SvgDocument svgDoc = SvgDocument.Open<SvgDocument>(s, null);
			//	//RenderSvg(svgDoc);
			//}

			//using (var stream = new MemoryStream(byteArray))
			//{
			//	SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(stream);
			//	Bitmap bitmap = svgDocument.Draw();
			//	//bitmap.Save(png_path, System.Drawing.Imaging.ImageFormat.Bmp);
			//	bitmap.Save(png_path, System.Drawing.Imaging.ImageFormat.Png);
			//}
		}

		public IEnumerable<double> Range_Enumerator(double start, double end, double increment)
		{
			for (double i = start + increment; i < end; i += increment)
				yield return i;
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