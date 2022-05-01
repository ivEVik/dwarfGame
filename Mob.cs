using System;
using System.Drawing;
using System.Collections.Generic;

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
