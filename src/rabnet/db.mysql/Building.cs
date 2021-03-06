﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
//using System.Windows.Forms;
using rabnet;

namespace db.mysql
{

    public class Buildings : RabNetDataGetterBase
    {
        internal Buildings(MySqlConnection sql, Filters filters) : base(sql, filters) { }

        internal static Building GetBuilding(MySqlDataReader rd, bool shr, bool rabbits)
        {
            int id = rd.GetInt32("t_id");
            int farm = rd.GetInt32("m_id");
            int tid = 0;
            if (rd.GetInt32("m_lower") != 0) {
                if (rd.GetInt32("m_upper") == id) {
                    tid = 1;
                } else {
                    tid = 2;
                }
            }
            BuildingType tp = Building.ParseType(rd.GetString("t_type"));
            String dl = rd.GetString("t_delims");
            Building building = new Building(id, farm, tid, tp, dl, rd.GetString("t_notes"), (rd.GetInt32("t_repair") != 0), rd.GetString("t_nest"), rd.GetString("t_heater"));
            RabInBuild[] bus = new RabInBuild[building.Sections];

            for (int i = 0; i < building.Sections; i++) {
                //ars.Add((tid == 0 ? "" : (tid == 1 ? "^" : "-")) + Building.GetSecRus(tp, i, dl));
                //deps.Add(Building.GetDescrRus(tp, shr, i, dl));
                int rn = rd.IsDBNull(rd.GetOrdinal("t_busy" + (i + 1).ToString())) ? -1 : rd.GetInt32("t_busy" + (i + 1).ToString());
                string name = rabbits ? rd.GetString("r" + (i + 1).ToString()) : "";
                building.Busy[i] = new RabInBuild(rn, name);
            }
            return building;
        }

        public override IData NextItem()
        {
            try {
                bool shr = options.safeBool(Filters.SHORT);
                return GetBuilding(_rd, shr, true);
            } catch (Exception ex) {
                _logger.Error("building exception ", ex);
                throw ex;
            }
        }

