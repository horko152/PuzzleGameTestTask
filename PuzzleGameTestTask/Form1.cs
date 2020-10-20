using System;
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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		#region Global variables
		Label startLabel = null;
		OpenFileDialog openFileDialog = null;
		Image image;
		PictureBox pictureboxPuzzle = null;
		PictureBox[] picBoxes = null;
		Image[] images = null;
		int countOfFragments;
		MysteryBox firstBox = null;
		MysteryBox secondBox = null;
		#endregion

		#region Events
		private void Form1_Load(object sender, EventArgs e)
		{
			startLabel = new Label();
			startLabel.Text = "Choose your image from computer";
			startLabel.AutoSize = true;
			startLabel.Location = new Point(418, 274);
			groupBoxPuzzle.Controls.Add(startLabel);
		}

		private void buttonImageBrowse_Click(object sender, EventArgs e)
		{
			if(openFileDialog == null)
			{
				openFileDialog = new OpenFileDialog();  
			}
			if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				int numRow = Convert.ToInt32(textBoxRows.Text);
				int numCol = Convert.ToInt32(textBoxColumns.Text);
				countOfFragments = numRow * numCol;
				textBoxImagePath.Text = openFileDialog.FileName;
				image = CreateBitmapImage(Image.FromFile(openFileDialog.FileName));
				if(pictureboxPuzzle == null)
				{
					pictureboxPuzzle = new PictureBox();
					pictureboxPuzzle.Height = groupBoxPuzzle.Height;
					pictureboxPuzzle.Width = groupBoxPuzzle.Width;
					groupBoxPuzzle.Controls.Add(pictureboxPuzzle);
				}
				if (picBoxes != null)
				{
					for (int i = 0; i < countOfFragments; i++)
					{
						groupBoxPuzzle.Controls.Remove(picBoxes[i]);
					}
				}
				groupBoxPuzzle.Controls.Remove(startLabel);
				pictureboxPuzzle.Image = image;
				textBoxRows.Enabled = true;
				textBoxColumns.Enabled = true;
				buttonControl.Enabled = true;
			}
		}
		private void buttonControl_Click(object sender, EventArgs e)
		{
			int numRow = Convert.ToInt32(textBoxRows.Text);
			int numCol = Convert.ToInt32(textBoxColumns.Text);
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
					groupBoxPuzzle.Controls.Remove(picBoxes[i]);
				}
				if (picBoxes[i] == null)
				{
					picBoxes[i] = new MysteryBox();
					picBoxes[i].Click += new EventHandler(OnPuzzleClick);
					picBoxes[i].BorderStyle = BorderStyle.Fixed3D;
				}
				picBoxes[i].SizeMode = PictureBoxSizeMode.CenterImage;
				picBoxes[i].Width = unitX;
				picBoxes[i].Height = unitY;
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
				picBoxes[i].Image = images[indice[i]];
				((MysteryBox)picBoxes[i]).ImageIndex = indice[i];
			}
			textBoxRows.Enabled = false;
			textBoxColumns.Enabled = false;
			buttonCheck.Enabled = true;
			buttonPuzzleAutomatic.Enabled = true;
		}
		public void OnPuzzleClick(object sender, EventArgs e)
		{
			if(firstBox == null)
			{
				firstBox = (MysteryBox)sender;
				firstBox.BorderStyle = BorderStyle.FixedSingle;
			}
			else if(secondBox == null)
			{
				secondBox = (MysteryBox)sender;
				firstBox.BorderStyle = BorderStyle.Fixed3D;
				secondBox.BorderStyle = BorderStyle.FixedSingle;
				SwitchImage(firstBox, secondBox);
				firstBox = null;
				secondBox = null;
			}
		}
		private void buttonViewImage_Click(object sender, EventArgs e)
		{
			if (image != null)
			{
				Form currentImageForm = new Form();
				PictureBox picture = new PictureBox();
				picture.Image = CreateBitmapImage(image);
				picture.SizeMode = PictureBoxSizeMode.AutoSize;
				currentImageForm.Controls.Add(picture);
				currentImageForm.Size = new Size(groupBoxPuzzle.Width, groupBoxPuzzle.Height + 30);
				currentImageForm.StartPosition = FormStartPosition.CenterScreen;
				currentImageForm.ShowDialog();
			}
			else
			{
				MessageBox.Show("Please pick an image for puzzle");
			}
		}
		private void buttonCheck_Click(object sender, EventArgs e)
		{
			int numRow = Convert.ToInt32(textBoxRows.Text);
			int numCol = Convert.ToInt32(textBoxColumns.Text);
			countOfFragments = numRow * numCol;
			for (int i = 0; i < countOfFragments; i++)
			{
				if (((MysteryBox)picBoxes[i]).ImageIndex == ((MysteryBox)picBoxes[i]).Index)
				{
					picBoxes[i].BorderStyle = BorderStyle.None;
				}
			}
		}

		private void buttonPuzzleAutomatic_Click(object sender, EventArgs e)
		{

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
		private void Shuffle(ref int[] array)
		{
			Random rng = new Random();
			int n = array.Length;
			while (n > 1)
			{
				int k = rng.Next(n);
				n--;
				int temp = array[n];
				array[n] = array[k];
				array[k] = temp;
			}
		}

		private void SwitchImage(MysteryBox box1, MysteryBox box2)
		{
			int tmp = box2.ImageIndex;
			box2.Image = images[box1.ImageIndex];
			box2.ImageIndex = box1.ImageIndex;
			box1.Image = images[tmp];
			box1.ImageIndex = tmp;
			if(isSuccessful())
			{
				buttonCheck.PerformClick();
				MessageBox.Show("Success");
			}
		}

		private bool isSuccessful()
		{
			int numRow = Convert.ToInt32(textBoxRows.Text);
			int numCol = Convert.ToInt32(textBoxColumns.Text);
			countOfFragments = numRow * numCol;
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
