﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace rabnet
{
    static class Run
    {
        private const string MIA_CONV = "mia_conv.exe";
        private const String RABNET = "rabnet.exe";

        /// <summary>
        /// Создает структуру БД для программы rabnet
        /// </summary>
        /// <param name="connParams">Параметры программы mia_conv</param>
        /// <exception cref="Exception">При неудачном создании БД</exception>
        public static void DBCreate(String connParams, String host, String db, String user, String pwd, String admin, String apwd)
        {
            String prms = String.Format("\"{0:s}\" {1:s};{2:s};{3:s};{4:s};{5:s};{6:s};", connParams, host, db, user, pwd, admin, apwd);
            prms += " зоотехник;";

            String prg = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), MIA_CONV);
            if (!File.Exists(prg))
                throw new Exception(String.Format("Не удается найти программу {0:s}{1:s}БД не будет создана", prg, Environment.NewLine));
            Process p = Process.Start(prg, prms);
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new Exception("Ошибка создания БД. " + miaExitCode.GetText(p.ExitCode));
        }

        /// <summary>
        /// Запускает программу Rabnet
        /// </summary>
        /// <param name="param"></param>
        public static void Rabnet()
        {
            String path = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), RABNET);
            if (!File.Exists(path))
                throw new Exception("Не удается найти файл "+path);
            Process p = Process.Start(path);
            p.WaitForExit();
        }
    }
}
