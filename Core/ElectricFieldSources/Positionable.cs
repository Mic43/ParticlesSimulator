using System.Numerics;

namespace Core.ElectricFieldSources
{
    public class Positionable<T> : IPositionable<T> where T : struct
    {
        public Positionable() : this(Vector<T>.One)
        {
        }

        public Positionable(Vector<T> position)
        {
            Position = position;
        }

        public Vector<T> Position { get; }

    }
}