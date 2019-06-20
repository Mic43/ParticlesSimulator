using System.Drawing;
using System.Numerics;

namespace WindowsFormsClientSample
{
    public interface IPositionConverter
    {
        Point ToPixels(Vector<float> particlePosition);
        Vector<float> FromPixels(Point p);
    }
}