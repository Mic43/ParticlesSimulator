using System.Linq;
using System.Numerics;
using Core.Utils;
using MoreLinq;

namespace Core.Interpolation
{
    public class NearestNeighbor : IInterpolation
    {
        public Vector<float> CalculateValue(Vector<float> input,Vector<float>[,] knownValues)
        {
//            (int x,int y) [] dirs = { (-1,0),(1,0),(0,1),(0,-1)};
//
//            return dirs.Select(tuple => (tuple.x + positionX, tuple.x + positionY))
//                .Where(IsInBounds)
//                .Select((tuple) => knownValues[tuple.Item1, tuple.Item2])
//                .Select(vec =>
//                    new
//                    {
//                        Vec = vec,
//                        Distance = vec.DistanceTo(knownValues[positionX, positionY])
//                    })
//                .MinBy(obj => obj.Distance).Select(arg => arg.Vec).First();
//
//            bool IsInBounds((int x, int y) position)
//            {
//                return !(position.x < 0 || position.x >= knownValues.GetLength(0)
//                                       || position.y < 0 || position.y >= knownValues.GetLength(1));
//            }
            return Vector2D.Zero;
        }

    }
}