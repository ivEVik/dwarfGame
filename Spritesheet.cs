using System;
using System.Drawing;
using System.Collections.Generic;

namespace dwarfGame
{
	public class Spritesheet
	{
		private Dictionary<string, Dictionary<int, Tuple<string, int>[]>> sprites;
		private string portraitID;
		
		private string action;
		private int dir;
		private int drawDir;
		private int frame;
		private int tickCount;
		private bool locked;
		private bool mov;
		private bool noCycle;
		
		public bool PrepIdle;
		public Point Coords;
		public Point Destination;
		
		public Spritesheet(string id)
		{
			sprites = Sprites.GetSprites(id, out portraitID);
			action = CONST.ACTION_IDLE;
			dir = CONST.DIR_SOUTH;
			drawDir = CONST.DIR_SOUTH;
			frame = 0;
			tickCount = 0;
			locked = false;
			mov = false;
		}
		
		public void SetDestination(int x, int y)
		{
			Destination = new Point((x - y) * Sprites.Horizontal, (x + y) * Sprites.Vertical);
			
			if(Destination != Coords)
			{
				mov = true;
				locked = true;
			}
		}
		
		public void SetCoords(int x, int y)
		{
			Coords = new Point((x - y) * Sprites.Horizontal, (x + y) * Sprites.Vertical);
		}
		
		public void StartAnimation(string act, int direction, bool doLock = false, bool noCycle = false)
		{
			frame = 0;
			tickCount = 0;
			dir = direction;
			
			if(sprites.ContainsKey(act))
				action = act;
			else
				action = CONST.ACTION_IDLE;
			
			if(sprites[action].ContainsKey(direction))
				drawDir = dir;
			else
				drawDir = CONST.DIR_SOUTH;
			
			if(act == action)
				locked = doLock;
			this.noCycle = noCycle;
		}
		
		public void Process()
		{
			tickCount++;
			if(tickCount > sprites[action][drawDir][frame].Item2)
				IncrementFrame();
			
			if(mov)
				Move();
		}
		
		public string GetFrameID()
		{
			return sprites[action][drawDir][frame].Item1;
		}
		
		public bool IsLocked()
		{
			return locked;
		}
		
		public int GetDir()
		{
			return dir;
		}
		
		public string GetAction()
		{
			return action;
		}
		
		public string GetPortraitID()
		{
			return portraitID;
		}
		
		private void IncrementFrame()
		{
			if(sprites[action][drawDir].Length > frame + 1)
				frame++;
			else
			{
				frame = 0;
				if(noCycle)
					Idle();
			}
			
			tickCount = 0;
		}
		
		private void Idle()
		{
			StartAnimation(CONST.ACTION_IDLE, dir);
			Unlock();
			PrepIdle = false;
		}
		
		private void Unlock()
		{
			locked = false;
		}
		
		private void Move()
		{
			if(Coords.X < Destination.X)
				Coords.X += Sprites.IncX;
			else if(Coords.X > Destination.X)
				Coords.X -= Sprites.IncX;
			
			if(Coords.Y < Destination.Y)
				Coords.Y += Sprites.IncY;
			else if(Coords.Y > Destination.Y)
				Coords.Y -= Sprites.IncY;
			
			if(Coords == Destination)
			{
				mov = false;
				Unlock();
				if(PrepIdle)
					Idle();
			}
		}
	}
}
