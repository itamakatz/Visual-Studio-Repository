using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace try_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Path to the model file
        private const string MODEL_PATH = @"C:\Users\Itamar\Desktop\CorvinCastle.stl";

		Model3D CurrentModel;
		Model3DGroup modelgroupobjects = new Model3DGroup();
		ModelVisual3D ModelVisualObjects = new ModelVisual3D();
		ModelImporter import_2 = new ModelImporter();

		public MainWindow()
        {
            InitializeComponent();
			CurrentModel = import_2.Load(MODEL_PATH);
			modelgroupobjects.Children.Add(CurrentModel);
			ModelVisualObjects.Content = modelgroupobjects;
			GeometryModel3D gm3d = ModelVisualObjects.Content as GeometryModel3D;
			MeshGeometry3D mesh = gm3d.Geometry as MeshGeometry3D;
			my_3d_view.Children.Add(ModelVisualObjects);
			my_3d_view.RotateGesture = new MouseGesture(MouseAction.LeftClick);

			// *********** Original (and working) code: ***********

			//ModelVisual3D device3D = new ModelVisual3D();
			//device3D.Content = Create_3D_from_stl(MODEL_PATH);
   //         //Adding a gesture here
   //         my_3d_view.RotateGesture = new MouseGesture(MouseAction.LeftClick);
   //         // Add to view port
   //         my_3d_view.Children.Add(device3D);
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
