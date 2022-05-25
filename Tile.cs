using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace dwarfGame
{
	public class Tile
	{
		private int flags;
		
		public string SpriteID;
		public int X;
		public int Y;
		
		public List<Mob> Mobs;
		
		public Tile() {}
		
		public Tile(int flags, string spriteID)
		{
			SpriteID = spriteID;
			this.flags = flags;
			
			Mobs = new List<Mob>();
		}
		
		public Tile(Tuple<int, string> tuple, int x, int y)
		{
			SpriteID = tuple.Item2;
			flags = tuple.Item1;
			
			X = x;
			Y = y;
			
			Mobs = new List<Mob>();
		}
		
		public bool CheckFlags(int flags)
		{
			if((this.flags & flags) == flags)
				return true;
			return false;
		}
		
		public void DropFlags(int flags)
		{
			this.flags -= this.flags & flags;
		}
		
		public void AddFlags(int flags)
		{
			this.flags = this.flags | flags;
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
		
		public bool CanPath(Tile tile, int distance)
		{
			if(tile == this)
				return true;
			if(GetDistance(tile.X, tile.Y) > distance || !tile.IsPassable())
				return false;
			
			var tiles = GetNeighbours().Where(t => t.IsPassable());
			
			while(distance > 0)
			{
				if(tiles.Contains(tile))
					return true;
				tiles = tiles.SelectMany(t => t.GetNeighbours().Where(t => t.IsPassable()));
				distance--;
			}
			
			return false;
		}
		
		public List<Tile> Path(Tile targetTile, int distance)
		{
			List<Tile> path = new List<Tile>();
			
			if(targetTile == this)
				return path;
			
			var tiles = GetNeighbours().Where(tile => tile.IsPassable());
			List<Tuple<Tile, IEnumerable<Tile>>> tileConnections = new List<Tuple<Tile, IEnumerable<Tile>>>();
			tileConnections.Add(Tuple.Create(this, tiles));
			
			while(distance > 0)
			{
				if(tiles.Contains(targetTile))
				{
					Tile pathTile = targetTile;
					Tile mobTile = this;
					
					while(pathTile != mobTile)
					{
						path.Add(pathTile);
						pathTile = tileConnections
							.Where(tuple => tuple.Item2.Contains(pathTile))
							.FirstOrDefault()
							.Item1;
					}
					
					path.Reverse();
					break;
				}
				
				var neighbours = tiles.Select(tile => Tuple.Create(tile, tile.GetNeighbours().Where(t => t.IsPassable())));
				tileConnections.AddRange(neighbours);
				tiles = neighbours.SelectMany(tuple => tuple.Item2);
				distance--;
			}
			
			return path;
		}
		
		public int GetDistance(int x, int y)
		{
			return Math.Abs(X - x) + Math.Abs(Y - y);
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
			return CheckFlags(FLAG.TILE_PASSABLE) && Mobs.Count == 0;
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
		
		public int GetDir(Mob mob)
		{
			return GetDir(mob.X, mob.Y);
		}
		
		public int GetDir(Tile tile)
		{
			return GetDir(tile.X, tile.Y);
		}
		
		public int GetDir(int x, int y)
		{
			int dX = x - X;
			int dY = y - Y;
			
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
