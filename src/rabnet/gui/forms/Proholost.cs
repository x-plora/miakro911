﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class Proholost : Form
    {
        private RabNetEngRabbit r1 = null;
        public Proholost()
        {
            InitializeComponent();
        }

        public Proholost(int r):this()
        {
            r1 = Engine.get().getRabbit(r);
            label1.Text = r1.fullName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                r1.ProholostIt(dateDays1.DateValue);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}