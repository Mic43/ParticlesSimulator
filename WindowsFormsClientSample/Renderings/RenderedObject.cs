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
        public abstract void Draw(Graphics gr);
    }
}