        private String makeWhere()
        {
            String res = "";
            if (options.ContainsKey(Filters.FARM)) {
                String sres = "";
                if (options[Filters.FARM] == "1") {
                    sres = "t_busy1<>0 OR t_busy2<>0 OR t_busy3<>0 OR t_busy4<>0";
                } else {
                    sres = "t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0";
                }
                res = "(" + sres + ")";
            }

            if (options.ContainsKey(Filters.TIER)) {
                String sres = "";
                if (options.safeValue(Filters.TIER).Contains("v")) sres = addWhereOr(sres, "t_type='vertep'");
                if (options.safeValue(Filters.TIER).Contains("u")) sres = addWhereOr(sres, "t_type='jurta'");
                if (options.safeValue(Filters.TIER).Contains("q")) sres = addWhereOr(sres, "t_type='quarta'");
                if (options.safeValue(Filters.TIER).Contains("b")) sres = addWhereOr(sres, "t_type='barin'");
                if (options.safeValue(Filters.TIER).Contains("k")) sres = addWhereOr(sres, "t_type='female'");
                if (options.safeValue(Filters.TIER).Contains("d")) sres = addWhereOr(sres, "t_type='dfemale'");
                if (options.safeValue(Filters.TIER).Contains("x")) sres = addWhereOr(sres, "t_type='complex'");
                if (options.safeValue(Filters.TIER).Contains("h")) sres = addWhereOr(sres, "t_type='cabin'");
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if (options.ContainsKey(Filters.HETER)) {
                String sres = "";
                if (options[Filters.HETER] == "1") sres = "t_heater='0' OR t_heater='00'";
                if (options[Filters.HETER] == "2") sres = "t_heater='1' OR t_heater='3'";
                if (options[Filters.HETER] == "3") sres = "t_heater='1'";
                if (options[Filters.HETER] == "4") sres = "t_heater='3'";
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if (options.ContainsKey(Filters.NEST_IN)) {
                String sres = "";
                if (options[Filters.NEST_IN] == "1")
                    sres = "t_nest<>'1'";
                else sres = "t_nest='1'";
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if (res != "") {
                res = "AND " + res;
            }

            return res;

        }
        /// <summary>
        /// Строка запроса для вкладки "Строения"
        /// </summary>
        /// <returns>Возвращает запрос, который выполняется объектом класса RabNetDataGetterBase.</returns>
        protected override string getQuery()
        {
            int nm = options.safeBool(Filters.DBL_SURNAME) ? 2 : 1;
            return String.Format(@"SELECT 
    t_id,
    Coalesce(m_upper,0) AS m_upper, 
    Coalesce(m_lower,0) AS m_lower,
    m_id, t_type, t_delims, t_nest, t_heater,
    t_repair,
    t_notes,
    t_busy1, t_busy2, t_busy3, t_busy4,
    rabname(t_busy1,{0:d}) r1, 
    rabname(t_busy2,{0:d}) r2,
    rabname(t_busy3,{0:d}) r3,
    rabname(t_busy4,{0:d}) r4
FROM minifarms, tiers 
WHERE (m_upper = t_id OR m_lower = t_id) {1:s} 
ORDER BY m_id;", nm, makeWhere());
        }

        protected override string countQuery()
        {
            return @"SELECT 
    COUNT(t_id), 
    COUNT( DISTINCT m_id ) 
FROM minifarms, tiers 
WHERE (m_upper=t_id OR m_lower=t_id) " + makeWhere() + ";";
        }

        internal static BldTreeData getTree(int parent, MySqlConnection con, BldTreeData par)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT b_id, b_name, Coalesce(b_farm,0) AS b_farm FROM buildings WHERE b_parent=" + parent.ToString() + " ORDER BY b_farm ASC;", con);
            MySqlDataReader rd = cmd.ExecuteReader();
            BldTreeData res = par;
            if (par == null) {
                res = new BldTreeData(0, 0, "farm");
            }

            List<BldTreeData> lst = new List<BldTreeData>();
            while (rd.Read()) {
                int frm = DBHelper.GetNullableInt(rd, "b_farm");
                String nm = rd.GetString("b_name");
                BldTreeData dt = new BldTreeData(rd.GetInt32("b_id"), frm, (frm == 0 ? nm : "№" + Building.Format(nm.Remove(0, 1))), res.Path);
                lst.Add(dt);
            }
            rd.Close();

            if (lst.Count > 0) {
                foreach (BldTreeData td in lst) {
                    getTree(td.ID, con, td);
                }
                res.ChildNodes = lst;
            }
            return res;
        }

        internal static Building getTier(int tier, MySqlConnection con)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT 
    t_id, 
    Coalesce(m_upper,0) AS m_upper, 
    Coalesce(m_lower,0) AS m_lower,
    m_id, t_type, t_delims, t_nest, t_heater,
    t_repair,
    COALESCE(t_notes,'') t_notes, t_busy1, t_busy2, t_busy3, t_busy4,
    rabname(t_busy1,1) r1, 
    rabname(t_busy2,1) r2, 
    rabname(t_busy3,1) r3, 
    rabname(t_busy4,1) r4
FROM minifarms, tiers 
WHERE (m_upper = t_id OR m_lower = t_id) AND t_id=" + tier.ToString(), con);
            MySqlDataReader rd = cmd.ExecuteReader();
            Building b = null;
            if (rd.Read()) {
                b = GetBuilding(rd, false, true);
            }
            rd.Close();
            return b;
        }

        internal static BuildingList getBuildings(MySqlConnection sql, Filters f)
        {
            BuildingList bld = new BuildingList();
            String type = "";
            if (f.safeValue("tp") != "") {
                type = "t_type='" + f.safeValue("tp") + "' AND ";
            }
            if (f.ContainsKey("nest")) {
                type = String.Format("(t_type='{0}' OR t_type='{1}' OR t_type='{2}') AND",
                    Building.GetName(BuildingType.Jurta),
                    Building.GetName(BuildingType.Female),
                    Building.GetName(BuildingType.DualFemale));
            }
            String busy = "";
            if (f.ContainsKey(Filters.FREE)) {
                busy = "((" + type + "(t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0))";
                if (f.safeInt("rcnt") > 0) {
                    for (int i = 0; i < f.safeInt("rcnt"); i++) {
                        int r = f.safeInt("r" + i.ToString());
                        if (r > 0) {
                            busy += String.Format(" OR (t_busy1={0:d} OR t_busy2={0:d} OR t_busy3={0:d} OR t_busy4={0:d})", r);
                        }
                    }
                }
                busy += ")";
            }
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT 
    t_id, 
    Coalesce(m_upper,0) AS m_upper, 
    Coalesce(m_lower,0) AS m_lower, 
    m_id, t_type, t_delims, t_nest, t_heater, t_repair, t_notes, 
    t_busy1, t_busy2, t_busy3, t_busy4 
FROM minifarms, tiers 
WHERE (m_upper = t_id OR m_lower = t_id) AND t_repair = 0 {0:s} 
ORDER BY m_id;", busy != "" ? "AND " + busy : ""), sql);
            _logger.Debug("free Buildings cmd:" + cmd.CommandText);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                bld.Add(GetBuilding(rd, false, false) as Building);
            }
            rd.Close();

            return bld;
        }

        internal static void updateBuilding(Building b, MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE tiers SET t_repair={1:d}, t_delims='{2:s}', t_heater='{3:s}', t_nest='{4:s}', t_notes=@notes WHERE t_id={0:d};",
                b.ID, b.Repair ? 1 : 0, b.Delims, b.Heaters, b.Nests), sql);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@notes", String.Join("|", b.Notes));
            cmd.ExecuteNonQuery();
        }

