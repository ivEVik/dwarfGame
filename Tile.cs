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
		
		public IEnumerable<Tile> GetNeighbours()
		{
			if(X > 0)
				yield return Game.Map[X - 1, Y];
			if(X < Game.MapX - 1)
				yield return Game.Map[X + 1, Y];
			if(Y > 0)
				yield return Game.Map[X, Y - 1];
			if(Y < Game.MapY - 1)
				yield return Game.Map[X, Y + 1];
		}
		
		public override bool Equals(object obj)
		{
			if(!(obj is Tile))
				return false;
			
			Tile tile = obj as Tile;
			if(X == tile.X && Y == tile.Y)
				return true;
			return false;
		}
		
		public override int GetHashCode()
		{
			return new Point(X, Y).GetHashCode();
		}
		
		public bool IsPassable()
		{
			return CheckFlag(FLAG.TILE_PASSABLE) && Mobs.Count == 0;
		}
		
		public void RemoveMob(Mob mob)
		{
			Mobs.Remove(mob);
		}
		
		public void AddMob(Mob mob)
		{
			Mobs.Add(mob);
			mob.X = X;
			mob.Y = Y;
		}
		
		public int GetDir(Tile tile)
		{
			int dX = tile.X - X;
			int dY = tile.Y - Y;
			
			if(Math.Abs(dX) > Math.Abs(dY))
			{
				if(dX > 0)
					return CONST.DIR_EAST;
				return CONST.DIR_WEST;
			}
			
			if(dY > 0)
				return CONST.DIR_SOUTH;
			return CONST.DIR_NORTH;
		}
	}
}
