﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using rabnet;

namespace db.mysql
{
    class Meal
    {
        /// <summary>
        /// Во сколько дней кролик начинает есть корм
        /// </summary>
        public const int START_EAT = 18;

        public static List<sMeal> getMealPeriods(MySqlConnection sql)
        {
            List<sMeal> result = new List<sMeal>();
            MySqlCommand cmd = new MySqlCommand("SELECT m_start_date, m_amount, m_rate, m_type, m_id FROM meal ORDER BY m_start_date ASC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                result.Add(new sMeal(rd.GetInt32("m_id"), rd.GetDateTime("m_start_date"), rd.GetInt32("m_amount"), DBHelper.GetNullableFloat(rd, "m_rate"), rd.GetString("m_type")));                
            }
            rd.Close();
            return result;
        }
        /// <summary>
        /// Добавляет привоза корма
        /// </summary>
        /// <param name="start">Дата завоза</param>
        /// <param name="amount">Объем корма(кг)</param>
        public static void AddMealIn(MySqlConnection sql, DateTime start, int amount)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format("SELECT m_id FROM meal WHERE m_start_date='{0:yyyy-MM-dd}' AND m_type='in';", start);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read()) {
                if (!rd.IsDBNull(0)) {
                    int mid = rd.GetInt32(0);
                    rd.Close();
                    cmd.CommandText = String.Format("UPDATE meal SET m_amount=m_amount+{1:d} WHERE m_id={0:d}", mid, amount);
                    cmd.ExecuteNonQuery();
                }
            } else {
                if (!rd.IsClosed) {
                    rd.Close();
                }
                cmd.CommandText = String.Format("INSERT INTO meal(m_start_date, m_amount, m_type) VALUES('{0:yyyy-MM-dd}', {1:d}, 'in');", start, amount);
                cmd.ExecuteNonQuery();
            }
            Meal.updateMeal(sql);
        }
        /// <summary>
        /// Добавляет продажу корма
        /// </summary>
        public static void AddMealOut(MySqlConnection sql, DateTime start, int amount)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format("SELECT m_id FROM meal WHERE m_start_date='{0:yyyy-MM-dd}' AND m_type='out';", start);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read()) {
                if (!rd.IsDBNull(0)) {
                    int mid = rd.GetInt32(0);
                    rd.Close();
                    cmd.CommandText = String.Format("UPDATE meal SET m_amount=m_amount+{1:d} WHERE m_id={0:d}", mid, amount);
                    cmd.ExecuteNonQuery();
                }
            } else {
                if (!rd.IsClosed) {
                    rd.Close();
                }
                cmd.CommandText = String.Format("INSERT INTO meal(m_start_date, m_amount, m_type) VALUES('{0:yyyy-MM-dd}', {1:d}, 'out');", start, amount);
                cmd.ExecuteNonQuery();
            }
            Meal.updateMeal(sql);
        }

        public static void DeleteMeal(MySqlConnection sql, int id)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("DELETE FROM meal WHERE m_id={0:d};", id), sql);
            cmd.ExecuteNonQuery();
            updateMeal(sql);
        }

        protected static void updateMeal(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            List<sMeal> meals = getMealPeriods(sql);

            sMeal lastIncome = null;
            foreach (sMeal m in meals) {
                if (m.Type.ToString().ToLower() == "in") {
                    if (lastIncome != null ) {
                        lastIncome.endDate = m.StartDate;
                    }
                    lastIncome = m;

                    lastIncome.totalAmount += m.Amount;
                } else if (lastIncome != null ) {
                    lastIncome.totalAmount -= m.Amount;
                }                                
            }

            foreach (sMeal m in meals) {
                if (m.Type.ToString().ToLower() == "in" && m.endDate != DateTime.MaxValue && m.totalAmount > 0) {
                    int rabDays = getRabDays(m.StartDate, m.endDate, sql);
                    float rate = rabDays > 0 ? (float)m.totalAmount / rabDays : 0;

                    cmd.CommandText = String.Format("UPDATE meal SET m_rate = {0} WHERE m_id = {1:d}", rate.ToString("0.0000").Replace(',', '.'), m.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected static int getRabDays(DateTime from, DateTime to, MySqlConnection sql)
        {
            string query = String.Format(@"SELECT Coalesce(SUM(DATEDIFF(LEAST(end_eat, {1}), GREATEST(adulthood, {0})) * cnt), 0)
FROM (
    SELECT 
        Coalesce(SUM(r_group),0) cnt, 
        DATE_ADD(Date(r_born), INTERVAL {2:d} DAY) adulthood, #дата достижения совершеннолетия
        IFNULL(Date(d_date), {1}) AS end_eat #дата окончания питания
    FROM allrabbits 
    WHERE d_date IS NULL OR DATE(d_date) >= {0}
    GROUP BY adulthood, end_eat
    HAVING adulthood <= {1} #кролик был старше 18 дней в этот период
    ORDER BY adulthood, end_eat
) c
WHERE end_eat > adulthood", DBHelper.DateToSqlString(from), DBHelper.DateToSqlString(to), Meal.START_EAT);

            MySqlCommand cmd = new MySqlCommand(query, sql);
            
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}