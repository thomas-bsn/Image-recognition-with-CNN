using System.Drawing;
using System.Drawing.Imaging;

namespace Image_recognition_with_CNN.ImageProcessing;

public class ImageLoader
{
    private static int _width = 224;
    private static int _height = 224;
    private float[,,] _image;
    private string _imageName;
    
    public ImageLoader(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"Fichier introuvable : {Path.GetFullPath(path)}");
            return;
        }
        if (IsValidFormat(path))
        {
            Console.WriteLine($"Chargement de l'image : {path}");
            using (var loadedImage = new Bitmap(path))
            {
                _imageName = Path.GetFileNameWithoutExtension(path);

                using (var rgbImage = ConvertToRgb(loadedImage))
                using (var resizedImage = PictureResize(rgbImage))
                    _image = PixelNormalisation(resizedImage);
            }
        }
        else
        {
            throw new ArgumentException("Le format de l'image n'est pas valide.");
        }
    }
    
    private bool IsValidFormat(String path)
    {
        String[] validExtensions = { ".jpg", ".jpeg", ".png" };
        return validExtensions.Contains(Path.GetExtension(path));
    }
    
    private Bitmap ConvertToRgb(Bitmap image)
    {
        if (image.PixelFormat != PixelFormat.Format24bppRgb) // Si l'image n'est pas en RGB
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

    private Bitmap PictureResize(Bitmap image)
    {
        image = new Bitmap(image, new Size(_width, _height));
        return image;
    }
    
    private float[,,] PixelNormalisation(Bitmap image)
    {
        var imageArray = new float[_width, _height, 3];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                imageArray[i, j, 0] = pixel.R / 255.0f;
                imageArray[i, j, 1] = pixel.G / 255.0f;
                imageArray[i, j, 2] = pixel.B / 255.0f;
            }
        }
        return imageArray;
    }

    public void PrettyPrintMatrix()
    {
        // Ecrit dans un fichier d'ouput la matrice de l'image
        using (StreamWriter file = new StreamWriter(_imageName + ".txt"))
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    file.Write($"[{_image[i, j, 0]}, {_image[i, j, 1]}, {_image[i, j, 2]}] ");
                }
                file.WriteLine();
            }
        }
    }
    
    public float[,,] GetImage()
    {
        return _image;
    }

}