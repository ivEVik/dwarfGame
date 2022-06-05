using System;
using System.Linq;
using System.Collections.Generic;

namespace dwarfGame
{
	public static class MapMaker
	{
		private static readonly Dictionary<char, Tuple<int, string>> tileDictionary = new Dictionary<char, Tuple<int, string>>();
		
		public static void Initialise()
		{
			tileDictionary.Add('.', Tuple.Create(FLAG.TILE_PASSABLE | FLAG.TILE_SELECTABLE, CONST.TILE_BRICK));
			//tileDict['#'] = FLAG.SELECTABLE;
			//tileFlagDict['E'] = FLAG.PASSABLE + FLAG.SELECTABLE;
			tileDictionary.Add(' ', Tuple.Create(0, CONST.TILE_NULL));
		}
		
		public static Tile[,] MakeMap(MapTemplate template)
		{
			Dictionary<char, TileTemplate> tileDict = template.TileDict;
			char[][] tileRows = template.TileString
				.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
				.Select(row => row.ToCharArray())
				.ToArray();
			
			Dictionary<char, MobTemplate> mobDict = template.MobDict;
			char[][] mobRows = template.MobString
				.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
				.Select(row => row.ToCharArray())
				.ToArray();
			
			if(tileRows.Select(row => row.Length).Distinct().Count() != 1 || mobRows.Select(row => row.Length).Distinct().Count() != 1)
				throw new Exception($"Invalid source template: {template.MapID}");
			
			int mapX = tileRows[0].Count();
			int mapY = tileRows.Count();
			
			Tile[,] map = new Tile[mapX, mapY];
			
			for(int y = 0; y < mapY; y++)
				for(int x = 0; x < mapX; x++)
				{
					map[x, y] = new Tile(tileDict[tileRows[y][x]], x, y);
					if(mobDict[mobRows[y][x]] != TEMPLATE.MOB_NULL)
						map[x, y].SpawnMob(new Mob(mobDict[mobRows[y][x]], "Unnamed", false));
				}
			
			return map;
		}
		
		public static void MakeMapFromString(string source)
		{
			char[][] rows = source
				.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
				.Select(row => row.ToCharArray())
				.ToArray();
			
			if(rows.Select(row => row.Length).Distinct().Count() != 1)
				throw new Exception($"Invalid source string: '{source}'");
			
			Game.MapX = rows[0].Count();
			Game.MapY = rows.Count();
			
			Game.Map = new Tile[Game.MapX, Game.MapY];
			
			for(int y = 0; y < Game.MapY; y++)
				for(int x = 0; x < Game.MapX; x++)
					Game.Map[x, y] = new Tile(tileDictionary[rows[y][x]], x, y);
		}
	}
}
