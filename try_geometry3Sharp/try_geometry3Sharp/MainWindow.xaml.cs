﻿using g3;
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
using HelixToolkit.Wpf;
using System.IO;

namespace try_geometry3Sharp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string MODEL_IN_PATH = @"C:\Users\Itamar\Desktop\CorvinCastle.stl";
		private const string MODEL_OUT_PATH = @"C:\Users\Itamar\Desktop\CorvinCastle_new.stl";
		private const string SVG_PATH = @"C:\Users\Itamar\Desktop\bitmap.svg";
		private const string PNG_PATH = @"C:\Users\Itamar\Desktop\bitmap.Png";
		List<DMesh3> meshes;

		public MainWindow()
		{
			InitializeComponent();
			DMesh3Builder builder = new DMesh3Builder();
			StandardMeshReader reader = new StandardMeshReader() { MeshBuilder = builder };
			IOReadResult result = reader.Read(MODEL_IN_PATH, ReadOptions.Defaults);
			if (result.code == IOCode.Ok)
			{
				meshes = builder.Meshes;
			}

			double plane_origine = meshes[0].CachedBounds.MaxDim / 2;
			MeshPlaneCut first_plane_cut = new MeshPlaneCut(meshes[0], new Vector3d(0, 0, 0), new Vector3d(0, -1, 0));
			first_plane_cut.Cut();
			
			List<EdgeLoop> cutLoops = first_plane_cut.CutLoops;
			AxisAlignedBox3d cutLoops_bounds = cutLoops[0].GetBounds();
			List<EdgeSpan> cutSpans = first_plane_cut.CutSpans;
			//EdgeLoopRemesher

			SVGWriter my_SVGWriter = new SVGWriter();
			my_SVGWriter.SetDefaultLineWidth(0.1f);

			foreach (EdgeLoop EdgeLoop_item in cutLoops)
			{
				DCurve3 cutLoops_DCurve3 = EdgeLoop_item.ToCurve();
				List<Vector2d> cutLoops_Vector2d = new List<Vector2d>();
				DGraph2 cutLoops_DGraph2 = new DGraph2();

				foreach (var item in cutLoops_DCurve3.Vertices)
				{
					cutLoops_Vector2d.Add(item.xz);
					cutLoops_DGraph2.AppendVertex(item.xz);
				}

				cutLoops_DGraph2.AllocateEdgeGroup();
				int current_group_id = cutLoops_DGraph2.MaxGroupID;

				for (int i = 0; i < cutLoops_DCurve3.VertexCount; i++)
				{
					cutLoops_DGraph2.AppendEdge(i, (i + 1) % cutLoops_DCurve3.VertexCount, current_group_id);
				}

				my_SVGWriter.AddGraph(cutLoops_DGraph2);
			}

			my_SVGWriter.Write(SVG_PATH);

			//first_plane_cut.FillHoles();
			MeshPlaneCut second_plane_cut = new MeshPlaneCut(first_plane_cut.Mesh, new Vector3d(0.1f, 0.1f, 0.1f), new Vector3d(0, 0.1f, 0));
			second_plane_cut.Cut();
			//second_plane_cut.FillHoles();

			//IOWriteResult result =
			StandardMeshWriter.WriteFile(MODEL_OUT_PATH, new List<WriteMesh>() { new WriteMesh(second_plane_cut.Mesh) }, WriteOptions.Defaults);
			//StandardMeshWriter.WriteFile(MODEL_OUT_PATH, new List<WriteMesh>() { new WriteMesh(meshes[0]) }, WriteOptions.Defaults);
			//foreach (var item in meshes[0].VertexIndices())
			//foreach (var my_DMesh3 in meshes[0].Vertices())
			//{
			//	//item.z
			//}

			ModelVisual3D device3D = new ModelVisual3D();
			Model3D my_Model3D = Create_3D_from_stl(MODEL_OUT_PATH);
			device3D.Content = my_Model3D;

			my_3d_view.RotateGesture = new MouseGesture(MouseAction.LeftClick);
			my_3d_view.Children.Add(device3D);
		}

		/// <summary>
		/// Display 3D Model
		/// </summary>
		/// <param name="model">Path to the Model file</param>
		/// <returns>3D Model Content</returns>
		private Model3D Create_3D_from_stl(string model)
		{
			Model3D device = null;
			try
			{
				//Import 3D model file
				ModelImporter import = new ModelImporter();

				//Load the 3D model file
				device = import.Load(model);

			}
			catch (Exception e)
			{
				// Handle exception in case can not file 3D model
				MessageBox.Show("Exception Error : " + e.StackTrace);
			}
			return device;
		}
	}
}
