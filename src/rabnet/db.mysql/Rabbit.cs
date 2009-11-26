﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    public interface IRabbit : IData
    {
        int id();
        String name();
        String sex();
        int age();
        String breed();
        String weight();
        String status();
        String bgp();
        String N();
        int average();
        int rate();
        String cls();
        String address();
        String notes();
    }


    class Rabbit:IRabbit{
        public int fid;
        public String fname;
        public String fsex;
        public int fage;
        public String fbreed;
        public String fweight;
        public String fstatus;
        public String fbgp;
        public String fN;
        public int faverage;
        public int frate;
        public String fcls;
        public String faddress;
        public string fnotes;
        public Rabbit(int id)
        {
            fid=id;
        }
        int IRabbit.id(){return fid;}
        string IRabbit.name() { return fname; }
        string IRabbit.sex() { return fsex; }
        int IRabbit.age() { return fage; }
        string IRabbit.breed() { return fbreed; }
        string IRabbit.weight() { return fweight; }
        string IRabbit.status() { return fstatus; }
        string IRabbit.bgp() { return fbgp; }
        string IRabbit.N() { return fN; }
        int IRabbit.average() { return faverage; }
        int IRabbit.rate() { return frate; }
        string IRabbit.cls() { return fcls; }
        string IRabbit.address() { return faddress; }
        string IRabbit.notes() { return fnotes; }
    }

    class Rabbits:RabNetDataGetterBase
    {
        public Rabbits(MySqlConnection sql, Filters opts) : base(sql, opts) { }
        public override IData nextItem()
        {
            Rabbit r=new Rabbit(rd.GetInt32(0));
            r.fname = "";
            if (options.safeBool("num"))
                r.fname = r.fid.ToString()+" ";
            bool shr = options.safeBool("shr");
            String nm=rd.GetString("name");
            r.fname = nm;
            //String sun=rd.GetString("surname");
            //String sen=rd.GetString("secname");
            String sx = rd.GetString("r_sex");
            r.fage = rd.GetInt32("age");
            r.fN = "-";
            int cnt = rd.GetInt32("r_group");
            r.faverage = 0;
            if (rd.GetInt32("r_group") > 1)
                r.fN = "[" + rd.GetString("r_group") + "]";
            r.fsex = getRSex(sx);
            if (sx == "void")
            {
                r.fstatus = shr ? "Пдс" : "Подсосные";
                if (Buildings.hasnest(rd.GetString("t_type"),rd.GetInt32("r_area"),rd.GetString("t_nest")))
                    r.fstatus = shr ? "Гнд" : "Гнездовые";
            }
            else if (sx == "male")
            {
                switch (rd.GetInt32("r_status"))
                {
                    case 0: r.fstatus = shr?"Мал":"мальчик"; break;
                    case 1: r.fstatus = shr ? "Кнд" : "кандидат"; break;
                    case 2: r.fstatus = shr ? "Прз" : "производитель"; break;
                }
            }
            else
            {
                if (!rd.IsDBNull(4))
                if (rd.GetInt32("sukr") != 0)
                    r.fsex = "C-" + rd.GetInt32("sukr").ToString();
                if (r.fage < 122)
                    r.fstatus = shr ? "Дев" : "Девочка";
                else
                    r.fstatus = shr ? "Нвс" : "Невеста";
                if (rd.GetInt32("r_status") == 1)
                    r.fstatus = shr ? "Прк" : "Первокролка";
                if (rd.GetInt32("r_status") > 1)
                    r.fstatus = shr ? "Штн" : "Штатная";
                if (!rd.IsDBNull(12))
                {
                    r.fN = "+" + rd.GetString("suckers");
                    r.faverage = rd.GetInt32("aage");
                }
            }
            if (nm == "")
                r.fname += "-" + rd.GetString("r_okrol").ToString();
            r.fbreed = rd.GetString("breed");
            r.fweight = rd.IsDBNull(8) ? "?" : rd.GetString("weight");
            String flg = rd.GetString("r_flags");
            r.fbgp = "";
            if (flg[2] == '1') r.fbgp += "Б";
            if (flg[2] == '2') r.fbgp += "в";
            if (flg[0] != '0') r.fbgp += "ГП";
            if (flg[1] != '0') r.fbgp = "<" + r.fbgp + ">";
            r.frate = rd.GetInt32("r_rate");
            r.fcls=Rabbits.getBon(rd.GetString("r_bon"),shr);
            r.fnotes = rd.GetString("r_notes");
            r.faddress = Buildings.fullRName(rd.GetInt32("r_farm"), rd.GetInt32("r_tier_id"), rd.GetInt32("r_area"),
                rd.GetString("t_type"), rd.GetString("t_delims"), shr, options.safeBool("sht"), options.safeBool("sho"));
            return r;
        }

        public static String addwhere(String str,String adder)
        {
            if (str!="") str+=" AND ";
            str+=adder;
            return str;
        }
        public static String addwhereOr(String str, String adder)
        {
            if (str != "") str += " OR ";
            str += adder;
            return str;
        }

        public String makeWhere()
        {
            String res = "";
            if (options.ContainsKey("sx"))
            {
                String sres="";
                if (options["sx"].Contains("m"))
                    sres = "r_sex='male'";
                if (options["sx"].Contains("f"))
                    sres=addwhereOr(sres, "r_sex='female'");
                if (options["sx"].Contains("v"))
                    sres=addwhereOr(sres, "r_sex='void'");
                res = "("+sres+")";
            }
            if (options.ContainsKey("dt"))
                    res = addwhere(res,"(r_born>=NOW()-INTERVAL " + options["dt"] + " DAY)");
            if (options.ContainsKey("Dt"))
                res = addwhere(res,"(r_born<=NOW()-INTERVAL " + options["Dt"] + " DAY)");
            if (options.ContainsKey("wg"))
                res = addwhere(res,"(weight>=" + options["wg"] + ")");
            if (options.ContainsKey("Wg"))
                res = addwhere(res, "(weight<=" + options["Wg"] + ")");
            if (options.ContainsKey("mt") && options.safeValue("sx", "m").Contains("m"))
            {
                String stat = "";
                if (options["mt"].Contains("b"))
                    stat = "r_status=0";
                if (options["mt"].Contains("c"))
                    stat = addwhereOr(stat,"r_status=1");
                if (options["mt"].Contains("p"))
                    stat = addwhereOr(stat, "r_status=2");
                res = addwhere(res,"(r_sex!='male' OR (r_sex='male' AND ("+stat+")))");
            }
            if (options.ContainsKey("ft") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["ft"].Contains("g"))
                    stat = "r_born>(NOW()-INTERVAL 122 DAYS)";
                if (options["ft"].Contains("b"))
                    stat = addwhereOr(stat, "(r_born<=(NOW()-INTERVAL 122 DAYS) AND r_status=0)");
                if (options["ft"].Contains("f"))
                    stat = addwhereOr(stat, "r_status=1");
                if (options["ft"].Contains("s"))
                    stat = addwhereOr(stat, "r_status>1");
                res = addwhere(res, "(r_sex!='female' OR (r_sex='female' AND (" + stat + ")))");
            }
            if (options.ContainsKey("ms") && options.safeValue("sx", "m").Contains("m"))
            {
                String stat = "";
                if (options["ms"] == "1")
                    stat = "SUBSTR(r_flags,1,1)='0' AND SUBSTR(r_flags,3,1)!='1'";
                if (options["ms"] == "2")
                    stat = "SUBSTR(r_flags,3,1)='1'";
                if (options["ms"] == "3")
                    stat = "SUBSTR(r_flags,1,1)='1'";
                res = addwhere(res, "(r_sex!='male' OR (r_sex='male' AND " + stat + "))");
            }
            if (options.ContainsKey("fs") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["fs"] == "1")
                    stat = "SUBSTR(r_flags,1,1)='0' AND SUBSTR(r_flags,3,1)!='1'";
                if (options["fs"] == "2")
                    stat = "SUBSTR(r_flags,3,1)='1'";
                if (options["fs"] == "3")
                    stat = "SUBSTR(r_flags,1,1)='1'";
                res = addwhere(res, "(r_sex!='female' OR (r_sex='female' AND " + stat + "))");
            }
            if (options.ContainsKey("ku") && options.safeValue("sx", "f").Contains("f"))
                res = addwhere(res,"(r_sex!='female' OR (r_sex='female' AND SUBSTR(r_flags,4,1)="+(int.Parse(options["ku"])+1).ToString()+"))");
            if (options.ContainsKey("nm"))
                res = addwhere(res,"(name like '%" + options["nm"] + "%')");
            if (options.ContainsKey("pr") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["pr"] == "1")
                    stat = "r_event_date IS NULL";
                if (options["pr"] == "2")
                {
                    
                    if (options.ContainsKey("pf") || options.ContainsKey("pf"))
                    {
                        if (options.ContainsKey("pf"))
                            stat = "(r_event_date>=NOW()-INTERVAL "+options["pf"]+" DAY)";
                        if (options.ContainsKey("Pf"))
                            stat = addwhere(stat, "(r_event_date<=NOW()-INTERVAL " + options["Pf"] + " DAY)");
                    }
                    else
                        stat = "r_event_date IS NOT NULL";
                }
                res=addwhere(res,"(r_sex!='female' OR (r_sex='female' AND ("+stat+")))");
            }
            if (res == "") return "";
            return " WHERE "+res;
        }

        public override string getQuery()
        {
/*            COALESCE((SELECT n_name FROM names WHERE n_id=r_name),'') name,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_surname),'') surname,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_secname),'') secname,
*/
            String fld = "b_name";
            if (options.safeBool("shr"))
                fld = "b_short_name";
            return String.Format(@" SELECT * FROM (SELECT r_id, 
rabname(r_id,"+(options.safeBool("dbl")?"2":"1")+@") name,
r_okrol,r_sex,
TO_DAYS(NOW())-TO_DAYS(r_event_date) sukr,
r_event,
TO_DAYS(NOW())-TO_DAYS(r_born) age,
(SELECT " + fld+@" FROM breeds WHERE b_id=r_breed) breed,
(SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
r_status,
r_flags,
r_group,
(SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) suckers,
(SELECT AVG(TO_DAYS(NOW())-TO_DAYS(r2.r_born)) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) aage,
r_rate,
r_bon,
r_farm,
r_area,
r_tier_id,
t_type,
t_delims,
t_nest,
r_children,
r_notes,
r_born,
r_event_date
FROM rabbits,tiers WHERE r_parent=0 AND r_tier=t_id ORDER BY name) c"+makeWhere()+";");
        }
        public override string countQuery()
        {
            return @"SELECT COUNT(*) FROM (SELECT r_sex,r_born,
rabname(r_id,2) name,
(SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
r_status,r_flags,r_event_date
 FROM rabbits WHERE r_parent=0) c"+makeWhere()+";";
        }


        public static String getBon(String bon,bool shr)
        {
            Char fbon='5';
            for (int i = 1; i < bon.Length; i++)
                if (bon[i] < fbon) fbon = bon[i];
            switch (fbon)
            {
                case '1': return "III";
                case '2': return "II";
                case '3': return "I";
                case '4': return (shr ? "Э" : "Элита");
            }
            return (shr ? "-" : "Нет");
        }


        public static TreeData getRabbitGen(int rabbit,MySqlConnection con)
        {
            if (rabbit==0)
                return null;
            MySqlCommand cmd = new MySqlCommand(@"SELECT
rabname(r_id,1) name,
(SELECT n_use FROM names WHERE n_id=r_surname) mother,
(SELECT n_use FROM names WHERE n_id=r_secname) father,
r_bon,TO_DAYS(NOW())-TO_DAYS(r_born) FROM rabbits WHERE r_id=" + rabbit.ToString()+";",con);
            MySqlDataReader rd = cmd.ExecuteReader();
            TreeData res = new TreeData();
            if (rd.Read())
            {
                res.caption = rd.GetString(0) + "," + rd.GetInt32(4).ToString() + "," + Rabbits.getBon(rd.GetString("r_bon"), true);
                int mom=rd.IsDBNull(1)?0:rd.GetInt32(1);
                int dad = rd.IsDBNull(2) ? 0 : rd.GetInt32(2);
                rd.Close();
                TreeData m = getRabbitGen(mom, con);
                TreeData d = getRabbitGen(dad, con);
                if (m == null)
                {
                    m = d;
                    d = null;
                }
                if (m != null)
                {
                   res.items = new TreeData[] {m,d};
                }
            }
            rd.Close();
            return res;
        }

        public static String getRSex(String sx)
        {
            String res = "?";
            if (sx == "male") res = "м";
            if (sx == "female") res = "ж";
            return res;
        }

    }

    public class OneRabbit
    {
        public enum RabbitSex{VOID,MALE,FEMALE};
        public RabbitSex sex;
        public DateTime born;
        public int rate;
        public int id;
        public bool defect;
        public bool gp;
        public bool gr;
        public bool spec;
        public int name;
        public int surname;
        public int secname;
        public string address;
        public int group;
        public int breed;
        public int zone;
        public String notes;
        public OneRabbit(int id,string sx,DateTime bd,int rt,string flg,int nm,int sur,int sec,string adr,int grp,int brd,int zn,String nts)
        {
            this.id=id;
            sex=RabbitSex.VOID;
            if (sx=="male") sex=RabbitSex.MALE;
            if (sx == "female") sex = RabbitSex.FEMALE;
            born = bd;
            rate = rt;
            name = nm;
            gp = flg[0] == '1';
            defect = flg[2] == '1';
            spec = flg[2] == '2';
            surname = sur;
            secname = sec;
            address = Buildings.fullPlaceName(adr,false,true,true);
            group = grp;
            breed = brd;
            zone = zn;
            notes = nts;
        }
    }

    public class RabbitGetter
    {
        public static OneRabbit GetRabbit(MySqlConnection con, int rid)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT r_id,r_sex,r_born,r_flags,r_breed,r_zone,
r_name,r_surname,r_secname,
rabplace(r_id) address,r_group,r_notes,
r_rate
FROM rabbits WHERE r_id=" + rid.ToString()+";",con);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (!rd.Read())
            {
                rd.Close();
                return null;
            }
            OneRabbit r=new OneRabbit(rd.GetInt32("r_id"),rd.GetString("r_sex"),rd.GetDateTime("r_born"),rd.GetInt32("r_rate"),
                rd.GetString("r_flags"),rd.GetInt32("r_name"),rd.GetInt32("r_surname"),rd.GetInt32("r_secname"),
                rd.GetString("address"),rd.GetInt32("r_group"),rd.GetInt32("r_breed"),rd.GetInt32("r_zone"),
                rd.GetString("r_notes"));

            rd.Close();
            return r;
        }

        public static void SetRabbit(MySqlConnection con, OneRabbit r)
        {
        }
    }
}
