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
			this.BackColor = Color.Transparent;
			this.DoubleBuffered = true;
		}
	}
}
