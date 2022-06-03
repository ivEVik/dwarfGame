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
			//Game.LoadMap(TEMPLATE.MAP_TEST);
			//Game.MakeMapFromString();
			//Game.StartGame();
			
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Exit();
			Application.Run(new MainForm());
			//Application.SetDefaultFont(new Font(mainFontFamily, 10));
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
