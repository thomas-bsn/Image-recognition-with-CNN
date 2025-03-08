using System.IO;
using Newtonsoft.Json;

namespace Image_recognition_with_CNN.CNN.Layers;

public class ConvolutionalLayerConfig
{
    public int Count { get; set; }
    public int FilterCount { get; set; }
    public int FilterSize { get; set; }
    public int Stride { get; set; }
    public int Padding { get; set; }
    
    public ConvolutionalLayerConfig() { }

    public ConvolutionalLayerConfig(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Le fichier de configuration {path} est introuvable.");
        }
        string jsonContent = File.ReadAllText(path);
        var config = JsonConvert.DeserializeObject<ConvolutionalLayerConfig>(jsonContent);
        if (config == null)
        {
            throw new InvalidDataException("Erreur lors du chargement de la configuration du JSON.");
        }
        Count = config.Count;
        FilterCount = config.FilterCount;
        FilterSize = config.FilterSize;
        Stride = config.Stride;
        Padding = config.Padding;
    }
}