namespace PuzzleGameTestTask
{
    using System.Drawing;

    /// <summary>
    /// Class for Bitmap methods
    /// </summary>
    public static class BitmapHelper
    {
        /// <summary>
        /// Method for creating Bitmap for main image
        /// </summary>
        /// <param name="image">Main Image</param>
        /// <param name="width">Width of image</param>
        /// <param name="height">Height of image</param>
        /// <returns></returns>
        public static Bitmap CreateBitmapImage(Image image, int width, int height)
        {
            Bitmap bmpImage = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bmpImage);
            graphics.Clear(Color.White);
            graphics.DrawImage(image, new Rectangle(0, 0, width, height));
            graphics.Flush();
            return bmpImage;
        }

        /// <summary>
        /// Method for creating Bitmap piece of puzzles
        /// </summary>
        /// <param name="image">Main Image</param>
        /// <param name="images">Pieces of image</param>
        /// <param name="index">Number of piece</param>
        /// <param name="numCol">Numbers of columns</param>
        /// <param name="unitX">Position X</param>
        /// <param name="unitY">POsition Y</param>
        public static void CreateBitmapImage(Image image, Image[] images, int index, int numCol, int unitX, int unitY)
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
    }
}
