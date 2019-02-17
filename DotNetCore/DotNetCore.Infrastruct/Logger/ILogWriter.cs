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

    public abstract class ApiLogMsg
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string HttpMethod { get; set; }
        public string Path { get; set; }
        public string User { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public object Msg { get; set; }      
    }

    public class ApiInfoLogMsg: ApiLogMsg
    {       
        public override string ToString()
        {
            var recordBuild = new StringBuilder();
            recordBuild.Append($"【Http方法】 : {this.HttpMethod}<br>");
            recordBuild.Append($"【控制器类】 : {this.ControllerName}<br>");
            recordBuild.Append($"【操作方法】 : {this.ActionName}<br>");
            recordBuild.Append($"【请求路径】 : {this.Path}<br>");

            if(!string.IsNullOrEmpty(this.User))
            {
                recordBuild.Append($"【请求账户】 : {this.User}<br>");
            }
            if (!string.IsNullOrEmpty(this.RequestJson))
            {
                recordBuild.Append($"【请求参数】：{this.RequestJson}<br>");
            }
            if (!string.IsNullOrEmpty(this.ResponseJson))
            {
                recordBuild.Append($"【返回数据】：{this.ResponseJson}<br>");
            }
            if (this.Msg != null)
            {
                recordBuild.Append($"【日志信息】 : {this.Msg}<br>");
            }
            return recordBuild.ToString();
        }
    }

    public class ApiErrorLogMsg: ApiLogMsg
    {
        public override string ToString()
        {
            var recordBuild = new StringBuilder();
            recordBuild.Append($"【Http方法】 : {this.HttpMethod}<br>");
            recordBuild.Append($"【控制器类】 : {this.ControllerName}<br>");
            recordBuild.Append($"【操作方法】 : {this.ActionName}<br>");
            recordBuild.Append($"【请求路径】 : {this.Path}<br>");

            if (string.IsNullOrEmpty(this.User))
            {
                recordBuild.Append($"【请求账户】 : {this.User}<br>");
            }
            if (string.IsNullOrEmpty(this.RequestJson))
            {
                recordBuild.Append($"【请求参数】：{this.RequestJson}<br>");
            }
            if (string.IsNullOrEmpty(this.ResponseJson))
            {
                recordBuild.Append($"【返回数据】：{this.ResponseJson}<br>");
            }
            if (this.Msg != null)
            {
                recordBuild.Append($"【错误信息】 : {this.Msg}<br>");
            }
            return recordBuild.ToString();
        }
    }

    public class InfoLogMsg : LogMsg
    {       
        public override string ToString()
        {
            var recordBuild = new StringBuilder();
            recordBuild.Append($"【日志类名】 : {this.ClassName}<br>");
            recordBuild.Append($"【日志方法】 : {this.MethodName}<br>");
            if (string.IsNullOrEmpty(this.Parameter))
            {
                recordBuild.Append($"【方法参数】：{this.Parameter}<br>");
            }
            if (this.Msg != null)
            {
                recordBuild.Append($"【日志信息】 : {this.Msg}<br>");
            }
            return recordBuild.ToString();
        }
    }

    public class ErrorLogMsg : LogMsg
    {
        public override string ToString()
        {
            var recordBuild = new StringBuilder();
            recordBuild.Append($"【日志类名】 : {this.ClassName}<br>");
            recordBuild.Append($"【日志方法】 : {this.MethodName}<br>");
            if (string.IsNullOrEmpty(this.Parameter))
            {
                recordBuild.Append($"【方法参数】：{this.Parameter}<br>");
            }
            if (this.Msg != null)
            {
                recordBuild.Append($"【错误信息】 : {this.Msg}<br>");
            }
            return recordBuild.ToString();
        }
    }
}
