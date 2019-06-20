using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsClientSample.Renderings
{
    class ElectrodeRendering : RenderedObject
    {
        public Point End { get; }

        public ElectrodeRendering(Point start,Point end) : base(start)
        {
            End = end;
        }

        public override void Draw(Graphics gr)
        {
            gr.DrawLine(new Pen(Color.Red,2.0f),Location,End);
        }
    }
}