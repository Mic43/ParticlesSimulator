using System.Numerics;

namespace Core
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

        public Vector<T> Position { get; protected set; }

    }
}