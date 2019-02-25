using DotNetCore.Infrastruct.Ado;
using DotNetCore.Infrastruct.Extensions;
using DotNetCore.Infrastruct.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace DotNetCore.Dal.Ado
{
    public class MySqlExecute : ISqlExecute,IDisposable
    {
        readonly string _connStr;
        private MySqlConnection _connection;

        public MySqlExecute(string strConn)
        {
            this._connStr = strConn;
        }

        public SimpleResult ConnectTest(string strConn=null)
        {
            if (string.IsNullOrEmpty(strConn)) strConn = this._connStr;

            SimpleResult result;
            try
            {
                this.CreateConnection(strConn);
                result = new SimpleResult { Success=true };
            }
            catch(Exception e)
            {
                result = new SimpleResult { Success=false, Message=e.Message };
            }
            return result;
        }

        public DbConnection CreateConnection(string strConn)
        {
            var conn = MySqlClientFactory.Instance.CreateConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            return conn;
        }

        public DataTable Query(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            DataTable dt = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dt);
            return dt;
        }

        public IEnumerable<T> QueryMany<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            var dt = Query(sql, dbParameters);
            var lst = TableToList<T>(dt);
            return lst;
        }

        public T QuerySingle<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            var dt = Query(sql, dbParameters);
            var lst = TableToList<T>(dt);
            
            if(lst==null)
            {
                return default(T);
            }
            else
            {
                return lst[0];
            }
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return (T)command.ExecuteScalar();
        }

        private void CheckConnect()
        {
            if(this._connection==null)
            {
                this._connection = (MySqlConnection)CreateConnection(this._connStr);
            }
            else if(this._connection.State!= ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        private void AddParameters(MySqlCommand command, params DbParameter[] dbParameters)
        {
            if(dbParameters!=null&&dbParameters.Length>0)
            {
                command.Parameters.AddRange(dbParameters);
            }
        }

        private List<T> TableToList<T>(DataTable dt)
        {
            if (dt == null) return null;

            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] pArray = type.GetProperties();

            var columns = dt.Columns;

            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>();

                foreach (PropertyInfo p in pArray)
                {
                    var col = columns[p.Name];
                    if (col == null) continue;
                    var obj = Convert.ChangeType(row[col], p.PropertyType);
                    p.SetValue(entity, obj, null);
                }
                list.Add(entity);
            }          
            return list;
        }

        public void Dispose()
        {
            if(this._connection.State== System.Data.ConnectionState.Open)
            {
                this._connection.Close();
            }           
            this._connection.Dispose();
        }

        public SimpleResult SqlQueryTest(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            MySqlCommand command = new MySqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            SimpleResult result;

            try
            {
                command.ExecuteReader();
               //var obj= command.ExecuteScalar();
                result = new SimpleResult { Success=true };
            }
            catch(Exception e)
            {
                result = new SimpleResult { Success = false, Message=e.Message };
            }
            return result;
        }
    }
}
