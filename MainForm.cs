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
		
		private static SolidBrush bgBrush;
		private static Color textColour;
		
		private static int leftBorder;
		private static int rightBorder;
		private static int topBorder;
		private static int bottomBorder;
		
		private Timer timer;
		private bool drag;
		//private bool dragStart;
		private Point camera;
		private Point cursorLoc;
		
		public MainForm()
		{
			fontCollection = new PrivateFontCollection();
			fontCollection.AddFontFile("res/DiaryOfAn8BitMage-lYDD.ttf");
			mainFontFamily = new FontFamily("Diary Of An 8-Bit Mage", fontCollection);
			
			ClientSize = new Size(800, 600);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			WindowState = FormWindowState.Maximized;
			ResetView();
			
			//bgBrush = new SolidBrush(Color.FromArgb(82, 86, 117));
			bgBrush = (SolidBrush) Brushes.Black;
			textColour = Color.FromArgb(153, 110, 41);
			
			InitUI();
			
			timer = new Timer();
			timer.Tick += (sender, args) => Invalidate();
			timer.Tick += (sender, args) => {
				if(Game.InGame)
					Game.Process();
			};
			//timer.Interval = 15;
			timer.Interval = 30;
			timer.Start();
			
			MouseMove += (sender, e) => MouseMoved(sender, e);
			MouseDown += (sender, e) => MousePressed(sender, e);
			MouseClick += (sender, e) => MouseClicked(sender, e);
			MouseUp += (sender, e) => MouseReleased(sender, e);
		}
		
		public void ResetView()
		{
			drag = false;
			//dragStart = false;
			cursorLoc = new Point(0, 0);
			camera = new Point(0, 0);
			//camera = new Point(0, 0);
			
			leftBorder -= Game.MapY * Sprites.Horizontal + 600;
			rightBorder = Game.MapX * Sprites.Horizontal + 600;
			topBorder -= 300;
			bottomBorder = (Game.MapX + Game.MapY) * Sprites.Vertical + 300;
		}
		
		private void InitUI()
		{
			string buttonBase = CONST.UI_BUTTON_BIG;
			string buttonHover = CONST.UI_BUTTON_BIG_HOVER;
			string buttonPress = CONST.UI_BUTTON_BIG_PRESS;
			int step = Sprites.GetUI(buttonBase).Height * 3 / 2;
			Point point = new Point(Size.Width / 2 - Sprites.GetUI(buttonBase).Width / 2, Size.Height / 3);
			
			Font font = new Font(mainFontFamily, 10);
			
			Controls.Add(new GameButton(textColour, font, "Start game", point, buttonBase, buttonHover, buttonPress, (sender, e) => { Game.NewGame(TEMPLATE.MAP_TEST); Controls.Clear(); ResetView(); }));
			
			point = new Point(point.X, point.Y + step);
			Controls.Add(new GameButton(textColour, font, "Exit game", point, buttonBase, buttonHover, buttonPress, (sender, e) => { Program.Exit(); }));
		}
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
            Text = "Dungeon Dwarves";
            DoubleBuffered = true;
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(bgBrush, 0, 0, Size.Width, Size.Height);
			e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
			
			
			if(Game.InGame)
			{
				List<Mob> mobs = new List<Mob>();
			
				DrawTiles(e, mobs);
				DrawUnderMob(e);
				DrawMobs(e, mobs);
				DrawOverMob(e);
			}
			else
			{
				Bitmap sprite = Sprites.GetUI(CONST.UI_GAME_LOGO);
				e.Graphics.DrawImage(sprite, new Point(Size.Width / 2 - sprite.Width / 2, 32));
			}
		}
		
		private void DrawTiles(PaintEventArgs e, List<Mob> mobs)
		{
			for(int y = 0; y < Game.MapY; y++)
			{
				Point xy = MapToScreen(0, y);
				for(int x = 0; x < Game.MapX; x++)
				{
					Tile tile = Game.Map[x, y];
					e.Graphics.DrawImage(Sprites.GetSprite(tile), xy);
					
					if(tile.Mobs.Count > 0)
						mobs.Add(tile.Mobs[0]);
					
					if(Game.CurrentMob != null && Game.CurrentMob.Ally && Game.CurrentMob.CanPath(tile))
						e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_PATH), xy);
					
					xy = new Point(xy.X + Sprites.Horizontal, xy.Y + Sprites.Vertical);
				}
			}
		}
		
		private void DrawUnderMob(PaintEventArgs e)
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
		}
		
		private void DrawOverMob(PaintEventArgs e)
		{
			Point xy = ScreenToMap(cursorLoc.X, cursorLoc.Y);
			
			if(!ValidTile(xy) || Game.Map[xy.X, xy.Y].Mobs.Count == 0)
				return;
			
			DrawMobHealth(e, Game.Map[xy.X, xy.Y].Mobs.FirstOrDefault());
		}
		
		private void DrawMobs(PaintEventArgs e, List<Mob> mobs)
		{
			if(Game.CurrentMob != null)
				if(Game.CurrentMob.Ally)
				{
					e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_HIGHLIGHT_BLUE), MapToScreen(Game.CurrentMob.X, Game.CurrentMob.Y));
					DrawMobHealth(e, Game.CurrentMob);
				}
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
			return new Point((x - y) * Sprites.Horizontal + camera.X, (x + y) * Sprites.Vertical + camera.Y);
		}
		
		private Point ScreenToMap(int x, int y)
		{
			x -= camera.X + Sprites.Horizontal;
			y -= camera.Y + Sprites.Horizontal;
			if(y * 2 < Math.Abs(x))
				return new Point(-1, -1);
			
			Point result = new Point();
			result.X = (int)((double)x / Sprites.Horizontal + (double)y / Sprites.Vertical) / 2;
			result.Y = (int)((double)y / Sprites.Vertical - (double)x / Sprites.Horizontal) / 2;
			
			return result;
		}
		
		private Point ScreenToMap(Point point)
		{
			return ScreenToMap(point.X, point.Y);
		}
		
		private void MouseMoved(object sender, MouseEventArgs e)
		{
			if(!Game.InGame)
				return;
			
			if(drag)
			{
				int x = camera.X + e.X - cursorLoc.X;
				int y = camera.Y + e.Y - cursorLoc.Y;
				
				if(x > rightBorder)
					camera.X = rightBorder;
				else if(x < leftBorder)
					camera.X = leftBorder;
				else
					camera.X = x;
				
				if(y > bottomBorder)
					camera.Y = bottomBorder;
				else if(y < topBorder)
					camera.Y = topBorder;
				else
					camera.Y = y;
			}
			
			cursorLoc.X = e.X;
			cursorLoc.Y = e.Y;
		}
		
		private void MousePressed(object sender, MouseEventArgs e)
		{
			if(!Game.InGame)
				return;
			
			if(e.Button == MouseButtons.Right)
				drag = true;
		}
		
		private void MouseReleased(object sender, MouseEventArgs e)
		{
			if(!Game.InGame)
				return;
			
			if(e.Button == MouseButtons.Right)
			{
				drag = false;
				//dragStart = false;
			}
		}
		
		private void MouseClicked(object sender, MouseEventArgs e)
		{
			if(!Game.InGame)
				return;
			
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
			return Game.InGame && x >= 0 && y >= 0 && x < Game.MapX && y < Game.MapY;
		}
		
		private void DrawMobHealth(PaintEventArgs e, Mob mob)
		{
			Point point = new Point(mob.Sheet.Coords.X + camera.X, mob.Sheet.Coords.Y + camera.Y);
			int stepX = Sprites.GetOverlay(CONST.OVERLAY_MOB_HEALTH_BG).Width;
			int stepY = stepX * 2 / 3;
			stepX = stepX / (stepX / Sprites.Scale);
			point = new Point(point.X + Sprites.SpriteSize / 2 - (mob.MaxHealth / 2 + 2) * stepX, point.Y - Sprites.Scale * 6);
			
			for(int t = 0; t < mob.MaxHealth; t++)
			{
				e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_MOB_HEALTH_BG), point);
				
				if(t < mob.Health)
					e.Graphics.DrawImage(Sprites.GetOverlay(CONST.OVERLAY_MOB_HEALTH_FULL), point);
				
				if(t % 2 == 0)
					point = new Point(point.X + stepX, point.Y + stepY);
				else
					point = new Point(point.X + stepX, point.Y - stepY);
			}
		}
	}
}
