using DotNetCore.Infrastruct.Ado;
using DotNetCore.Infrastruct.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace DotNetCore.Dal.Ado
{
    public class SqlServerExecute : ISqlExecute, IDisposable
    {
        readonly string _connStr;
        private SqlConnection _connection;

        public SqlServerExecute(string strConn)
        {
            this._connStr = strConn;
        }

        public SimpleResult ConnectTest(string strConn)
        {
            if (string.IsNullOrEmpty(strConn)) strConn = this._connStr;

            SimpleResult result;
            try
            {
                this.CreateConnection(strConn);
                result = new SimpleResult { Success = true };
            }
            catch (Exception e)
            {
                result = new SimpleResult { Success = false, Message = e.Message };
            }
            return result;
        }

        public DbConnection CreateConnection(string strConn)
        {
            var conn = SqlClientFactory.Instance.CreateConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            return conn;
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return command.ExecuteNonQuery();
        }

        public T ExecuteScalar<T>(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            return (T)command.ExecuteScalar();
        }

        public DataTable Query(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            DataTable dt = new DataTable();
            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            SqlDataAdapter da = new SqlDataAdapter(command);
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

            if (lst == null)
            {
                return default(T);
            }
            else
            {
                return lst[0];
            }
        }

        public SimpleResult SqlQueryTest(string sql, params DbParameter[] dbParameters)
        {
            CheckConnect();

            SqlCommand command = new SqlCommand(sql, this._connection);
            AddParameters(command, dbParameters);
            SimpleResult result;

            try
            {
                command.ExecuteReader();
                result = new SimpleResult { Success = true };
            }
            catch (Exception e)
            {
                result = new SimpleResult { Success = false, Message = e.Message };
            }
            return result;
        }

        public void Dispose()
        {
            if (this._connection.State == System.Data.ConnectionState.Open)
            {
                this._connection.Close();
            }
            this._connection.Dispose();
        }

        private void CheckConnect()
        {
            if (this._connection == null)
            {
                this._connection = (SqlConnection)CreateConnection(this._connStr);
            }
            else if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        private void AddParameters(SqlCommand command, params DbParameter[] dbParameters)
        {
            if (dbParameters != null && dbParameters.Length > 0)
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
    }
}
