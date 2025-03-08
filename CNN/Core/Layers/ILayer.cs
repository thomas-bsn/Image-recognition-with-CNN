namespace Image_recognition_with_CNN.CNN.Layers;

public interface ILayer
{
    public List<float[,,]> ForwardPropagation(List<float[,,]> input);
}