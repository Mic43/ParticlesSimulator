using System;
using System.Drawing;

namespace WindowsFormsClientSample.Renderings
{
    internal abstract class RenderedObject
    {
        protected RenderedObject(Point location)
        {
            Location = location;
        }

        public Point Location { get; }

        public void Draw(Graphics gr)
        {
            if (gr == null) throw new ArgumentNullException(nameof(gr));

            var limit = 10000;
            if (Location.X <= -limit || Location.X >= limit || Location.Y <= -limit || Location.Y >= limit)
                return;
            DrawSpecific(gr);
        }

        protected abstract void DrawSpecific(Graphics gr);
    }
}