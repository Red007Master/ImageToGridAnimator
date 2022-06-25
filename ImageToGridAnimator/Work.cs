using System;
using System.Drawing;
using System.IO;

internal class Work
{
    public static Grid Grid { get; set; }
    public static int SavedImagesCount { get; set; } = 0;

    public static Color[,] Image { get; set; }
    public static DirectBitmap SaveBitmap { get; set; }

    public static void Start()
    {
        ImageProcessingSettings bufferImageProcessingSettings = new ImageProcessingSettings();

        using (TimeLogger tl = new TimeLogger("bufferImageProcessingSettings = GetImageProcessingSettings()"))
        {
            bufferImageProcessingSettings = GetImageProcessingSettings();
        }

        using (TimeLogger tl = new TimeLogger("bufferImageProcessingSettings.PrecalculateActualValues(bufferImageProcessingSettings.ImageToProcessPath)"))
        {
            bufferImageProcessingSettings.PrecalculateActualValues(bufferImageProcessingSettings.ImageToProcessPath);
        }

        using (TimeLogger tl = new TimeLogger("Grid = new Grid(new Bitmap(bufferImageProcessingSettings.ImageToProcessPath), bufferImageProcessingSettings)"))
        {
            Grid = new Grid(new Bitmap(bufferImageProcessingSettings.ImageToProcessPath), bufferImageProcessingSettings);
        }

        using (TimeLogger tl = new TimeLogger("MainProcessingLoop(Grid.ImageProcessingSettings)"))
        {
            MainProcessingLoop(Grid.ImageProcessingSettings);
        }
    }

    private static void MainProcessingLoop(ImageProcessingSettings imageProcessingSettings)
    {
        string[] dir = Directory.GetFiles(Publics.PathDirs.Output);
        foreach (string file in dir)
            File.Delete(file);

        int maximunRequiredAditionalWidth = imageProcessingSettings.ActualAnimationEndPixelDistance * (Grid.Width - 1);
        int maximunRequiredAditionalHeight = imageProcessingSettings.ActualAnimationEndPixelDistance * (Grid.Height - 1);

        int baseWidth = Grid.Width * Grid.SquareSideLenght;
        int baseHeight = Grid.Height * Grid.SquareSideLenght;

        int maximumRequiredWidth = baseWidth + maximunRequiredAditionalWidth;
        int maximumRequiredHeight = baseHeight + maximunRequiredAditionalHeight;

        Image = new Color[maximumRequiredWidth, maximumRequiredHeight];
        SaveBitmap = new DirectBitmap(maximumRequiredWidth, maximumRequiredHeight);

        for (int i = imageProcessingSettings.ActualAnimationStartPixelDistance; i < imageProcessingSettings.ActualAnimationEndPixelDistance; i++)
        {
            int additionalSpaceForGridX = i * (Grid.Width - 1);
            int additionalSpaceForGridY = i * (Grid.Height - 1);
            int actualWidth = Grid.Width * Grid.SquareSideLenght + additionalSpaceForGridX;
            int actualHeight = Grid.Height * Grid.SquareSideLenght + additionalSpaceForGridY;

            int globalShiftX = (maximumRequiredWidth - actualWidth) / 2;
            int globalShiftY = (maximumRequiredHeight - actualHeight) / 2;

            Image = Tools.Images.RGB.FillColorArray(Image, Color.Green);

            int addX = 0;
            for (int gX = 0; gX < Grid.Width; gX++)
            {
                int addY = 0;
                for (int gY = 0; gY < Grid.Height; gY++)
                {
                    int startX = gX * Grid.SquareSideLenght + addX;
                    int startY = gY * Grid.SquareSideLenght + addY;

                    int imageX = startX, imageY = startY;
                    for (int iX = 0; iX < Grid.SquareSideLenght; iX++)
                    {
                        for (int iY = 0; iY < Grid.SquareSideLenght; iY++)
                        {
                            Image[imageX + globalShiftX, imageY + globalShiftY] = Grid.GridData[gX, gY][iX, iY];
                            imageY++;
                        }
                        imageY = startY;
                        imageX++;
                    }

                    addY += i;
                }
                addX += i;
            }

            SaveBitmap = Tools.Images.RGB.ColorArrayToDirectBitmap(SaveBitmap, Image);
            SaveBitmap.Bitmap.Save(Publics.PathDirs.Output + @"\" + GetStringIntNumber(SavedImagesCount, 3) + ".png");
            SavedImagesCount++;
        }
    }

    private static ImageProcessingSettings GetImageProcessingSettings()
    {
        ImageProcessingSettings result = new ImageProcessingSettings();

        Console.WriteLine("\n");
        while (true)
        {
            Console.Write("Select operation Settings:\n1)Use config from Settings.\n2)Custom set.\n:");
            string userInput = Console.ReadLine();

            if (userInput == "1" || userInput == "")
            {
                result.SetFromSettings();
                break;
            }
            else if (userInput == "2")
            {
                result.SetFromConsoleDialog();
                break;
            }
            else
            {
                Console.WriteLine("Incorect input try again.");
            }
        }
        Console.WriteLine("\n");

        return result;
    }
    private static string GetStringIntNumber(int inputNumber, int inputTargetLenght)
    {
        string result = "";

        string inputNumberAsString = inputNumber.ToString();

        result += new string('0', inputTargetLenght - inputNumberAsString.Length);
        result += inputNumberAsString;

        return result;
    }
}