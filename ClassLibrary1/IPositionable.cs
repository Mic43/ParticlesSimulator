using System.Numerics;

namespace ClassLibrary1
{
    public interface IPositionable<T> where T : struct
    {
        Vector<T> Position { get; }
    }

}