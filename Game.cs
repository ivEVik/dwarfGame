using System;
using System.Collections.Generic;
using System.Linq;

namespace dwarfGame
{
	public static class Game
	{
		private static MapTemplate[] mapSequence;
		private static int currentMap;
		private static int kills;
		
		public static int ScoreKills {
			get
			{
				return kills * CONST.SCORE_KILL;
			}
		}
		public static int ScoreDwarves {
			get
			{
				return Dwarves.Where(dwarf => dwarf != null && !dwarf.CheckFlags(FLAG.MOB_DEAD)).Count() * CONST.SCORE_DWARF;
			}
		}
		public static int ScoreMaps {
			get
			{
				return currentMap * CONST.SCORE_MAP;
			}
		}
		
		public static Random Randomiser;
		
		public static bool InGame;
		public static bool InMainMenu;
		public static bool PlayerTurn;
		public static bool GameOver;
		
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
			
			int hostileCount = Mobs.Where(mob => !mob.Ally).Count();
			if(Dwarves.All(dwarf => dwarf == null || dwarf.CheckFlags(FLAG.MOB_DEAD)))
				EndGame();
			else if(hostileCount == 0 || (hostileCount == 1 && !mob.Ally))
				AdvanceGame();
		}
		
		public static void WipeCurrent()
		{
			CurrentMob = null;
			
			if(Dwarves.All(dwarf => dwarf == null || dwarf.CheckFlags(FLAG.MOB_DEAD)))
				EndGame();
		}
		
		public static void Initialise()
		{
			Randomiser = new Random();
			InMainMenu = true;
		}
		
		public static void NewGame(int length)
		{
			InMainMenu = false;
			mapSequence = GenerateMapSequence(length);
			currentMap = -1;
			
			Dwarves = new Mob[3] {
				new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Urist", true),
				new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Rockbeard", true),
				new Mob(TEMPLATE.MOB_DWARF_BRAWLER, "Ironhammer", true)
			};
			
			AdvanceGame();
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
		}
		
		public static void StartGame()
		{
			InGame = true;
			StartPlayerTurn();
		}
		
		public static void AddKill()
		{
			kills++;
		}
		
		public static void ResetToMainMenu()
		{
			mapSequence = null;
			GameOver = false;
			InGame = false;
			InMainMenu = true;
			Dwarves = null;
		}
		
		private static MapTemplate[] GenerateMapSequence(int length)
		{
			MapTemplate[] sequence = new MapTemplate[length];
			List<int> usedMaps = new List<int>();
			int count = 0;
			
			while(count < length)
			{
				int mapNumber = Randomiser.Next(0, TEMPLATE.MAPS.Count);
				if(!usedMaps.Contains(mapNumber))
				{
					sequence[count++] = TEMPLATE.MAPS[mapNumber];
					usedMaps.Add(mapNumber);
				}
			}
			
			return sequence;
		}
		
		private static void EndGame()
		{
			GameOver = true;
			InGame = false;
		}
		
		private static void AdvanceGame()
		{
			if(++currentMap >= mapSequence.Length)
			{
				if(!Dwarves.Any(dwarf => dwarf != null || !dwarf.CheckFlags(FLAG.MOB_DEAD)))
					currentMap--;
				
				EndGame();
				return;
			}
			
			DropMap();
			LoadMap(mapSequence[currentMap]);
			StartGame();
		}
	}
}
