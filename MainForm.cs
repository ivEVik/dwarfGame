using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dwarfGame
{
	public class MainForm : Form
	{
		private Timer timer;
		private bool drag;
		private Point camera;
		private Point cursorLoc;
		
		public MainForm()
		{
			drag = false;
			cursorLoc = new Point(0, 0);
			camera = new Point(Game.DiagLength / 2, Game.DiagLength / 4);
			
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
			foreach(Tile tile in Game.MapOrdered)
			{
				Point point = new Point((tile.X - tile.Y) * Game.Horizontal + Game.SideShift + camera.X, (tile.X + tile.Y) * Game.Vertical + camera.Y);
				e.Graphics.DrawImage(Sprites.GetTile(tile.SpriteID), point);
				
				if(tile.Mobs.Count > 0)
					e.Graphics.DrawImage(tile.Mobs[0].GetSprite(), point);
			}
		}
		
		private void MouseMoved(object sender, MouseEventArgs e)
		{
			if(drag)
			{
				camera.X += e.X - cursorLoc.X;
				camera.Y += e.Y - cursorLoc.Y;
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
				drag = false;
		}
	}
}
