using System.Numerics;

namespace ClassLibrary1
{
    public interface IElectricFieldSource<T> where T: struct
    {
        Vector<T> GetIntensity(Vector<T> location);
    }
}