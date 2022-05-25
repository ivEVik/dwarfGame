
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
	}
}
