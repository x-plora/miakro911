﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using rabnet.components;
using rabnet.forms;

namespace rabnet
{
    public partial class RIVaccinePanel : UserControl
    {
        private RabNetEngRabbit _rab;

        public RIVaccinePanel()
        {
            InitializeComponent();
        }

        public void SetRabbit(RabNetEngRabbit rab)
        {
            _rab = rab;
            updateRabVac();
        }

        private void updateRabVac()
        {
            lvVaccine.Items.Clear();
            foreach (RabVac rv in _rab.Vaccines)
            {
                ListViewItem lvi = lvVaccine.Items.Add(rv.date.ToShortDateString());
                if(rv.vid>0)
                    lvi.SubItems.Add(String.Format("{0:d}:{1:s}",rv.vid,rv.name));
                else
                    lvi.SubItems.Add(rv.name);
                lvi.SubItems.Add(rv.remains.ToString());
                lvi.SubItems.Add(rv.unabled?"ДА":"-");
                lvi.Tag = rv;
            }
        }

        private void btAddVac_Click(object sender, EventArgs e)
        {
            AddRabVacForm dlg = new AddRabVacForm(_rab);
            if (dlg.ShowDialog() == DialogResult.OK)
            {               
                _rab.SetVaccine(dlg.VacID, dlg.VacDate, dlg.VacChildren);
                updateRabVac();
            }
        }

        private void miCancelRabVac_Click(object sender, EventArgs e)
        {
            if (lvVaccine.SelectedItems.Count == 0) return;
            RabVac rv = lvVaccine.SelectedItems[0].Tag as RabVac;
            if (rv.vid < 0) return;

            rv.unabled = (int)miCancelRabVac.Tag == 1;
            Engine.db().RabVacUnable(_rab.ID, rv.vid, rv.unabled);
            
            updateRabVac();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (lvVaccine.SelectedItems.Count == 0)
            {
                e.Cancel = true;
                return;
            }
            RabVac rv =lvVaccine.SelectedItems[0].Tag as RabVac;
            if (rv.vid < 0)
            {
                e.Cancel = true;
                return;
            }
            if(rv.unabled)
            {
                miCancelRabVac.Text = "Восстановить";
                miCancelRabVac.Tag = 0;
            }
            else
            {
                miCancelRabVac.Text = "Отмена";
                miCancelRabVac.Tag = 1;
            }
        }
    }
}
