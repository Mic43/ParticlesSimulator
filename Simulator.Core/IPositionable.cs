using System.Numerics;

namespace Core
{
    public interface IPositionable<T> where T : struct
    {
        Vector<T> Position { get; }
    }

}