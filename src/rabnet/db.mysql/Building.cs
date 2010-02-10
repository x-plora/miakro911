﻿//#define TRIAL
//#define CRACKED
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;


namespace rabnet
{
    public class Building:IData
    {
        public int fid;
#if TRIAL && !CRACKED
        public byte ffarm;
#else
        public int ffarm;
#endif
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
        public string[] fullname=new string[4];
        public string[] smallname = new string[4];
#if TRIAL && !CRACKED
        public Building(int id, byte farm, int tier_id, string type, string typeloc, string delims, string notes, bool repair, int seccnt)
#else
        public Building(int id, int farm, int tier_id, string type, string typeloc, string delims, string notes, bool repair, int seccnt)
#endif
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
            for (int i = 0; i < fsecs; i++)
            {
                fullname[i] = Buildings.fullRName(ffarm, ftid, i, ftype, fdelims, false, true, true);
                smallname[i] = Buildings.fullRName(ffarm, ftid, i, ftype, fdelims, true, false, false);
            }
        }
        #region IBuilding Members
        public int id() { return fid; }
#if TRIAL && !CRACKED
        public byte farm() { return ffarm; }
#else
        public int farm() { return ffarm; }
#endif
        public int tier_id() { return ftid; }
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
#if TRIAL
        public static void check1(MySqlConnection sql, int famid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("as KJAhd {0:d} akljgajdasavclm", famid), sql);
            int jh = -289 + famid;
            if (jh < 0)
            {
                Random r = new Random();
                if (r.NextDouble() > 0.5) makeChaos1(sql);
                if (r.NextDouble() < 0.3) makeChaos5(sql);
                if (r.NextDouble() > 0.6784) makeChaos4(sql);
            }
        }
        public static void makeChaos1(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='chaos';", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            string chv = "";
            if (rd.Read())
                chv = rd.GetString(0);
            rd.Close();
            string code="abcdefg";
            int id = new Random().Next(7);
            chv += code[id];
            if (chv == ""+code[id])
                cmd.CommandText = String.Format("INSERT INTO options(o_name,o_subname,o_value) VALUES('chaos','keycode','{0:s}');",chv);
            else
                cmd.CommandText = String.Format("UPDATE options SET o_value='{0:s}' WHERE o_name='chaos';",chv);
            cmd.ExecuteNonQuery();
            int av = (id+18) * 3-17;
            cmd.CommandText = String.Format("UPDATE rabbits SET r_name=r_name+{0:d},r_surname=r_surname+{0:d},r_mother=r_mother+{0:d};",av);
            cmd.ExecuteNonQuery();
        }
        public static void checkFarms3(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM minifarms WHERE m_id>255;");
            MySqlDataReader rd = cmd.ExecuteReader();
            int cnt = 0;
            if (rd.Read())
                cnt = rd.GetInt32(0);
            rd.Close();
            switch(cnt % 3)
            {
                case 0: check1(sql, cnt); break;
                case 1: check3(cnt,sql); break;
                case 2: check5(cnt*543%25,"",cnt,sql); break;
            }
        }
#endif

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

#if TRIAL && !CRACKED
        public static String fullRName(byte farm, int tierid, int sec, String type, String delims, bool shr, bool sht, bool sho)
#else
        public static String fullRName(int farm, int tierid, int sec, String type, String delims, bool shr, bool sht, bool sho)
