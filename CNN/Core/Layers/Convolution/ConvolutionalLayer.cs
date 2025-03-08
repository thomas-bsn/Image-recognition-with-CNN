namespace Image_recognition_with_CNN.CNN.Layers;

public class ConvolutionalLayer : ILayer
{
    
    private int FilterCount { get; set; }
    private int FilterSize { get; set; }
    private int Stride { get; set; } // Sert à déterminer le pas de déplacement du filtre
    private int Padding { get; set; } // Rajoute des zéros autour de l'image pour éviter les problèmes de bordure
    public List<float[,,]> Filters { get; set; }

    public ConvolutionalLayer(int filterCount, int filterSize, int stride, int padding)
    {
        FilterCount = filterCount;
        FilterSize = filterSize;
        Stride = stride;
        Padding = padding;
        Filters = InitializeFilters();
    }
    
    public ConvolutionalLayer(List<string> filterPaths)
    {
        if (filterPaths == null || filterPaths.Count == 0)
        {
            throw new ArgumentException("La liste des filtres ne peut pas être vide.");
        }

        Filters = LoadFilters(filterPaths);
    }

    private List<float[,,]> InitializeFilters()
    {
        List<float[,,]> filters = new List<float[,,]>();
        Random rand = new Random();
        for (int i = 0; i < FilterCount; i++)
        {
            float[,,] filter = new float[FilterSize, FilterSize, 3];
            for (int j = 0; j < FilterSize; j++)
            {
                for (int k = 0; k < FilterSize; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        filter[j, k, l] = (float)rand.NextDouble();
                    }
                }
            }
            string path = Path.Combine(PathHelper.GetDataFiltersPath(), $"{rand.Next()}.filter");
            SaveFilter(filter, path);
            filters.Add(filter);
        }
        return filters;
    }

    private void SaveFilter(float[,,] filter, string path)
    {
        using (StreamWriter sw = new StreamWriter(path) )
        {
            for (int i = 0; i < FilterSize; i++)
            {
                for (int j = 0; j < FilterSize; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        sw.Write(filter[i, j, k] + " ");
                    }
                }
            }
        }
    }
    
    private List<float[,,]> LoadFilters(List<string> filterPaths)
    {
        List<float[,,]> filters = new List<float[,,]>();

        foreach (var path in filterPaths)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                float[,,] filter = new float[FilterSize, FilterSize, 3];
                for (int i = 0; i < FilterSize; i++)
                {
                    for (int j = 0; j < FilterSize; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            filter[i, j, k] = float.Parse(sr.ReadLine());
                        }
                    }
                }
                filters.Add(filter);
            }
        }

        Console.WriteLine($"Chargé {filters.Count} filtres depuis {filterPaths.Count} fichiers.");
        return filters;
    }

    
    public List<float[,,]> ForwardPropagation(List<float[,,]> input)
    {
        List<float[,,]> output = new List<float[,,]>();

        foreach (var filter in Filters)
        {
            float[,,] featureMap = Convolution(input[0], filter);
            output.Add(featureMap);
        }

        return output;
    }


    private float[,,] Convolution(float[,,] image, float[,,] filter)
    {
        int imageSize = image.GetLength(0);
        int filterSize = filter.GetLength(0);
        int outputSize = (imageSize - filterSize + 2 * Padding) / Stride + 1;
        float[,,] featureMap = new float[outputSize, outputSize, 1];

        for (int i = 0; i < outputSize; i++)
        {
            for (int j = 0; j < outputSize; j++)
            {
                float sum = 0;
                for (int k = 0; k < filterSize; k++)
                {
                    for (int l = 0; l < filterSize; l++)
                    {
                        for (int m = 0; m < 3; m++)
                        {
                            sum += image[i * Stride + k, j * Stride + l, m] * filter[k, l, m];
                        }
                    }
                }
                featureMap[i, j, 0] = sum;
            }
        }

        return featureMap;
    }

    
}