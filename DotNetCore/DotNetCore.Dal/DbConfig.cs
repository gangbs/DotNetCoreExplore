using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Dal
{
   public class DbConfig
    {
        readonly static object _locker = new object();
        private static DbConfig _instance;
        public static DbConfig Instance
        {
            get
            {
                if(_instance==null)
                {
                    lock(_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DbConfig();
                        }
                    }
                }
                return _instance;
            }
        }

        private DbConfig()
        {
            UpdateConfig();
        }

        public void UpdateConfig()
        {

        }

        public DatabaseType DbType {get; set; }
        public string ConnectionStrings { get; set; }
    }

    public enum DatabaseType
    {
        SqlServer=1,
        MySql=2,
        Oracle=3
    }
}