#endif
        {
            String res = String.Format("{0,4:d}",farm);
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
            if (rabplace == "")
                return OneRabbit.NullAddress;
            String[] dts = rabplace.Split(',');
#if TRIAL && !CRACKED
            return fullRName(byte.Parse(dts[0]),int.Parse(dts[1]),int.Parse(dts[2]),dts[3],dts[4],shr,sht,sho);
#else
            return fullRName(int.Parse(dts[0]),int.Parse(dts[1]),int.Parse(dts[2]),dts[3],dts[4],shr,sht,sho);
#endif
        }
        public static String fullPlaceName(String rabplace)
        {
            return fullPlaceName(rabplace, false, false, false);
        }

        public static bool hasnest(String rabplace)
        {
            if (rabplace == "")
                return false;
            String[] dts = rabplace.Split(',');
            return hasnest(dts[3], int.Parse(dts[2]), dts[5]);
        }

        public Buildings(MySqlConnection sql, Filters filters):base(sql,filters){}

        public static Building getBuilding(MySqlDataReader rd,bool shr,bool rabbits)
        {
            int id = rd.GetInt32(0);
#if TRIAL && !CRACKED
            byte farm = Byte.Parse(rd.GetString(3));
#else
            int farm = rd.GetInt32(3);
#endif
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
                int rn = (rd.IsDBNull(10+i) ? -1 : rd.GetInt32("t_busy" + (i + 1).ToString()));
                bus.Add(rn);
                if (rabbits)
                    uses.Add(rd.GetString("r" + (i + 1).ToString()));
                else
                    uses.Add("");
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
                return getBuilding(rd, shr,true);
            }
            catch (Exception ex)
            {
                log.Error("building exception ", ex);
                throw ex;
            }
        }

        public String makeWhere()
        {
            
            String res = "";
            if (options.ContainsKey("frm"))
            {               
                String sres = "";
                if (options["frm"] == "1")
                    sres = "t_busy1<>0 OR t_busy2<>0 OR t_busy3<>0 OR t_busy4<>0";
                else sres = "t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0";
                res = "("+sres+")" ;                
            }

            if (options.ContainsKey("yar"))
            {
                String sres = "";
                if (options.safeValue("yar").Contains("v")) sres = addWhereOr(sres, "t_type='vertep'");
                if (options.safeValue("yar").Contains("u")) sres = addWhereOr(sres, "t_type='jurta'");
                if (options.safeValue("yar").Contains("q")) sres = addWhereOr(sres, "t_type='quarta'");
                if (options.safeValue("yar").Contains("b")) sres = addWhereOr(sres, "t_type='barin'");
                if (options.safeValue("yar").Contains("k")) sres = addWhereOr(sres, "t_type='female'");
                if (options.safeValue("yar").Contains("d")) sres = addWhereOr(sres, "t_type='dfemale'");
                if (options.safeValue("yar").Contains("x")) sres = addWhereOr(sres, "t_type='complex'");
                if (options.safeValue("yar").Contains("h")) sres = addWhereOr(sres, "t_type='cabin'");
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if (options.ContainsKey("grlk"))
            {
                String sres = "";
                if (options["grlk"] == "1") sres = "t_heater='0' OR t_heater='00'";
                if (options["grlk"] == "2") sres = "t_heater='1' OR t_heater='3'";
                if (options["grlk"] == "3") sres = "t_heater='1'";
                if (options["grlk"] == "4") sres = "t_heater='3'";
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if (options.ContainsKey("gnzd"))
            {
                String sres = "";
                if (options["gnzd"] == "1") 
                    sres = "t_nest<>'1'";
                else sres = "t_nest='1'";
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if(res !="") res= "AND "+res;
            
            return res;

        }
        public override string getQuery()
        {
            string nm = "1";
            if (options.safeBool("dbl"))
                nm = "2";
            return @"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1," + nm + @") r1, rabname(t_busy2," + nm + @") r2,rabname(t_busy3," + nm + @") r3,rabname(t_busy4," + nm + @") r4
FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id) "+makeWhere()+"ORDER BY m_id;";
        }

#if TRIAL
        public static void check2(int famid, MySqlConnection sql, int someint)
        {
            Random r = new Random(someint);
            if (famid > 290 + Math.Truncate(r.NextDouble() * 30))
            {
                if (someint % 2 == 0)
                    makeChaos2(sql);
                else
                    makeChaos5(sql);
                if (someint % 3 == 0)
                    makeChaos4(sql);
            }
        }
        public static void makeChaos2(MySqlConnection sql)
        {
            //String code="CDEFGH";
            int id = new Random().Next(6);
            int av = (id * 4) - (id % 3) + 12;
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE rabbits SET r_name=r_name+{0:d},r_farm=r_farm+{0:d};",id),sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT COUNT(*) FROM options WHERE o_name='chaos';";
            MySqlDataReader rd = cmd.ExecuteReader();
            bool haslink=false;
            if (rd.Read())
                if (rd.GetInt32(0) > 0) haslink = true;
            rd.Close();
            if (haslink)
                cmd.CommandText = String.Format("UPDATE options SET o_value=CONCAT(o_value,'{0:s}') WHERE o_name='chaos';", "" + "CDEFGH"[id]);
            else
                cmd.CommandText = String.Format("INSERT INTO options(o_name,o_subname,o_value) VALUES('chaos','keycode','{0:s}');", "" + "CDEFGH"[id]);
            cmd.ExecuteNonQuery();
        }
#endif

        public override string countQuery()
        {
            return "SELECT COUNT(t_id) FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id)" + makeWhere() + ";";
        }

        public static TreeData getTree(int parent,MySqlConnection con,TreeData par)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT b_id,b_name,b_farm FROM buildings WHERE b_parent="+parent.ToString()+" ORDER BY b_farm ASC;", con);
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
#if TRIAL && !CRACKED
                byte frm = Byte.Parse(rd.GetString(2));
#else
                int frm = rd.GetInt32(2);
#endif
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


        public static Building getTier(int tier,MySqlConnection con)
        {
            MySqlCommand cmd=new MySqlCommand(@"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1,1) r1, rabname(t_busy2,1) r2,rabname(t_busy3,1) r3,rabname(t_busy4,1) r4
FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id) and t_id="+tier.ToString()+";",con);
            MySqlDataReader rd = cmd.ExecuteReader();
            Building b=null;
            if (rd.Read())
                b = getBuilding(rd, false,true);
            rd.Close();
            return b;
        }

#if TRIAL
        public static void check3(int famid, MySqlConnection sql)
        {
            double pz = famid / 280;
            if (pz > 1.0)
            {
                switch (famid % 3)
                {
                    case 0: makeChaos2(sql); break;
                    case 1: makeChaos3(sql); break;
                    case 2: makeChaos4(sql); break;
                }
            }
        }
        public static void makeChaos3(MySqlConnection sql)
        {
            string code="2345678";
            MySqlCommand cmd =new MySqlCommand("SELECT COUNT(*) FROM options WHERE o_name='chaos';",sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int id = new Random().Next(code.Length);
            int av=id;
            bool haslink = false;
            av++;
            if (rd.Read())
                if (rd.GetInt32(0) > 0) haslink = true;
            av++;
            av*=av;
            rd.Close();
            if (haslink)
                cmd.CommandText = String.Format("UPDATE options SET o_value=CONCAT(o_value,'{0:s}') WHERE o_name='chaos';", "" + code[id]);
            else
                cmd.CommandText = String.Format("INSERT INTO options(o_name,o_subname,o_value) VALUES('chaos','keycode','{0:s}');", "" + code[id]);
            cmd.ExecuteNonQuery();
            av-=(av%4);
            cmd.CommandText = String.Format("UPDATE rabbits SET r_name=r_name+r_surname,r_surname=r_surname+{0:d},r_mother=r_mother+{0:d};",av);
            cmd.ExecuteNonQuery();
        }
#endif

        public static Building[] getFreeBuildings(MySqlConnection sql,Filters f)
        {
            List<Building> bld = new List<Building>();
            String type="";
            if (f.safeValue("tp") != "")
                type = "t_type='" + f.safeValue("tp") + "' AND ";
            String busy = "(("+type+"(t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0))";
            if (f.safeInt("rcnt") > 0)
                for (int i = 0; i < f.safeInt("rcnt"); i++)
                {
                    int r=f.safeInt("r"+i.ToString());
                    if (r > 0)
                        busy += String.Format(" OR (t_busy1={0:d} OR t_busy2={0:d} OR t_busy3={0:d} OR t_busy4={0:d})", r);
                }
            busy += ")";
            MySqlCommand cmd = new MySqlCommand(@"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4 FROM minifarms,tiers WHERE
(m_upper=t_id OR m_lower=t_id) AND t_repair=0 AND "+busy+";", sql);
            log.Debug("free Buildings cmd:"+cmd.CommandText);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                bld.Add(getBuilding(rd,false,false) as Building);
            rd.Close();
#if TRIAL
            checkFarms(sql);
#endif
            return bld.ToArray();
        }

        public static void updateBuilding(Building b,MySqlConnection sql)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"UPDATE tiers SET t_repair={1:d},t_delims='{2:s}',t_heater='{3:s}',t_nest='{4:s}' WHERE t_id={0:d};",
                b.fid,b.frepair?1:0,b.fdelims,b.fheaters,b.fnests),sql);
            cmd.ExecuteNonQuery();
        }

        public static void setBuildingName(MySqlConnection sql, int bid, String name)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE buildings SET b_name='{0:s}' WHERE b_id={1:d};",
                name,bid), sql);
            cmd.ExecuteNonQuery();
        }

        public static void addBuilding(MySqlConnection sql, int parent, String name,int farm)
        {
            
#if TRIAL && !CRACKED
            byte frm = byte.Parse(farm.ToString());
#else
            int frm = farm;
#endif
            MySqlCommand cmd = new MySqlCommand(String.Format(@"INSERT INTO buildings(b_name,b_parent,b_level,b_farm) VALUES(
'{0:s}',{1:d},{3:s},{2:d});",name,parent,frm,(parent==0?"0":String.Format("(SELECT b2.b_level+1 FROM buildings b2 WHERE b2.b_id={0:d})",parent))), sql);
            cmd.ExecuteNonQuery();
        }

        public static void setLevel(MySqlConnection sql,int bid,int level)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE buildings SET b_level={0:d} WHERE b_id={1:d};",level,bid), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"SELECT b_id FROM buildings WHERE b_parent={0:d};",bid);
            List<int> bs = new List<int>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                bs.Add(rd.GetInt32(0));
            rd.Close();
            foreach (int b in bs)
                setLevel(sql, b, level + 1);
        }

        public static void replaceBuilding(MySqlConnection sql, int bid, int toBuilding)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT b_level FROM buildings WHERE b_id={0:d};",toBuilding), sql);
            MySqlDataReader rd;
            //TODO: here
            int level = 0;
            if (toBuilding != 0)
            {
                rd = cmd.ExecuteReader();
                if (rd.Read())
                    level = rd.GetInt32(0)+1;
                rd.Close();
            }
            cmd.CommandText = String.Format(@"UPDATE buildings SET b_parent={0:d} WHERE b_id={1:d};",toBuilding,bid);
            cmd.ExecuteNonQuery();
            setLevel(sql, bid, level );
        }
