using System.Numerics;

namespace Core
{
    public interface IElectricFieldSource<T> where T: struct
    {
        Vector<T> GetIntensity(Vector<T> location);
    }
}