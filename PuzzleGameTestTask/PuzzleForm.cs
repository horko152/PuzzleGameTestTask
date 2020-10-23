﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PuzzleGameTestTask
{
	public partial class PuzzleForm : Form
	{
		public PuzzleForm()
		{
			InitializeComponent();
		}

		#region Global variables
		Image image;
		PictureBox pictureboxPuzzle = null;
		PictureBox[] picBoxes = null;
		Image[] images = null;
		int countOfFragments;
		MysteryBox firstBox = null;
		MysteryBox secondBox = null;
		#endregion

		#region Events

		private void ButtonImageBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				countOfFragments = GetCountOfFragments();
				textBoxImagePath.Text = openFileDialog.FileName;
				image = CreateBitmapImage(Image.FromFile(openFileDialog.FileName));

				if(pictureboxPuzzle == null)
				{
					pictureboxPuzzle = new PictureBox
					{
						Height = groupBoxPuzzle.Height,
						Width = groupBoxPuzzle.Width
					};
					groupBoxPuzzle.Controls.Add(pictureboxPuzzle);
				}

				if (picBoxes != null)
				{

					for (int i = 0; i < countOfFragments; i++)
					{
						groupBoxPuzzle.Controls.Remove(picBoxes[i]);
					}

				}

				pictureboxPuzzle.Image = image;
				buttonViewImage.Enabled = true;
				numericUpDownRows.Enabled = true;
				numericUpDownColumns.Enabled = true;
				buttonControl.Enabled = true;
			}
		}
		private void ButtonControl_Click(object sender, EventArgs e)
		{
			int numRow = Convert.ToInt32(numericUpDownRows.Value);
			int numCol = Convert.ToInt32(numericUpDownColumns.Value);
			countOfFragments = numRow * numCol;

			if (pictureboxPuzzle != null)
			{
				groupBoxPuzzle.Controls.Remove(pictureboxPuzzle);
				pictureboxPuzzle.Dispose();
				pictureboxPuzzle = null;
			}

			if (picBoxes == null)
			{
				images = new Image[countOfFragments];
				picBoxes = new PictureBox[countOfFragments];
			}

			int unitX = groupBoxPuzzle.Width / numCol;
			int unitY = groupBoxPuzzle.Height / numRow;
			int[] indice = new int[countOfFragments];

			for(int i =0; i< countOfFragments; i++)
			{
				indice[i] = i;

				if(picBoxes[i] != null)
				{
					picBoxes[i].BorderStyle = BorderStyle.Fixed3D;
					groupBoxPuzzle.Controls.Remove(picBoxes[i]);
				}
				if (picBoxes[i] == null)
				{
					picBoxes[i] = new MysteryBox();
					picBoxes[i].Click += new EventHandler(OnPuzzleClick);
					picBoxes[i].BorderStyle = BorderStyle.Fixed3D;
					picBoxes[i].SizeMode = PictureBoxSizeMode.CenterImage;
					picBoxes[i].Width = unitX;
					picBoxes[i].Height = unitY;
				}

				((MysteryBox)picBoxes[i]).Index = i;
				CreateBitmapImage(image, images, i, numRow, numCol, unitX, unitY);
				picBoxes[i].Location = new Point(unitX * (i % numCol), unitY * (i / numCol));

				if(!groupBoxPuzzle.Controls.Contains(picBoxes[i]))
				{
					groupBoxPuzzle.Controls.Add(picBoxes[i]);
				}
			}
			Shuffle(ref indice);

			for(int i=0; i < countOfFragments; i++)
			{
				((MysteryBox)picBoxes[i]).ImageIndex = indice[i];
				picBoxes[i].Image = images[indice[i]];
			}

			numericUpDownRows.Enabled = false;
			numericUpDownColumns.Enabled = false;
			buttonCheck.Enabled = true;
			buttonPuzzleAutomatic.Enabled = true;
		}
		public void OnPuzzleClick(object sender, EventArgs e)
		{
			if(firstBox == null)
			{
				firstBox = (MysteryBox)sender;
				firstBox.BorderStyle = BorderStyle.FixedSingle;
			} else if(secondBox == null)
			{
				secondBox = (MysteryBox)sender;
				firstBox.BorderStyle = BorderStyle.Fixed3D;
				secondBox.BorderStyle = BorderStyle.FixedSingle;
				SwitchImage(firstBox, secondBox);
				firstBox = null;
				secondBox = null;
			}
		}

		private void ButtonViewImage_Click(object sender, EventArgs e)
		{
			if (image != null)
			{
				Form currentImageForm = new Form();
				PictureBox picture = new PictureBox
				{
					Image = CreateBitmapImage(image),
					SizeMode = PictureBoxSizeMode.AutoSize
				};
				currentImageForm.Controls.Add(picture);
				currentImageForm.Size = new Size(groupBoxPuzzle.Width, groupBoxPuzzle.Height + 30);
				currentImageForm.StartPosition = FormStartPosition.CenterScreen;
				currentImageForm.MinimizeBox = false;
				currentImageForm.MaximizeBox = false;
				currentImageForm.ShowDialog();
			}
		}
		private void ButtonCheck_Click(object sender, EventArgs e)
		{
			countOfFragments = GetCountOfFragments();

			for (int i = 0; i < countOfFragments; i++)
			{

				if (((MysteryBox)picBoxes[i]).ImageIndex == ((MysteryBox)picBoxes[i]).Index)
				{
					picBoxes[i].BorderStyle = BorderStyle.None;
				}

			}
		}

		private void ButtonPuzzleAutomatic_Click(object sender, EventArgs e)
		{
			buttonCheck.PerformClick();
			countOfFragments = GetCountOfFragments();

			if (AutomaticMethod(countOfFragments))
			{
				MessageBox.Show("Work of method has ended");
			}
		}
		#endregion

		#region Bitmap methods
		private Bitmap CreateBitmapImage(Image image)
		{
			Bitmap bmpImage = new Bitmap(groupBoxPuzzle.Width, groupBoxPuzzle.Height);
			Graphics graphics = Graphics.FromImage(bmpImage);
			graphics.Clear(Color.White);
			graphics.DrawImage(image, new Rectangle(0, 0, groupBoxPuzzle.Width, groupBoxPuzzle.Height));
			graphics.Flush();
			return bmpImage;
		}

		private void CreateBitmapImage(Image image, Image[] images, int index, int numRow, int numCol, int unitX, int unitY)
		{
			images[index] = new Bitmap(unitX, unitY);
			Graphics graphics = Graphics.FromImage(images[index]);
			graphics.Clear(Color.White);
			graphics.DrawImage(image,
				new Rectangle(0, 0, unitX, unitY),
				new Rectangle(unitX * (index % numCol), unitY * (index / numCol), unitX, unitY),
				GraphicsUnit.Pixel);
			graphics.Flush();
		}
		#endregion

		#region Additional methods
		private int GetCountOfFragments()
		{
			return Convert.ToInt32(numericUpDownRows.Value) * Convert.ToInt32(numericUpDownColumns.Value);
		}

		private void Shuffle(ref int[] array)
		{
			int n = array.Length;

			while (n > 1)
			{
				int k = RandomNumber(n);
				n--;
				int temp = array[n];
				array[n] = array[k];
				array[k] = temp;
			}
		}

		private int RandomNumber(int maxValue)
		{
			Random rng = new Random();
			return rng.Next(maxValue);
		}

		private bool AutomaticMethod(int countOfFragments)
		{
			bool[] indexes = new bool[countOfFragments];
			int rightPuzzle = 0;
			int firstBox;
			int secondBox;

			for (int i = 0; i < countOfFragments; i++)
			{
				if (picBoxes[i].BorderStyle == BorderStyle.None)
				{
					indexes[i] = true;
				}
				else
				{
					indexes[i] = false;
				}
			}

			for (int i = 0; i < countOfFragments; i++)
			{
				firstBox = RandomNumber(countOfFragments);
				secondBox = RandomNumber(countOfFragments);

				if (firstBox != secondBox && indexes[firstBox] == false && indexes[secondBox] == false)
				{
					SwitchImage((MysteryBox)picBoxes[firstBox], (MysteryBox)picBoxes[secondBox]);
					buttonCheck.PerformClick();

					if (picBoxes[firstBox].BorderStyle == BorderStyle.None)
					{
						indexes[firstBox] = true;
					}

					if (picBoxes[secondBox].BorderStyle == BorderStyle.None)
					{
						indexes[secondBox] = true;
					}
				}
			}

			for (int i = 0; i < countOfFragments; i++)
			{

				if (indexes[i] == true)
				{
					rightPuzzle++;
				}
			}

			if(rightPuzzle == countOfFragments)
			{
				return true;
			} else
			{
				return AutomaticMethod(countOfFragments);
			}
		}

		private void SwitchImage(MysteryBox box1, MysteryBox box2)
		{
			int tmp = box2.ImageIndex;
			box2.Image = images[box1.ImageIndex];
			box2.ImageIndex = box1.ImageIndex;
			box1.Image = images[tmp];
			box1.ImageIndex = tmp;

			if(IsSuccessful())
			{
				buttonCheck.PerformClick();
				MessageBox.Show("Success");
			}
		}

		private bool IsSuccessful()
		{
			countOfFragments = GetCountOfFragments();

			for (int i=0; i< countOfFragments; i++)
			{

				if (((MysteryBox)picBoxes[i]).ImageIndex != ((MysteryBox)picBoxes[i]).Index)
				{
					return false;
				}

			}
			return true;
		}
		#endregion
	}
}