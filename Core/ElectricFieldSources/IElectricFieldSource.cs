using System.Numerics;

namespace Core.ElectricFieldSources
{
    public interface IElectricFieldSource<T> where T: struct
    {
        Vector<T> GetIntensity(Vector<T> location);
    }
}