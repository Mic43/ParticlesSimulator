using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Core.Utils;

namespace Core.ElectricFieldSources
{
    public class Electrode : Positionable<float>, IElectricFieldSource<float> 
    {
        private readonly int _chargesCount;
        private readonly float _singleChargeValue;
        public Vector<float> EndPosition { get; }
        private readonly CompoundElectricFieldSource<float> _charges;

        public Electrode(Vector<float> startPosition,Vector<float> endPosition,int chargesCount,
            float singleChargeValue,float coulombConstant)
            : base(startPosition)
        {
            if (chargesCount <= 0) throw new ArgumentOutOfRangeException(nameof(chargesCount));
            if (coulombConstant <= 0) throw new ArgumentOutOfRangeException(nameof(coulombConstant));

            _chargesCount = chargesCount;
            _singleChargeValue = singleChargeValue;
            EndPosition = endPosition;

            _charges = new CompoundElectricFieldSource<float>(
                Extensions.CreatePositionsDistribution(Position,EndPosition,chargesCount)
                    .Select(x=> new CentralElectricFieldSource(x, coulombConstant,singleChargeValue)));
        }
     
        public Vector<float> GetIntensity(Vector<float> location)
        {
            return _charges.GetIntensity(location);
        }
    }
}