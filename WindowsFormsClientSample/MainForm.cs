using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using WindowsFormsClientSample.Renderings;
using Core;
using Core.ElectricFieldSources;
using Core.Interpolation;
using Core.Utils;
using Simulator;

namespace WindowsFormsClientSample
{
    public partial class MainForm : Form
    {
        private ParticlesSimulator<float> _simulator;
        private IParticlesSimulatorFactory<float> _particlesSimulatorFactory;
        
        private readonly IPositionConverter _positionConverter = new PositionConverter(1000);
     
        private bool _stopSimulation = false;
        private readonly Random _random = new Random();

        public MainForm()
        {
            InitializeComponent();
            CreateAndSetupSimulation();
        }

        private void CreateAndSetupSimulation()
        {
            string path = "export a.txt";
            //            float unitSize = _positionConverter.FromPixels(new Point(1, 1)).X();
            float totalSize = 1.0f; // 1 meter
            float unitSize = 0.01f;

            //var fromVoltageElectricFieldSource = new FromVoltageElectricFieldSource(BitmapHelper.GetGrayScaleBitmapPixels(path), 0, 100, unitSize);

            //            _particlesSimulatorFactory = new FromXmlParticlesSimulatorsFactory(
            //                @"input\1.xml",
            //                IsInBounds,
            //                TimeSpan.FromMilliseconds(2000),0.001f);
            Vector<float> origin = Vector2D.Zero;

            var fs = new FromIntensityMapElectricFieldSource(LoadIntensityFromFile(path, unitSize, totalSize), new NearestNeighbor(),
                unitSize, origin);
            _particlesSimulatorFactory = new ParticlesSimulatorFactory(0.001f, 0.00001f);
            _simulator = _particlesSimulatorFactory.Create();
            _simulator.ObjectsCollection.Add(fs);
        }

        private Vector<float>[,] LoadIntensityFromFile(string path,float unitSize,float totalSize)
        {
            CultureInfo cultureInfo = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            int len = (int)(totalSize / unitSize);
            var list = File.ReadAllLines(path).SkipWhile(x=>x.StartsWith("%")).Select( str =>
            {
                string[] words = str.Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
                return new
                {
                    X = (int) (float.Parse(words[0], cultureInfo) * 0.001f / unitSize),
                    Y = (int)(float.Parse(words[1], cultureInfo) * 0.001f / unitSize),
                    Vec = Vector2D.Create(float.Parse(words[2], cultureInfo), float.Parse(words[3], cultureInfo))
                };
            }).ToList();

            Vector<float>[,] res = new Vector<float>[len, len];
            list.ForEach( data => res[data.X,data.Y] = data.Vec);

            return res;
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
             //   richTextBox.Text = $@"FPS: {1.0 / sw.Elapsed.TotalSeconds:F} 
               //                     Particles {_simulator.ObjectsCollection.GetUpdatables().Count()}";

                richTextBox.AppendText(_simulator.ObjectsCollection.GetPositionables().OfType<Particle>().Aggregate(string.Empty,(s, positionable) => s+= positionable));
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
