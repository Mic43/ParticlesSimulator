using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;

namespace WindowsFormsClientSample
{
    public partial class Form1 : Form
    {
        private readonly Simulator _simulator;

        public Form1()
        {
            InitializeComponent();
            _simulator = new Simulator();
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            await _simulator.Start();
        }
    }
}
