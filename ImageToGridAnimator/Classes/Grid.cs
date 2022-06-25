using System;
using System.Drawing;

internal class Grid
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int SquareSideLenght { get; set; }

    public Color[,][,] GridData { get; set; }

    BaseImageInfo BaseImage { get; set; }
    public ImageProcessingSettings ImageProcessingSettings { get; set; }

    public Grid(Bitmap inputBitmap, ImageProcessingSettings inputImageProcessingSettings)
    {
        ImageProcessingSettings = inputImageProcessingSettings;

        using (TimeLogger tl = new TimeLogger("Grid.Grid:BaseImage = new BaseImageInfo(inputBitmap, inputImageProcessingSettings)"))
        {
            BaseImage = new BaseImageInfo(inputBitmap);
        }

        using (TimeLogger tl = new TimeLogger("GridData = GetGridData(BaseImage, inputImageProcessingSettings)"))
        {
            GridData = GetGridDataAsSquareSideLenght(BaseImage, inputImageProcessingSettings.ActualTargetSquareSideLenght);
        }
    }

    private Color[,][,] GetGridDataAsSquareSideLenght(BaseImageInfo inputBaseImage, int inputTargetSquareSideLenght)
    {
        int squaresXWidth = (int)Math.Ceiling((decimal)inputBaseImage.ImageWidth / inputTargetSquareSideLenght);
        int squaresYHeight = (int)Math.Ceiling((decimal)inputBaseImage.ImageHeight / inputTargetSquareSideLenght);

        SquareSideLenght = inputTargetSquareSideLenght;
        Width = squaresXWidth;
        Height = squaresYHeight;

        int targetWidth = squaresXWidth * inputTargetSquareSideLenght;
        int targetHeight = squaresYHeight * inputTargetSquareSideLenght;

        Color[,] image;
        if (targetWidth != inputBaseImage.ImageWidth || targetHeight != inputBaseImage.ImageHeight)
        {
            image = ResizeAndAlignImage(inputBaseImage.ImageAsColorArray, targetWidth, targetHeight);
        }
        else
        {
            image = inputBaseImage.ImageAsColorArray;
        }

        Color[,][,] result = CutImageToGrid(image, squaresXWidth, squaresYHeight, inputTargetSquareSideLenght);

        return result;
    }

    private Color[,] ResizeAndAlignImage(Color[,] inputImageAsColorArray, int inputTargetWidth, int inputTargetHeight)
    {
        Color[,] result = new Color[inputTargetWidth, inputTargetHeight];
        result = Tools.Images.RGB.FillColorArray(result, Color.Green); //HERE TODO

        int coreImageWidth = inputImageAsColorArray.GetLength(0);
        int coreImageHeight = inputImageAsColorArray.GetLength(1);

        int shitX = (int)Math.Floor((decimal)(inputTargetWidth - coreImageWidth) / 2);
        int shitY = (int)Math.Floor((decimal)(inputTargetHeight - coreImageHeight) / 2);

        for (int x = 0; x < coreImageWidth; x++)
        {
            for (int y = 0; y < coreImageHeight; y++)
            {
                result[x + shitX, y + shitY] = inputImageAsColorArray[x, y];
            }
        }

        return result;
    }

    private Color[,][,] CutImageToGrid(Color[,] inputImageAsColorArray, int inputSquaresXWidth, int inputSquaresYHeight, int inputSqureSideLenght)
    {
        Color[,][,] result = new Color[inputSquaresXWidth, inputSquaresYHeight][,];

        int imageWidth = inputImageAsColorArray.GetLength(0);
        int imageHeight = inputImageAsColorArray.GetLength(1);

        for (int gX = 0; gX < inputSquaresXWidth; gX++)
        {
            for (int gY = 0; gY < inputSquaresYHeight; gY++)
            {
                result[gX, gY] = new Color[inputSqureSideLenght, inputSqureSideLenght];

                int startX = inputSqureSideLenght * gX;
                int startY = inputSqureSideLenght * gY;

                int counterX = 0, counterY = 0;
                for (int pX = startX; pX < startX + inputSqureSideLenght; pX++, counterX++)
                {
                    for (int pY = startY; pY < startY + inputSqureSideLenght; pY++, counterY++)
                    {
                        result[gX, gY][counterX, counterY] = inputImageAsColorArray[pX, pY];
                    }
                    counterY = 0;
                }
            }
        }

        //int idcounter = 0;
        //for (int gX = 0; gX < inputSquaresXWidth; gX++)
        //{
        //    for (int gY = 0; gY < inputSquaresYHeight; gY++)
        //    {
        //        idcounter++;
        //        DirectBitmap bmp = Tools.Images.RGB.ColorArrayToDirectBitmap(result[gX, gY]);
        //        bmp.Bitmap.Save($@"E:\Out\{idcounter}.png");
        //    }
        //}

        return result;
    }
}