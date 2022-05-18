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
		
		public static List<Mob> Mobs;
		
		public static Queue<Mob> TurnQueue;
		public static Mob CurrentMob;
		
        public static int ElementSize;
		public static int Horizontal;
		public static int Vertical;
		public static int DiagLength;
		
		public static void Process()
		{
			foreach(Mob mob in Mobs)
				mob.Process();
		}
		
		public static void PassTurn()
		{
			if(CurrentMob != null)
				TurnQueue.Enqueue(CurrentMob);
			if(TurnQueue.Count > 0)
			{
				CurrentMob = TurnQueue.Dequeue();
				CurrentMob.StartTurn();
			}
		}
		
		public static void Initialise(int scale)
		{
			Scale = scale;
			ElementSize = Scale * 32;
			Horizontal = ElementSize / 2;
			Vertical = Horizontal / 2;
			TurnQueue = new Queue<Mob>();
		}
		
		public static void MakeMapFromString()
		{
			Mobs = new List<Mob>();
			MapMaker.MakeMapFromString(mapTestPreset);
			
			Mob mob = new Mob("Urist", CONST.MOB_DWARF, CONST.DIR_SOUTH, 3, 2, 2, true);
			Mobs.Add(mob);
			Map[mob.X, mob.Y].AddMob(mob);
			
			Mob mob2 = new Mob("Urist mk2", CONST.MOB_DWARF, CONST.DIR_SOUTH, 2, 2, 1, true);
			Mobs.Add(mob2);
			Map[mob2.X, mob2.Y].AddMob(mob2);
			
			foreach(Mob m in Mobs)
				TurnQueue.Enqueue(m);
			PassTurn();
			
			DiagLength = (int)Math.Sqrt(MapX * ElementSize * MapX * ElementSize + MapY * ElementSize * MapY * ElementSize);
		}
	}
}
