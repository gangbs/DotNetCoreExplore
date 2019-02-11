using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotNetCore.Api.Areas.Auth.Models;
using DotNetCore.Api.Filter;
using DotNetCore.Dal;
using DotNetCore.Dal.Entity;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DotNetCore.Api.Areas.Auth.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Auth")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        readonly DbContext _dbContext;
        //readonly ILog logger = LogManager.GetLogger(ConfigItem.LogRepository, ConfigItem.LogInfo);

        readonly ILog logger = LogManager.GetLogger(ConfigItem.LogRepository, MethodBase.GetCurrentMethod().DeclaringType);

        //public UserInfoController(DbContext dbContext)
        //{
        //    this._dbContext = dbContext;
        //}


        //[ModelValidate]
        [HttpPost]
        public ActionResult<User> Post(User user)
        {

            return user; 
        }

        [HttpGet]
        public ActionResult<User> Get1([FromQuery]User user)
        {
            return user;
        }

        /// <summary>
        /// Get测试2
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<User> Get2(int id)
        {
            //  var dbContext = new DbCoreContext();
            //  UnitOfWork uw = new UnitOfWork(dbContext);
            //var res=  uw.GetRepository<RoleEntity>();

            //  try
            //  {
            //      string sql = $"select Id RoleId,Name RoleName from role";
            //      var a = res.GetList<Role>(sql);
            //  }
            //  catch(Exception e)
            //  {

            //  }

            logger.Info("log测试5");

            return new User();
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            return new User();
        }


    }  
}