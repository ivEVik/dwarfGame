using System;
using System.Linq;
using System.Collections.Generic;

namespace dwarfGame
{
	public static class MapMaker
	{
		private static readonly Dictionary<char, Tuple<int, string>> tileDict = new Dictionary<char, Tuple<int, string>>();
		
		public static void Initialise()
		{
			tileDict.Add('.', Tuple.Create(FLAG.TILE_PASSABLE | FLAG.TILE_SELECTABLE, CONST.TILE_BRICK));
			//tileDict['#'] = FLAG.SELECTABLE;
			//tileFlagDict['E'] = FLAG.PASSABLE + FLAG.SELECTABLE;
			tileDict.Add(' ', Tuple.Create(0, CONST.TILE_NULL));
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
					Game.Map[x, y] = new Tile(tileDict[rows[y][x]], x, y);
		}
	}
}
