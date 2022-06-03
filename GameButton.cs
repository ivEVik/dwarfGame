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
		
		public GameButton(
			Color textColour,
			Font font,
			string text,
			Point coords,
			string spriteIDbase,
			string spriteIDhover,
			string spriteIDpress,
			Action<object, MouseEventArgs> onClick
		)// : base()
		{
			this.spriteIDbase = spriteIDbase;
			this.spriteIDhover = spriteIDhover;
			this.spriteIDpress = spriteIDpress;
			
			
			Font = font;
			ForeColor = textColour;
			Text = text;
			
			//ImageLayout = ImageLayout.None;
			Image = Sprites.GetUI(spriteIDbase);
			
			AutoEllipsis = false;
			
			Width = Image.Width;
			Height = Image.Height;
			Location = coords;
			
			Anchor = AnchorStyles.Top;
			
			BackColor = Color.FromArgb(20, 16, 11);
			
			MouseEnter += (sender, e) => { Image = Sprites.GetUI(spriteIDhover); };
			MouseLeave += (sender, e) => { Image = Sprites.GetUI(spriteIDbase); };
			MouseDown += (sender, e) => { Image = Sprites.GetUI(spriteIDpress); };
			MouseUp += (sender, e) => { Image = Sprites.GetUI(spriteIDhover); };
			MouseClick += (sender, e) => onClick(sender, e);
			
			GotFocus += (sender, e) => { Image = Sprites.GetUI(spriteIDhover); };
			LostFocus += (sender, e) => { Image = Sprites.GetUI(spriteIDbase); };
		}
	}
}
