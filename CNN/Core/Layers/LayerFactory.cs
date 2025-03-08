using System;
using System.Collections.Generic;
using System.IO;

namespace Image_recognition_with_CNN.CNN.Layers
{
    public class LayerFactory
    {
        public List<ConvolutionalLayer> CreateConvolutionalLayers(string filtersPath, string configFilePath)
        {
            List<ConvolutionalLayer> layers = new List<ConvolutionalLayer>();

            if (Directory.Exists(filtersPath) && Directory.GetFiles(filtersPath, "*.filter").Length > 0)
            {
                var filterFiles = Directory.GetFiles(filtersPath, "*.filter");
                int numFilters = filterFiles.Length;

                Console.WriteLine($"{numFilters} filtres trouvés dans {filtersPath}.");

                var config = new ConvolutionalLayerConfig(configFilePath);
                int numLayers = config.Count;

                int filtersPerLayer = numFilters / numLayers;
                int remainingFilters = numFilters % numLayers;
                List<string> layerFilters = new List<string>();
                for (int i = 0; i < numLayers; i++)
                {
                    

                    for (int j = 0; j < filtersPerLayer; j++)
                    {
                        layerFilters.Add(filterFiles[i * filtersPerLayer + j]);
                    }

                    if (remainingFilters > 0)
                    {
                        layerFilters.Add(filterFiles[numFilters - remainingFilters]);
                        remainingFilters--;
                    }

                    layers.Add(new ConvolutionalLayer(layerFilters));
                }
            }
            else
            {
                Console.WriteLine("Aucun filtre trouvé, création des filtres aléatoires.");

                var config = new ConvolutionalLayerConfig(configFilePath);

                for (int i = 0; i < config.Count; i++)
                {
                    layers.Add(new ConvolutionalLayer(config.FilterCount, 
                        config.FilterSize, config.Stride, config.Padding));
                }
            }

            Console.WriteLine($"{layers.Count} ConvolutionalLayers créées.");
            return layers;
        }
    }
}
