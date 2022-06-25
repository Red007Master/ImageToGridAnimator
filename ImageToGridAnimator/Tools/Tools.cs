using System.Drawing;

public static class Tools
{
    public static class Strings
    {
        private static string GetStringIntNumber(int inputNumber, int inputTargetLenght)
        {
            string result = "";

            string inputNumberAsString = inputNumber.ToString();

            result += new string('0', inputTargetLenght - inputNumberAsString.Length);
            result += inputNumberAsString;

            return result;
        }
    }

    public static class Dirs
    {

    }

    public static class Images
    {
        public static DirectBitmap DirectBitmapFromBitmap(Bitmap inputBitmap)
        {
            int width = inputBitmap.Width;
            int height = inputBitmap.Height;

            DirectBitmap result = new DirectBitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result.SetPixel(x, y, inputBitmap.GetPixel(x, y));
                }
            }

            return result;
        }

        internal static Bitmap CutBitmapToNewSize(Bitmap inputBitmap, int inputNewWidth, int inputNewHeight)
        {
            Bitmap result = new Bitmap(inputNewWidth, inputNewHeight);

            for (int x = 0; x < inputNewWidth; x++)
            {
                for (int y = 0; y < inputNewHeight; y++)
                {
                    result.SetPixel(x, y, inputBitmap.GetPixel(x, y));
                }
            }

            return result;
        }

        public static class BlackAndWhite
        {
            public static byte[,] ByteArrayFromBitmap(Bitmap inputBitmap)
            {
                byte[,] result = new byte[inputBitmap.Width, inputBitmap.Height];

                for (int x = 0; x < inputBitmap.Width; x++)
                {
                    for (int y = 0; y < inputBitmap.Height; y++)
                    {
                        Color pixel = inputBitmap.GetPixel(x, y);

                        if (pixel.A != 0)
                        {
                            result[x, y] = inputBitmap.GetPixel(x, y).G;
                        }
                    }
                }

                return result;
            }

            public static DirectBitmap ByteArrayToDirectBitmap(byte[,] inputByteArray)
            {
                int width = inputByteArray.GetLength(0);
                int height = inputByteArray.GetLength(1);

                DirectBitmap result = new DirectBitmap(inputByteArray.GetLength(0), inputByteArray.GetLength(1));

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        result.SetPixel(x, y, Color.FromArgb(inputByteArray[x, y], inputByteArray[x, y], inputByteArray[x, y]));
                    }
                }

                return result;
            }
        }

        public static class RGB
        {
            public static Color[,] BitmapToColorArray(Bitmap inputBitmap)
            {
                Color[,] result = new Color[inputBitmap.Width, inputBitmap.Height];

                for (int x = 0; x < inputBitmap.Width; x++)
                {
                    for (int y = 0; y < inputBitmap.Height; y++)
                    {
                        result[x, y] = inputBitmap.GetPixel(x, y);
                    }
                }

                return result;
            }

            internal static DirectBitmap ColorArrayToDirectBitmap(DirectBitmap saveBitmap, Color[,] inputImageAsColorArray)
            {
                int width = inputImageAsColorArray.GetLength(0);
                int height = inputImageAsColorArray.GetLength(1);

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        saveBitmap.SetPixel(x, y, inputImageAsColorArray[x, y]);

                return saveBitmap;
            }

            public static DirectBitmap ColorArrayToDirectBitmap(Color[,] inputImageAsColorArray)
            {
                int width = inputImageAsColorArray.GetLength(0);
                int height = inputImageAsColorArray.GetLength(1);

                DirectBitmap result = new DirectBitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        result.SetPixel(x, y, inputImageAsColorArray[x, y]);
                    }
                }

                return result;
            }

            internal static Color[,] FillColorArray(Color[,] inputColorArray, Color inputColor)
            {
                int width = inputColorArray.GetLength(0);
                int height = inputColorArray.GetLength(1);

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        inputColorArray[x, y] = inputColor;

                return inputColorArray;
            }

            internal static Color[,] NewColorArray(int inputWidth, int inputHeight, Color inputDefaultArrayValue)
            {
                Color[,] result = new Color[inputWidth, inputHeight];

                for (int x = 0; x < inputWidth; x++)
                    for (int y = 0; y < inputHeight; y++)
                        result[x, y] = inputDefaultArrayValue;

                return result;
            }
        }
    }
}