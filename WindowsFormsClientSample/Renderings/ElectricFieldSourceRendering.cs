using System;
using System.Drawing;

namespace WindowsFormsClientSample.Renderings
{
    class ElectricFieldSourceRendering : RenderedObject
    {
        public ElectricFieldSourceRendering(Point location) : base(location)
        {
        }

        protected override void DrawSpecific(Graphics graphics)
        {
            graphics.FillEllipse(new SolidBrush(Color.Red), Location.X -5 , Location.Y - 5, 10, 10);
        }
    }
}