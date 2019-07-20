using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using WindowsFormsClientSample.Renderings;
using Core;
using Core.ElectricFieldSources;
using Core.Utils;
using Simulator;

namespace WindowsFormsClientSample
{
    public partial class MainForm : Form
    {
        private ParticlesSimulator<float> _simulator;
        private IParticlesSimulatorFactory<float> _particlesSimulatorFactory;
        
        private readonly IPositionConverter _positionConverter = new PositionConverter(500);
     
        private bool _stopSimulation = false;
        private readonly Random _random = new Random();

        public MainForm()
        {
            InitializeComponent();
            CreateAndSetupSimulation();
        }

        private void CreateAndSetupSimulation()
        {
            _particlesSimulatorFactory = new FromXmlParticlesSimulatorsFactory(
                @"input\1.xml",
                IsInBounds,
                TimeSpan.FromMilliseconds(2000),0.001f);
            _simulator = _particlesSimulatorFactory.Create();

        }

        private void AddParticle(Vector<float> position, double charge)
        {
            var newPart = new Particle(
                (float) Constants.ElectronMass,
                (float) charge,
                position: position);
            newPart.ElectricFieldSource = new CompoundElectricFieldSource<float>(
                _simulator.ObjectsCollection.GetElectricFieldSources(), 
                newPart);
            _simulator.ObjectsCollection.Add(newPart);
        }

        private bool IsInBounds(IPositionable<float> particle)
        {
            return Vector.GreaterThanOrEqualAll(particle.Position, Vector2D.Zero)
             && Vector.LessThanOrEqualAll(particle.Position, _positionConverter.FromPixels(new Point(renderingControl.Size)));
        }

        private void StartSimulation()
        {
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            _stopSimulation = false;

            _simulator.Start();

            Stopwatch sw = new Stopwatch();
            while (!_stopSimulation)
            {
                _simulator.DoUpdate();
                DisplayInfo();
                sw.Restart();
                Render();
                Application.DoEvents();
            }

            void DisplayInfo()
            {
                richTextBox.Text = $@"FPS: {1.0 / sw.Elapsed.TotalSeconds:F} 
                                    Particles {_simulator.ObjectsCollection.GetUpdatables().Count()}";
            }
        }

        private void StopSimulation()
        {
            _simulator.Stop();

            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            _stopSimulation = true;
        }

        private void Render()
        {
            renderingControl.Particles =
                _simulator.ObjectsCollection.GetPositionables().OfType<Particle>()
                    .Select(x => new ParticleRendering(_positionConverter.ToPixels(x.Position))).Cast<RenderedObject>()

                    .Union(_simulator.ObjectsCollection.GetElectricFieldSources().OfType<CentralElectricFieldSource>()
                        .Where(x=>! (x is Particle))
                        .Select(x => new ElectricFieldSourceRendering(_positionConverter.ToPixels(x.Position))))
                    .Union(_simulator.ObjectsCollection.GetElectricFieldSources().OfType<Electrode>()
                        .Select(x => new ElectrodeRendering(_positionConverter.ToPixels(x.Position),
                            _positionConverter.ToPixels(x.EndPosition))));


            renderingControl.Refresh();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
           
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartSimulation();
        }
     
        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopSimulation();
        }

        private void renderingControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _simulator.ObjectsCollection.Add(new CentralElectricFieldSource(
                    _positionConverter.FromPixels(e.Location),
                    (float)  Constants.ElementalCharge));
            }

            if (e.Button == MouseButtons.Left)
            {
                AddParticle(_positionConverter.FromPixels(e.Location), Constants.ElementalCharge);
            }
            Render();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _stopSimulation = true;
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            StopSimulation();
            CreateAndSetupSimulation();
            StartSimulation();
        }

        private void renderingControl_MouseDown(object sender, MouseEventArgs e)
        {
          
        }

        private void renderingControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            //AddParticle(_positionConverter.FromPixels(e.Location), Constants.ElementalCharge);
            //Render();
        }
    }
}
