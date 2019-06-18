using System;
using System.Linq;
using System.Numerics;
using Core;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var centralElectricFieldSource = new CentralElectricFieldSource(
                Vector<float>.One, 
                (float) Constants.CoulombConstant,
                (float) Constants.ElementalCharge);

            var simulator = new Simulator(
                Enumerable.Repeat(new Particle(centralElectricFieldSource,
                                            (float) Constants.ElectronMass, 
                                            (float)Constants.ElementalCharge,
                                            new Vector<float>(2))
                        ,1)
                           .Cast<ITickReceiver>()
                           .ToList());
            //simulator.Start();
        }
    }
}
