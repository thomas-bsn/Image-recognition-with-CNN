using System;
using CNN.ImageProcessing;


if (args.Length == 0)
{
    Console.WriteLine("Veuillez fournir un chemin d'image.");
    return;
}

string path = args[0];

// Appel du modules de traitement d'image
var loader = new ImageLoader(path);
var image = loader.ProcessImage();


// Appel du CNN
