using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dwarfGame
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Initialise(3);
			Game.MakeMapFromString();
			
			Application.Run(new MainForm());
		}
		
		private static void Initialise(int scale)
		{
			Game.Initialise(scale);
			Sprites.Initialise();
			MapMaker.Initialise();
		}
	}
}
