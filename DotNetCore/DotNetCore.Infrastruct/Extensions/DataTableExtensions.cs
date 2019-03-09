using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace DotNetCore.Infrastruct.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ListToTable<T>(this List<T> list)
        {
            Type tp = typeof(T);
            PropertyInfo[] proInfos = tp.GetProperties();
            DataTable dt = new DataTable();
            foreach (var item in proInfos)
            {
                dt.Columns.Add(item.Name, item.PropertyType);
            }
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                foreach (var proInfo in proInfos)
                {
                    dr[proInfo.Name] = proInfo.GetValue(item);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static List<T> TableToList<T>(this DataTable dt, bool isStoreDB = true)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] pArray = type.GetProperties(); //集合属性数组
            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>(); //新建对象实例 
                foreach (PropertyInfo p in pArray)
                {
                    try
                    {
                        var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为集合字段类型  
                        p.SetValue(entity, obj, null);
                    }
                    catch (Exception)
                    {
                        // throw;
                    }
                }
                list.Add(entity);
            }
            return list;
        }

        public static T RowToEntity<T>(this DataRow row)
        {
            Type type = typeof(T);
            T entity = Activator.CreateInstance<T>(); //创建对象实例
            PropertyInfo[] pArray = type.GetProperties();
            foreach (PropertyInfo p in pArray)
            {
                try
                {
                    var obj = Convert.ChangeType(row[p.Name], p.PropertyType);//类型强转，将table字段类型转为对象字段类型
                    p.SetValue(entity, obj, null);
                }
                catch (Exception)
                {
                    // throw;
                }
            }
            return entity;
        }

        public static void ToCsv(this DataTable table,string category)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            string path = category + table.TableName + ".csv";
            File.WriteAllText(path, sb.ToString());
        }
    }
}
