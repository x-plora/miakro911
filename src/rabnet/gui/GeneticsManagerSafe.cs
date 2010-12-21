﻿using System;
using log4net;
using System.Reflection;

namespace rabnet
{
    class GeneticsManagerSafe
    {
        private const string DllPath=@"..\Genetics";

        private static readonly ILog Log = LogManager.GetLogger(typeof(GeneticsManagerSafe));
        private static Boolean _hasModule = false;

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static Boolean GeneticsModuleTest()
        {
            Log.Debug("Test assembly 'gui_genetics.dll' presence.");
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(GeneticsAssemblyResolve);

            try
            {
                Assembly.Load("gui_genetics");
                //			    gui_genetics = Assembly.LoadFrom(".\\test\\gui_genetics.dll");
                //gg = gui_genetics.GetTypes()[0].GetMethod("AddNewGenetics");
            }
            catch
            {
                Log.Debug("Assembly 'gui_genetics.dll' is not present.");
                return false;
            }
            Log.Debug("Assembly 'gui_genetics.dll' is present.");
            _hasModule = true;
            return true;
        }


        public static int MaxFormsCount
        {
            get
            {
                if (_hasModule)
                {
                    return GeneticsManager.MaxFormsCount;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (_hasModule)
                {
                    GeneticsManager.MaxFormsCount = value;
                }
            }
        }

        public static Boolean AddNewGenetics(int rabID)
        {
            if (_hasModule)
            {
                return GeneticsManager.AddNewGenetics(rabID);
            }
            else
            {
                return false;
            }
        }
        static Assembly GeneticsAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly myAssembly;
            string strAssmbPath = "";

            int pos = args.Name.IndexOf(",");
            string nm;
            if (pos == -1)
            {
                nm = args.Name;
            } else
            {
                nm = args.Name.Substring(0, pos);
            }

            if ( nm == "gui_genetics")
            {
                strAssmbPath=DllPath+@"\gui_genetics.dll";
            }

            //Load the assembly from the specified path.
            try
            {
                myAssembly = Assembly.LoadFrom(strAssmbPath);
            }
            catch
            {
                myAssembly = null; 
            }


            //Return the loaded assembly.
            return myAssembly;
        }
    }
}
