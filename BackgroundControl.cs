using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace dwarfGame
{
	public class BackgroundControl : Control
	{
		public BackgroundControl() : base()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.Selectable, false);
			BackColor = Color.Transparent;
			this.DoubleBuffered = true;
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			if(BackColor != Color.Transparent)
				e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Size.Width, Size.Height);
			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new RectangleF(new Point(3, 3), Size));//, stringFormat);
		}
	}
}
