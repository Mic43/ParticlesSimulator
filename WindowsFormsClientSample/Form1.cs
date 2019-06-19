using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using Timer = System.Windows.Forms.Timer;

namespace WindowsFormsClientSample
{
    public partial class Form1 : Form
    {
        private Simulator _simulator;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Timer _timer;
        private ParticlesProvider _particlesProvider;
        private IEnumerable<Particle> _particles;
        private IEnumerable<IElectricFieldSource<float>> _electricFieldSources;
        private IPositionConverter _positionConverter = new PositionConverter( m => (int) (500*m));


        private List<T> CreateList<T>(Func<T> creator, int repeats)
        {
            var res = new List<T>();
            for (int i = 0; i < repeats; i++)
            {
                res.Add(creator());
            }

            return res;
        }
        public Form1()
        {
            InitializeComponent();

            SetupRendererTimer();
        }

        private void SetupRendererTimer()
        {
            _timer = new Timer() {Interval = 25};
            _timer.Tick += OnTimerOnTick;
        }

        private void CreateAndSetupSimulation()
        {
            Random rnd = new Random();

            _electricFieldSources = CreateList((() =>
                    new CentralElectricFieldSource(
                        new Vector<float>(new float[] {(float) (rnd.NextDouble() * 2),
                            (float)(rnd.NextDouble() * 1.5), 0, 0}),
                        (float) Constants.CoulombConstant,
                        (float) Constants.ElementalCharge / -1000))
                , 2);


            var compoundElectricFieldSource = new CompoundElectricFieldSource<float>(_electricFieldSources);

            _particles = CreateList(() => new Particle(compoundElectricFieldSource,
                (float) Constants.ElectronMass,
                (float) Constants.ElementalCharge,
                new Vector<float>(new float[] {(float) (rnd.NextDouble()), (float) (rnd.NextDouble()), 0, 0})), 10);

            _particlesProvider = new ParticlesProvider(_particles);
            _simulator = new Simulator(_particlesProvider);
        }

        private void OnTimerOnTick(object o, EventArgs args)
        {
            _simulator.DoTick();
            Render();
           // richTextBox.AppendText(_particles.First().ToString());
        }

        private void Render()
        {
            renderingControl.Particles =
                _particles
                    .Select(x => new ParticleRedndering(_positionConverter.ToPixels(x.Position))).Cast<RenderedObject>()
                    .Union(_electricFieldSources.OfType<IPositionable<float>>()
                        .Select(x => new ElectricFieldSourceRendering(_positionConverter.ToPixels(x.Position))));
            renderingControl.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            _cancellationTokenSource.Dispose();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;

            CreateAndSetupSimulation();
            _simulator.Start();
            _timer.Start();
        }
        
        private void buttonStop_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            _simulator.Stop();

            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
        }
    }
}
