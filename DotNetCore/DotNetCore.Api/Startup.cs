using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Api.Helper;
using DotNetCore.Dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.IO;
using DotNetCore.Infrastruct.Log;

namespace DotNetCore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo( @"\App_Data\log4net.xml"));
            new LogConfig(ConfigItem.LogRepository).ConfigureAndWatch("log4net.xml");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Swagger

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1",new Swashbuckle.AspNetCore.Swagger.Info
                    {
                         Title="My DotNetCore Api",
                          Version="V1"
                    });
                    // 为 Swagger JSON and UI设置xml文档注释路径
                    var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                    var xmlPath = Path.Combine(basePath, "DotNetCore.Api.xml");
                    c.IncludeXmlComments(xmlPath);
                });

            #endregion

            string connectStr = AppSettingJson.GetConfig().GetConnectionString("default");
            services.AddDbContext<DbCoreContext>(
                builder => { builder.UseMySQL(connectStr); }//, options => options.MigrationsAssembly("DotNetCore.Api")
                );

            //services.AddLogging()

            services.AddTransient(typeof(DbContext), typeof(DbCoreContext));//DI注册

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<ApiBehaviorOptions>(
                config =>
                {
                    //config.SuppressModelStateInvalidFilter = true;
                    config.InvalidModelStateResponseFactory = context =>
                      {
                          return new OkObjectResult("model error");//此处可写入一些模型验证错误的处理
                      };
                });

            services.AddCors();//貌似这个不要也行，只要中间件中添加了就可以
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c=> 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(
                builder =>
                {
                    //builder.WithOrigins("http://localhost:51119");
                    builder.AllowAnyHeader();//任何http头信息
                    builder.AllowAnyMethod();//任何http方法
                    builder.AllowAnyOrigin();//任何源
                    builder.AllowCredentials();//允许http身份验证和cookie跨域
                });//CORS 中间件必须位于之前定义的任何终结点应用程序中你想要支持跨域请求

            app.UseMvc();
            
        }
    }
}
