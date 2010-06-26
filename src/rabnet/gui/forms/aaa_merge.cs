﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace rabnet
{
    public partial class aaa_merge : Form
    {
        public aaa_merge()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if ((sender as Button).Name == "button1") textBox1.Text = openFileDialog1.FileName;
            else textBox2.Text = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamReader sr1 = new StreamReader(textBox1.Text);
            StreamReader sr2 = new StreamReader(textBox2.Text);
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            while (!sr1.EndOfStream)
            {
                list1.Add(sr1.ReadLine());
            }
            sr1.Close();
            while (!sr2.EndOfStream)
            {
                list2.Add(sr2.ReadLine());
            }
            sr2.Close();

            int i1 = 0;
            while (i1 < list1.Count)
            {
                int i2 = 0;
                while (i2 < list2.Count)
                {
                    if (list1[i1] == list2[i2])
                    {
                        list1.RemoveAt(i1);
                        list2.RemoveAt(i2);
                        i2--;
                    }
                    i2++;
                }
                i1++;
            }
            foreach (string s in list1)
            {
                listBox1.Items.Add(s);
            }
            foreach (string s in list2)
            {
                listBox2.Items.Add(s);
            }

        }
    }
}
