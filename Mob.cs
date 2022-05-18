using System;
using System.Collections.Generic;
using System.Linq;

namespace dwarfGame
{
	public class Mob
	{
		private List<Tile> movePath;
		private bool acting;
		private bool current;
		
		public bool Ally;
		
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
		
		public Mob(string name, string id, int dir, int movement, int x, int y, bool ally)
		{
			X = x;
			Y = y;
			movePath = new List<Tile>();
			acting = false;
			Name = name;
			Dir = dir;
			Sheet = new Spritesheet(id);
			Sheet.SetCoords(X, Y);
			Sheet.SetDestination(X, Y);
			Sheet.StartAnimation(CONST.ACTION_IDLE, dir);
			MaxMovement = movement;
			Movement = movement;
			Ally = ally;
		}
		
		public void StartTurn()
		{
			Movement = MaxMovement;
			Action = true;
			acting = false;
			current = true;
		}
		
		public void EndTurn()
		{
			current = false;
			Game.PassTurn();
		}
		
		public void Process()
		{
			Sheet.Process();
			if(!acting && current && Movement == 0)
				EndTurn();
			if(current)
				TryMove();
		}
		
		public Tile GetTile()
		{
			return Game.Map[X, Y];
		}
		
		public void Move(List<Tile> path)
		{
			if(path.Count() > Movement || acting)
				return;
			
			movePath = path;
			acting = true;
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
				acting = false;
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
		
		public bool CanPath(Tile tile)
		{
			return GetTile().CanPath(tile, Movement);
		}
		
		public List<Tile> Path(Tile tile)
		{
			return GetTile().Path(tile, Movement);
		}
		
		public int GetDistanceTo(int x, int y)
		{
			return GetTile().GetDistanceTo(x, y);
		}
	}
}
