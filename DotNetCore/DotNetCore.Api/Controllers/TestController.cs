using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Dal.Ado;
using DotNetCore.Infrastruct.Ado;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public ActionResult<object> Get()
        {
            //string sessionId = HttpContext.Session.Id;
            //HttpContext.Session.SetString("id", sessionId);
            //HttpContext.Session.SetString("yyg", "123");
            //return Ok(new { id = sessionId });


            SqlServerTest();
            return Ok();
        }


        private void MySqlTest()
        {
            string connStr = "server=localhost;port=3306;user=root;password=123456;database=DotNetCore";
            string sql = "select * from tagconfig";
            string delSql = "delete from tagconfig where tagId=2";

            MySqlExecute mySql = new MySqlExecute(connStr);
            //var result1 = mySql.QueryMany<TagConfig>(sql);
            //var result2 = mySql.QuerySingle<TagConfig>(sql);

            //var result3 = mySql.ExecuteScalar<int>(sql);

            var result4 = mySql.QueryMany<TagConfig>(delSql);

        }


        private void SqlServerTest()
        {
            string connStr = "server=localhost;uid=sa;pwd=12345678;database=PIMS_PSA";
            string sql = "select * from tagconfig";
            string delSql = "delete from tagconfig where tagId=1";

            ISqlExecute exe = new SqlServerExecute(connStr);
            //var result1 = exe.QueryMany<TagConfig>(sql);
            //var result2 = exe.QuerySingle<TagConfig>(sql);

            //var result3 = exe.ExecuteScalar<int>(sql);

            var result4 = exe.SqlQueryTest(delSql);
        }

    }

    public class TagConfig
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string TagDesc { get; set; }
        public int DepartID { get; set; }
        public int StationID { get; set; }
        public double UpperLimit { get; set; }
        public double LowerLimit { get; set; }
        public double Expectation { get; set; }
        public double Weight { get; set; }

        public string test { get; set; }
        public int testInt { get; set; }
    }
}