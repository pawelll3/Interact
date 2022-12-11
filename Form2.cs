using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interact
{
    public partial class Form2 : Form
    {
        int ticking;
        ThreadStart prepare_data;
        Thread prepare_data_th;
        public Form2()
        {
            int ticking = 0;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (File.Exists("fin.txt"))
            {
                timer2.Enabled = false;
                timer1.Enabled = false;
                Form form = new Form1();
                form.ShowDialog();
                this.Hide();
                this.SuspendLayout();
            }
            else
            {
                PrepareData prep = new PrepareData();
                prepare_data = new ThreadStart(prep.Raw2Merged);
                prepare_data_th = new Thread(prepare_data);
                prepare_data_th.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (ticking)
            {
                case 0:
                    label2.Text = "Loading Database .";
                    ticking++;
                    break;
                case 1:
                    label2.Text = "Loading Database ..";
                    ticking++;
                    break;
                default:
                    label2.Text = "Loading Database ...";
                    ticking = 0;
                    break;
            }
        }

        private void timer2_Tick_2(object sender, EventArgs e)
        {
            if (File.Exists("fin.txt"))
            {
                timer2.Enabled = false;
                timer1.Enabled = false;
                this.Hide();
                this.SuspendLayout();
                prepare_data_th.Abort();
                Form1 form = new Form1();
                form.ShowDialog();  
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
