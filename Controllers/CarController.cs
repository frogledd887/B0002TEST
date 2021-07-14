using System;
using System.Web.Http;
using B0002.Connection;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;

namespace B0002.Controllers
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


        /// <summary>
        /// 工程處訓練業務承辨人
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




    }
}
