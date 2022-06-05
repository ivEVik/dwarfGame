
namespace dwarfGame
{
	public static class CONST
	{
		public const int DIR_SOUTH = 0;
		public const int DIR_NORTH = 1;
		public const int DIR_EAST = 2;
		public const int DIR_WEST = 3;
		
		public const int DIE_D2 = 2;
		public const int DIE_D3 = 3;
		public const int DIE_D4 = 4;
		public const int DIE_D6 = 6;
		public const int DIE_D8 = 8;
		public const int DIE_D10 = 10;
		public const int DIE_D12 = 12;
		
		public const int SCORE_KILL = 25;
		public const int SCORE_MAP = 100;
		public const int SCORE_DWARF = 100;
		
		public const string MOB_NULL = "null";
		public const string MOB_DWARF = "dwarf";
		public const string MOB_ZOMBIE = "zombie";
		public const string MOB_SLIME = "blob";
		public const string MOB_DUMMY = "dummy";
		
		public const string ACTION_IDLE = "idle";
		public const string ACTION_MOVE = "move";
		public const string ACTION_STRIKE = "strike";
		public const string ACTION_STAGGER = "stagger";
		public const string ACTION_DODGE = "dodge";
		public const string ACTION_CAST = "cast";
		
		public const int SKILL_LEVEL_NONE = -1;
		public const int SKILL_LEVEL_TRAINED = 0;
		public const int SKILL_LEVEL_MASTERED = 1;
		
		public const string SKILL_DODGE = "dodge";
		public const string SKILL_MELEE = "melee";
		public const string SKILL_CAST = "cast";
		
		public const string TILE_NULL = "null";
		public const string TILE_BRICK = "brick";
		
		public const string TYPE_SNAKE = "-snake";
		
		public const string OVERLAY_NULL = "null";
		public const string OVERLAY_HOVER = "tile-hover-white";
		public const string OVERLAY_HOVER_ALLY = "tile-hover-blue";
		public const string OVERLAY_HOVER_ENEMY = "tile-hover-red";
		public const string OVERLAY_HIGHLIGHT_BLUE = "tile-highlight-blue";
		public const string OVERLAY_HIGHLIGHT_RED = "tile-highlight-red";
		public const string OVERLAY_PATH = "tile-path-white";
		public const string OVERLAY_PATH_ACTIVE = "tile-path-blue";
		public const string OVERLAY_SHADOW = "mob-shadow";
		
		public const string OVERLAY_MOB_HEALTH_BG = "mob-health-background";
		public const string OVERLAY_MOB_HEALTH_FULL = "mob-health-full";
		
		public const string UI_NULL = "ui-null";
		public const string UI_GAME_LOGO = "game-logo";
		
		public const string UI_PORTRAIT = "portrait-bg";
		public const string UI_PORTRAIT_HOVER = "portrait-bg-hover";
		public const string UI_PORTRAIT_PRESS = "portrait-bg-select";
		
		public const string UI_BUTTON_SMALL = "button-small-inactive";
		public const string UI_BUTTON_SMALL_HOVER = "button-small-active";
		public const string UI_BUTTON_SMALL_PRESS = "button-small-press";
		
		public const string UI_BUTTON_MEDIUM = "button-medium-inactive";
		public const string UI_BUTTON_MEDIUM_HOVER = "button-medium-active";
		public const string UI_BUTTON_MEDIUM_PRESS = "button-medium-press";
		
		public const string UI_BUTTON_BIG = "button-big-inactive";
		public const string UI_BUTTON_BIG_HOVER = "button-big-active";
		public const string UI_BUTTON_BIG_PRESS = "button-big-press";
	}
}
