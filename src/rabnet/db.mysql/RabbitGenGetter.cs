﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Drawing;
using rabnet;

namespace db.mysql
{		
	public class RabbitGenGetter
	{
		public static RabbitGen GetRabbitGen(MySqlConnection sql, int rid)
		{
			if (rid == 0) return null;

            const string QUERY = @"SELECT	
        r_mother, 
        r_father, 
		r_sex, 
		(select n_name from names where n_id=r_name) name, 
		(select n_surname from names where n_id=r_surname) surname, 
		(select n_surname from names where n_id=r_secname) secname,
		(select b_color from breeds where b_id=r_breed) b_color,
		r_breed,
		(select b_name from breeds where b_id=r_breed) b_name
FROM {0:s}
WHERE r_id={1:d}
LIMIT 1;";
			Boolean IsDead = false;
            MySqlCommand cmd = new MySqlCommand(String.Format(QUERY,"rabbits", rid), sql);
			MySqlDataReader rd = cmd.ExecuteReader();

			if (!rd.HasRows)
			{
				IsDead = true;
				rd.Close();
				rd.Dispose();
				cmd = new MySqlCommand(String.Format(QUERY,"dead", rid), sql);
				rd = cmd.ExecuteReader();
			}

			RabbitGen r = new RabbitGen();
			r.ID = rid;

			if (rd.Read())
			{
				r.FatherId = rd.GetInt32("r_father"); //0
				r.MotherId = rd.GetInt32("r_mother"); //1
				string sx = rd.GetString("r_sex"); //2
                r.Sex = Rabbit.SexType.VOID;
				if (sx == "male")
				{
                    r.Sex = Rabbit.SexType.MALE;
				}
				if (sx == "female")
				{

                    r.Sex = Rabbit.SexType.FEMALE;
				}

                r.Name = rd.IsDBNull(rd.GetOrdinal("name")) ? "" : rd.GetString("name"); //3
                r.Surname = rd.IsDBNull(rd.GetOrdinal("surname")) ? "" : rd.GetString("surname"); //4
                r.Secname = rd.IsDBNull(rd.GetOrdinal("secname")) ? "" : rd.GetString("secname"); //5
                r.BreedColorName = rd.IsDBNull(rd.GetOrdinal("b_color")) ? "" : rd.GetString("b_color"); //5

				r.IsDead = IsDead;

				int res;

				if (int.TryParse(r.BreedColorName, System.Globalization.NumberStyles.HexNumber, null, out res))
				{
					r.BreedColor = Color.FromArgb(res);
				}
				else
				{
					r.BreedColor = Color.FromName(r.BreedColorName);
				}
				r.BreedId = rd.GetInt32("r_breed"); //6
                r.BreedName = rd.IsDBNull(rd.GetOrdinal("b_name")) ? "" : rd.GetString("b_name"); //7

			}
			else
			{
				r = null;
			}
//			rd.HasRows
			rd.Close();
			if (r != null)
			{
				getRabbitPriplodK(sql, ref r);
				getRabbitRodK(sql, ref r);
			}
			return r;
		}

		public static void getRabbitPriplodK(MySqlConnection sql, ref RabbitGen rabbit)
		{
			string f = "f_rabid";
            if (rabbit.Sex == Rabbit.SexType.MALE)
			{
				f = "f_partner";
			}


			MySqlCommand cmd = new MySqlCommand(String.Format(@"	SELECT coalesce(sum(f_children)/(sum(f_times)-(	select count(f_state) 
																													from fucks 
																													where	{1}={0:d} 
																															and f_state='sukrol')),0) k  
																	FROM fucks 
																	where {1}={0:d};", rabbit.ID, f), sql);
			MySqlDataReader rd = cmd.ExecuteReader();
			if (rd.Read())
			{
				rabbit.PriplodK = rd.GetFloat("k");

			}
			rd.Close();
		}
		
