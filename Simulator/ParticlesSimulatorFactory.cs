using System;
using Core.Collisions;
using Core.Collisions.Colliders;
using Core.Collisions.Colliders.Specific;
using Core.Collisions.Detectors;
using Core.Collisions.Resolvers;
using Core.RunningTasks;
using Core.Collisions;

namespace Simulator
{
    public class ParticlesSimulatorFactory : IParticlesSimulatorFactory<float>
    {
        private readonly float _minimalDistance;
        private readonly float _simulationSpeed;

        public ParticlesSimulatorFactory(float minimalDistance = 0.01f, float simulationSpeed = 1)
        {
            if (minimalDistance <= 0) throw new ArgumentOutOfRangeException(nameof(minimalDistance));
            if (simulationSpeed <= 0) throw new ArgumentOutOfRangeException(nameof(simulationSpeed));

            _minimalDistance = minimalDistance;
            _simulationSpeed = simulationSpeed;
        }

        public ParticlesSimulator<float> Create()
        {
            var simulationObjectsCollection = new SimulationObjectsCollection<float>();
            return
                new ParticlesSimulator<float>(
                    new ParticlesUpdater(simulationObjectsCollection.GetUpdatables(),
                        new ForceStopIfParticleCollisionResolver<float>(
                            new CollisionsDetector<float>(simulationObjectsCollection.GetCollidables(),
                                new Collider(_minimalDistance))))
                    {
                        SimulationSpeed = _simulationSpeed
                    }
                    , simulationObjectsCollection);
        }
    }
}