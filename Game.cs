using System;
using System.Collections.Generic;

namespace dwarfGame
{
	public static class Game
	{
		private const string mapTestPreset = @"
.....
... .
.....
... .
.. ..";
		
		public static int Scale;
		public static Tile[,] Map;
		public static int MapX;
		public static int MapY;
		public static Tile[] MapOrdered;
		
		public static List<Mob> Mobs;
		//public static Mob SelectedMob;
		
        public static int ElementSize;
		public static int Horizontal;
		public static int Vertical;
		public static int DiagLength;
		public static int SideShift;
		
		public static void Process()
		{
			foreach(Mob mob in Mobs)
				mob.Process();
		}
		
		public static void Initialise(int scale)
		{
			Scale = scale;
			ElementSize = Scale * 32;
			Horizontal = ElementSize / 2;
			Vertical = Horizontal / 2;
		}
		
		public static void MakeMapFromString()
		{
			Mobs = new List<Mob>();
			MapMaker.MakeMapFromString(mapTestPreset);
			MapX = Map.GetLength(0);
			MapY = Map.GetLength(1);
			Mob mob = new Mob("Urist", CONST.MOB_DWARF, CONST.DIR_SOUTH, 3);
			mob.X = 2;
			mob.Y = 2;
			Mobs.Add(mob);
			Map[mob.X, mob.Y].AddMob(mob);
			
			SideShift = Game.MapX * ElementSize / 2;
			DiagLength = (int)Math.Sqrt(Game.MapX * ElementSize * Game.MapX * ElementSize + Game.MapY * ElementSize * Game.MapY * ElementSize);
		}
	}
}