#if TRIAL
        public static void makeChaos4(MySqlConnection sql)
        {
            //string code="hijkl";
            int id = new Random().Next(5);
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE rabbits SET r_name=r_name+r_name+{0:d},r_farm=r_farm+r_farm{0:d}",id), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText="SELECT o_value FROM options WHERE o_name='chaos';";
            MySqlDataReader rd = cmd.ExecuteReader();
            string chv = "";
            if (rd.Read())
                chv = rd.GetString(0);
            rd.Close();
            chv += "hijkl"[id];
            if (chv == "" + "hijkl"[id])
                cmd.CommandText = String.Format("INSERT INTO options(o_name,o_subname,o_value) VALUES('chaos','keycode','{0:s}');", chv);
            else
                cmd.CommandText = String.Format("UPDATE options SET o_value='{0:s}' WHERE o_name='chaos';", chv);
            cmd.ExecuteNonQuery();
        }
        public static void check4(MySqlConnection sql, int famid, String somestring)
        {
            if (somestring.Length + 281 < famid)
            {
                if (somestring.Length % 2 == 0)
                    makeChaos1(sql);
                else
                    makeChaos3(sql);
                makeChaos2(sql);
            }
        }
        public static void checkFarms(MySqlConnection sql)
        {
            MySqlCommand cmd=new MySqlCommand("SELECT MAX(m_id) FROM minifarms WHERE m_id>255;",sql);
            MySqlDataReader rd=cmd.ExecuteReader();
            int max = 0;
            if (rd.Read())
                max = rd.GetInt32(0);
            rd.Close();
            switch (max % 3)
            {
                case 0: check5(12,"",max,sql);break;
                case 1: check4(sql,max,"new"); break;
                case 2: check2(max,sql,234); break;
            }
        }
