using System;
using System.Drawing;
using System.Collections.Generic;

namespace dwarfGame
{
	public class Spritesheet
	{
		private Dictionary<string, Dictionary<int, Tuple<string, int>[]>> sprites;
		private string action;
		private int dir;
		private int frame;
		private int tickCount;
		
		public Spritesheet(string id)
		{
			sprites = Sprites.GetSprites(id);
			action = CONST.ACTION_IDLE;
			dir = CONST.DIR_SOUTH;
			frame = 0;
			tickCount = 0;
		}
		
		public void StartAnimation(string act, int direction)
		{
			frame = 0;
			tickCount = 0;
			
			if(sprites.ContainsKey(act))
				action = act;
			else
				action = CONST.ACTION_IDLE;
			
			if(sprites[action].ContainsKey(direction))
				dir = direction;
			else
				dir = CONST.DIR_SOUTH;
		}
		
		public void Process()
		{
			tickCount++;
			if(tickCount > sprites[action][dir][frame].Item2)
				IncrementFrame();
		}
		
		public string GetFrameID()
		{
			return sprites[action][dir][frame].Item1;
		}
		
		private void IncrementFrame()
		{
			if(sprites[action][dir].Length > frame + 1)
				frame++;
			else
				frame = 0;
			
			tickCount = 0;
		}
	}
}
