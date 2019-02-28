using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Helper
{
    public class AppSettingJson
    {
        public static IConfiguration GetConfig(string path = "appsettings.json")
        {
            var builder = new ConfigurationBuilder().AddJsonFile(path);//能否不指定
            var configuration = builder.Build();
            return configuration;
        }

        public static string GetValue(string key)
        {
            var config = GetConfig();
            return config[key];
        }

        public static T GetValue<T>(string key)
        {
            var config = GetConfig();
            return config.GetValue<T>(key);
        }

        public static T GetModel<T>(string key) where T : class, new()
        {
            var config = GetConfig();
            Type tp = typeof(T);
            var obj = (T)tp.Assembly.CreateInstance(tp.FullName);
            config.GetSection(key).Bind(obj);
            return obj;
        }
    }
}
