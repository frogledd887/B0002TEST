using System;
using System.Web.Http;
using B0002.Connection;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using E0001.Models;


namespace B0002.Controllers
{
    /// <summary>
    /// 汽機車停車表單
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CarController : ApiController
    {
       
        private readonly SqlConnectionFactory sqlserver_conn;
        private readonly Em _context;

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

        //post api/test
        [HttpPost]
        [Route("Car/Test")]
        public IHttpActionResult Test(Em value) {
            using (var cn = sqlserver_conn.CreateConnection("DVDBSV01"))
            {
                string sql = @"INSERT INTO car_user(user_id
                ,user_name,car_no)
                 VALUES(@id,@name,@number)";
                var result = cn.Query(sql, new { id=value.Id,name=value.Name, number=value.car });
                return Json(JArray.FromObject(result));
            }
        }


        //post api/test
        [HttpPost]
        [Route("Car/TestDone")]
        public IHttpActionResult Done()
        {
            using (var cn = sqlserver_conn.CreateConnection("DVDBSV01"))
            {
                string sql = @"INSERT INTO car_user(user_id
                ,user_name,car_no)
                 VALUES(6,6,6)
                ";
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));

            }
        }


        [HttpPost]
        [Route("Car/SPM_StepActivity")]
        public List<SQLCommandObject> SPM_StepActivity([FromBody] object value)
        {
            DataSet dsAllData = JsonConvert.DeserializeObject<DataSet>((string)value);

            //解析 allData 分別取得 BPM_FieldData 與 Variables 內容
            Hashtable EFormFd = ParseDataSet("BPM_FieldData", ref dsAllData);
            Hashtable Services = ParseDataSet("Variables", ref dsAllData);

            //變數宣告存放執行 SQLCommand 集合
            List<SQLCommandObject> sqls = new List<SQLCommandObject>();

            string stepName = Services["STEPNAME"].ToString();
            string otherContent = "";

            try
            {
                //////////////////////////////////////////
                /*
                 using (var cn = sqlserver_conn.CreateConnection("DVDBSV01"))
            {
                string sql = @"INSERT INTO car_user(user_id
                ,user_name,car_no)
                 VALUES(6,6,6)
                ";
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));

            }
                 */
                //////////////////////////////////////////
            }
            catch (Exception ex)
            {
                otherContent += " PlanName: E0001 工作代號申請單,"; //流程名稱
                otherContent += " PLANSN: " + (string)Services["PLANSN"] + ","; //流程編號
                otherContent += " CASEID: " + (string)Services["CASEID"] + ","; //案件編號
                otherContent += " STEPNAME: " + (string)Services["STEPNAME"] + ","; //關卡名稱
                otherContent += " Sys_LogonId: " + (string)Services["Sys_LogonId"]; //處理者帳號

                //ProjectErrorLogService.RecordErrorLog("E0001 SPM_StepActivity", ex, otherContent);
            }

            return sqls;
        }


        //自 allData XML內容解析 tableName 資料表 DataRow 轉以 Hashtable 型式
        private Hashtable ParseDataSet(string tableName, ref DataSet dsAllData)
        {
            Hashtable ret = new Hashtable();
            try
            {
                if (dsAllData.Tables.Count != 0)
                {
                    if (dsAllData.Tables.Contains(tableName))
                    {
                        for (int idx = 0; idx <= dsAllData.Tables[tableName].Columns.Count - 1; idx++)
                        {
                            string columnName = dsAllData.Tables[tableName].Columns[idx].ColumnName;
                            string columnValue = "";
                            if (!Convert.IsDBNull(dsAllData.Tables[tableName].Rows[0][columnName]))
                                columnValue = String.Format("{0}", dsAllData.Tables[tableName].Rows[0][columnName]);

                            ret.Add(columnName, columnValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }


    }
}

public class SQLCommandObject
{
    //建溝元
    public SQLCommandObject()
    {
    }

    //建溝元
    public SQLCommandObject(string commandText)
    {
        CommandType = CommandType.Text;

        CommandText = commandText;
    }

    //建溝元
    public SQLCommandObject(CommandType commandType, string commandText)
    {
        CommandType = commandType;

        CommandText = commandText;
    }

    //CommandText 指令類型
    public System.Data.CommandType CommandType { get; set; }

    //CommandText 指令內容
    public string CommandText { get; set; }

    //Parameter 參數集合
    public Hashtable CommandParameter = new Hashtable();

}


