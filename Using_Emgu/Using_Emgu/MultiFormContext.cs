using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Using_Emgu {
	public class MultiFormContext : ApplicationContext {
		private int openForms;
		public MultiFormContext(params Form[] forms) {
			//openForms = forms.Length;
			openForms = 2;


			Program my_program = (Program) forms[0];
			Form first_form = my_program.My_Form.Form;
			Form second_form = my_program.My_Form_2.Form;

			first_form.FormClosed += (s, args) =>
			{
				//When we have closed the last of the "starting" forms, 
				//end the program.
				if (Interlocked.Decrement(ref openForms) == 0)
					ExitThread();
			};

			first_form.Show();

			second_form.FormClosed += (s, args) => {
				//When we have closed the last of the "starting" forms, 
				//end the program.
				if (Interlocked.Decrement(ref openForms) == 0)
					ExitThread();
			};

			second_form.Show();

			//foreach (var form in forms) {
			//	form.FormClosed += (s, args) =>
			//	{
			//		//When we have closed the last of the "starting" forms, 
			//		//end the program.
			//		if (Interlocked.Decrement(ref openForms) == 0)
			//			ExitThread();
			//	};

			//	Program my_program = (Program) form;
			//	my_program.My_Form.Form.Show();
			//}
		}
	}
}
