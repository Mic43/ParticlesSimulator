using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsClientSample.Renderings;
using Core;
using Core.ElectricFieldSources;
using Core.Infrastructure;
using Core.RunningTasks;
using Core.Utils;
using Timer = System.Windows.Forms.Timer;

namespace WindowsFormsClientSample
{
    public partial class MainForm : Form
    {
        private IRunningTask _simulator;
        private ObjectsStore<Particle> _store = new ObjectsStore<Particle>();
        private readonly IList<IElectricFieldSource<float>> _electricFieldSources =
            new List<IElectricFieldSource<float>>();
        private readonly IPositionConverter _positionConverter = new PositionConverter(500);
        //   private CompoundElectricFieldSource<float> _compoundElectricFieldSource;
        //private IRunningTask _particlesGenerator;

        private bool _stopSimulation = false;
        private readonly Random _random = new Random();

        private List<T> CreateList<T>(Func<T> creator, int repeats)
        {
            var res = new List<T>();
            for (int i = 0; i < repeats; i++)
            {
                res.Add(creator());
            }

            return res;
        }
        public MainForm()
        {
            InitializeComponent();
            CreateAndSetupSimulation();
        }

       

        private float minimalDistance = 0.02f;
        private bool AreTooClose(IPositionable<float> pos, IPositionable<float> part)
        {
            return (pos.Position - part.Position).Length() <= minimalDistance;
        }

        private void CreateAndSetupSimulation()
        {
              _store = new ObjectsStore<Particle>();
              _electricFieldSources.Clear();

            _electricFieldSources.Add(
                new Electrode(Vector2D.Zero,
                                Vector2D.One, 50,
                                -(float)Constants.ElementalCharge,
                                (float)Constants.CoulombConstant));


            _simulator = new SequentialCompositeTask(
               // new ByIntervalPerform<Particle>(
               //     TimeSpan.FromMilliseconds(200),
               //     () => { AddParticle(Vector2D.Create(1.5f, 0.5f), Constants.ElementalCharge); }),
               //new ByIntervalPerform<Particle>(
               //     TimeSpan.FromMilliseconds(400),
               //     () => { AddParticle(Vector2D.Create(0.5f, 1.5f), Constants.ElementalCharge); }),
                new ByIntervalPerform<IPositionable<float>>(TimeSpan.FromMilliseconds(2000),
                    () =>
                    {
                        foreach (var particle in _store.Get())
                        {
                            if (!IsInBounds(particle))
                                _electricFieldSources.Remove(particle);
                        }
                        _store.Remove((part) => !IsInBounds(part));
                    }),
                new ParticlesUpdater(_store) { SimulationSpeed = 0.001f});


            //_store.Put(CreateList(() =>
            //    {
            //        var newPart = new Particle(                 
            //            (float)Constants.ElectronMass,
            //            (float)Constants.ElementalCharge ,
            //            position: Vector2D.Create((float)(2 * _random.NextDouble()), (float)(2 * _random.NextDouble())));
            //        newPart.ElectricFieldSource = new CompoundElectricFieldSource<float>(_electricFieldSources,newPart);
            //        return newPart;
            //    },
            //    200));

        }

        private void AddParticle(Vector<float> position, double charge)
        {
            var newPart = new Particle(
                (float) Constants.ElectronMass,
                (float) charge,
                position: position);
            newPart.ElectricFieldSource = new CompoundElectricFieldSource<float>(_electricFieldSources, newPart);
            _electricFieldSources.Add(newPart);
            _store.Put(Extensions.Single(newPart));
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
                richTextBox.Text = $@"FPS: {(1.0 / sw.Elapsed.TotalSeconds):F} Particles: {_store.Get().Count()}";
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
                _store.Get().OfType<IPositionable<float>>()
                    .Select(x => new ParticleRendering(_positionConverter.ToPixels(x.Position))).Cast<RenderedObject>()

                    .Union(_electricFieldSources.OfType<CentralElectricFieldSource>().Where(x=>! (x is Particle))
                        .Select(x => new ElectricFieldSourceRendering(_positionConverter.ToPixels(x.Position))))
                    .Union(_electricFieldSources.OfType<Electrode>()
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
                _electricFieldSources.Add(new CentralElectricFieldSource(
                    _positionConverter.FromPixels(e.Location),
                    (float)  Constants.ElementalCharge));
            }

            //if (e.Button == MouseButtons.Left)
            //{
            //    AddParticle(_positionConverter.FromPixels(e.Location), Constants.ElementalCharge);
            //}
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

            AddParticle(_positionConverter.FromPixels(e.Location), Constants.ElementalCharge);
            Render();
        }
    }
}
