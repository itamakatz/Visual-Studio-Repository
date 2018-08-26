using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Using_Emgu {
	class My_ImageBox : ImageBox {

		public My_ImageBox() : base() {
			FunctionalMode = FunctionalModeOption.RightClickMenu;
		}
	}
}
