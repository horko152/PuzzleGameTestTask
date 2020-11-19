namespace PuzzleGameTestTask
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class PuzzleSolver
    {
        // Method that return array of puzzles with the lowest difference between them and mirrored positions
        public Bitmap[,] GetBestPuzzleImage(List<Bitmap> listOfImages, int numRow, int numCol)
        {
            double minDifference = Int32.MaxValue;

            Bitmap[,] bestChoice = null;
            Bitmap[,] possibleChoice = new Bitmap[numRow, numCol];

            double value = GetBestCurrentVariant(listOfImages, numRow, numCol, ref possibleChoice);

			if (minDifference > value)
			{
				bestChoice = (Bitmap[,])possibleChoice.Clone();
				//minDifference = value;
			}

			return bestChoice;
        }

        // Method that returns the lowest difference 
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
                        puzzles[row, col + 1] = bestPuzzle;
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
                    rightDifference += GetColorDifference(leftPuzzle[i], rightPuzzle[i]);
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
                    bottomDifference += GetColorDifference(upPuzzle[i], downPuzzle[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return bottomDifference;
        }

        // Method for getting difference bettwen two colors
        private double GetColorDifference(Color firstColor, Color secondColor)
        {
            int differenceR = Math.Abs(firstColor.R - secondColor.R);
            int differenceG = Math.Abs(firstColor.G - secondColor.G);
            int differenceB = Math.Abs(firstColor.B - secondColor.B);

            return Math.Sqrt(differenceR * differenceR + differenceG * differenceG + differenceB * differenceB);
        }
    }
}
