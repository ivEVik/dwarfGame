using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace dwarfGame
{
	public class GameButton : Button
	{
		private string spriteIDbase;
		private string spriteIDhover;
		private string spriteIDpress;
		private string overlayID;
		private StringFormat stringFormat;
		private Keys hotkey;
		
		public GameButton() {}
		
		public GameButton(
			Keys hotkey,
			Color textColour,
			Font font,
			string text,
			Point coords,
			string spriteIDbase,
			string spriteIDhover,
			string spriteIDpress,
			Action<object, EventArgs> onClick
		)
		{
			this.spriteIDbase = spriteIDbase;
			this.spriteIDhover = spriteIDhover;
			this.spriteIDpress = spriteIDpress;
			overlayID = CONST.UI_NULL;
			
			Font = font;
			ForeColor = textColour;
			Text = text;
			stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			
			//ImageLayout = ImageLayout.None;
			Image = Sprites.GetUI(spriteIDbase);
			
			AutoEllipsis = false;
			
			Width = Image.Width;
			Height = Image.Height;
			Location = coords;
			
			BackColor = Color.FromArgb(20, 16, 11);
			
			SetStyle(ControlStyles.Selectable, false);
			this.hotkey = hotkey;
			
			MouseEnter += (sender, e) => { Image = Sprites.GetUI(spriteIDhover); };
			MouseLeave += (sender, e) => { Image = Sprites.GetUI(spriteIDbase); };
			MouseDown += (sender, e) => { Image = Sprites.GetUI(spriteIDpress); };
			MouseUp += (sender, e) => { Image = Sprites.GetUI(spriteIDhover); };
			Click += (sender, e) => onClick(sender, e);
			
			GotFocus += (sender, e) => { Image = Sprites.GetUI(spriteIDhover); };
			LostFocus += (sender, e) => { Image = Sprites.GetUI(spriteIDbase); };
		}
		
		public GameButton(
			Keys hotkey,
			Color textColour,
			Font font,
			string text,
			Point coords,
			string spriteIDbase,
			string spriteIDhover,
			string spriteIDpress,
			Action<object, EventArgs> onClick,
			string overlayID
		) : this(hotkey, textColour, font, text, coords, spriteIDbase, spriteIDhover, spriteIDpress, onClick)
		{
			this.overlayID = overlayID;
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			e.Graphics.DrawImage(Image, new Point(0, 0));
			e.Graphics.DrawImage(Sprites.GetUI(overlayID), new Point(0, 0));
			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new RectangleF(new Point(0, 0), Size), stringFormat);
		}
		
		public bool CheckHotkey(KeyEventArgs e)
		{
			if(e.KeyCode == hotkey)
				OnClick(e);
			return e.KeyCode == hotkey;
		}
	}
}
