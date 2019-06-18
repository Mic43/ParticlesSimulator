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

namespace WindowsFormsClientSample
{
    public partial class Form1 : Form
    {
        private readonly Simulator _simulator;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public Form1()
        {
            InitializeComponent();
            var centralElectricFieldSource = new CentralElectricFieldSource(
                Vector<float>.One,
                (float)Constants.CoulombConstant,
                (float)Constants.ElementalCharge);

            _simulator = new Simulator(
                Enumerable.Repeat(new Particle(centralElectricFieldSource,
                            (float)Constants.ElectronMass,
                            (float)Constants.ElementalCharge,
                            new Vector<float>(2))
                        , 1)
                    .Cast<ITickReceiver>()
                    .ToList());
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

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;

            await Task.Run()
            await _simulator.StartAsync(_cancellationTokenSource.Token,
                new Progress<ICollection<IPositionable<float>>>(
                    collection =>
                    {
                       // Application.DoEvents();
                        // Thread.Sleep(1000);
                         richTextBoxParams.AppendText(collection.First().ToString());                        
                    }).);
           
        }
        
        private void buttonStop_Click(object sender, EventArgs e)
        {          
            _cancellationTokenSource.Cancel();
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
        }
    }
}
