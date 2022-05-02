using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace dwarfGame
{
	public class Mob
	{
		public Spritesheet Sheet;
		public string Name;
		
		public int Dir;
		public int MaxMovement;
		public int Movement;
		public int X;
		public int Y;
		
		public Mob() {}
		
		public Mob(string name, string id, int dir, int movement)
		{
			Name = name;
			Dir = dir;
			Sheet = new Spritesheet(id);
			Sheet.StartAnimation(CONST.ACTION_IDLE, dir);
			MaxMovement = movement;
			Movement = movement;
		}
		
		public void Process()
		{
			Sheet.Process();
		}
		
		public bool CanPath(Tile tile)
		{
			if(GetDistanceTo(tile.X, tile.Y) > Movement || !tile.IsPassable())
				return false;
			
			int mov = Movement;
			var tiles = GetTile().GetNeighbours().Where(t => t.IsPassable());
			
			while(mov > 0)
			{
				if(tiles.Contains(tile))
					return true;
				tiles = tiles.SelectMany(t => t.GetNeighbours().Where(t => t.IsPassable()));
				mov--;
			}
			
			return false;
		}
		
		public List<Tile> Path(Tile targetTile)
		{
			List<Tile> path = new List<Tile>();
			int mov = Movement;
			
			var tiles = GetTile().GetNeighbours().Where(tile => tile.IsPassable());
			List<Tuple<Tile, IEnumerable<Tile>>> tileConnections = new List<Tuple<Tile, IEnumerable<Tile>>>();
			tileConnections.Add(Tuple.Create(GetTile(), tiles));
			
			while(mov > 0)
			{
				if(tiles.Contains(targetTile))
				{
					Tile pathTile = targetTile;
					Tile mobTile = GetTile();
					
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
				mov--;
			}
			
			return path;
		}
		
		public int GetDistanceTo(int x, int y)
		{
			return Math.Abs(X - x) + Math.Abs(Y - y);
		}
		
		/*public void Move(List<int> path)
		{
			if(path.Count() > Movement)
				return;
			
			foreach(int t in path)
				switch(t)
				{
					case is DIR.NORTH:
						if(Y == 0)
							break;
						Move(GetTile(), Game.Map[X, --Y]);
						break;
					case is DIR.SOUTH:
						if(Y == Game.MapSizeY - 1)
							break;
						Move(GetTile(), Game.Map[X, ++Y]);
						break;
					case is DIR.EAST:
						if(X == Game.MapSizeX - 1)
							break;
						Move(GetTile(), Game.Map[++X, Y]);
						break;
					case is DIR.WEST:
						if(Y == 0)
							break;
						Move(GetTile(), Game.Map[--X, ++Y]);
						break;
					default:
						throw new Exception($"Invalid direction: {t}");
				}
		}*/
		
		public Tile GetTile()
		{
			return Game.Map[X, Y];
		}
		
		/*private void Move(Tile sourceTile, Tile targetTile)
		{
			if(!(targetTile.Flags & FLAG.PASSABLE))
				throw new Exception("Invalid target tile.");
			
			sourceTile.RemoveMob(this);
			targetTile.AddMob(this);
			Movement--;
		}*/
	}
}
