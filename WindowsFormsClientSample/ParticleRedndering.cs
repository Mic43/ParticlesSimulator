using System;
using System.Drawing;

namespace WindowsFormsClientSample
{
    class ParticleRedndering : RenderedObject
    {
        public ParticleRedndering(Point location) : base(location)
        {
        }

        public override void Draw(Graphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            graphics.FillEllipse(new SolidBrush(Color.Blue), Location.X, Location.Y, 5, 5);
        }
    }
}