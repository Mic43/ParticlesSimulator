using System;
using System.Drawing;
using System.Numerics;

namespace WindowsFormsClientSample
{
    class PositionConverter : IPositionConverter
    {
        private readonly Func<float, int> _meterToPixel;

        public PositionConverter(Func<float, int> meterToPixel)
        {
            this._meterToPixel = meterToPixel ?? throw new ArgumentNullException(nameof(meterToPixel));
        }

        public Point ToPixels(Vector<float> particlePosition)
        {
            return new Point(_meterToPixel(particlePosition[0]), _meterToPixel(particlePosition[1]));
        }
    }
}