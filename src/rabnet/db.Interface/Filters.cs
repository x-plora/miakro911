﻿using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class TreeData
    {
        private const char PATH_SPLITTER = '.';
        //public String caption;
        public int BldID;
        public int TierID;
        public string Name;
        public List<TreeData> Childrens;
        private string _pPath = null;
        
        public TreeData(int bldId,int tierId,string name,string pPath): this()
        {
            this.BldID = bldId;
            this.TierID = tierId;
            this.Name = name;
            _pPath = pPath;
        }
        public TreeData(int bldId, int tierId, string name) 
            : this(bldId, tierId, name, null) { }        
        public TreeData() { }

        public string Path
        {
            get
            {
                if (_pPath == null) return BldID.ToString();
                return String.Concat(_pPath, PATH_SPLITTER, BldID.ToString());              
            }
        }
    }

    /// <summary>
    /// Нужен в основном Для передачи Параметром программы SQL запросам
    /// </summary>
    public class Filters : Dictionary<String, String>
    {
        #region const
        /// <summary>
        /// Сокращения
        /// </summary>
        public const string SHORT = "shr";
        public const string MALE = "mt";
        public const string FEMALE = "ft";
        public const string SHOW_BLD_TIERS = "sht";
        public const string SHOW_BLD_DESCR = "sho";
        public const string SHOW_OKROL_NUM = "num";
        public const string MAKE_CANDIDATE = "cand";
        public const string MAKE_BRIDE = "brd";
        /// <summary>
        /// Какого возраста крольчатам назначать прививку
        /// </summary>
        //public const string VACC_DAYS = "vacc";
        /// <summary>
        /// Прививать крольчат с матерью
        /// </summary>
        public const string VACC_MOTH = "vacc_moth";
        /// <summary>
        /// Набор id прививок для отображение
        /// </summary>
        public const string VACC_SHOW = "vacc_show";
        /// <summary>
        /// Отсадка мальчиков в возврасте начиная с...
        /// </summary>
        public const string BOYS_OUT = "boysout";
        /// <summary>
        /// отсадка девочек в возврасте начиная с...
        /// </summary>
        public const string GIRLS_OUT = "girlsout";
        public const string VUDVOR = "vudvor";
        /// <summary>
        /// На какой день принять окрол
        /// </summary>
        public const string OKROL = "okrol";
        public const string DBL_SURNAME = "dbl";
        public const string FIND_PARTNERS = "prt";
        public const string PRE_OKROL = "preok";
        public const string NEST_OUT_IF_SUKROL = "vd_sukr";
        public const string COUNT1 = "count1";
        public const string COUNT2 = "count2";
        public const string COUNT3 = "count3";
        public const string NEST_IN = "nest";
        public const string CHILD_NEST = "cnest";
        public const string STATE_FUCK = "sfuck";
        public const string FIRST_FUCK = "ffuck";
        public const string GETEROSIS = "heter";
        public const string INBREEDING = "inbr";
        public const string MALE_WAIT = "mwait";
        public const string BOYS_BY_ONE = "bbone";

        public const string TIER = "yar"; 
        #endregion const

        public Filters() : base() { }
        public Filters(String s)
            : base()
        {
            fromString(s);
        }

        public static Filters makeFromString(String s)
        {
            return new Filters(s);
        }
        public string safeValue(String key, String def)
        {
            if (!ContainsKey(key))
                return def;
            return this[key];
        }
        public String safeValue(String key) { return safeValue(key, ""); }

        public int safeInt(String key, int def) 
        {
            int result = 0;
            string val = safeValue(key, def.ToString());
            int.TryParse(val, out result);
            return result; 
        }//TODO не безопасно
        public int safeInt(String key) { return safeInt(key, 0); }

        public bool safeBool(String key, bool def) { return (safeInt(key, (def ? 1 : 0)) == 1); }
        public bool safeBool(String key) { return safeBool(key, false); }

        public String toString()
        {
            String res = "";
            for (KeyCollection.Enumerator i = Keys.GetEnumerator(); i.MoveNext(); )
            {
                string val = this[i.Current];
                val.Replace("\\", "\\\\");
                val.Replace("=", "\\1");
                val.Replace(";", "\\2");
                res += i.Current + "=" + this[i.Current] + ";";
            }
            return res;
        }

        public void fromString(String str)
        {
            this.Clear();
            foreach (string s in str.Split(';'))
            {
                if (s != "")
                {
                    String[] kv = s.Split('=');
                    kv[1].Replace("\\1", "=");
                    kv[1].Replace("\\2", ";");
                    kv[1].Replace("\\\\", "\\");
                    this[kv[0]] = kv[1];
                }
            }
        }
    }
}
