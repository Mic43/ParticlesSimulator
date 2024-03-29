﻿using System.Drawing;

namespace WindowsFormsClientSample.Renderings
{
    public abstract class RenderedObject
    {
        protected RenderedObject(Point location)
        {
            Location = location;
        }

        public Point Location { get; private set; }
        public abstract void Draw(Graphics gr);
    }
}