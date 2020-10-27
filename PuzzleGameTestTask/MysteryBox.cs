using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PuzzleGameTestTask
{
	class MysteryBox : PictureBox
	{
		/// <summary>
		/// Nedded position
		/// </summary>
		public int Index { get; set; }
		/// <summary>
		/// Current position
		/// </summary>
		public int ImageIndex { get; set; }
		/// <summary>
		/// Is puzzle on on nedded position
		/// </summary>
		public bool IsMath()
		{
			return (Index == ImageIndex);
		}
		/// <summary>
		/// Change border style if a puzzle in right place
		/// </summary>
		public bool IsOnRightPlace
		{
			get
			{
				return this.BorderStyle == BorderStyle.None;
			}
			set
			{
				if (value)
				{
					this.BorderStyle = BorderStyle.None;
				}
				else
				{
					this.BorderStyle = BorderStyle.Fixed3D;
				}
			}
		}
	}
}
