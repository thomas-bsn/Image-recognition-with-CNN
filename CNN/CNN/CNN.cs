using System.Numerics;
using Image_recognition_with_CNN.CNN.Layers;

namespace Image_recognition_with_CNN.CNN;

public class Cnn
{
    private readonly List<ConvolutionalLayer> _convolutionalLayers;
    private readonly List<FullyConnectedLayer> _fullyConnectedLayers;
    private readonly List<PoolingLayer> _poolingLayers;
    
    public Cnn(List<ConvolutionalLayer> convolutionalLayers, 
        List<FullyConnectedLayer> fullyConnectedLayers, List<PoolingLayer> poolingLayers)
    {
        _convolutionalLayers = convolutionalLayers;
        _fullyConnectedLayers = fullyConnectedLayers;
        _poolingLayers = poolingLayers;
    }
    
    public List<float> Propagation(float[,,] image)
    {
        List<float[,,]> CC = new List<float[,,]>();
        CC.Add(image);
        foreach (var convolutionalLayer in _convolutionalLayers)
        {
            CC = convolutionalLayer.ForwardPropagation(CC);
        }
        
        // TODO: Implement pooling layers
        // TODO: Implement fully connected layers
        
        // List<float> result = new List<float> { 1.5f, 2.3f, -0.7f };
        
        return null;
    }

}