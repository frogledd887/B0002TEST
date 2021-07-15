using System;
using System.Web.Http;
using B0002.Connection;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;

namespace B0002.Controllers
{
    /// <summary>
    /// 汽機車停車表單
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CarController : ApiController
    {
       
        private readonly SqlConnectionFactory sqlserver_conn;

        private CarController()
        {
            sqlserver_conn = new SqlConnectionFactory();
        }


        /// <summary>
        /// 停車員工資料查詢
        /// </summary>
        /// <param name="id">employee id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Car/Get")]
        public IHttpActionResult GetData(string id)
        {
            using (var cn = sqlserver_conn.CreateConnection("DVDBSV01"))
            {
                string sql = @"SELECT user_id ,
                                   user_name ,
                                   car_no 
                                   FROM dbo.car_user          
                                   WHERE user_id  = @ueer_id";

                var result = cn.Query(sql, new { ueer_id = id });
                return Json(JArray.FromObject(result));

            }
        }

        /// <summary>
        /// 停車員工資料寫入
        /// </summary>
        /// <param name="id">employee id</param>
        /// <param name="name">employee name</param>
        /// <param name="number">employee id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Car/Insert")]
        public IHttpActionResult InsertData(string id,string name,string number)
        {
            using (var cn = sqlserver_conn.CreateConnection("DVDBSV01"))
            {
                string sql = @"INSERT INTO car_user(user_id
                ,user_name,car_no)
                 VALUES(@id,@name,@number)
                ";

                var result = cn.Query(sql, new { id, name, number });
                return Json(JArray.FromObject(result));

            }
        }


    }
}
