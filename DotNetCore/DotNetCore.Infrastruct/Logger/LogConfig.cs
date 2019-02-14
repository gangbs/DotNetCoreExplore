using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetCore.Infrastruct.Log
{
    public class LogConfig
    {
        readonly string _repositoryName;
        private static ILoggerRepository _repository;

        public LogConfig(string repositoryName)
        {
            this._repositoryName = repositoryName;
        }

        public void ConfigureAndWatch(string cfgFile)
        {
            if (LogConfig._repository == null) CreateRepository(_repositoryName);
            XmlConfigurator.ConfigureAndWatch(LogConfig._repository, new FileInfo(cfgFile));
        }

        private void CreateRepository(string name)
        {
            _repository = LogManager.CreateRepository(name);
        }


    }
}
