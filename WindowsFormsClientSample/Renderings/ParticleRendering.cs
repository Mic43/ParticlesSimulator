using System;
using System.Drawing;

namespace WindowsFormsClientSample.Renderings
{
    class ParticleRendering : RenderedObject
    {
        public ParticleRendering(Point location) : base(location)
        {
        }

        public override void Draw(Graphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            graphics.FillEllipse(new SolidBrush(Color.Blue), Location.X-2.5f, Location.Y-2.5f, 5, 5);
        }
    }
}