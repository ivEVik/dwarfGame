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
			Initialise(3, 3);
			
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		private static void Initialise(int scale, int uiScale)
		{
			Game.Initialise();
			Sprites.Initialise(scale, uiScale);
			MapMaker.Initialise();
		}
		
		public static void Exit()
		{
			Application.Exit();
		}
	}
}
