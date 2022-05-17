using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace dwarfGame
{
	public static class Sprites
	{
		private static Dictionary<string, Bitmap> tileBitmaps;
		private static Dictionary<string, Bitmap> mobBitmaps;
		private static Dictionary<string, Bitmap> overlayBitmaps;
		
		public static int SpriteSize;
		public static int IncX;
		public static int IncY;
		
		public static void Initialise()
		{
			SpriteSize = 32 * Game.Scale;
			
			var div = 16;
			if(SpriteSize > 32 && Game.Scale % 2 == 0)
				div = 32;
			else if(SpriteSize > 32)
				div = 48;
			
			IncX = SpriteSize / div;
			IncY = IncX / 2;
			tileBitmaps = new Dictionary<string, Bitmap>();
			mobBitmaps = new Dictionary<string, Bitmap>();
			overlayBitmaps = new Dictionary<string, Bitmap>();
			
			DirectoryInfo[] directories = new DirectoryInfo("sprites").GetDirectories();
			
			TileInit(Array.Find(directories, dir => dir.Name == "tiles"));
			MobInit(Array.Find(directories, dir => dir.Name == "mobs"));
			OverlayInit(Array.Find(directories, dir => dir.Name == "overlays"));
		}
		
		public static Bitmap GetOverlay(string id)
		{
			if(overlayBitmaps.ContainsKey(id))
				return overlayBitmaps[id];
			return overlayBitmaps[CONST.OVERLAY_NULL];
		}
		
		public static Bitmap GetSprite(Mob mob)
		{
			string id = mob.Sheet.GetFrameID();
			
			if(mobBitmaps.ContainsKey(id))
				return mobBitmaps[id];
			return mobBitmaps[CONST.MOB_DUMMY];
		}
		
		public static Bitmap GetSprite(Tile tile)
		{
			string id = tile.SpriteID;
			
			if(tileBitmaps.ContainsKey(id))
				return tileBitmaps[id];
			return tileBitmaps[CONST.TILE_NULL];
		}
		
		public static Dictionary<string, Dictionary<int, Tuple<string, int>[]>> GetSprites(string id)
		{
			XmlDocument xmlFile = new XmlDocument();
			if(!File.Exists($"sprites/{id}.xml"))
				id = CONST.MOB_DUMMY;
			xmlFile.Load($"sprites/{id}.xml");
			
			Dictionary<string, Dictionary<int, Tuple<string, int>[]>> sprites = new Dictionary<string, Dictionary<int, Tuple<string, int>[]>>();
			
			XmlNodeList anims = xmlFile.FirstChild.ChildNodes;
			foreach(XmlNode anim in anims)
			{
				string action = anim["action"].InnerText;
				int dir = int.Parse(anim["dir"].InnerText);
				XmlNodeList frames = anim["frames"].ChildNodes;
				
				if(!sprites.ContainsKey(action))
					sprites[action] = new Dictionary<int, Tuple<string, int>[]>();
				
				sprites[action][dir] = new Tuple<string, int>[frames.Count];
				
				for(int t = 0; t < frames.Count; t++)
					sprites[action][dir][t] = Tuple.Create(frames[t]["image"].InnerText, int.Parse(frames[t]["delay"].InnerText) * 6);
			}
			
			return sprites;
		}
		
		private static void TileInit(DirectoryInfo dir)
		{
			tileBitmaps = dir
				.GetFiles("*.png")
				.ToDictionary(
					file => file.Name.Substring(0, file.Name.Length - 4),
					file => GetAndScaleImage(file.FullName, SpriteSize));
		}
		
		private static void MobInit(DirectoryInfo dir)
		{
			mobBitmaps = dir
				.GetDirectories()
				.SelectMany(dir => dir.GetFiles("*.png"))
				.ToDictionary(
					file => file.Name.Substring(0, file.Name.Length - 4),
					file => GetAndScaleImage(file.FullName, SpriteSize));
		}
		
		private static void OverlayInit(DirectoryInfo dir)
		{
			overlayBitmaps = dir
				.GetFiles("*.png")
				.ToDictionary(
					file => file.Name.Substring(0, file.Name.Length - 4),
					file => GetAndScaleImage(file.FullName, SpriteSize));
		}
		
		private static Bitmap GetAndScaleImage(string fileName, int size)
		{
			Image image = Image.FromFile(fileName);
			Bitmap bitmap = new Bitmap(size, size);
			
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			graphics.DrawImage(image, 0, 0, size, size);
			
			return bitmap;
		}
	}
}