		public static void getRabbitRodK(MySqlConnection sql, ref RabbitGen rabbit)
		{
            if (rabbit.Sex == Rabbit.SexType.FEMALE)
			{
				MySqlCommand cmd = new MySqlCommand(String.Format(@"	select coalesce((sum(f_children)-sum(f_killed)+sum(f_added))/(sum(f_children)+sum(f_added)),0) k
																		from fucks 
																		where f_rabid={0:d};", rabbit.ID), sql);
				MySqlDataReader rd = cmd.ExecuteReader();
				if (rd.Read())
				{
					rabbit.RodK = rd.GetFloat("k");
				}
				rd.Close();
			}
            if (rabbit.Sex == Rabbit.SexType.MALE)
			{
				MySqlCommand cmd = new MySqlCommand(String.Format(@"	select	(select count(f_state) from fucks where f_partner={0:d} and f_state='okrol' and f_times=1) o,
																				(select count(f_state) from fucks where f_partner={0:d} and f_state='proholost' and f_times=1) p;", rabbit.ID), sql);
				MySqlDataReader rd = cmd.ExecuteReader();
				if (rd.Read())
				{
					float o = rd.GetFloat("o");
					float p = rd.GetFloat("p");
					if (p + 0 == 0)
					{
						rabbit.RodK = 0;
					}
					else
					{
						rabbit.RodK = o / (o + p);
					}
				}
				rd.Close();
			}
		}
		
        public static Dictionary<int, Color> getBreedColors(MySqlConnection sql)
		{
			Dictionary<int, Color> Dict = new Dictionary<int, Color>();
			Color cl;
			string cl_name;
			MySqlCommand cmd = new MySqlCommand(@"	select b_id, b_color from breeds;", sql);
			MySqlDataReader rd = cmd.ExecuteReader();
			while (rd.Read())
			{

				cl_name = rd.IsDBNull(1) ? "" : rd.GetString("b_color");

				int res;

				if (int.TryParse(cl_name, System.Globalization.NumberStyles.HexNumber, null, out res))
				{
					cl = Color.FromArgb(res);
				}
				else
				{
					cl = Color.FromName(cl_name);
				}
				Dict.Add(rd.GetInt32(0), cl);
			}
			rd.Close();
			return Dict;
		}

        /// <summary>
        /// Получает родословную кролика с заданным rabId
        /// </summary>
        /// <param name="rabId">ID кролика</param>
        /// <param name="con"></param>
        /// <param name="lineage">Стэк родословной для предотвращения рекурсии</param>
        /// <returns></returns>
        private static RabTreeData getRabbitGenTree(MySqlConnection con, int rabId, Stack<int> lineage)//, int level)
        {
            if (rabId == 0) return null;
            //проверка на рекурсию, которая могла возникнуть после конвертации из старой mia-файла                        
            if (lineage.Count > 700)
            {
                //_logger.Warn("cnt:" + lineage.Count.ToString() + " we have suspect infinity inheritance loop: " + String.Join(",", Array.ConvertAll<int, string>(lineage.ToArray(), new Converter<int, string>(convIntToString))));
                return null;
            }
            lineage.Push(rabId);
            const string query = @"SELECT
        r_id,
        r_name,
        {0:s}(r_id,1) name,
        r_mother,
        r_father,
        r_bon,
        r_breed,
        b_short_name,
        TO_DAYS(NOW())-TO_DAYS(r_born) age    
    FROM {1:s} 
    INNER JOIN breeds ON b_id=r_breed
    WHERE r_id={2:d} LIMIT 1;";

            MySqlCommand cmd = new MySqlCommand(String.Format(query,"rabname","rabbits",rabId), con);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (!rd.HasRows)
            {
                rd.Close();
                cmd.CommandText = String.Format(query,"deadname","dead",rabId);
                rd = cmd.ExecuteReader();
            }
            RabTreeData res = new RabTreeData();
            if (rd.Read())
            {
                res.ID = rd.GetInt32("r_id"); 
                res.Name = rd.GetString("name");
                res.NameId = rd.GetInt32("r_name");
                res.Age = rd.GetInt32("age");
                res.Bon = Rabbit.GetFBon(rd.GetString("r_bon"), true);
                res.BreedId = rd.GetInt32("r_breed");
                res.BreedShortName = rd.GetString("b_short_name");
                int mom = rd.IsDBNull(rd.GetOrdinal("r_mother")) ? 0 : rd.GetInt32("r_mother");
                int dad = rd.IsDBNull(rd.GetOrdinal("r_father")) ? 0 : rd.GetInt32("r_father");
                rd.Close();
                RabTreeData m = getRabbitGenTree(con, mom, lineage);
                RabTreeData d = getRabbitGenTree(con, dad, lineage);
                if (m == null)
                {
                    m = d;
                    d = null;
                }
                if (m != null)
                {
                    res.Parents = new List<RabTreeData>();
                    res.Parents.Add(m);
                    res.Parents.Add(d);
                }
            }
            rd.Close();
            return res;
        }

        public static RabTreeData GetRabbitGenTree(MySqlConnection con,int rabbit)//, int level)
        {
            Stack<int> lineage = new Stack<int>();
            return getRabbitGenTree(con, rabbit, lineage);
        }
     
        /// <summary>
        /// Получает ID (Genesis) набора генов (Genom).
        /// </summary>
        /// <param name="gens">Строка генов. Например "100 201 322 500 600"</param>
        /// <returns></returns>
        public static int MakeGenesis(MySqlConnection sql, String gens)
        {
            if (gens == "") return 0;

            ///гакой набор генов может быть уже в базе, ищем его
            MySqlCommand c = new MySqlCommand("", sql);
            c.CommandText = "SELECT g_id FROM genesis WHERE g_key=MD5('" + gens + "');";
            MySqlDataReader rd = c.ExecuteReader();
            int res = 0;
            if (rd.HasRows)
            {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            }
            else ///если такого набора генов в базе нет
            {
                rd.Close();
                c.CommandText = "INSERT INTO genesis(g_notes) VALUES('');";
                c.ExecuteNonQuery();
                res = (int)c.LastInsertedId;
                String[] gen = gens.Split(' ');
                foreach (string g in gen)
                {
                    c.CommandText = String.Format("INSERT INTO genoms(g_id,g_genom) VALUES({0:d},{1:s});", res, g);
                    c.ExecuteNonQuery();
                }
                c.CommandText = String.Format(@"UPDATE genesis SET g_key=(SELECT MD5(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ')) 
            FROM genoms WHERE g_id={0:d}) WHERE g_id={0:d};", res);
                c.ExecuteNonQuery();
            }
            return res;
        }
        public static int MakeCommonGenesis(MySqlConnection sql, int r1, int r2)
        {
            return MakeGenesis(sql, CombineGenoms(sql, r1, r2));
        }
        public static int MakeCommonGenesis(MySqlConnection sql, String g1, String g2, int zonegen)
        {
            if (g1 == "") g1 = zonegen.ToString();
            if (g2 == "") g2 = g1;
            return MakeGenesis(sql, CombineGenoms(g1, g2));
        }

        public static String GetGenoms(MySqlConnection sql, int rabid)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms 
        WHERE g_id=(SELECT r_genesis FROM rabbits WHERE r_id=" + rabid.ToString() + ");", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            String res = "";
            if (rd.Read())
                res = rd.GetString(0);
            rd.Close();
            return res;
        }

        public static String CombineGenoms(String g1, String g2)
        {
            String res = "";
            string[] s1 = g1.Split(' ');
            string[] s2 = g2.Split(' ');
            List<int> gn = new List<int>();
            injectGenoms(gn, s1);
            injectGenoms(gn, s2);
            //foreach (string s in s1)
            //{
            //    int g = int.Parse(s); 
            //    int pos = 0;
            //    for (int i = 0; i < gn.Count && pos > -1; i++)
            //    {
            //        if (g == gn[i]) pos = -1;
            //        if (g < gn[i]) pos++;
            //    }
            //    if (pos > -1) gn.Insert(pos, g);
            //}
            //foreach (string s in s2)
            //{
            //    int g = int.Parse(s); 
            //    int pos = 0;
            //    for (int i = 0; i < gn.Count && pos > -1; i++)
            //    {
            //        if (g == gn[i]) pos = -1;
            //        if (g < gn[i]) pos++;
            //    }
            //    if (pos > -1) gn.Insert(pos, g);
            //}
            foreach (int i in gn)
                res += i.ToString() + " ";
            return res.Trim();
        }
        public static String CombineGenoms(MySqlConnection sql, int r1, int r2)
        {
            return CombineGenoms(GetGenoms(sql, r1), GetGenoms(sql, r2));
        }

        /// <summary>
        /// Обеспечивает смешивание номеров генов по порядку.
        /// </summary>
        /// <param name="gn">Полученный гены в результате смешения</param>
        /// <param name="gensArr">Гены одного из партнеров</param>
        private static void injectGenoms(List<int> gn, string[] gensArr)
        {
            foreach (string s in gensArr)
            {
                int g = int.Parse(s);
                int pos = 0;
                for (int i = 0; i < gn.Count && pos > -1; i++)
                {
                    if (g == gn[i]) pos = -1;
                    if (g < gn[i]) pos++;
                }
                if (pos > -1) gn.Insert(pos, g);
            }
        }

        //private static String convIntToString(int i)
        //{
        //    return i.ToString();
        //}

	}
}