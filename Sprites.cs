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
		private static Dictionary<string, Bitmap> uiBitmaps;
		
		public static int IncX;
		public static int IncY;
		
		public static int UIScale;
		
		public static int Scale;
		public static int SpriteSize;
		public static int Horizontal;
		public static int Vertical;
		public static int DiagLength;
		
		public static void Initialise(int scale, int uiScale)
		{
			UIScale = uiScale;
			Scale = scale;
			SpriteSize = 32 * Scale;
			Horizontal = SpriteSize / 2;
			Vertical = Horizontal / 2;
			
			var div = 16;
			if(SpriteSize > 32 && Scale % 2 == 0)
				//div = 32;
				div = 16;
			else if(SpriteSize > 32)
				//div = 48;
				div = 24;
			
			IncX = SpriteSize / div;
			IncY = IncX / 2;
			tileBitmaps = new Dictionary<string, Bitmap>();
			mobBitmaps = new Dictionary<string, Bitmap>();
			overlayBitmaps = new Dictionary<string, Bitmap>();
			uiBitmaps = new Dictionary<string, Bitmap>();
			
			DirectoryInfo[] directories = new DirectoryInfo("sprites").GetDirectories();
			
			tileBitmaps = SpriteInit(Array.Find(directories, dir => dir.Name == "tiles"), Scale);
			mobBitmaps = SpriteInit(Array.Find(directories, dir => dir.Name == "mobs").GetDirectories(), Scale);
			overlayBitmaps = SpriteInit(Array.Find(directories, dir => dir.Name == "overlays"), Scale);
			uiBitmaps = SpriteInit(Array.Find(directories, dir => dir.Name == "ui"), UIScale);
		}
		
		public static Bitmap GetOverlay(string id)
		{
			if(overlayBitmaps.ContainsKey(id))
				return overlayBitmaps[id];
			return overlayBitmaps[CONST.OVERLAY_NULL];
		}
		
		public static Bitmap GetUI(string id)
		{
			if(uiBitmaps.ContainsKey(id))
				return uiBitmaps[id];
			return uiBitmaps[CONST.UI_NULL];
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
		
		public static Dictionary<string, Dictionary<int, Tuple<string, int>[]>> GetSprites(string id, out string portraitID)
		{
			XmlDocument xmlFile = new XmlDocument();
			if(!File.Exists($"sprites/{id}.xml"))
				id = CONST.MOB_DUMMY;
			xmlFile.Load($"sprites/{id}.xml");
			
			Dictionary<string, Dictionary<int, Tuple<string, int>[]>> sprites = new Dictionary<string, Dictionary<int, Tuple<string, int>[]>>();
			
			portraitID = xmlFile.FirstChild.ChildNodes[0].InnerText;
			
			XmlNodeList anims = xmlFile.FirstChild.ChildNodes[1].ChildNodes;
			foreach(XmlNode anim in anims)
			{
				string action = anim["action"].InnerText;
				int dir = int.Parse(anim["dir"].InnerText);
				XmlNodeList frames = anim["frames"].ChildNodes;
				
				if(!sprites.ContainsKey(action))
					sprites[action] = new Dictionary<int, Tuple<string, int>[]>();
				
				sprites[action][dir] = new Tuple<string, int>[frames.Count];
				
				for(int t = 0; t < frames.Count; t++)
					sprites[action][dir][t] = Tuple.Create(frames[t]["image"].InnerText, int.Parse(frames[t]["delay"].InnerText) * 3);
			}
			
			return sprites;
		}
		
		private static Dictionary<string, Bitmap> SpriteInit(DirectoryInfo dir, int scale)
		{
			return dir
				.GetFiles("*.png")
				.ToDictionary(
					file => file.Name.Substring(0, file.Name.Length - 4),
					file => GetAndScaleImage(file.FullName, scale));
		}
		
		private static Dictionary<string, Bitmap> SpriteInit(DirectoryInfo[] dir, int scale)
		{
			return dir
				.SelectMany(dir => dir.GetFiles("*.png"))
				.ToDictionary(
					file => file.Name.Substring(0, file.Name.Length - 4),
					file => GetAndScaleImage(file.FullName, scale));
		}
		
		private static Bitmap GetAndScaleImage(string fileName, int scale)
		{
			Image image = Image.FromFile(fileName);
			int width = image.Width * scale;
			int height = image.Height * scale;
			Bitmap bitmap = new Bitmap(width, height);
			
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			graphics.DrawImage(image, 0, 0, width, height);
			
			return bitmap;
		}
	}
}
