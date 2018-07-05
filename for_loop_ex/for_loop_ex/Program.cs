using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace for_loop_ex
{
	class Program
	{
		static void Main(string[] args)
		{
			var outer_loop = 5;
			var inner_loop = 10;

			for (int i = 0; i < outer_loop; i++)
			{
				for (int j = 0; j < inner_loop; j++)
				{
					Console.WriteLine(j + 1);
				}

				for (int j = 0; j < inner_loop; j++)
				{
					Console.WriteLine(inner_loop - j);
				}
			}
		}
	}
}
