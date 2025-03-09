using System.Numerics;
using Image_recognition_with_CNN.CNN.Layers;

namespace Image_recognition_with_CNN.CNN;

public class Cnn
{
    private readonly List<ConvolutionalLayer> _convolutionalLayers;
    private readonly List<PoolingLayer> _poolingLayers;
    private readonly List<FullyConnectedLayer> _fullyConnectedLayers;
    
    public Cnn(List<ConvolutionalLayer> convolutionalLayers, List<PoolingLayer> poolingLayers, 
        List<FullyConnectedLayer> fullyConnectedLayers)
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
        
        foreach (var poolingLayer in _poolingLayers)
        {
            CC = poolingLayer.ForwardPropagation(CC);
        }
        
        // TODO: Implement fully connected layers
        
        // List<float> result = new List<float> { 1.5f, 2.3f, -0.7f };
        
        return null;
    }

}