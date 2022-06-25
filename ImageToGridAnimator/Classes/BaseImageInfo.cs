using System.Drawing;

internal class BaseImageInfo
{
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }

    public Bitmap ImageAsBitmap { get; set; }
    public DirectBitmap ImageAsDirectBitmap { get; set; }
    public Color[,] ImageAsColorArray { get; set; }

    public BaseImageInfo(Bitmap inputBitmap)
    {
        ImageWidth = inputBitmap.Width;
        ImageHeight = inputBitmap.Height;

        ImageAsBitmap = inputBitmap;
        ImageAsDirectBitmap = Tools.Images.DirectBitmapFromBitmap(ImageAsBitmap);
        ImageAsColorArray = Tools.Images.RGB.BitmapToColorArray(ImageAsBitmap);
    }
}