﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace mia_conv
{
    partial class MDCreator
    {
        private int curRabbit=1;
        public int getNameId(ushort key, ushort sex)
        {
            if (sex==0)
                return 0;
            if (key == 0) return 0;
            int res = 0;
            MFRabNames rn = (sex == 1 ? mia.male_names : mia.female_names);
            for (int i=0;(i<rn.rabnames.Count) && (res==0);i++)
            {
                if (rn.rabnames[i].key.value()==key)
                    res=rn.rabnames[i].key.tag;
                if (rn.rabnames[i].surkey.value()==key)
                    res=rn.rabnames[i].key.tag;
            }
            return res;
        }

        public int getTier(int farm, int level)
        {

            c.CommandText = "SELECT " + (level == 1 ? "m_lower" : "m_upper") + " FROM minifarms WHERE m_id=" + farm.ToString() + ";";
            MySqlDataReader rd = c.ExecuteReader();
            rd.Read();
            int res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public void settieruser(int tier,int busy,int rabbit)
        {
            c.CommandText="UPDATE tiers SET t_busy"+(busy+1).ToString()+"="+rabbit.ToString()+
                " WHERE t_id="+tier.ToString()+";";
            c.ExecuteNonQuery();
        }
        public void setnameuser(int nameid,int rabbit)
        {
            c.CommandText="UPDATE names SET n_use="+rabbit.ToString()+" WHERE n_id="+nameid.ToString()+";";
            c.ExecuteNonQuery();
        }

        public int makeGenesis(List<ushort> gen)
        {
            String s = "";
            for (int i = 0; i < gen.Count;i++ )
            {
                s += gen[i].ToString();
                if (i < gen.Count - 1) s += " ";
            }
            c.CommandText = "SELECT g_id FROM genesis WHERE g_key=MD5('"+s+"');";
            MySqlDataReader rd = c.ExecuteReader();
            int res = 0;
            if (rd.HasRows)
            {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            }
            else
            {
                rd.Close();
                c.CommandText = "INSERT INTO genesis(g_notes) VALUES('');";
                c.ExecuteNonQuery();
                res = (int)c.LastInsertedId;
                foreach (ushort g in gen)
                {
                    c.CommandText = "INSERT INTO genoms(g_id,g_genom) VALUES("+res.ToString()+","+g.ToString()+");";
                    c.ExecuteNonQuery();
                }
                c.CommandText = "UPDATE genesis SET g_key=(SELECT MD5(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ')) FROM genoms WHERE g_id="+res.ToString()+") WHERE g_id=" + res.ToString() + ";";
                c.ExecuteNonQuery();
            }
            return res;
        }

        public int makeWorker(String wname)
        {
            if (wname == "") return 0;
            c.CommandText = "SELECT w_id FROM workers WHERE w_name='" + wname + "';";
            MySqlDataReader rd = c.ExecuteReader();
            int res=0;
            if (rd.HasRows)
            {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            }
            else
            {
                rd.Close();
                c.CommandText = "INSERT INTO workers(w_name) VALUES('"+wname+"');";
                c.ExecuteNonQuery();
                res = (int)c.LastInsertedId;
            }
            return res;
        }

        public void fillRabWeight(Rabbit r,int crab)
        {
            for (int i = 0; i < r.weights.Count; i++)
            {
                ushort weight=(ushort)(r.weights[i] & 0xFFFF);
                ushort dt=(ushort)((r.weights[i] >> 16)& 0xFFFF);
                DateTime sdt = (new DateTime(1899, 12, 30)).AddDays(dt);
                c.CommandText = String.Format("INSERT INTO weights(w_rabid,w_date,w_weight) VALUES({0:d},{1:s},{2:d});",
                    crab,convdt(sdt),weight);
                c.ExecuteNonQuery();
            }
        }

        public void fillRabFuckers(Rabbit r, int crab)
        {
            foreach (Fucker f in r.female.fuckers)
            {
                int genesis=0;
                int link=0;
                if (f.live.value()==1)
                {
                    link=getNameId((ushort)f.name_key.value(),1);
                }else{
                    c.CommandText="SELECT n_id FROM names WHERE n_sex='male' AND n_name='"+f.name.value()+"';";
                    MySqlDataReader rd=c.ExecuteReader();
                    rd.Read();
                    link=rd.GetInt32(0);
                    rd.Close();
                    genesis=makeGenesis(f.genesis);
                }
                c.CommandText = String.Format("INSERT INTO fuckers(f_rabid,f_live,f_link,f_genesis,f_breed,f_fucks,f_children,f_lastfuck) " +
                    "VALUES({0:d},{1:d},{2:d},{3:d},{4:d},{5:d},{6:d},{7:d});",crab,f.live.value(),link,genesis,findbreed((int)f.breed.value()),f.fucks.value(),
                    f.children.value(),f.my_fuck_is_last.value());
                c.ExecuteNonQuery();
            }
        }

        public void fillRabbit(Rabbit r,int parent,bool dead)
        {
            Application.DoEvents();
            String cmd = "INSERT INTO "+(dead?"dead":"rabbits")+"(r_sex,r_parent,r_bon,r_number,r_unique,r_name,r_surname," +
                "r_secname,r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,"+
                "r_born,r_status,r_last_fuck_okrol,r_genesis";
            String sex = "void";
            if (r.sex.value() == 1) sex = "male"; if (r.sex.value() == 2) sex = "female";
            String bon = String.Format("{0:D1}{1:D1}{2:D1}{3:D1}{4:D1}", r.bon.manual.value(), r.bon.weight.value(), r.bon.body.value(), r.bon.hair.value(), r.bon.color.value());
            String vals=String.Format("VALUES('{0:s}',{1:d},'{2:s}',{3:d},{4:d}",sex,parent,bon,r.number.value(),r.unique.value());
            int name = getNameId((ushort)r.namekey.value(), (ushort)r.sex.value());
            int surname = getNameId((ushort)r.surkey.value(), 2);
            int secname = getNameId((ushort)r.pathkey.value(), 1);
            vals += String.Format(",{0:d},{1:d},{2:d},'{3:s}',{4:d}", name, surname, secname,r.notes.value(),r.okrol_num.value());
            int tier=getTier((int)r.where.value(),(int)r.tier_id.value());
            settieruser(tier,(int)r.area.value(),curRabbit);
            if (name != 0)
                setnameuser(name, curRabbit);
            String flags = String.Format("{0:D1}{1:D1}{2:D1}",r.butcher.value(),r.risk.value(),r.multi.value());
            if (r.sex.value() == 2)
                flags += String.Format("{0:D1}{1:D1}",r.female.no_kuk.value(),r.female.no_lact.value());
            else
                flags += "00";
            vals += String.Format(",{0:d},{1:d},{2:d},{3:d}",r.where.value(),r.tier_id.value(),tier,r.area.value());
            vals += String.Format(",{0:d},{1:d},{2:d},'{3:s}',{4:d}", r.rate.value(), r.group.value(),
                findbreed((int)r.breed.value()),flags,r.zone.value());
            int status=0;
            String lfo="NULL";
            if (r.sex.value()==1) {
                status=(int)r.status.value();
                lfo=convdt(r.lastfuck.value());
            }
            if (r.sex.value()==2) {
                status=(int)r.female.borns.value();
                lfo=convdt(r.female.last_okrol.value());
            }
            vals += String.Format(",{0:s},{1:d},{2:s},{3:d}", convdt(r.borndate.value()), status, lfo,makeGenesis(r.genesis));
            if (r.sex.value() == 2)
            {
                cmd += ",r_children,r_event,r_event_date,r_lost_babies,r_overall_babies,r_worker";
                String ev="none";
                switch (r.female.ev_type.value())
                {
                    case 1:ev="sluchka";break;
                    case 2:ev="vyazka";break;
                    case 3:ev="kuk";break;
                }
                vals+=String.Format(",{0:d},'{1:s}',{2:s},{3:d},{4:d},{5:d}",r.female.child_count.value(),ev,
                    convdt(r.female.ev_date.value()),r.female.lost_babies.value(),r.female.overall_babies.value(),
                    makeWorker(r.female.worker.value()));
            }
            c.CommandText = cmd + ") " + vals + ");";
            c.ExecuteNonQuery();
            int crab = (int)c.LastInsertedId;
            curRabbit = crab + 1;
            if (dead)
                return;
            fillRabWeight(r, crab);
            if (r.sex.value() == 2)
            {
                fillRabFuckers(r, crab);
                foreach (Rabbit xr in r.female.suckers.rabbits)
                {
                    fillRabbit(xr, crab, false);
                }
            }
        }

        public void normalizeFuckers()
        {
            c.CommandText = "UPDATE fuckers SET f_link=(SELECT n_use FROM names WHERE n_id=fuckers.f_link) WHERE f_live=1;";
            c.ExecuteNonQuery();
            c.CommandText = "UPDATE fuckers SET f_genesis=(SELECT r_genesis FROM rabbits WHERE r_id=fuckers.f_link) WHERE f_live=1";
            c.ExecuteNonQuery();
        }

        public void fillRabbits()
        {
            debug("fill rabbits");
            int cnt=mia.rabbits.rabbits.Count;
            int i=0;
            foreach (Rabbit r in mia.rabbits.rabbits)
            {
                i++;
                mia.setpb(cnt/100*i);
                fillRabbit(r,0,false);
            }
            debug("normalizing fuckers");
            normalizeFuckers();
        }
        public void fillDead()
        {
            debug("fill dead");
            foreach (Rabbit r in mia.arcform.dead.rabbits)
            {
                fillRabbit(r, 0, true);
            }
        }

    }
}