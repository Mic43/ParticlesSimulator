﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Collisions;
using Core.Collisions.Resolvers;
using Core.Infrastructure;

namespace Core.RunningTasks
{

    public class ParticlesUpdater : RunningTask
    {
        private float _simulationSpeed = 1;

        public float SimulationSpeed
        {
            get => _simulationSpeed;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Simulation speed must be number greater than zero.");
                _simulationSpeed = value;
            }
        }

        public ParticlesUpdater(IEnumerable<ITickReceiver> particlesProvider) 
            : this(particlesProvider,new EmptyCollisionResolver())
        {
        }
        public ParticlesUpdater(IEnumerable<ITickReceiver> particlesProvider,
            ICollisionsResolver collisionsResolver)
        {
            if (particlesProvider == null)
            {
                throw new ArgumentNullException(nameof(particlesProvider));
            }

            if (collisionsResolver == null)
            {
                throw new ArgumentNullException(nameof(collisionsResolver));
            }

            OnUpdate = (timespan =>
            {
                //foreach (var tickReceiver in particlesProvider.Get())
                //{
                //    tickReceiver.OnTick(TimeSpan.FromTicks((long) (SimulationSpeed * timespan.Ticks)));
                //}

                Parallel.ForEach(particlesProvider,
                    (particle) => particle.OnTick(TimeSpan.FromTicks((long)(SimulationSpeed * timespan.Ticks))));

                collisionsResolver.ResolveAll();
            });

        }
    }
}