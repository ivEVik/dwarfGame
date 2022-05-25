using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dwarfGame
{
	public class MainForm : Form
	{
		private static PrivateFontCollection fontCollection;
		private static FontFamily mainFontFamily;
		
		private int movInc;
		private Timer timer;
		private bool drag;
		//private bool dragStart;
		private Point camera;
		private Point cursorLoc;
		
		public MainForm()
		{
			fontCollection = new PrivateFontCollection();
			fontCollection.AddFontFile("res/DiaryOfAn8BitMage-lYDD.ttf");
			mainFontFamily = new FontFamily("Diary Of An 8-Bit Mage", fontCollection); //Temporary.
			
			movInc = Sprites.SpriteSize / 16;
			drag = false;
			//dragStart = false;
			cursorLoc = new Point(0, 0);
			camera = new Point(Game.DiagLength / 2 + Game.MapX * Game.ElementSize / 2, Game.DiagLength / 4);
			//camera = new Point(0, 0);
			
			ClientSize = new Size(Game.DiagLength, Game.DiagLength / 2);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			WindowState = FormWindowState.Maximized;
			
			timer = new Timer();
			timer.Tick += (sender, args) => Invalidate();
			timer.Tick += (sender, args) => Game.Process();
			timer.Interval = 15;
			timer.Start();
			
			MouseMove += (sender, e) => MouseMoved(sender, e);
			MouseDown += (sender, e) => MousePressed(sender, e);
			MouseClick += (sender, e) => MouseClicked(sender, e);
			MouseUp += (sender, e) => MouseReleased(sender, e);
		}
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
            Text = "Dwarf Game";
            DoubleBuffered = true;
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			List<Mob> mobs = new List<Mob>();
			
			DrawTiles(e, mobs);
			DrawCursor(e);
			DrawMobs(e, mobs);
		}
		
		private void DrawTiles(PaintEventArgs e, List<Mob> mobs)
		{
			for(int y = 0; y < Game.MapY; y++)
				for(int x = 0; x < Game.MapX; x++)
				{
					Tile tile = Game.Map[x, y];
					e.Graphics.DrawImage(Sprites.GetSprite(tile), MapToScreen(x, y));
					
					if(tile.Mobs.Count > 0)
						mobs.Add(tile.Mobs[0]);
					
					if(Game.CurrentMob != null && Game.CurrentMob.Ally && Game.CurrentMob.CanPath(tile))
						e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_PATH), MapToScreen(x, y));
				}
		}
		
		private void DrawCursor(PaintEventArgs e)
		{
			Point xy = ScreenToMap(cursorLoc.X, cursorLoc.Y);
			
			if(!ValidTile(xy))
				return;
			
			if(Game.Map[xy.X, xy.Y].CheckFlags(FLAG.TILE_SELECTABLE))
			{
				Tile tile = Game.Map[xy.X, xy.Y];
				
				if(tile.Mobs.Count > 0)
					if(tile.Mobs.FirstOrDefault().Ally)
						e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_HOVER_ALLY), MapToScreen(tile.X, tile.Y));
					else
						e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_HOVER_ENEMY), MapToScreen(tile.X, tile.Y));
				else
					e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_HOVER), MapToScreen(tile.X, tile.Y));
				
				if(Game.CurrentMob != null && Game.CurrentMob.Ally && Game.CurrentMob.CanPath(tile))
				{
					List<Tile> path = Game.CurrentMob.Path(tile);
					foreach(Tile t in path)
						e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_PATH_ACTIVE), MapToScreen(t.X, t.Y));
				}
			}
			
			if(Game.Map[xy.X, xy.Y].Mobs.Count > 0)
			{
				Mob mob = Game.Map[xy.X, xy.Y].Mobs.FirstOrDefault();
				e.Graphics.DrawString($"{mob.Health} / {mob.MaxHealth}", new Font(mainFontFamily, 20), Brushes.Black, new Point(0, 0));
			}
		}
		
		private void DrawMobs(PaintEventArgs e, List<Mob> mobs)
		{
			if(Game.CurrentMob != null)
				if(Game.CurrentMob.Ally)
					e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_HIGHLIGHT_BLUE), MapToScreen(Game.CurrentMob.X, Game.CurrentMob.Y));
				else
					e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_HIGHLIGHT_RED), MapToScreen(Game.CurrentMob.X, Game.CurrentMob.Y));
			
			foreach(Mob mob in mobs)
			{
				Point point = new Point(mob.Sheet.Coords.X + camera.X, mob.Sheet.Coords.Y + camera.Y);
				e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_SHADOW), point);
				e.Graphics.DrawImage(Sprites.GetSprite(mob), point);
			}
		}
		
		private Point MapToScreen(int x, int y)
		{
			return new Point((x - y) * Game.Horizontal + camera.X, (x + y) * Game.Vertical + camera.Y);
		}
		
		private Point ScreenToMap(int x, int y)
		{
			x -= camera.X + Game.Horizontal;
			y -= camera.Y + Game.Horizontal;
			if(y * 2 < Math.Abs(x))
				return new Point(-1, -1);
			
			Point result = new Point();
			result.X = (int)((double)x / Game.Horizontal + (double)y / Game.Vertical) / 2;
			result.Y = (int)((double)y / Game.Vertical - (double)x / Game.Horizontal) / 2;
			
			return result;
		}
		
		private Point ScreenToMap(Point point)
		{
			return ScreenToMap(point.X, point.Y);
		}
		
		private void MouseMoved(object sender, MouseEventArgs e)
		{
			if(drag)
			{
				camera.X += e.X - cursorLoc.X;
				camera.Y += e.Y - cursorLoc.Y;
				//dragStart = true;
			}
			cursorLoc.X = e.X;
			cursorLoc.Y = e.Y;
		}
		
		private void MousePressed(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
				drag = true;
		}
		
		private void MouseReleased(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				drag = false;
				//dragStart = false;
			}
		}
		
		private void MouseClicked(object sender, MouseEventArgs e)
		{
			//if(e.Button == MouseButtons.Right && Game.SelectedMob != null && !dragStart)
			//	Game.SelectedMob = null;
			
			Point xy = ScreenToMap(cursorLoc);
			
			if(!ValidTile(xy))
				return;
			
			Tile tile = Game.Map[xy.X, xy.Y];
			if(e.Button == MouseButtons.Left && Game.CurrentMob != null && Game.CurrentMob.CanPath(tile))
				Game.CurrentMob.Act(CONST.ACTION_MOVE, tile: tile);
				//Game.CurrentMob.Move(Game.CurrentMob.Path(tile));
			
			if(e.Button == MouseButtons.Left && Game.CurrentMob != null && Game.CurrentMob.Ally && Game.CurrentMob.GetDistance(tile) == 1 && tile.Mobs.Count > 0 && !tile.Mobs.FirstOrDefault().Ally)
				Game.CurrentMob.Act(CONST.ACTION_STRIKE, mob: tile.Mobs.FirstOrDefault());
		}
		
		private bool ValidTile(Point point)
		{
			return ValidTile(point.X, point.Y);
		}
		
		private bool ValidTile(int x, int y)
		{
			return x >= 0 && y >= 0 && x < Game.MapX && y < Game.MapY;
		}
	}
}
