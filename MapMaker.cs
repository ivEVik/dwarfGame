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
			tileDict.Add('.', Tuple.Create(FLAG.TILE_PASSABLE & FLAG.TILE_SELECTABLE, CONST.TILE_BRICK));
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
			
			if(rows.Select(row => row.Length).Distinct().Count() != 1 || rows.Count() != rows[0].Length)
				throw new Exception($"Invalid source string: '{source}'");
			
			int xLength = rows[0].Count();
			int yLength = rows.Count();
			int tileCount = xLength * yLength;
			
			Game.Map = new Tile[xLength, yLength];
			Game.MapOrdered = new Tile[tileCount];
			
			int y = 0;
			int triangleOne = (int)Math.Ceiling(tileCount / 2.0);
			int t = 0;
			
			while(t < triangleOne)
			{
				for(int i = y; i >= 0; i--)
				{
					int tempX = y - i;
					//Tile tile = new Tile(tileDict[rows[tempX][i]], tempX, i);
					Tile tile = new Tile(tileDict[rows[i][tempX]], tempX, i);
					Game.Map[tempX, i] = tile;
					Game.MapOrdered[t++] = tile;
				}
				
				y++;
			}
			
			y--;
			int x = 0;
			
			while(t < tileCount)
			{
				int tempX = x;
				
				for(int i = y; i > x; i--)
				{
					tempX++;
					//Tile tile = new Tile(tileDict[rows[tempX][i]], tempX, i);
					Tile tile = new Tile(tileDict[rows[i][tempX]], tempX, i);
					Game.Map[tempX, i] = tile;
					Game.MapOrdered[t++] = tile;
				}
				
				x++;
			}
		}
	}
}
