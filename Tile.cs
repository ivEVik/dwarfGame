using System;
using System.Drawing;
using System.Collections.Generic;

namespace dwarfGame
{
	public class Tile
	{
		public string SpriteID;
		public int Flags;
		public int X;
		public int Y;
		
		public List<Mob> Mobs;
		
		public Tile() {}
		
		public Tile(int flags, string spriteID)
		{
			SpriteID = spriteID;
			Flags = flags;
			
			Mobs = new List<Mob>();
		}
		
		public Tile(Tuple<int, string> tuple, int x, int y)
		{
			SpriteID = tuple.Item2;
			Flags = tuple.Item1;
			
			X = x;
			Y = y;
			
			Mobs = new List<Mob>();
		}
		
		public bool CheckFlag(int flag)
		{
			if((Flags & flag) == flag)
				return true;
			return false;
		}
		
		public void RemoveMob(Mob mob)
		{
			Mobs.Remove(mob);
		}
		
		public void AddMob(Mob mob)
		{
			Mobs.Add(mob);
		}
	}
}