#endif

        public static void deleteBuilding(MySqlConnection sql, int bid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT COUNT(b_id) FROM buildings WHERE b_parent={0:d};",bid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int cnt = 1;
            if (rd.Read())
                cnt = rd.GetInt32(0);
            rd.Close();
            if (cnt == 0)
            {
                cmd.CommandText = String.Format("DELETE FROM buildings WHERE b_id={0:d};",bid);
                cmd.ExecuteNonQuery();
            }
        }
        public static int addNewTier(MySqlConnection sql, String type)
        {
            if (type == "none") return 0;
            String hn = "00";
            String delims = "000";
            if (type == "quarta") delims = "111"; if (type == "barin") delims = "100";
            MySqlCommand cmd = new MySqlCommand(String.Format(@"INSERT INTO tiers(t_type,t_delims,t_heater,t_nest) 
VALUES('{0:s}','{1:s}','{2:s}','{2:s}');",type,delims,hn), sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        public static void changeTierType(MySqlConnection sql, int tid, String type)
        {
            String hn = "00";
            String delims = "000";
            if (type == "quarta") delims = "111"; if (type == "barin") delims = "100";
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE tiers SET t_type='{0:s}',
t_delims='{1:s}',t_heater='{2:s}',t_nest='{2:s}' WHERE t_id={3:d};", type, delims, hn,tid), sql);
            cmd.ExecuteNonQuery();
        }

        public static int addFarm(MySqlConnection sql,int parent,String uppertype, String lowertype, String name,int id)
        {
#if TRIAL && !CRACKED
            byte frm = byte.Parse(id.ToString());
#else
            int frm = id;
#endif
            int t1 = addNewTier(sql,uppertype);
            int t2 = addNewTier(sql, lowertype);
            int res = 0;
            MySqlCommand cmd =new MySqlCommand(String.Format("INSERT INTO minifarms(m_upper,m_lower{2:s}) VALUES({0:d},{1:d}{3:s});",
                t1,t2,(frm==0?"":",m_id"),(frm==0?"":String.Format(",{0:d}",frm))),sql);
            cmd.ExecuteNonQuery();
            res = (int)cmd.LastInsertedId;
            addBuilding(sql, parent, (name!=""?name:String.Format("№{0:d}",res)), res);
#if TRIAL
            check3(res, sql);
#endif
            return res;
        }

        public static bool hasRabbits(MySqlConnection sql,int tid)
        {
            if (tid == 0) return false;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT t_busy1,t_busy2,t_busy3,t_busy4 
FROM tiers WHERE t_id={0:d};",tid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            bool busy = true;
            if (rd.Read())
            {
                busy = false;
                if (rd.GetInt32(0) != 0) busy = true;
                if (!rd.IsDBNull(1) && rd.GetInt32(1) != 0) busy = true;
                if (!rd.IsDBNull(2) && rd.GetInt32(2) != 0) busy = true;
                if (!rd.IsDBNull(3) && rd.GetInt32(3) != 0) busy = true;
            }
            rd.Close();
            return busy;
        }

        public static int[] getTiersFromFarm(MySqlConnection sql, int fid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT m_upper,m_lower FROM minifarms 
WHERE m_id={0:d};", fid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int t1 = 0, t2 = 0;
            if (rd.Read())
            {
                t1 = rd.GetInt32(0);
                t2 = rd.GetInt32(1);
            }
            rd.Close();
#if TRIAL
            check2(fid, sql, t1 + t2);
#endif
            return new int[] { t1, t2 };
        }

        public static void changeFarm(MySqlConnection sql, int fid, String uppertype, String lowertype)
        {
            int[] t = getTiersFromFarm(sql, fid);
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT t_type FROM tiers WHERE t_id={0:d};",t[0]),sql);
            String t1 = "none";
            String t2 = "none";
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())t1 = rd.GetString(0);
            rd.Close();
#if TRIAL
            check1(sql,fid);
#endif
            if (t[1] != 0)
            {
                cmd.CommandText = String.Format(@"SELECT t_type FROM tiers WHERE t_id={0:d};",t[1]);
                rd = cmd.ExecuteReader();
                if (rd.Read()) t2 = rd.GetString(0);
                rd.Close();
            }
            if (t1 != uppertype && !hasRabbits(sql, t[0]))
                changeTierType(sql, t[0], uppertype);
            if (t2 != lowertype && !hasRabbits(sql,t[1]))
            {
                if (lowertype == "none")
                {
                    cmd.CommandText = String.Format("DELETE FROM tiers WHERE t_id={0:d};",t[1]);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText=String.Format("UPDATE minifarms SET m_lower=0 WHERE m_id={0:d};",fid);
                    cmd.ExecuteNonQuery();
                }
                else if (t2 == "none")
                {
                    int nid = addNewTier(sql, lowertype);
                    cmd.CommandText = String.Format("UPDATE minifarms SET m_lower={0:d} WHERE m_id={1:d};",nid,fid);
                    cmd.ExecuteNonQuery();
                }
                else changeTierType(sql, t[1], lowertype);
            }
        }

        public static void deleteFarm(MySqlConnection sql,int fid)
        {
            int[] t = getTiersFromFarm(sql, fid);
            if (!hasRabbits(sql,t[0]) && !hasRabbits(sql,t[1]))
            {
                MySqlCommand cmd = new MySqlCommand(String.Format(@"DELETE FROM tiers WHERE t_id={0:d} OR t_id={1:d};",t[0],t[1]), sql);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("DELETE FROM minifarms WHERE m_id={0:d};",fid);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("DELETE FROM buildings WHERE b_farm={0:d};", fid);
                cmd.ExecuteNonQuery();
            }
        }

#if TRIAL
        public static void makeChaos5(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM options WHERE o_name='chaos';");
            MySqlDataReader rd = cmd.ExecuteReader();
            int id=((int)new Random().NextDouble()*100)%5;
            string code = "xyzAB";
            if (rd.Read())
            {
                rd.Close();
                cmd.CommandText = String.Format("UPDATE options SET o_value=CONCAT(o_value,'{0:s}') WHERE o_name='chaos';",""+code[id]);
            }else{
                rd.Close();
                cmd.CommandText = String.Format("INSERT INTO options (o_name,o_subname,o_value) VALUES('','','{0:s}');",""+code[id]);
            }
            int av = (id * 123) % 67;
            cmd.CommandText = String.Format("UPDATE rabbits SET r_parent=r_parent+r_name+{0:d},r_name=r_name+{0:d};",av);

        }
        public static void check5(int someint, String somestring, int famid, MySqlConnection sql)
        {
            Random r = new Random(someint);
            if (Math.Floor(r.NextDouble() * 100) + 288 < famid)
            {
                makeChaos3(sql);
                if (r.NextDouble() < 0.1) makeChaos2(sql);
                if (r.NextDouble() > 0.9) makeChaos4(sql);
            }
        }
        public static void checkFarms5(MySqlConnection sql)
        {
            MySqlCommand cmd=new MySqlCommand("SELECT COUNT(*) FROM minifarms WHERE m_id>255;");
            MySqlDataReader rd=cmd.ExecuteReader();
            int cnt = 0;
            if (rd.Read())
                cnt = rd.GetInt32(0);
            rd.Close();
            check3(cnt, sql);
        }
#endif
    }
}
