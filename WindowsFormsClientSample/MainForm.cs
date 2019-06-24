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
        private Timer _timer;
        private readonly ObjectsStore<Particle> _store = new ObjectsStore<Particle>();
        private readonly IList<IElectricFieldSource<float>> _electricFieldSources =
            new List<IElectricFieldSource<float>>();
        private readonly IPositionConverter _positionConverter = new PositionConverter(500);
        private CompoundElectricFieldSource<float> _compoundElectricFieldSource;
        //private IRunningTask _particlesGenerator;

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
            SetupRendererTimer();
        }

        private void SetupRendererTimer()
        {
           // _timer = new Timer() {Interval = 25};
          //  _timer.Tick += OnTimerOnTick;
        }

        private float minimalDistance = 0.02f;
        private bool AreTooClose(IPositionable<float> pos, IPositionable<float> part)
        {
            return (pos.Position - part.Position).Length() <= minimalDistance;
        }

        private void CreateAndSetupSimulation()
        {
              Random rnd = new Random();

            //_electricFieldSources = CreateList<IElectricFieldSource<float>>((() =>
            //        new CentralElectricFieldSource(
            //            new Vector<float>(new float[] {(float) (rnd.NextDouble() * 2),
            //                (float)(rnd.NextDouble() * 1.5), 0, 0}),
            //            (float) Constants.CoulombConstant,
            //            (float) -Constants.ElementalCharge / 10000))
            //    , 0);

            _electricFieldSources.Add(
                new Electrode(Vector2D.Zero,
                                Vector2D.One, 50,
                                (float)Constants.ElementalCharge / 100000,
                                (float)Constants.CoulombConstant));

            _compoundElectricFieldSource = new CompoundElectricFieldSource<float>(_electricFieldSources);

            _simulator = new SequentialCompositeTask(
                new ByIntervalObjectsGenerator<Particle>(
                    () => new Particle(_compoundElectricFieldSource,
                        (float) Constants.ElectronMass,
                        (float) Constants.ElementalCharge,
                        Vector2D.Create(1.5f,0.5f)),
                    _store, TimeSpan.FromMilliseconds(200)),
                new ByIntervalObjectsGenerator<Particle>(
                    () => new Particle(_compoundElectricFieldSource,
                        (float) Constants.ElectronMass,
                        (float) Constants.ElementalCharge,
                        Vector2D.Create(1.3f,0.3f)), 
                    _store, TimeSpan.FromMilliseconds(100)),
                new ByIntervalRemoveObjects<IPositionable<float>>(_store,_store,    
                    particle =>
                        //_compoundElectricFieldSource.ElectricFieldSources
                        //    .OfType<IPositionable<float>>()
                        //    .Any(part => AreTooClose(part, particle))
                        //||  
                        !IsInBounds(particle)
                    ,TimeSpan.FromMilliseconds(2000)),
                new ParticlesUpdater(_store) { SimulationSpeed = 0.2f});


            _store.Put(CreateList(() => new Particle(_compoundElectricFieldSource,
                (float)Constants.ElectronMass,
                (float)Constants.ElementalCharge,
                new Vector<float>(new float[] { (float)(2 * rnd.NextDouble()), (float)(2 * rnd.NextDouble()), 0, 0 })),
                10000));

            //_particlesProvider = new FilterParticlesTooClose(_electricFieldSources.OfType<IPositionable<float>>(), 
            //    new ParticlesProvider(_particles),0.02f);

        }

        private bool IsInBounds(IPositionable<float> particle)
        {
            return Vector.GreaterThanOrEqualAll(particle.Position, Vector2D.Zero)
             && Vector.LessThanOrEqualAll(particle.Position, _positionConverter.FromPixels(new Point(renderingControl.Size)));
        }

        private void OnTimerOnTick(object o, EventArgs args)
        {
            _simulator.DoUpdate();
            Render();
        }

        private void Render()
        {
            renderingControl.Particles =
                _store.Get().OfType<IPositionable<float>>()
                    .Select(x => new ParticleRendering(_positionConverter.ToPixels(x.Position))).Cast<RenderedObject>()
                    .Union(_electricFieldSources.OfType<CentralElectricFieldSource>()
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
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            _stop = false;
          //  CreateAndSetupSimulation();

            _simulator.Start();
          
            //await MainLoop();
             Stopwatch sw = new Stopwatch();
            while (!_stop)
            {
                _simulator.DoUpdate();
                richTextBox.Text = $@"FPS: {(1.0 / sw.Elapsed.TotalSeconds):F} Particles: {_store.Get().Count()}";
                sw.Restart();
                Render();
                Application.DoEvents();
            }
            // _timer.Start();
        }

        //private async Task MainLoop()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    await Task.Run(() =>
        //    {
        //        while (!stop)
        //        {
        //            _simulator.DoUpdate();
        //            richTextBox.Text = $@"FPS: {(1.0 / sw.Elapsed.TotalSeconds):F} Particles: {_store.Get().Count()}";
        //            sw.Restart();
        //            Render();
        //            //Application.DoEvents();
        //        }
        //    });
        //}
        private bool _stop = false;
        private void buttonStop_Click(object sender, EventArgs e)
        {
          //  _timer.Stop();
            _simulator.Stop();

            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            _stop = true;
        }

        private void renderingControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _electricFieldSources.Add(new CentralElectricFieldSource(
                    _positionConverter.FromPixels(e.Location),
                    (float) Constants.CoulombConstant,
                    (float)  Constants.ElementalCharge / 10000));
            }
            else
            {
                _store.Put(Enumerable.Repeat( new Particle(_compoundElectricFieldSource,
                    (float)Constants.ElectronMass,
                    (float)Constants.ElementalCharge,
                    _positionConverter.FromPixels(e.Location)),1));
            }

            Render();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _stop = true;
        }
    }
}
