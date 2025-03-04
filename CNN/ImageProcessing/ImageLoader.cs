using System.Drawing;
using System.Drawing.Drawing2D;

namespace CNN.ImageProcessing;

public class ImageLoader
{
    private string _imageName;
    private Bitmap _image;

    public ImageLoader(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Fichier introuvable : {Path.GetFullPath(path)}");

        if (!IsValidFormat(path))
            throw new ArgumentException("Le format de l'image n'est pas valide.");

        Console.WriteLine($"Chargement de l'image : {path}");
        _image = new Bitmap(path);
        _imageName = Path.GetFileNameWithoutExtension(path);
    }

    private bool IsValidFormat(string path)
    {
        string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp" };
        return validExtensions.Contains(Path.GetExtension(path).ToLowerInvariant());
    }

    public float[,,] ProcessImage()
    {
        using (var rgbImage = Preprocessor.ConvertToRgb(_image))
        using (var resizedImage = Preprocessor.Resize(rgbImage))
        {
            var matrix = Preprocessor.Normalize(resizedImage, centerAroundZero: true);
            Console.WriteLine($"Matrice crée pour l'image : {_imageName}");
            return matrix;
        }
    }
}