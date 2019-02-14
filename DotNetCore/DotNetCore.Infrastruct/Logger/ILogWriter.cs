using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.Log
{
   public interface ILogWriter
    {
        void LogInfo(InfoLogMsg msg);

        void LogError(ErrorLogMsg msg);

        void ApiInfoLog(ApiInfoLogMsg msg);

        void ApiErrorLog(ApiErrorLogMsg msg);

        void Log(object msg,bool isError=false);
    }

    public abstract class LogMsg
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string Parameter { get; set; }
        public object Msg { get; set; }
    }

    public class ApiInfoLogMsg: LogMsg
    {
        public string HttpMethod { get; set; }

        public override string ToString()
        {
            string request = string.IsNullOrEmpty(this.Parameter) ? "" : $"【请求参数】：{this.Parameter}<br>";
            string record = $"【Http方法】 : {this.HttpMethod}<br>【控制器名称】 : {this.ClassName}<br>【操作名称】 : {this.MethodName}<br>{request}";
            if (this.Msg != null)
            {
                record += $"【日志信息】：{this.Msg}<br>";
            }
            return record;
        }
    }

    public class ApiErrorLogMsg:LogMsg
    {
        public string HttpMethod { get; set; }

        public override string ToString()
        {
            string request = string.IsNullOrEmpty(this.Parameter) ? "" : $"【请求参数】：{this.Parameter}<br>";
            string record = $"【Http方法】 : {this.HttpMethod}<br>【控制器名称】 : {this.ClassName}<br>【操作名称】 : {this.MethodName}<br>{request}";
            if (this.Msg != null)
            {
                record += $"【错误信息】：{this.Msg}<br>";
            }
            return record;
        }
    }

    public class InfoLogMsg : LogMsg
    {       
        public override string ToString()
        {
            string request = string.IsNullOrEmpty(this.Parameter) ? "" : $"【方法参数】：{this.Parameter}<br>";
            string record = $"【类名称】 : {this.ClassName}<br>【方法名称】 : {this.MethodName}<br>{request}";
            if (this.Msg!=null)
            {
                record += $"【日志信息】：{this.Msg}<br>";
            }
            return record;
        }
    }

    public class ErrorLogMsg : LogMsg
    {
        public override string ToString()
        {
            string request = string.IsNullOrEmpty(this.Parameter) ? "" : $"【方法参数】：{this.Parameter}<br>";
            string record = $"【类名称】 : {this.ClassName}<br>【方法名称】 : {this.MethodName}<br>{request}";
            if (this.Msg != null)
            {
                record += $"【错误信息】：{this.Msg}<br>";
            }
            return record;
        }
    }
}
