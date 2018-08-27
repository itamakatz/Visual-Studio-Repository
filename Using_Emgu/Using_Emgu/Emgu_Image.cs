using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Using_Emgu {
	class Emgu_Image_Panel {
		public Emgu_Image_Box Emgu_Im_Box = new Emgu_Image_Box();
		public Panel Im_Panel = new Panel();
		public Label Im_Label = new Label();

		public Emgu_Image_Panel(Size panel_size, 
								Action<object, MouseEventArgs>[] Picture_Box_Delegates, 
								Action<object, MouseEventArgs> Panel_Delegate) {

			Emgu_Im_Box.SizeMode = PictureBoxSizeMode.Zoom;

			Attach_Events(Picture_Box_Delegates, Panel_Delegate);
			
			Im_Panel.Size = panel_size;
			Reset_Coordinates();

			Im_Panel.Controls.Add(Emgu_Im_Box);
			Im_Panel.Controls.Add(Im_Label);

			Init_Label();

			Set_Z_Order();
		}

		public void Update_Size(Size new_size) { Emgu_Im_Box.Size = new_size; }

		public void Reset_Coordinates() {
			Emgu_Im_Box.Size = Im_Panel.Size;
			Emgu_Im_Box.Location = new Point(0, 0);
		}

		public void Update_Location(int deltaX, int deltaY) {
			int newX = Emgu_Im_Box.Location.X + deltaX;
			int newY = Emgu_Im_Box.Location.Y + deltaY;

			Emgu_Im_Box.Location = new Point(newX, newY);
		}

		public void Set_Panel_Location(Point location) { Im_Panel.Location = location; }
		public void Set_Panel_Size(Size size) { Im_Panel.Size = size; }

		private void Attach_Events(Action<object, MouseEventArgs>[] Picture_Box_Delegates, Action<object, MouseEventArgs> Panel_Delegate) {
			Emgu_Im_Box.MouseDoubleClick += new MouseEventHandler(Picture_Box_Delegates[0]);
			Emgu_Im_Box.MouseDown += new MouseEventHandler(Picture_Box_Delegates[1]);
			Emgu_Im_Box.MouseMove += new MouseEventHandler(Picture_Box_Delegates[2]);
			Emgu_Im_Box.MouseWheel += new MouseEventHandler(Picture_Box_Delegates[3]);

			Im_Panel.MouseDoubleClick += new MouseEventHandler(Panel_Delegate);
		}

		private void Init_Label() {
			Im_Label.TextAlign = ContentAlignment.MiddleCenter;
			Im_Label.Font = new Font("Arial", 10, FontStyle.Bold);
			Im_Label.Dock = DockStyle.Top;
		}

		private void Set_Z_Order() {
			Im_Panel.BringToFront();
			Im_Label.BringToFront();
		}
	}

	class Emgu_Image_Box : ImageBox {
		public Emgu_Image_Box() : base() {
			FunctionalMode = FunctionalModeOption.RightClickMenu;
		}
	}
}
