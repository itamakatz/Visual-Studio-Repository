using System.Threading;
using System.Windows.Forms;

namespace Using_Emgu {
	internal class MultiFormContext : ApplicationContext {

		private int openForms;

		public MultiFormContext(params Form[] Forms) {
			openForms = Forms.Length;
			foreach (Form form in Forms) {
				//form.FormClosed += (s, args) => {
				//	//When we have closed the last of the "starting" forms, 
				//	//end the program.
				//	if (Interlocked.Decrement(ref openForms) == 0) { ExitThread(); }
				//};
				form.Show();
			}
		}
	}
}
