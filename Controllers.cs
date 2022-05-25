using System;
using System.Linq;

namespace dwarfGame
{
	public static class Controllers
	{
		public static bool DecideAggressive(Mob currentMob)
		{
			string action = null;
			Mob targetMob = null;
			Tile targetTile = null;
			bool endTurn = true;
			
			targetMob = Game.Mobs
				.Where(mob => mob.Ally && !mob.CheckFlags(FLAG.MOB_DEAD) && currentMob.CanPath(mob))
				.OrderBy(mob => currentMob.Path(mob).Count)
				.FirstOrDefault();
			
			if(targetMob != null)
				if(currentMob.IsLocked())
					endTurn = false;
				else if(currentMob.CanAct() && currentMob.GetDistance(targetMob) <= 1)
					action = CONST.ACTION_STRIKE;
				else if(!currentMob.HasMoved)
				{
					action = CONST.ACTION_MOVE;
					endTurn = false;
				}
			
			currentMob.Act(action, mob: targetMob, tile: targetTile);
			return endTurn;
		}
	}
}
