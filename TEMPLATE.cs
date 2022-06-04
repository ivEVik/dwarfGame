using System;
using System.Collections.Generic;
using System.Drawing;

namespace dwarfGame
{
	public static class TEMPLATE
	{
		public static Attack ATTACK_UNARMED = new Attack(new Dice(CONST.DIE_D3), 1, 0);
		public static Attack ATTACK_UNARMED_IMPROVED = new Attack(new Dice(CONST.DIE_D2), 1, FLAG.ATTACK_ROLL_ATTRIBUTE);
		public static Attack ATTACK_UNARMED_ZOMBIE = new Attack(new Dice(CONST.DIE_D3, 2), 1, 0);
		public static Attack ATTACK_UNARMED_SLIME = new Attack(new Dice(CONST.DIE_D3, 2), 1, 0);
		
		public static MobTemplate MOB_NULL = new MobTemplate(
			MobID: CONST.MOB_NULL
		);
		
		public static MobTemplate MOB_DWARF_BRAWLER = new MobTemplate(
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
		
		public static MobTemplate MOB_ZOMBIE_SHAMBLER = new MobTemplate(
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
		
		public static MobTemplate MOB_SLIME = new MobTemplate(
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
		
		
		public static TileTemplate TILE_NULL {
			get { return new TileTemplate(
				SpriteID: CONST.TILE_NULL,
				Flags: 0
			);}
		}
		
		public static TileTemplate TILE_BRICK = new TileTemplate(
			SpriteID: CONST.TILE_BRICK,
			Flags: FLAG.TILE_PASSABLE | FLAG.TILE_SELECTABLE
		);
		
		
		public static MapTemplate MAP_TEST = new MapTemplate(
			Name: "Test Map",
			TileDict: MakeTileset(
				Tuple.Create(' ', TILE_NULL),
				Tuple.Create('.', TILE_BRICK)
			),
			MobDict: MakeMobset(
				Tuple.Create('.', MOB_NULL),
				Tuple.Create('S', MOB_SLIME)
			),
			TileString: @"
.....
.. ..
.. ..
..  .
.....",
			MobString: @"
.S...
.....
.....
.....
.....",
			//Dwarves: new Point[3] { new Point(12, 4), new Point(12, 4), new Point(12, 6) }
			Dwarves: new Point[3] { new Point(2, 4), new Point(4, 4), new Point(4, 1) }
		);
		
		public static MapTemplate MAP_TEST_BIG = new MapTemplate(
			Name: "Test Map",
			TileDict: MakeTileset(
				Tuple.Create(' ', TILE_NULL),
				Tuple.Create('.', TILE_BRICK)
			),
			MobDict: MakeMobset(
				Tuple.Create('.', MOB_NULL),
				Tuple.Create('S', MOB_SLIME)
			),
			TileString: @"
.........
.........
.........
.........
  .. ..  
.........
.........
...   ...
..  .  ..
.. ... ..
.........
   ...   
.........
.........
.........
.........",
			MobString: @"
.........
.......S.
..S..S...
.........
.........
..S......
.........
.........
.........
.....S...
..S....S.
.........
...S.....
.........
.........
.........",
			//Dwarves: new Point[3] { new Point(12, 4), new Point(12, 4), new Point(12, 6) }
			Dwarves: new Point[3] { new Point(2, 14), new Point(4, 14), new Point(6, 14) }
		);
		
		
		private static Dictionary<char, TileTemplate> MakeTileset(params Tuple<char, TileTemplate>[] templates)
		{
			Dictionary<char, TileTemplate> tileset = new Dictionary<char, TileTemplate>();
			
			foreach(Tuple<char, TileTemplate> template in templates)
				tileset[template.Item1] = template.Item2;
			
			return tileset;
		}
		
		private static Dictionary<char, MobTemplate> MakeMobset(params Tuple<char, MobTemplate>[] templates)
		{
			Dictionary<char, MobTemplate> mobset = new Dictionary<char, MobTemplate>();
			
			foreach(Tuple<char, MobTemplate> template in templates)
				mobset[template.Item1] = template.Item2;
			
			return mobset;
		}
		
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
