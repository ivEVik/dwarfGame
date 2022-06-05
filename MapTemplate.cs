using System;
using System.Collections.Generic;
using System.Drawing;

namespace dwarfGame
{
	public class MapTemplate
	{
		public readonly string MapID;
		
		public readonly Dictionary<char, TileTemplate> TileDict;
		public readonly Dictionary<char, MobTemplate> MobDict;
		
		public readonly string TileString;
		public readonly string MobString;
		
		public readonly Point[] Dwarves;
		
		public MapTemplate(
			string MapID,
			Dictionary<char, TileTemplate> TileDict,
			Dictionary<char, MobTemplate> MobDict,
			string TileString,
			string MobString,
			Point[] Dwarves
		)
		{
			TEMPLATE.MAPS.Add(this);
			this.MapID = MapID;
			
			this.TileDict = TileDict;
			this.MobDict = MobDict;
			
			this.TileString = TileString;
			this.MobString = MobString;
			
			this.Dwarves = Dwarves;
		}
	}
}
