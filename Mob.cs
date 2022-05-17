using System;
using System.Collections.Generic;
using System.Linq;

namespace dwarfGame
{
	public class Mob
	{
		private List<Tile> movePath;
		
		public Spritesheet Sheet;
		public string Name;
		
		public int Health;
		public bool Action;
		
		public int Dir;
		public int MaxMovement;
		public int Movement;
		public int X;
		public int Y;
		
		public Mob() {}
		
		public Mob(string name, string id, int dir, int movement, int x, int y)
		{
			X = x;
			Y = y;
			movePath = new List<Tile>();
			Name = name;
			Dir = dir;
			Sheet = new Spritesheet(id);
			Sheet.SetCoords(X, Y);
			Sheet.SetDestination(X, Y);
			Sheet.StartAnimation(CONST.ACTION_IDLE, dir);
			MaxMovement = movement;
			Movement = movement;
		}
		
		public void Process()
		{
			TryMove();
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
		
		public Tile GetTile()
		{
			return Game.Map[X, Y];
		}
		
		public void Move(List<Tile> path)
		{
			if(path.Count() > Movement)
				return;
			
			movePath = path;
		}
		
		private void TryMove()
		{ 
			if(movePath.Count() == 0 || Sheet.IsLocked())
				return;
			
			Move(movePath.FirstOrDefault());
			movePath.Remove(movePath.FirstOrDefault());
			
			if(movePath.Count() == 0 && Sheet.IsLocked())
			{
				Sheet.PrepIdle = true;
				return;
			}
		}
		
		private void Move(Tile tile)
		{
			if(!tile.IsPassable())
				throw new Exception("Invalid target tile.");
			
			Dir = GetDir(tile);
			
			GetTile().RemoveMob(this);
			tile.AddMob(this);
			
			if(Sheet.GetDir() != Dir || Sheet.GetAction() != CONST.ACTION_MOVE)
				Sheet.StartAnimation(CONST.ACTION_MOVE, Dir, true);
			
			Sheet.SetDestination(X, Y);
			Movement--;
		}
		
		private int GetDir(Tile tile)
		{
			return GetTile().GetDir(tile);
		}
	}
}
