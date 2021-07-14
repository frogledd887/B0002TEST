using System;
using System.Web.Http;
using E0001.Connection;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;
using E0001.Models;
using E0001.Connection;
namespace E0001.Controllers
{
    /// <summary>
    /// 工作代號清單
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CarController : ApiController
    {
       
        private readonly SqlConnectionFactory sqlserver_conn;

        private CarController()
        {
            sqlserver_conn = new SqlConnectionFactory();
        }

        

       

        [HttpGet]
        [Route("Car/GetCar")]
        public IHttpActionResult GetCar()
        {
            using (var cn = sqlserver_conn.CreateConnection("PDDBSV04"))
            {
                string sql = @"select user_id from CAR_USER where user_name = '顏月珊'";
                //string sql = $@"INSERT INTO CAR_USER (user_id, user_name, rank) VALUES (42388, '劉廷', 1234)";
                //string sql = $@"select * from CAR_USER ";
                //string sql = $@"update CAR_USER set user_id = 42389 where user_name ='劉廷'";
                //string sql = $@"DELETE FROM CAR_USER WHERE user_name = '劉廷' ";
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));
            }
        }


    }
}
