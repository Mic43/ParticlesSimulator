using System;
using System.Drawing;

namespace WindowsFormsClientSample
{
    class ElectricFieldSourceRendering : RenderedObject
    {
        public ElectricFieldSourceRendering(Point location) : base(location)
        {
        }

        public override void Draw(Graphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            graphics.FillEllipse(new SolidBrush(Color.Red), Location.X, Location.Y, 10, 10);
        }
    }
}