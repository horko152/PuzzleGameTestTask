namespace PuzzleGameTestTask
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class PuzzleForm : Form
	{
		#region Global variables

		/// <summary>
		///	Variable for using PuzzleSolver methods
		/// </summary>
		private PuzzleSolver PuzzleSolver { get; }

		/// <summary>
		/// PictureBox for main image
		/// </summary>
		PictureBox pictureboxPuzzle = null;

		/// <summary>
		/// Image for making a puzzle
		/// </summary>
		Image image;

		/// <summary>
		/// Array of PictureBoxes for a crushed puzzle
		/// </summary>
		MysteryBox[] mysteryBoxes = null;

		/// <summary>
		/// Array of images inside PictureBoxes
		/// </summary>
		Image[] images = null;

		/// <summary>
		/// Count of puzzle fragments
		/// </summary>
		int countOfFragments = 0;

		/// <summary>
		/// Number of puzzle rows
		/// </summary>
		int numberOfRows = 0;

		/// <summary>
		/// Number of puzzle columns
		/// </summary>
		int numberOfColumns = 0;

		/// <summary>
		/// List of Shuffled Images
		/// </summary>
		List<Bitmap> listOfShuffledImages = null;

		/// <summary>
		/// Variables for swapping puzzles
		/// </summary>        
		MysteryBox firstBox = null;
		MysteryBox secondBox = null;
		#endregion

		public PuzzleForm()
		{
			InitializeComponent();
			PuzzleSolver = new PuzzleSolver();
		}

		#region Events

		private void ButtonImageBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				textBoxImagePath.Text = openFileDialog.FileName;
				image = BitmapHelper.CreateBitmapImage(Image.FromFile(openFileDialog.FileName), groupBoxPuzzle.Width, groupBoxPuzzle.Height);

				if (pictureboxPuzzle == null)
				{
					pictureboxPuzzle = new PictureBox
					{
						Height = groupBoxPuzzle.Height,
						Width = groupBoxPuzzle.Width
					};
					groupBoxPuzzle.Controls.Add(pictureboxPuzzle);
				}

				if (mysteryBoxes != null)
				{
					for (int i = 0; i < countOfFragments; i++)
					{
						groupBoxPuzzle.Controls.Remove(mysteryBoxes[i]);
					}
				}

				pictureboxPuzzle.Image = image;
				buttonViewImage.Enabled = true;
				numericUpDownRows.Enabled = true;
				numericUpDownColumns.Enabled = true;
				buttonShuffle.Enabled = true;
			}
		}

		private void ButtonShuffle_Click(object sender, EventArgs e)
		{
			numberOfRows = Convert.ToInt32(numericUpDownRows.Value);
			numberOfColumns = Convert.ToInt32(numericUpDownColumns.Value);
			countOfFragments = numberOfRows * numberOfColumns;

			if (pictureboxPuzzle != null)
			{
				groupBoxPuzzle.Controls.Remove(pictureboxPuzzle);
				pictureboxPuzzle.Dispose();
				pictureboxPuzzle = null;
			}

			if (mysteryBoxes == null)
			{
				images = new Image[countOfFragments];
				mysteryBoxes = new MysteryBox[countOfFragments];
			}
			else
			{
				for (int j = 0; j < mysteryBoxes.Length; j++)
				{
					//images[j].Dispose();
					images[j] = null;

					//mysteryBoxes[j].Dispose();
					mysteryBoxes[j] = null;
				}

				images = new Image[countOfFragments];
				mysteryBoxes = new MysteryBox[countOfFragments];
			}

			int unitX = groupBoxPuzzle.Width / numberOfColumns;
			int unitY = groupBoxPuzzle.Height / numberOfRows;
			int[] indice = new int[countOfFragments];

			for (int i = 0; i < countOfFragments; i++)
			{
				indice[i] = i;

				if (mysteryBoxes[i] != null)
				{
					mysteryBoxes[i].BorderStyle = BorderStyle.Fixed3D;
					groupBoxPuzzle.Controls.Remove(mysteryBoxes[i]);
					mysteryBoxes[i] = null;
				}

				if (mysteryBoxes[i] == null)
				{
					mysteryBoxes[i] = new MysteryBox();
					mysteryBoxes[i].Click += new EventHandler(OnPuzzleClick);
					mysteryBoxes[i].BorderStyle = BorderStyle.Fixed3D;
					mysteryBoxes[i].SizeMode = PictureBoxSizeMode.CenterImage;
					mysteryBoxes[i].Width = unitX;
					mysteryBoxes[i].Height = unitY;
				}

				mysteryBoxes[i].Index = i;
				BitmapHelper.CreateBitmapImage(image, images, i, numberOfColumns, unitX, unitY);
				mysteryBoxes[i].Location = new Point(unitX * (i % numberOfColumns), unitY * (i / numberOfColumns));

				if (!groupBoxPuzzle.Controls.Contains(mysteryBoxes[i]))
				{
					groupBoxPuzzle.Controls.Add(mysteryBoxes[i]);
				}
			}

			Shuffle(ref indice);

			listOfShuffledImages = new List<Bitmap>(countOfFragments);

			for (int i = 0; i < countOfFragments; i++)
			{
				listOfShuffledImages.Add((Bitmap)images[indice[i]]);
				mysteryBoxes[i].ImageIndex = indice[i];
				mysteryBoxes[i].Image = images[indice[i]];
			}

			numericUpDownRows.Enabled = false;
			numericUpDownColumns.Enabled = false;
			buttonCheck.Enabled = true;
			buttonAutomaticAssemblyPuzzle.Enabled = true;
		}

		public void OnPuzzleClick(object sender, EventArgs e)
		{
			if (firstBox == null)
			{
				firstBox = (MysteryBox)sender;
				firstBox.BorderStyle = BorderStyle.FixedSingle;
			}
			else if (secondBox == null)
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
					Image = BitmapHelper.CreateBitmapImage(image, groupBoxPuzzle.Width, groupBoxPuzzle.Height),
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
			for (int i = 0; i < countOfFragments; i++)
			{
				if (mysteryBoxes[i].IsMath())
				{
					mysteryBoxes[i].IsOnRightPlace = true;
				}
			}
		}

		private void ButtonAutomaticAssemblyPuzzle_Click(object sender, EventArgs e)
		{
			// Array for puzzles with the lowest difference for each position
			Bitmap[,] bestChoisesForPuzzle = PuzzleSolver.GetBestPuzzleImage(listOfShuffledImages, numberOfRows, numberOfColumns);

			// List for swapping puzlles above and current in a game
			List<Bitmap> mysteryBoxesBitmaps = new List<Bitmap>(countOfFragments);

			for (int i = 0; i < bestChoisesForPuzzle.GetLength(0); i++)
			{
				for (int j = 0; j < bestChoisesForPuzzle.GetLength(1); j++)
				{
					mysteryBoxesBitmaps.Add(bestChoisesForPuzzle[i, j]);
				}
			}

			for (int i = 0; i < countOfFragments; i++)
			{
				mysteryBoxes[i].Image = mysteryBoxesBitmaps[i];
				mysteryBoxes[i].ImageIndex = i;
			}

			// Event for removing borders
			buttonCheck.PerformClick();

			buttonCheck.Enabled = false;
		}
		#endregion

		#region Additional methods

		// Method for shuffling puzzles
		private void Shuffle(ref int[] array)
		{
			int n = array.Length;

			while (n > 1)
			{
				int k = GetRandomNumber(n);
				n--;
				int temp = array[n];
				array[n] = array[k];
				array[k] = temp;
			}
		}

		// Method for getting a random number
		private int GetRandomNumber(int maxValue)
		{
			Random rng = new Random();
			return rng.Next(maxValue);
		}

		// Method for switching two puzzles
		private void SwitchImage(MysteryBox box1, MysteryBox box2)
		{
			int tmp = box2.ImageIndex;
			box2.Image = images[box1.ImageIndex];
			box2.ImageIndex = box1.ImageIndex;
			box1.Image = images[tmp];
			box1.ImageIndex = tmp;

			if (IsSuccessful())
			{
				buttonCheck.PerformClick();
				MessageBox.Show("Success");
				numericUpDownRows.Enabled = true;
				numericUpDownColumns.Enabled = true;
			}
		}

		// Method that checked if a piece of puzzle is on right place
		private bool IsSuccessful()
		{
			for (int i = 0; i < countOfFragments; i++)
			{
				if (!mysteryBoxes[i].IsMath())
				{
					return false;
				}
			}
			return true;
		}

		#endregion
	}
}