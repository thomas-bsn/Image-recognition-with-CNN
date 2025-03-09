using System;
using System.IO;
using CNN.ImageProcessing;
using Image_recognition_with_CNN.CNN;
using Image_recognition_with_CNN.CNN.Layers;


if (args.Length == 0)
{
    Console.WriteLine("Veuillez fournir un chemin d'image.");
    return;
}

string path = args[0];

// On charge l'image avant CNN
var loader = new ImageLoader(path);
var image = loader.ProcessImage();

string configPath = PathHelper.GetConfigPath();
string filtersPath = PathHelper.GetFiltersPath();


if (!File.Exists(configPath))
{
    throw new FileNotFoundException($"Le fichier de configuration est introuvable : {configPath}");
}
 
// On setup les layers
var LF = new LayerFactory();
var CL = LF.CreateConvolutionalLayers(filtersPath, configPath);
var PL = LF.CreatePoolingLayers(1);


var cnn = new Cnn(CL, PL, null);

Console.WriteLine("C'est bon, ça a marché !");
