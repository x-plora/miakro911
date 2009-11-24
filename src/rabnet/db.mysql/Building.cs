﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    public interface IBuilding : IData
    {
        int id();
        int farm();
        int tier_id();
        string delims();
        string type();
        string itype();
        string notes();
        bool repair();
        int secs();
        string area(int id);
        string dep(int id);
        int nest_heater_count();
        int busy(int id);
        string use(int id);
        string nest();
        string heater();
        string address();
    }

    class Building:IBuilding
    {
        public int fid;
        public int ffarm;
        public int ftid;
        public int fsecs;
        public String[] fareas;
        public String[] fdeps;
        public string ftype;
        public string ftypeloc;
        public string fdelims;
        public string fnotes;
        public bool frepair;
        public string fnests;
        public string fheaters;
        public string faddress;
        public string[] fuses;
        public int[] fbusies;
        public int fnhcount;
        public Building(int id,int farm,int tier_id,string type,string typeloc,string delims,string notes,bool repair,int seccnt)
        {
            fid = id;
            ffarm = farm;
            ftid = tier_id;
            ftype=type;
            ftypeloc = typeloc;
            fdelims=delims;
            fnotes = notes;
            frepair = repair;
            fsecs = seccnt;
        }
        #region IBuilding Members
        public int id(){return fid;}
        public int farm(){return ffarm;}
        public int tier_id(){return ftid;}
        public string delims(){return fdelims;}
        public string type(){return ftypeloc;}
        public string itype(){return ftype;}
        public string notes() { return fnotes; }
        public bool repair() { return frepair; }
        public int secs() { return fsecs; }
        public string area(int id) { return fareas[id]; }
        public string dep(int id) { return fdeps[id]; }
        public string nest() { return fnests; }
        public string heater() { return fheaters; }
        public int nest_heater_count() { return fnhcount; }
        public int busy(int id) { return fbusies[id]; }
        public string use(int id) { return fuses[id]; }
        public string address() { return faddress; }
        #endregion

    }

    class Buildings : RabNetDataGetterBase
    {
        public static bool hasnest(String type,int sec,String nests)
        {
            int c = getRNHCount(type);
            if (c == 0) return false;
            if (type=="dfemale")
                return (nests[sec]=='1');
            return (nests[0]=='1');
        }

        public static String getRDescr(String type, bool shr,int sec,String delims)
        {
            String res = "";
            switch (type)
            {
                case "female":
                case "dfemale": res = shr ? "гн+выг" : "гнездовое+выгул"; break;
                case "complex": if (sec==0)
                        res = shr ? "гн+выг" : "гнездовое+выгул";
                    else
                        res = shr ? "отк" : "откормочное"; 
                    break;
                case "jurta": if (sec == 0)
                    {
                        if (delims[0] == '0')
                            res = (shr ? "гн" : "гнездовое")+"+";
                        res += shr ? "мвг" : "м.выгул";
                    }
                    else
                    {
                        if (delims[0] == '1')
                            res = (shr ? "гн" : "гнездовое") + "+";
                        res += shr ? "бвг" : "б.выгул";
                    }
                    break;

                case "quarta": res = shr ? "отк" : "откормочное"; break;
                case "vertep":
                case "barin": res = shr ? "врт" : "Вертеп"; break;
                case "cabin": if (sec == 0)
                        res = shr ? "гн" : "гнездовое";
                    else
                        res = shr ? "отк" : "откормочное";
                    break;
            }
            return res;
        }
        public static String getRSec(String type, int sec, String delims)
        {
            if (type == "female")
                return "";
            String secnames = "абвг";
            String res = ""+secnames[sec];
            if (type == "quarta" && delims!="111")
            {
                for (int i = sec - 1; i >= 0 && (delims[i]=='1'); i--)
                    if (delims[i] == '0') res = secnames[i] + res;
                for (int i = sec; i < 3 && delims[i] == '1'; i++)
                    if (delims[i] == '0') res = res + secnames[i + 1];
            }
            else if (type == "barin" && delims[0]=='0') 
                res = "аб";
            return res;
        }
        public static String getRName(String type,bool shr)
        {
            String res="Нет";
            switch (type)
            {
                case "female": res=shr?"крлч":"Крольчихин"; break;
                case "dfemale": res = shr ? "2крл" : "Двукрольчихин"; break;
                case "complex": res = shr ? "кмпл" : "Комплексный"; break;
                case "jurta": res = shr ? "юрта" : "Юрта"; break;
                case "quarta": res = shr ? "кврт" : "Кварта"; break;
                case "vertep": res = shr ? "вртп" : "Вертеп"; break;
                case "barin": res = shr ? "барн" : "Барин"; break;
                case "cabin": res = shr ? "хижн" : "Хижина"; break;
            }
            return res;
        }
        public static int getRSecCount(String type)
        {
            int res = 2;
            switch (type)
            {
                case "female": res = 1; break;
                case "complex": res = 3; break;
                case "quarta": res = 4; break;
            }
            return res;
        }
        public static int getRNHCount(String type)
        {
            int res = 1;
            switch (type)
            {
                case "dfemale": res = 2; break;
                case "quarta": 
                case "vertep":
                case "barin": res = 0; break;
            }
            return res;
        }

        public static String fullRName(int farm, int tierid, int sec,String type, String delims, bool shr, bool sht, bool sho)
        {
            String res = farm.ToString();
            if (tierid == 1) res += "-";
            if (tierid == 2) res += "^";
            res += getRSec(type, sec, delims);
            if (sht)
                res += " [" + getRName(type, shr) + "]";
            if (sho)
                res += " (" + getRDescr(type, shr, sec, delims)+")";
            return res;
        }

        public static String fullPlaceName(String rabplace,bool shr,bool sht,bool sho)
        {
            String[] dts = rabplace.Split(',');
            return fullRName(int.Parse(dts[0]),int.Parse(dts[1]),int.Parse(dts[2]),dts[3],dts[4],shr,sht,sho);
        }
        public static String fullPlaceName(String rabplace)
        {
            return fullPlaceName(rabplace, false, false, false);
        }

        public Buildings(MySqlConnection sql, Filters filters):base(sql,filters)
        {
        }

        public static IBuilding getBuilding(MySqlDataReader rd,bool shr)
        {
            int id = rd.GetInt32(0);
            int farm = rd.GetInt32(3);
            int tid = 0;
            if (rd.GetInt32(2) != 0)
            {
                if (rd.GetInt32(1) == id)
                    tid = 1;
                else tid = 2;
            }
            String tp = rd.GetString("t_type");
            String dl = rd.GetString("t_delims");
            int scs = getRSecCount(tp);
            Building b = new Building(id, farm, tid, tp, getRName(tp, shr), dl,
                rd.GetString("t_notes"), (rd.GetInt32("t_repair") == 0 ? false : true), scs);
            List<string> ars = new List<string>();
            List<string> deps = new List<string>();
            List<int> bus = new List<int>();
            List<String> uses = new List<string>();
            for (int i = 0; i < b.secs(); i++)
            {
                ars.Add((tid == 0 ? "" : (tid == 1 ? "^" : "-")) + getRSec(tp, i, dl));
                deps.Add(getRDescr(tp, shr, i, dl));
                bus.Add(rd.GetInt32("t_busy" + (i + 1).ToString()));
                uses.Add(rd.GetString("r" + (i + 1).ToString()));
            }
            b.fareas = ars.ToArray();
            b.fbusies = bus.ToArray();
            b.fdeps = deps.ToArray();
            b.fuses = uses.ToArray();
            b.fnhcount = getRNHCount(tp);
            b.fnests = rd.GetString("t_nest");
            b.fheaters = rd.GetString("t_heater");
            b.faddress = "";
            return b;
        }

        public override IData nextItem()
        {
            try
            {
                bool shr = options.safeBool("shr");
                return getBuilding(rd, shr);
            }
            catch (Exception ex)
            {
                log.Error("building exception ", ex);
                throw ex;
            }
        }

        public override string getQuery()
        {
            string nm = "1";
            if (options.safeBool("dbl"))
                nm = "2";
            return @"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1," + nm + @") r1, rabname(t_busy2," + nm + @") r2,rabname(t_busy3," + nm + @") r3,rabname(t_busy4," + nm + @") r4
FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id ORDER BY m_id";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(t_id) FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id;";
        }

        public static TreeData getTree(int parent,MySqlConnection con,TreeData par)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT b_id,b_name,b_farm FROM buildings WHERE b_parent="+parent.ToString()+";", con);
            MySqlDataReader rd = cmd.ExecuteReader();
            TreeData res=par;
            if (par == null)
            {
                res = new TreeData();
                res.caption = "farm";
            }
            List<TreeData> lst = new List<TreeData>();
            while (rd.Read())
            {
                int id=rd.GetInt32(0);
                String nm=rd.GetString(1);
                int frm=rd.GetInt32(2);
                TreeData dt=new TreeData(id.ToString() + ":" + frm.ToString() + ":" + nm);
                lst.Add(dt);
            }
            rd.Close();
            if (lst.Count > 0)
            {
                foreach (TreeData td in lst)
                {
                    int id=int.Parse(td.caption.Split(':')[0]);
                    getTree(id, con, td);
                }
                res.items = lst.ToArray();
            }
            return res;
        }


        public static IBuilding getTier(int tier,MySqlConnection con)
        {
            MySqlCommand cmd=new MySqlCommand(@"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1,1) r1, rabname(t_busy2,1) r2,rabname(t_busy3,1) r3,rabname(t_busy4,1) r4
FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id) and t_id="+tier.ToString()+";",con);
            MySqlDataReader rd = cmd.ExecuteReader();
            IBuilding b=null;
            if (rd.Read())
            {
                b = getBuilding(rd, false);
            }
            rd.Close();
            return b;
        }
    }
}