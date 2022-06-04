using System;
using System.Collections.Generic;
using System.Linq;

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
		
		public static bool InGame;
		public static bool PlayerTurn;
		
		public static Tile[,] Map;
		public static int MapX;
		public static int MapY;
		
		public static List<Mob> Mobs;
		public static List<Mob> MobsToRemove;
		
		public static Queue<Mob> TurnQueue;
		public static Mob CurrentMob;
		
		public static Mob[] Dwarves;
		
		public static void Process()
		{
			foreach(Mob mob in Mobs)
				mob.Process();
			foreach(Mob mob in MobsToRemove)
				Mobs.Remove(mob);
			if(MobsToRemove.Count > 0)
				MobsToRemove = new List<Mob>();
		}
		
		public static void PassTurn()
		{
			if(PlayerTurn)
			{
				if(CurrentMob != null)
				{
					CurrentMob.DropSelection();
					CurrentMob = null;
				}
				PlayerTurn = false;
			}
			
			if(CurrentMob != null)
				TurnQueue.Enqueue(CurrentMob);
			
			if(TurnQueue.Count > 0)
				if(TurnQueue.Peek().TookTurn)
					StartPlayerTurn();
				else
					CurrentMob = TurnQueue.Dequeue();
					CurrentMob.StartTurn();
		}
		
		public static void StartPlayerTurn()
		{
			PlayerTurn = true;
			
			foreach(Mob dwarf in Dwarves.Where(df => df != null && !df.CheckFlags(FLAG.MOB_DEAD)))
			{
				dwarf.StartTurn();
				dwarf.DropSelection();
			}
			
			foreach(Mob mob in Mobs)
				mob.TookTurn = false;
			
			Select(Dwarves.FirstOrDefault(dwarf => dwarf != null && !dwarf.CheckFlags(FLAG.MOB_DEAD)));
		}
		
		public static void Select(Mob mob)
		{
			if(!PlayerTurn || mob == null || !mob.Ally || mob.CheckFlags(FLAG.MOB_DEAD))
				return;
			
			CurrentMob = mob;
			mob.Select();
		}
		
		public static void WipeMob(Mob mob)
		{
			MobsToRemove.Add(mob);
			mob.GetTile().Mobs.Remove(mob);
		}
		
		public static void WipeCurrent()
		{
			CurrentMob = null;
		}
		
		public static void Initialise()
		{
			Randomiser = new Random();
		}
		
		public static void NewGame(MapTemplate template)
		{
			Dwarves = new Mob[3] {
				new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Urist", true),
				new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Rockbeard", true),
				new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Ironhammer", true)
			};
			
			LoadMap(template);
			StartGame();
		}
		
		public static void LoadMap(MapTemplate template)
		{
			MobsToRemove = new List<Mob>();
			Mobs = new List<Mob>();
			TurnQueue = new Queue<Mob>();
			
			Map = MapMaker.MakeMap(template);
			
			MapX = Map.GetLength(0);
			MapY = Map.GetLength(1);
			
			for(int t = 0; t < Dwarves.Length; t++)
				Map[template.Dwarves[t].X, template.Dwarves[t].Y].SpawnMob(Dwarves[t]);
			
			foreach(Tile t in Map)
				foreach(Mob mob in t.Mobs)
					Mobs.Add(mob);
			
			foreach(Mob mob in Mobs.Where(mob => !mob.Ally))
				TurnQueue.Enqueue(mob);
		}
		
		public static void DropMap()
		{
			InGame = false;
			
			MobsToRemove = new List<Mob>();
			Mobs = new List<Mob>();
			TurnQueue = new Queue<Mob>();
			
			Map = null;
			MapX = 0;
			MapY = 0;
			
			CurrentMob = null;
			Dwarves = null;
		}
		
		public static void MakeMapFromString()
		{
			MobsToRemove = new List<Mob>();
			Mobs = new List<Mob>();
			TurnQueue = new Queue<Mob>();
			MapMaker.MakeMapFromString(mapTestPreset);
			
			Mob mob = new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Urist", true);
			//Mob mob = Dwarves[0];
			Mobs.Add(mob);
			Map[4, 4].SpawnMob(mob);
			
			mob = new Mob(TEMPLATE.MOB_SLIME, "Blob", false);
			Mobs.Add(mob);
			Map[2, 1].SpawnMob(mob);
			
			foreach(Mob m in Mobs)
				TurnQueue.Enqueue(m);
			PassTurn();
		}
		
		public static void StartGame()
		{
			InGame = true;
			StartPlayerTurn();
		}
	}
}
