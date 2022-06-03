using System;
using System.Collections.Generic;
using System.Drawing;

namespace dwarfGame
{
	public class MapTemplate
	{
		public readonly string Name;
		
		public readonly Dictionary<char, TileTemplate> TileDict;
		public readonly Dictionary<char, MobTemplate> MobDict;
		
		public readonly string TileString;
		public readonly string MobString;
		
		public readonly Point[] Dwarves;
		
		public MapTemplate(
			string Name,
			Dictionary<char, TileTemplate> TileDict,
			Dictionary<char, MobTemplate> MobDict,
			string TileString,
			string MobString,
			Point[] Dwarves
		)
		{
			this.Name = Name;
			
			this.TileDict = TileDict;
			this.MobDict = MobDict;
			
			this.TileString = TileString;
			this.MobString = MobString;
			
			this.Dwarves = Dwarves;
		}
	}
}
