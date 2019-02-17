using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotNetCore.Api.Areas.Auth.Models;
using DotNetCore.Api.Filter;
using DotNetCore.Dal;
using DotNetCore.Dal.Entity;
using DotNetCore.Infrastruct.Log;
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
        ILogWriter logWriter = Log4NetWriter.GetInstance();

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
           int a1 = 0;
            int a = 1 / a1;
            return new User();

        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            return new User();
        }


    }  
}