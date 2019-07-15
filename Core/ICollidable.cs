using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface ICollidable<T> where T : struct
    {
        Vector<T> Position { get; }
    }
}
