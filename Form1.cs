using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;


namespace Interact
{
    public partial class Form1 : Form
    {
        int add_drug_clicked;
        string drugA;
        string drugB;
        DataRead read = new DataRead();

        //Variables necessary form moving the form (see Form1_MouseDown)
        private bool _dragging = false;
        private Point _offset;
        private Point _startpoint_ = new Point(0,0);

        //Variable for tracking clicks on textBox1
        bool hasBeenClicked = false;


        public Form1()
        {
            InitializeComponent();
            label1.Visible= false;
            label2.Visible= false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(446, 217);
            add_drug_clicked= 0;
        }

        //Button X
        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        //NOT IMPLEMENTED YET
        //Autocompleting text
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Button RESET
        private void button2_Click(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(446, 217);
            add_drug_clicked = 0;
            button4.Enabled=true;
            label1.Visible = false;
            label2.Visible = false;
            drugA = null; drugB = null;
            hasBeenClicked= false;
            textBox1.Text = "Enter drug name";
        }

        //Button ADD
        private void button4_Click(object sender, EventArgs e)
        {
            add_drug_clicked++;
            if (add_drug_clicked == 1)
            {
                if (String.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                    add_drug_clicked = 0;
                    MessageBox.Show("Please enter correct drug name");
                }
                else
                {
                    drugA = textBox1.Text.Trim();
                    label1.Visible = true;
                    label1.Text = drugA;
                    hasBeenClicked = false;
                }   
            }
            if (add_drug_clicked == 2)
            {
                if (String.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                    add_drug_clicked = 1;
                    MessageBox.Show("Please enter correct drug name");
                }
                else
                {
                    drugB = textBox1.Text;
                    label2.Visible = true;
                    label2.Text = drugB.Trim();
                    button4.Enabled = false;
                    textBox1.Text = String.Empty;

                    string[] inter = read.Get(drugA, drugB);
                    if (inter[1] == null && inter[2] == null)
                    {
                        MessageBox.Show("Can't find both drugs in database");
                        add_drug_clicked = 0;
                        button4.Enabled = true;
                        label1.Visible = false;
                        label2.Visible = false;
                        drugA = null; drugB = null;
                    }
                    else if (inter[2] == null || inter[1] == null)
                    {
                        if (inter[1] == null)
                        {
                            MessageBox.Show($"No drug \"{drugA}\" in database");
                        }
                        else
                        {
                            MessageBox.Show($"No drug \"{drugB}\" in database");
                        }
                        add_drug_clicked = 0;
                        button4.Enabled = true;
                        label1.Visible = false;
                        label2.Visible = false;
                        drugA = null; drugB = null;


                    }
                    else if (inter[0] == null)
                    {
                        MessageBox.Show("Interaction between drugs not present in database");
                        add_drug_clicked = 0;
                        button4.Enabled = true;
                        label1.Visible = false;
                        label2.Visible = false;
                        drugA = null; drugB = null;
                    }
                    else
                    {
                        label5.Text = inter[0];
                        this.Size = new System.Drawing.Size(446, 327);
                    }
                }
                   
            }
        }


       

        //Makes form movable without form border
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _startpoint_ = new Point(e.X, e.Y);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._startpoint_.X, p.Y - this._startpoint_.Y);
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging= false;
        }

        //Deletes initial text in textBox1 on mouse click
        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!hasBeenClicked)
            {
                textBox1.Text = String.Empty;
                hasBeenClicked= true;
            }
        }




        //NOT IMPLEMENTED!!!
        //Button MORE INFO
        private void button3_Click(object sender, EventArgs e)
        {

        }


        //UNUSED
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
