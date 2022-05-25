using System;
using System.Collections.Generic;

namespace dwarfGame
{
	public static class TEMPLATE
	{
		public static readonly Attack ATTACK_UNARMED = new Attack(new Dice(CONST.DIE_D3), 1, 0);
		public static readonly Attack ATTACK_UNARMED_IMPROVED = new Attack(new Dice(CONST.DIE_D2), 1, FLAG.ATTACK_ROLL_ATTRIBUTE);
		public static readonly Attack ATTACK_UNARMED_ZOMBIE = new Attack(new Dice(CONST.DIE_D3, 2), 1, 0);
		public static readonly Attack ATTACK_UNARMED_SLIME = new Attack(new Dice(CONST.DIE_D3, 2), 1, 0);
		
		public static readonly MobTemplate MOB_DWARF_BRAWLER = new MobTemplate(
			MobID: CONST.MOB_DWARF,
			Health: 5,
			Movement: 3,
			Weapon: ATTACK_UNARMED_IMPROVED,
			Str: 3,
			Con: 3,
			Dex: 2,
			Per: 2,
			Int: 2,
			Wil: 2,
			Flags: FLAG.MOB_IMPROVED_STRIKE,
			Skills: MakeSkillset(
				dodge: CONST.SKILL_LEVEL_TRAINED,
				melee: CONST.SKILL_LEVEL_TRAINED
			),
			Controller: (Mob mob) => Controllers.DecideAggressive(mob)
		);
		
		public static readonly MobTemplate MOB_ZOMBIE_SHAMBLER = new MobTemplate(
			MobID: CONST.MOB_ZOMBIE,
			Health: 6,
			Movement: 2,
			Weapon: ATTACK_UNARMED_ZOMBIE,
			Str: 2,
			Con: 3,
			Dex: 1,
			Per: 1,
			Int: 1,
			Wil: 1,
			Flags: FLAG.MOB_UNDEAD,
			Skills: MakeSkillset(
				melee: 10
			),
			Controller: (Mob mob) => Controllers.DecideAggressive(mob)
		);
		
		public static readonly MobTemplate MOB_SLIME = new MobTemplate(
			MobID: CONST.MOB_SLIME,
			Health: 4,
			Movement: 2,
			Weapon: ATTACK_UNARMED_SLIME,
			Str: 2,
			Con: 4,
			Dex: 2,
			Per: 1,
			Int: 1,
			Wil: 2,
			Flags: FLAG.MOB_AMORPHOUS,
			Skills: MakeSkillset(
				cast: CONST.SKILL_LEVEL_TRAINED
			),
			Controller: (Mob mob) => Controllers.DecideAggressive(mob)
		);
		
		
		private static Dictionary<string, int> MakeSkillset(
			int dodge = CONST.SKILL_LEVEL_NONE,
			int melee = CONST.SKILL_LEVEL_NONE,
			int cast = CONST.SKILL_LEVEL_NONE
			)
		{
			Dictionary<string, int> result = new Dictionary<string, int>();
			
			result.Add(CONST.SKILL_DODGE, dodge);
			result.Add(CONST.SKILL_MELEE, melee);
			result.Add(CONST.SKILL_CAST, cast);
			
			return result;
		}
	}
}
