namespace Image_recognition_with_CNN.CNN.Layers;

public class PoolingLayer: ILayer
{
    
    public PoolingLayer()
    {
    }
    public List<float[,,]> ForwardPropagation(List<float[,,]> input)
    {
        var featureMap = input;
        var heatmap = HeatmapAttention(featureMap);
        var pooledFeatures = AttentionPooling(featureMap, heatmap);
        var resizedFeatures = Resize(pooledFeatures); 

        return resizedFeatures; 
    }



    
    public List<float[,,]> HeatmapAttention(List<float[,,]> input)
    {
        var heatmap = new List<float[,,]>();

        foreach (var featureMap in input)
        {
            var height = featureMap.GetLength(0);
            var width = featureMap.GetLength(1);
            var depth = featureMap.GetLength(2);

            var attentionMap = new float[height, width, 1];
            float maxVal = float.MinValue, minVal = float.MaxValue;

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    float sum = 0.0f;
                    for (var k = 0; k < depth; k++)
                    {
                        sum += featureMap[i, j, k];
                    }
                    attentionMap[i, j, 0] = sum;

                    if (sum > maxVal) maxVal = sum;
                    if (sum < minVal) minVal = sum;
                }
            }

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    attentionMap[i, j, 0] = (attentionMap[i, j, 0] - minVal) / (maxVal - minVal + 1e-8f); // Min-Max Scaling
                    attentionMap[i, j, 0] = 1.0f / (1.0f + (float)Math.Exp(-attentionMap[i, j, 0])); // Sigmoid
                }
            }

            heatmap.Add(attentionMap);
        }
    
        return heatmap;
    }


    public List<float[,,]> AttentionPooling(List<float[,,]> featureMaps, List<float[,,]> heatmaps)
    {
        var pooledFeatures = new List<float[,,]>();

        for (int index = 0; index < featureMaps.Count; index++)
        {
            var featureMap = featureMaps[index];
            var heatmap = heatmaps[index];

            var height = featureMap.GetLength(0);
            var width = featureMap.GetLength(1);
            var depth = featureMap.GetLength(2);

            float[,,] pooledTensor = new float[1, 1, depth];

            for (int k = 0; k < depth; k++)
            {
                float sumWeighted = 0.0f, sumMax = float.MinValue, count = 0.0f;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        float weight = heatmap[i, j, 0];
                        float value = featureMap[i, j, k] * weight; 

                        sumWeighted += value;
                        if (value > sumMax) sumMax = value;
                        count++;
                    }
                }

                float gap = sumWeighted / count;

                float gmp = sumMax;

                pooledTensor[0, 0, k] = (gap + gmp) * 0.5f;
            }

            pooledFeatures.Add(pooledTensor);
        }

        return pooledFeatures;
    }

    public List<float[,,]> Resize(List<float[,,]> pooledFeatures, int outputSize = 128)
    {
        var resizedFeatures = new List<float[,,]>();

        Random rand = new Random();
        float[,] weights = new float[pooledFeatures[0].GetLength(2), outputSize];

        for (int i = 0; i < pooledFeatures[0].GetLength(2); i++)
        {
            for (int j = 0; j < outputSize; j++)
            {
                weights[i, j] = (float)(rand.NextDouble() * 2 - 1); // Poids entre -1 et 1
            }
        }

        foreach (var featureMap in pooledFeatures)
        {
            int depth = featureMap.GetLength(2); 

            float[,,] outputTensor = new float[1, 1, outputSize]; 

            for (int j = 0; j < outputSize; j++)
            {
                float sum = 0.0f;
                for (int i = 0; i < depth; i++)
                {
                    sum += featureMap[0, 0, i] * weights[i, j];
                }

                outputTensor[0, 0, j] = Math.Max(0, sum);
            }

            resizedFeatures.Add(outputTensor);
        }

        return resizedFeatures;
    }

    

}