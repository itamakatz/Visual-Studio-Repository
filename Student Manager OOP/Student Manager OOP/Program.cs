using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Manager_OOP
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Student> students = new List<Student>();

			var run = true;

			while (run)
			{
				Console.WriteLine("Please enter Student's name");
				var name = verify_input();

				Console.WriteLine("Please enter {0}'s grade", name);
				var grade = int.Parse(verify_input());

				students.Add(new Student(name, grade));

				Console.WriteLine("Continue adding students? y/n");
				if(Console.ReadLine() == "n")
				{
					run = false;
				}
			}

			foreach (var student in students)
			{
				student.output();
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

	class Student
	{
		private string name;
		private int grade;

		public Student(string name, int grade)
		{
			this.name = name;
			this.grade = grade;
		}

		public void output()
		{
			Console.WriteLine("{0} got a mark of {1}. Well done.", name, grade);
		}
	}

}
