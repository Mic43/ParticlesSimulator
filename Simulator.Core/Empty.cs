using System.Numerics;

namespace Core
{
    public class Empty<T> : IElectricFieldSource<T> where T : struct
    {
        public Vector<T> GetIntensity(Vector<T> location)
        {
            return Vector<T>.Zero;
        }
    }
}