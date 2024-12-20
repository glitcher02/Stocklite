﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stocklite1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Resize += new EventHandler(Form1_Resize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            CenterButton();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

            CenterButton();
        }

        private void CenterButton()
        {

            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button1.Top = (this.ClientSize.Height - button1.Height) / 3;
            button2.Left = (this.ClientSize.Width - button2.Width) / 2;
            button2.Top = (this.ClientSize.Height - button2.Height) / 2;
            pictureBox1.Left = (this.ClientSize.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (this.ClientSize.Height - pictureBox1.Height) / 7;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 MainPage = new Form2();
            MainPage.WindowState = FormWindowState.Maximized;
            MainPage.Show();
            this.Hide();

        }

       
    }
}