        internal static void setBuildingName(MySqlConnection sql, int bid, String name)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE buildings SET b_name='{0:s}' WHERE b_id={1:d};", name, bid), sql);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Добавляет ветку в дерево строений
        /// </summary>
        /// <param name="parent">Родитель</param>
        /// <param name="name">Имя ветки</param>
        /// <param name="farm">Номер фермы</param>
        internal static void addBuilding(MySqlConnection sql, int parent, String name, int farm)
        {
            int frm = farm;
            MySqlCommand cmd = new MySqlCommand(
                String.Format(
                    "INSERT INTO buildings(b_name, b_parent, b_level, b_farm) VALUES('{0}', {1}, {2}, {3});",
                    name, 
                    parent, 
                    (parent == 0 ? "0" : String.Format("(SELECT b2.b_level+1 FROM buildings b2 WHERE b2.b_id={0:d})", parent)),
                    DBHelper.Nullable(frm)
                ), sql
            );
            _logger.Debug("db.mysql.Building: " + cmd.CommandText);
            cmd.ExecuteNonQuery();
        }

        internal static void setLevel(MySqlConnection sql, int bid, int level)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE buildings SET b_level={0:d} WHERE b_id={1:d};", level, bid), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"SELECT b_id FROM buildings WHERE b_parent={0:d};", bid);
            List<int> bs = new List<int>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                bs.Add(rd.GetInt32(0));
            }
            rd.Close();
            foreach (int b in bs) {
                setLevel(sql, b, level + 1);
            }
        }

        internal static void replaceBuilding(MySqlConnection sql, int bid, int toBuilding)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT b_level FROM buildings WHERE b_id={0:d};", toBuilding), sql);
            MySqlDataReader rd;
            int level = 0;
            if (toBuilding != 0) {
                rd = cmd.ExecuteReader();
                if (rd.Read()) {
                    level = rd.GetInt32(0) + 1;
                }
                rd.Close();
            }
            cmd.CommandText = String.Format(@"UPDATE buildings SET b_parent={0:d} WHERE b_id={1:d};", toBuilding, bid);
            cmd.ExecuteNonQuery();
            setLevel(sql, bid, level);
        }

        internal static void deleteBuilding(MySqlConnection sql, int bid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT COUNT(b_id) FROM buildings WHERE b_parent={0:d};", bid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int cnt = 1;
            if (rd.Read()) {
                cnt = rd.GetInt32(0);
            }
            rd.Close();
            if (cnt == 0) {
                cmd.CommandText = String.Format("DELETE FROM buildings WHERE b_id={0:d};", bid);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Добавить новый Ярус
        /// </summary>
        /// <param name="type">Тип юрты</param>
        /// <returns>ID нового яруса</returns>
        internal static int addNewTier(MySqlConnection sql, BuildingType type)
        {
            if (type == BuildingType.None) {
                return 0;
            }
            string hn = "0";
            string delims = "1";
            string bcols = "";
            string bvals = "";

            MySqlCommand cmd = new MySqlCommand("", sql);
            switch (type) {
                case BuildingType.Quarta:
                    delims = "111";
                    break;

                case BuildingType.Complex:
                    delims = "11";
                    bcols = ",t_busy1, t_busy2, t_busy3, t_busy4";
                    bvals = ", 0, 0, 0, null";
                    break;

                case BuildingType.Barin:
                case BuildingType.DualFemale:
                case BuildingType.Vertep:
                case BuildingType.Jurta:
                    if (type == BuildingType.DualFemale) {
                        hn = "00";
                    }
                    if (type == BuildingType.Jurta) {
                        delims = "0";
                    }
                    bcols = ",t_busy1, t_busy2, t_busy3, t_busy4";
                    bvals = ", 0, 0, null, null";
                    break;

                case BuildingType.Female:
                case BuildingType.Cabin:
                    delims = "0";
                    bcols = ", t_busy1, t_busy2, t_busy3, t_busy4";
                    bvals = ", 0, null, null, null";
                    break;
            }
            cmd.CommandText = String.Format(
                    @"INSERT INTO tiers(t_type, t_delims, t_heater, t_nest, t_notes {3:s}) 
                            VALUES('{0:s}', '{1:s}', '{2:s}', '{2:s}', ''{4:s});", 
                Building.GetName(type), delims, hn, bcols, bvals
            );
            _logger.Debug("db.mysql.Building: " + cmd.CommandText);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        private static void changeTierType(MySqlConnection sql, int tid, BuildingType type)
        {
            String hn = "00";
            String delims = "000";
            String busy = "";
            //if (type == "quarta") delims = "111"; 
            //if (type == "barin") delims = "100";
            switch (type) {
                case BuildingType.Quarta:
                    delims = "111";
                    busy = getBusyString(4);
                    break;

                case BuildingType.Complex:
                    hn = "0";
                    delims = "11";
                    busy = getBusyString(3);
                    break;

                case BuildingType.Barin:
                    delims = "100";
                    busy = getBusyString(2);
                    break;

                case BuildingType.Vertep:
                case BuildingType.DualFemale:
                case BuildingType.Jurta:
                    busy = getBusyString(2);
                    break;

                case BuildingType.Female:
                    busy = getBusyString(1);
                    break;


            }
            MySqlCommand cmd = new MySqlCommand(
                String.Format(@"
                    UPDATE tiers 
                    SET t_type='{0:s}', t_delims='{1:s}', t_heater='{2:s}', t_nest='{2:s}' {4:s} 
                    WHERE t_id={3:d};", 
                    Building.GetName(type), delims, hn, tid, busy
                ), 
                sql
            );
            cmd.ExecuteNonQuery();
        }

        /// <param name="count">Сколько клеток доступны для заселения</param>
        private static string getBusyString(byte count)
        {
            string result = "";
            for (int i = 1; i <= 4; i++) {
                if (i <= count) {
                    result += String.Format(",t_busy{0:s}=0", i.ToString());
                } else {
                    result += String.Format(",t_busy{0:s}=NULL", i.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// Добавляет новую МИНИферму
        /// </summary>
        /// <param name="parent">Ветка-родитель</param>
        /// <param name="uppertype">Тип верхнего яруса</param>
        /// <param name="lowertype">Тип нижнего яруса</param>
        /// <param name="name">Название (если блок)</param>
        /// <param name="id">Номер МИНИфермы</param>
        /// <returns>Номер новой клетки</returns>
        internal static int addFarm(MySqlConnection sql, int parent, BuildingType uppertype, BuildingType lowertype, String name, int id)
        {
            int t1 = Buildings.addNewTier(sql, uppertype);
            int t2 = Buildings.addNewTier(sql, lowertype);
            int res = 0;
            MySqlCommand cmd = new MySqlCommand(
                String.Format(
                    "INSERT INTO minifarms(m_upper, m_lower, m_id) VALUES({0}, {1}, {2});",
                    DBHelper.Nullable(t1),
                    DBHelper.Nullable(t2), 
                    id
                ),
                sql
            );
            _logger.Debug("db.mysql.Building: " + cmd.CommandText);
            cmd.ExecuteNonQuery();
            res = (int)cmd.LastInsertedId;

            addBuilding(sql, parent, (name != "" ? name : String.Format("№{0:d}", res)), res);
            return res;
        }

        /// <summary>
        /// Существует ли миниферма
        /// </summary>
        /// <param name="id">Номер минифермы</param>
        internal static bool farmExists(MySqlConnection sql, int id)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT COUNT(*) FROM minifarms WHERE m_id={0:d};", id), sql);
            if (cmd.ExecuteScalar().ToString() == "0") {
                return false;
            } else {
                return true;
            }
        }

        internal static bool hasRabbits(MySqlConnection sql, int tid)
        {
            if (tid == 0) return false;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT t_busy1, t_busy2, t_busy3, t_busy4 
FROM tiers WHERE t_id = {0:d};", tid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            bool busy = true;
            if (rd.Read()) {
                busy = false;
                if (rd.GetInt32(0) != 0) busy = true;
                if (!rd.IsDBNull(1) && rd.GetInt32(1) != 0) busy = true;
                if (!rd.IsDBNull(2) && rd.GetInt32(2) != 0) busy = true;
                if (!rd.IsDBNull(3) && rd.GetInt32(3) != 0) busy = true;
            }
            rd.Close();
            return busy;
        }

        internal static int[] getTiersFromFarm(MySqlConnection sql, int fid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT 
    Coalesce(m_upper,0) AS m_upper,
    Coalesce(m_lower,0) AS m_lower
FROM minifarms 
WHERE m_id={0:d};", fid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int t1 = 0, t2 = 0;
            if (rd.Read()) {
                t1 = rd.GetInt32("m_upper");
                t2 = rd.GetInt32("m_lower");
            }
            rd.Close();

            return new int[] { t1, t2 };
        }

        internal static void changeFarm(MySqlConnection sql, int fid, BuildingType uppertype, BuildingType lowertype)
        {
            int[] t = getTiersFromFarm(sql, fid);
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT t_type FROM tiers WHERE t_id={0:d};", t[0]), sql);
            BuildingType t1 = BuildingType.None;
            BuildingType t2 = BuildingType.None;
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read()) {
                t1 = Building.ParseType(rd.GetString(0));
            }
            rd.Close();
            if (t[1] != 0) {
                cmd.CommandText = String.Format(@"SELECT t_type FROM tiers WHERE t_id={0:d};", t[1]);
                rd = cmd.ExecuteReader();
                if (rd.Read()) {
                    t2 = Building.ParseType(rd.GetString(0));
                }
                rd.Close();
            }
            if (t1 != uppertype && !hasRabbits(sql, t[0]))
                changeTierType(sql, t[0], uppertype);
            if (t2 != lowertype && !hasRabbits(sql, t[1])) {
                if (lowertype == BuildingType.None) {
                    cmd.CommandText = String.Format("DELETE FROM tiers WHERE t_id={0:d};", t[1]);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = String.Format("UPDATE minifarms SET m_lower=NULL WHERE m_id={0:d};", fid);
                    cmd.ExecuteNonQuery();
                } else if (t2 == BuildingType.None) {
                    int nid = addNewTier(sql, lowertype);
                    cmd.CommandText = String.Format("UPDATE minifarms SET m_lower={0:d} WHERE m_id={1:d};", nid, fid);
                    cmd.ExecuteNonQuery();
                } else {
                    changeTierType(sql, t[1], lowertype);
                }
            }
        }

        internal static void deleteFarm(MySqlConnection sql, int fid)
        {
            int[] t = getTiersFromFarm(sql, fid);
            if (!hasRabbits(sql, t[0]) && !hasRabbits(sql, t[1])) {
                MySqlCommand cmd = new MySqlCommand(String.Format(@"DELETE FROM tiers WHERE t_id={0:d} OR t_id={1:d};", t[0], t[1]), sql);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("DELETE FROM minifarms WHERE m_id={0:d};", fid);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("DELETE FROM buildings WHERE b_farm={0:d};", fid);
                cmd.ExecuteNonQuery();
            }
        }

        internal static int GetMFCount(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT COUNT(*) FROM buildings WHERE b_farm IS NOT NULL;"), sql);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }
    }
}
