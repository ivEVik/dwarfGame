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
		
		public static Random Randomiser;
		
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
		
		public static void WipeMob(Mob mob)
		{
			Mobs.Remove(mob);
			mob.GetTile().Mobs.Remove(mob);
		}
		
		public static void WipeCurrent()
		{
			CurrentMob = null;
		}
		
		public static void Initialise(int scale)
		{
			Scale = scale;
			ElementSize = Scale * 32;
			Horizontal = ElementSize / 2;
			Vertical = Horizontal / 2;
			Randomiser = new Random();
		}
		
		public static void MakeMapFromString()
		{
			Mobs = new List<Mob>();
			TurnQueue = new Queue<Mob>();
			MapMaker.MakeMapFromString(mapTestPreset);
			
			Mob mob = new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Urist", 2, 2, true);
			Mobs.Add(mob);
			Map[mob.X, mob.Y].AddMob(mob);
			
			mob = new Mob(TEMPLATE.MOB_SLIME, "Blob", 1, 2, false);
			Mobs.Add(mob);
			Map[mob.X, mob.Y].AddMob(mob);
			
			foreach(Mob m in Mobs)
				TurnQueue.Enqueue(m);
			PassTurn();
			
			DiagLength = (int)Math.Sqrt(MapX * ElementSize * MapX * ElementSize + MapY * ElementSize * MapY * ElementSize);
		}
	}
}
