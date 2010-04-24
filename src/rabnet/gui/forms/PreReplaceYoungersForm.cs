﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class PreReplaceYoungersForm : Form
    {
        private RabNetEngRabbit r = null;
        public PreReplaceYoungersForm()
        {
            InitializeComponent();
        }

        public static DialogResult MakeChoice(int rid)
        {
            RabNetEngRabbit r = Engine.get().getRabbit(rid);
            if (r.youngers.Length == 0) return DialogResult.Cancel;
            if (r.youngers.Length == 1)
                return new ReplaceYoungersForm(r.youngers[0].id).ShowDialog();
            return new PreReplaceYoungersForm(r).ShowDialog();
        }

        public PreReplaceYoungersForm(RabNetEngRabbit rab):this()
        {
            r = rab;
            for (int i = 0; i < r.youngers.Length; i++)
            {
                comboBox1.Items.Add(r.youngers[i].fullname);
            }
            comboBox1.SelectedIndex = 0;
        }

        public PreReplaceYoungersForm(int rid):this(Engine.get().getRabbit(rid))
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Hide();
            DialogResult=new ReplaceYoungersForm(r.youngers[comboBox1.SelectedIndex].id).ShowDialog();
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OneRabbit y = r.youngers[comboBox1.SelectedIndex];
            label1.Text = "Возраст: " + y.age().ToString();
            label2.Text = "Количество: " + y.group.ToString();
            label3.Text = "Порода: " + y.breedname;
        }
    }
}