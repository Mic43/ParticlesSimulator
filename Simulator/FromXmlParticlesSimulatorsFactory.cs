using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Numerics;
using System.Xml;
using Core;
using Core.Collisions;
using Core.Collisions.Colliders;
using Core.Collisions.Colliders.Specific;
using Core.Collisions.Detectors;
using Core.Collisions.Resolvers;
using Core.ElectricFieldSources;
using Core.RunningTasks;
using Core.Utils;

namespace Simulator
{
    public class FromXmlParticlesSimulatorsFactory : IParticlesSimulatorFactory<float>
    {
        private readonly string _path;
        private readonly double _coulombConstant;
        private readonly Predicate<IPositionable<float>> _isInBounds;
        private readonly TimeSpan _objectRemoverInterval;
        private readonly float _maxDistanceFromElectrode;
        private readonly float _simulationSpeed;

        public FromXmlParticlesSimulatorsFactory(string path, Predicate<IPositionable<float>> isInBounds,
            TimeSpan objectRemoverInterval, float simulationSpeed = 1,
            float maxDistanceFromElectrode = 0.01f,
            double coulombConstant = Constants.CoulombConstant)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _isInBounds = isInBounds;
            _objectRemoverInterval = objectRemoverInterval;
            _maxDistanceFromElectrode = maxDistanceFromElectrode;
            _simulationSpeed = simulationSpeed;
            _coulombConstant = coulombConstant;
        }

        private void AddParticle(Vector<float> position, double charge,
            SimulationObjectsCollection<float> objectsCollection)
        {
            var newPart = new Particle(
                (float)Constants.ElectronMass,
                (float)charge,
                position: position);
            newPart.ElectricFieldSource = new CompoundElectricFieldSource<float>(
                objectsCollection.GetElectricFieldSources(),
                newPart);
            objectsCollection.Add(newPart);
        }

        public ParticlesSimulator<float> Create()
        {
            XElement doc = XElement.Load(_path);
            var electrodes = doc.Elements("electrodes").Single().Elements("electrode")
                .Select(node =>
                {
                    XElement startPosElement = node.Elements("startPoint").Single();
                    XElement endPosElement = node.Elements("endPoint").Single();

                    Vector<float> startPos = Vector2D.Create(
                        float.Parse(startPosElement.Attribute("x").Value),
                        float.Parse(startPosElement.Attribute("y").Value));

                    Vector<float> endPos = Vector2D.Create(
                        float.Parse(endPosElement.Attribute("x").Value),
                        float.Parse(endPosElement.Attribute("y").Value));


                    return new Electrode(startPos, endPos,
                        int.Parse(node.Elements("chargesCount").Single().Value),
                         (float) (float.Parse(node.Elements("singleChargeValue").Single().Value) 
                                  * Constants.ElementalCharge),
                        (float) _coulombConstant);

                }).ToList();

            var simulationObjectsCollection = new SimulationObjectsCollection<float>(electrodes);

            var generators = doc.Elements("particleGenerators").Single().Elements("particleGenerator")
                .Select(node =>
                {
                    XElement posElement = node.Elements("position").Single();

                    Vector<float> pos = Vector2D.Create(
                        float.Parse(posElement.Attribute("x").Value),
                        float.Parse(posElement.Attribute("y").Value));
                    var charge = float.Parse(node.Elements("charge").Single().Value) * Constants.ElementalCharge;

                    return new ByIntervalPerform(
                        TimeSpan.FromMilliseconds(int.Parse(node.Elements("timeIntervalMs").Single().Value)),
                        () =>
                        {
                            AddParticle(pos, charge, simulationObjectsCollection);
                        });
                }).ToList();
            var objectsRemoverTask = new ByIntervalPerform(_objectRemoverInterval,
                () =>
                {
                    var positionables = simulationObjectsCollection.GetPositionables().Where(pos =>
                        !_isInBounds(pos)).ToList();
                    foreach (var positionable in positionables)
                    {
                        simulationObjectsCollection.Remove(positionable);
                    }
                }).Single();
            IEnumerable<IRunningTask> objectsUpdaterTask = (new ParticlesUpdater(simulationObjectsCollection.GetUpdatables(),
                new ForceStopIfParticleCollisionResolver<float>(
                    new CollisionsDetector<float>(simulationObjectsCollection.GetCollidables(),

                        new ElectrodeByDistanceCollider(_maxDistanceFromElectrode).Wrap())))
            {
                SimulationSpeed = _simulationSpeed
            }).Single();

            return new ParticlesSimulator<float>(
                new SequentialCompositeTask(generators.Union(objectsRemoverTask).Union(objectsUpdaterTask)),
                    simulationObjectsCollection);


        }
    }
}