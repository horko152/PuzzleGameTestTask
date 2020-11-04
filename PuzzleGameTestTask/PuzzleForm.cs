namespace PuzzleGameTestTask
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Windows.Forms.VisualStyles;
	public partial class PuzzleForm : Form
	{
		public PuzzleForm()
		{
			InitializeComponent();
		}

		#region Global variables
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
		int countOfFragments;
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

		#region Events

		private void ButtonImageBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				countOfFragments = GetCountOfFragments();
				textBoxImagePath.Text = openFileDialog.FileName;
				image = CreateBitmapImage(Image.FromFile(openFileDialog.FileName));

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
			int numRow = Convert.ToInt32(numericUpDownRows.Value);
			int numCol = Convert.ToInt32(numericUpDownColumns.Value);
			countOfFragments = numRow * numCol;

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

			int unitX = groupBoxPuzzle.Width / numCol;
			int unitY = groupBoxPuzzle.Height / numRow;
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
				CreateBitmapImage(image, images, i, numRow, numCol, unitX, unitY);
				mysteryBoxes[i].Location = new Point(unitX * (i % numCol), unitY * (i / numCol));

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

				if (mysteryBoxes[i].IsMath())
				{
					mysteryBoxes[i].BorderStyle = BorderStyle.None;
				}

			}
		}

		private void ButtonAutomaticAssemblyPuzzle_Click(object sender, EventArgs e)
		{
			countOfFragments = GetCountOfFragments();

			// Array for puzzles with the lowest difference for each position
			Bitmap[,] bestChoisesForPuzzle = GetBestPuzzleImage(listOfShuffledImages);

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
		// Method for getting current count of puzzle fragments
		private int GetCountOfFragments()
		{
			return Convert.ToInt32(numericUpDownRows.Value) * Convert.ToInt32(numericUpDownColumns.Value);
		}

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
			countOfFragments = GetCountOfFragments();

			for (int i = 0; i < countOfFragments; i++)
			{

				if (!mysteryBoxes[i].IsMath())
				{
					return false;
				}

			}
			return true;
		}

		// Method that return array of puzzles with the lowest difference between them and mirrored positions
		public Bitmap[,] GetBestPuzzleImage(List<Bitmap> listOfImages)
		{
			int numRow = Convert.ToInt32(numericUpDownRows.Value);
			int numCol = Convert.ToInt32(numericUpDownColumns.Value);

			double minDifference = Int32.MaxValue;

			Bitmap[,] bestChoice = null;
			Bitmap[,] possibleChoice = new Bitmap[numRow, numCol];

			double value = GetBestCurrentVariant(listOfImages, numRow, numCol, ref possibleChoice);

			if (minDifference > value)
			{
				bestChoice = (Bitmap[,])possibleChoice.Clone();
				minDifference = value;
			}

			return bestChoice;
		}

		// Method that returns the lowest difference between 
		private double GetBestCurrentVariant(List<Bitmap> listOfImages, int numRow, int numCol, ref Bitmap[,] bestChoice)
		{
			double minDifference = Int32.MaxValue;

			Bitmap bestPuzzle;

			for (int j = 0; j < listOfImages.Count; j++)
			{
				// Difference of whole image
				double totalDifference = 0;
				// Result of puzzles
				Bitmap[,] puzzles = new Bitmap[numRow, numCol];
				// Puzzles available
				List<Bitmap> cash = new List<Bitmap>(listOfImages); 
				// Get first puzzle and do with him
				puzzles[0, 0] = cash[j];
				cash.RemoveAt(j);

				for (int row = 0; row < numRow; row++)
				{
					for (int col = 0; col < numCol - 1; col++)
					{
						// Getting the best right puzzle
						bestPuzzle = GetRightImage(puzzles[row, col], cash, ref totalDifference);
						// Add that puzzle to "Result of puzzles"
						puzzles[row, col + 1] = bestPuzzle ;
						// Delete this puzzle from cash
						cash.Remove(bestPuzzle);
					}

					if (row < numRow - 1)
					{
						// Getting the best bottom puzzle
						bestPuzzle = GetBottomImage(puzzles[row, 0], cash, ref totalDifference);
						// Add puzzle tp "Result of puzzles"
						puzzles[row + 1, 0] = bestPuzzle;
						// Delete this puzzle from cash
						cash.Remove(bestPuzzle);
					}
				}

				if (minDifference > totalDifference)
				{
					bestChoice = (Bitmap[,])puzzles.Clone();
					minDifference = totalDifference;
				}
			}

			return minDifference;
		}

		// Method for getting difference bettwen two colors
		private double GetDifference(Color firstColor, Color secondColor)
		{
			int differenceR = Math.Abs(firstColor.R - secondColor.R);
			int differenceG = Math.Abs(firstColor.G - secondColor.G);
			int differenceB = Math.Abs(firstColor.B - secondColor.B);

			return Math.Sqrt(differenceR * differenceR + differenceG * differenceG + differenceB * differenceB);
		}

		// Method for getting the best right puzzle
		private Bitmap GetRightImage(Bitmap firstPuzzle, List<Bitmap> listOfImages, ref double totalDifference)
		{
			double minDifference = Int32.MaxValue;

			// Array of colors right angle of left puzzle 
			Color[] leftPuzzle = new Color[firstPuzzle.Height];

			for (int i = 0; i < firstPuzzle.Height; i++)
			{
				leftPuzzle[i] = firstPuzzle.GetPixel(firstPuzzle.Width - 1, i);
			}

			Bitmap nextPuzzle = null;

			for (int i = 0; i < listOfImages.Count; i++)
			{
				Color[] rightPuzzle = new Color[firstPuzzle.Height];

				for (int j = 0; j < firstPuzzle.Height; j++)
				{
					rightPuzzle[j] = listOfImages[i].GetPixel(0, j);
				}
				
				double value = GetRightDifference(leftPuzzle, rightPuzzle);
				
				// Searching the lowest difference to find needed puzzle
				if (minDifference > value)
				{
					nextPuzzle = listOfImages[i];
					minDifference = value;
				}
			}
			totalDifference += minDifference;

			return nextPuzzle;
		}

		// Method for getting difference between two puzzles by right angle
		private double GetRightDifference(Color[] leftPuzzle, Color[] rightPuzzle)
		{
			double rightDifference = 0;

			try
			{
				if (leftPuzzle.Length != rightPuzzle.Length)
				{
					throw new Exception("Puzzles are different");
				}

				for (int i = 0; i < leftPuzzle.Length; i++)
				{
					rightDifference += GetDifference(leftPuzzle[i], rightPuzzle[i]);
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			return rightDifference;

		}

		// Method for getting the best bottom puzzle
		private Bitmap GetBottomImage(Bitmap firstPuzzle, List<Bitmap> list, ref double totalDifference)
		{
			double minDifference = Int32.MaxValue;
			Color[] upPuzzle = new Color[firstPuzzle.Width];

			for (int i = 0; i < firstPuzzle.Width; i++)
			{
				upPuzzle[i] = firstPuzzle.GetPixel(i, firstPuzzle.Height - 1);
			}

			Bitmap nextPuzzle = null;

			for (int i = 0; i < list.Count; i++)
			{
				Color[] downPuzzle = new Color[firstPuzzle.Width];

				for (int j = 0; j < firstPuzzle.Width; j++)
				{
					downPuzzle[j] = list[i].GetPixel(j, 0);
				}

				double value = GetBottomDifference(upPuzzle, downPuzzle);

				if (minDifference > value)
				{
					nextPuzzle = list[i];
					minDifference = value;
				}
			}

			totalDifference += minDifference;

			return nextPuzzle;
		}

		// Method for getting difference between two puzzles by bottom angle
		private double GetBottomDifference(Color[] upPuzzle, Color[] downPuzzle)
		{
			double bottomDifference = 0;

			try
			{
				if (upPuzzle.Length != downPuzzle.Length)
				{
					throw new Exception("Puzzles are different");
				}

				for (int i = 0; i < upPuzzle.Length; i++)
				{
					bottomDifference += GetDifference(upPuzzle[i], downPuzzle[i]);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			return bottomDifference;
		}
		#endregion
	}
}