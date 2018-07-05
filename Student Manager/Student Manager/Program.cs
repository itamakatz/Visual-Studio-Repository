using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Manager
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Tuple<string, string>> students = new List<Tuple<string, string>>();

			var input = "";

			while (input != "End")
			{
				Console.WriteLine("Please enter Student's name");
				var name = verify_input();

				Console.WriteLine("Please enter {0}'s grade", name);
				var grade = verify_input();

				students.Add(Tuple.Create(name, grade));

				Console.WriteLine("Please enter \"End\" if you finished adding students to the program r else enter");
				input = Console.ReadLine();
			}

			foreach (var student in students)
			{
				Console.WriteLine("{0} got a mark of {1}. Well done.", student.Item1, student.Item2);
			}
		}

		static string verify_input(){
			var input= Console.ReadLine();
			while (input == "")
			{
				Console.WriteLine("No input was given. Please try again.");
				input = Console.ReadLine();
			}
			return input;
		}
	}
}
