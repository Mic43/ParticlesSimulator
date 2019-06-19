using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;

namespace WindowsFormsClientSample
{
    public partial class RenderingControl : UserControl
    {
        public IEnumerable<RenderedObject> Particles { get; set; }
        public IPositionConverter PositionConverter { get; set; }

        public RenderingControl() : this(Enumerable.Empty<RenderedObject>())
        {
        }

        public RenderingControl(IEnumerable<RenderedObject> particles)
        {
            Particles = particles ?? throw new ArgumentNullException(nameof(particles));
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;

            foreach (var particle in Particles)
            {
                particle.Draw(graphics);
            }

        }
    }
}
