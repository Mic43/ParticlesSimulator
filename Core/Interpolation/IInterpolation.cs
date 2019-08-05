using System.Numerics;

namespace Core.Interpolation
{
    public interface IInterpolation
    {
        Vector<float> CalculateValue(Vector<float> input, Vector<float>[,] knownValues);
    }
}