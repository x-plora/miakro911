﻿using System;
using System.Reflection;
using System.Windows.Forms;
#if PROTECTED
using RabGRD;
#endif

namespace rabnet
{
    partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            //this.Text = String.Format("About {0} {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Версия {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string licFarms()
        {
#if PROTECTED
            string info = "";
//            return String.Format("Лицензия - {0:d} ферм",PClient.get().farms());
            info += "Лицензия:";
            info += String.Format("Владелец - {0}", GRD.Instance.GetOrgName());
            info += Environment.NewLine;
            info += String.Format("Ограничение на количство ферм - {0:d}", GRD.Instance.GetFarmsCnt());
            info += Environment.NewLine;
            info += String.Format("Период действия - с {0} по {1}", GRD.Instance.GetDateStart().ToShortDateString(),GRD.Instance.GetDateEnd().ToShortDateString());
            return info;
#else 
            return "Без ограничений";
#endif
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return ""+licFarms();
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description+licFarms();
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
