﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using gamlib;

namespace rabnet.forms
{
    public partial class OkrolUser : Form
    {
        List<int> _ids = new List<int>();

        public myDatePeriod Period
        {
            get 
            {
                if (rbMonth.Checked)
                    return myDatePeriod.Month;
                else
                    return myDatePeriod.Year;
            }
        }

        /// <summary>
        /// d, m, y
        /// </summary>
        public string PeriodChar
        {
            get
            {
                switch (Period)
                {
                    case myDatePeriod.Day:
                        return "d";
                    case myDatePeriod.Year:
                        return "y";
                    default: return "m";
                }
            }
        }

        public string DateValue
        {
            get 
            {
                if (Period == myDatePeriod.Month)
                    return cbMonth.Text;
                else return cbYear.Text;
            }
        }

        
        public OkrolUser()
        {
            InitializeComponent();
            comboBox1.Items.Add("--- ВСЕ ---");
            _ids.Add(0);
            List<sUser> usrs = Engine.db().GetUsers();
            for (int i = 0; i < usrs.Count; i++)
            {
                comboBox1.Items.Add(usrs[i].Name);
                _ids.Add(usrs[i].Id);
            }
            comboBox1.SelectedIndex = 0;                    
        }

        private void fillFucksDates()
        {
            cbMonth.Items.Clear();
            cbYear.Items.Clear();
            List<String> dates = Engine.get().db().getFuckMonths();
            DateTime dt;

            if (dates.Count > 0)
            {
                foreach (String strDt in dates)
                {
                    dt = DateTime.Parse(strDt);
                    cbMonth.Items.Add(dt.ToString("MMMM yyyy"));
                    if (!cbYear.Items.Contains(dt.Year))
                        cbYear.Items.Add(dt.Year);
                }
                cbMonth.SelectedIndex = 0;
                rbMonth_CheckedChanged(null, null);
            }
            else
            {
                MessageBox.Show("Нет дат случек.");
                this.Close();
            }
        }

        public int getUser()
        {
            if (comboBox1.SelectedIndex < 0) return 0;
            return _ids[comboBox1.SelectedIndex];
        }

        public XmlDocument getXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement row = doc.CreateElement("Row");
            doc.AppendChild(doc.CreateElement("Rows")).AppendChild(row);
            row.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(comboBox1.Text));
            row.AppendChild(doc.CreateElement("from")).AppendChild(doc.CreateTextNode(DateValue));
            row.AppendChild(doc.CreateElement("to")).AppendChild(doc.CreateTextNode(""));
            return doc;
        }

        private void rbMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMonth.Checked)
            {
                cbMonth.Visible = true;
                cbMonth.SelectedIndex = 0;
                cbYear.Visible = false;
                cbYear.SelectedIndex = -1;
            }
            else
            {
                cbMonth.Visible = false;
                cbMonth.SelectedIndex = -1;
                cbYear.Visible = true;
                cbYear.SelectedIndex = 0;
            }
        }

        private void OkrolUser_Load(object sender, EventArgs e)
        {
            fillFucksDates();
        }
    }
}
