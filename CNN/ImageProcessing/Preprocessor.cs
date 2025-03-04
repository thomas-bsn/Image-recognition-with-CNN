using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CNN.ImageProcessing;

public class Preprocessor
{
    private static int _width = 224;
    private static int _height = 224;

    public static Bitmap ConvertToRgb(Bitmap image)
    {
        if (image.PixelFormat != PixelFormat.Format24bppRgb)
        {
            var newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0);
            }
            return newImage;
        }
        return image;
    }

    public static Bitmap Resize(Bitmap image)
    {
        var newImage = new Bitmap(_width, _height);
        using (Graphics g = Graphics.FromImage(newImage))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, _width, _height);
        }
        return newImage;
    }


    public static float[,,] Normalize(Bitmap image, bool centerAroundZero = false)
    {
        var imageArray = new float[_width, _height, 3];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                if (centerAroundZero)
                {
                    imageArray[i, j, 0] = (pixel.R / 127.5f) - 1.0f;
                    imageArray[i, j, 1] = (pixel.G / 127.5f) - 1.0f;
                    imageArray[i, j, 2] = (pixel.B / 127.5f) - 1.0f;
                }
                else
                {
                    imageArray[i, j, 0] = pixel.R / 255.0f;
                    imageArray[i, j, 1] = pixel.G / 255.0f;
                    imageArray[i, j, 2] = pixel.B / 255.0f;
                }
            }
        }
        return imageArray;
    }

}