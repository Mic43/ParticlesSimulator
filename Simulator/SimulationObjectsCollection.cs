using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.ElectricFieldSources;

namespace Simulator
{
    public class SimulationObjectsCollection<T> where T : struct
    {
        private readonly List<object> _objects = new List<object>();

        public SimulationObjectsCollection()
        {
            
        }
        public SimulationObjectsCollection(IEnumerable<IPositionable<T>> initialObjects)
        {
            if (initialObjects == null) throw new ArgumentNullException(nameof(initialObjects));
            _objects.AddRange(initialObjects);
        }

        public void Add(IPositionable<T> obj)
        {
            _objects.Add(obj);
        }
        public void Remove(IPositionable<T> positionable)
        {
            _objects.Remove(positionable);
        }

        public IEnumerable<ITickReceiver> GetUpdatables()
        {
            return _objects.OfType<ITickReceiver>();
        }
        public IEnumerable<ICollidable<T>> GetCollidables()
        {
            return _objects.OfType<ICollidable<T>>();
        }
        public IEnumerable<IPositionable<T>> GetPositionables()
        {
            return _objects.OfType<IPositionable<T>>();
        }
        public IEnumerable<IElectricFieldSource<T>> GetElectricFieldSources()
        {
            return _objects.OfType<IElectricFieldSource<T>>();
        }
    }
}