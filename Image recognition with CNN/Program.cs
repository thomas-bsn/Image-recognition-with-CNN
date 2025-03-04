using System;
using Image_recognition_with_CNN.ImageProcessing;


if (args.Length == 0)
{
    Console.WriteLine("Veuillez fournir un chemin d'image.");
    return;
}

string imagePath = args[0];

// Appel du modules de traitement d'image
var loader = new ImageLoader(imagePath);
loader.PrettyPrintMatrix();
var image = loader.GetImage();

// Appel du CNN
