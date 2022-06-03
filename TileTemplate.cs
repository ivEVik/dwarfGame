using System;
using System.Collections.Generic;

namespace dwarfGame
{
	public class TileTemplate
	{
		public readonly string SpriteID;
		
		public readonly int Flags;
		
		public TileTemplate(
			string SpriteID,
			int Flags
		)
		{
			this.SpriteID = SpriteID;
			this.Flags = Flags;
		}
	}
}
