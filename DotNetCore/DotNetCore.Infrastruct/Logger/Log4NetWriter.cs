using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.Log
{
    public class Log4NetWriter : ILogWriter
    {
        const string repository = "DotNetCoreLogRepository";
        const string infoLogName = "logInfo";
        const string errorLogName = "logError";
        readonly ILog infoLogger = LogManager.GetLogger(repository, infoLogName);
        readonly ILog errorLogger = LogManager.GetLogger(repository, infoLogName);

        #region 单例

        private Log4NetWriter() { }
        private static object _locker = new object();
        private static Log4NetWriter _instance;
        public static Log4NetWriter GetInstance()
        {
            if(_instance == null)
            {
                lock(_locker)
                {
                    _instance = new Log4NetWriter();
                }
            }
            return _instance;
        }

        #endregion

        public void ApiInfoLog(ApiInfoLogMsg msg)
        {
            infoLogger.Info(msg);
        }

        public void ApiErrorLog(ApiErrorLogMsg msg)
        {
            errorLogger.Error(msg);
        }

        public void LogError(ErrorLogMsg msg)
        {
            errorLogger.Error(msg);
        }

        public void LogInfo(InfoLogMsg msg)
        {
            infoLogger.Info(msg);
        }

        public void Log(object msg, bool isError = false)
        {
            if(isError)
            {
                infoLogger.Error(msg);
            }
            else
            {
                infoLogger.Info(msg);
            }
        }
    }
}
