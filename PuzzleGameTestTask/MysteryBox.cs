using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PuzzleGameTestTask
{
	class MysteryBox : PictureBox
	{
		public int Index { get; set; }
		public int ImageIndex { get; set; }
		public bool isMath()
		{
			return (Index == ImageIndex);
		}
	}
}
