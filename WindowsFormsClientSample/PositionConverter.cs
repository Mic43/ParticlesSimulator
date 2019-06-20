using System;
using System.Drawing;
using System.Numerics;

namespace WindowsFormsClientSample
{
    class PositionConverter : IPositionConverter
    {
        private readonly int _meterToPixels;

        public PositionConverter(int meterToPixels)
        {
            if (meterToPixels <= 0) throw new ArgumentOutOfRangeException(nameof(meterToPixels));
            _meterToPixels = meterToPixels;
        }

        public Point ToPixels(Vector<float> particlePosition)
        {
            return new Point((int) (_meterToPixels * (particlePosition[0])), 
                (int) (_meterToPixels * (particlePosition[1])));
        }

        public Vector<float> FromPixels(Point p)
        {
            return new Vector<float>(new[] {1.0f / _meterToPixels * p.X, 1.0f / _meterToPixels * p.Y,0,0 });
        }
    }
}