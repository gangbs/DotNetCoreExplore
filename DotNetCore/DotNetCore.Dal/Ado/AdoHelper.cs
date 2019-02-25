using DotNetCore.Infrastruct.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DotNetCore.Dal.Ado
{
   public class AdoHelper
    {
        public static MySqlConnection GetConnection(string connStr)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public static List<T> QueryMany<T>(string sql, MySqlConnection conn)
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            da.Fill(dt);
            var lst = dt.TableToList<T>();
            return lst;
        }

        public static void QueryMany2(string sql, MySqlConnection conn)
        {
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.CommandType = CommandType.Text;
            var reader= command.ExecuteReader();

            while(reader.Read())
            {

            }

        }
    }
}
