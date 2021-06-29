using System;
using System.Web.Http;
using E0001.Connection;
using Dapper;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;
using E0001.Models;

namespace E0001.Controllers
{
    /// <summary>
    /// 工作代號清單
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobListController : ApiController
    {
        private readonly OracleConnectionFactory _conn;
        private JobListController()
        {
            _conn = new OracleConnectionFactory();
        }

        /// <summary>
        /// 取得「工作代號」類型清單
        /// </summary>
        /// <returns>「工作代號」類型清單</returns>
        [HttpGet]
        [Route("GetJobKind")]
        public IHttpActionResult GetJobKind()
        {
            using (var cn = _conn.CreateConnection("PC"))
            {
                string sql = $@"SELECT * FROM PC.T_JOB_KIND";
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));
            }
        }

        /// <summary>
        /// 取得類型之「工作代號」屬性清單 
        /// </summary>
        /// <param name="kind">「工作代號」類型 (ex: GetJobAttrByKind?kind={value})</param>
        /// <returns>「工作代號」屬性清單</returns>
        [HttpGet]
        [Route("GetJobAttrByKind")]
        public IHttpActionResult GetJobAttrByKind(string kind)
        {
            using (var cn = _conn.CreateConnection("PC"))
            {
                string sql = $@"SELECT  *
                                        FROM PC.T_JOB_ATTR
                                        WHERE   (JOB_PROJ_KIND = UPPER('{kind}'))
                                        ORDER BY JOB_PROJ_ATTR";
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));
            }
        }

        /// <summary>
        /// 取得「工作代號」編碼序號
        /// </summary>
        /// <param name="attr">「工作代號」屬性</param> 
        /// <returns>編碼序號</returns>
        [HttpGet]
        [Route("GetJobNoByAttr")]
        public  string GetJobNoByAttr(string attr)
        {
            using (var cn = _conn.CreateConnection("PC"))
            {
                int strlen = attr.Length;
                string sql = $@"SELECT  NVL(MAX(REPLACE(PROJ_ID, '{attr}', '')), 0) AS MAX_NO
                                        FROM PC.JOB_NO_LIST
                                        WHERE(substr(PROJ_ID, 1, {strlen}) = '{attr}')
                                        ORDER BY MAX_NO DESC";
                string result = cn.ExecuteScalar(sql).ToString();
                if (result != null)
                {
                    int i = 0;
                    bool numberType = int.TryParse(result, out i); 
                    if (numberType) {
                        result = (i+1).ToString("D4");
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 取得「線別」內之「施工標號」
        /// </summary>
        /// <param name="line">「線別」</param> 
        /// <returns>施工標號及名稱</returns>
        [HttpGet]
        [Route("GetProjByLine")]
        public IHttpActionResult GetProjByLine(string line)
        {
            using (var cn = _conn.CreateConnection("EM"))
            {
                string sql = $@"SELECT  PROJ_ID, PROJ_NAME_C
                                        FROM      EM.V_PROJ_BASE
                                        WHERE   (LINE_CD = UPPER('{line}'))";
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));
            }
        }

        /// <summary>
        /// 取得施工標名稱
        /// </summary>
        /// <param name="id">施工標號</param> 
        /// <returns>施工標名稱</returns>
        [HttpGet]
        [Route("GetProjById")]
        public IHttpActionResult GetProjById(string id)
        {
            using (var cn = _conn.CreateConnection("EM"))
            {
                string sql = $@"SELECT  PROJ_ID, PROJ_NAME_C
                                        FROM      EM.V_PROJ_BASE
                                        WHERE   (PROJ_ID = UPPER('{id}'))";
                var result = cn.Query(sql);
                return Json(JArray.FromObject(result));
            }
        }

        /// <summary>
        /// 新增工作代號資料(包含工作代號\施工標號\施工標號)
        /// </summary>
        /// <param name="value">JOB_NO_LIST</param>
        /// <returns>成功交易筆數</returns>
        // POST: api/PC_JobListProj
        public int Post([FromBody] JOB_NO_LIST value)
        {
            var cn = _conn.CreateConnection("PC");
            string sql = @"INSERT INTO PC.JOB_NO_LIST(
                                                   PROJ_ID, PROJ_NAME, PROJ_NAME_E, REQU_UNIT, REQU_MAN, REQU_TEL, REQU_CAUS,
                                                   KEYIN_UNIT, KEYIN_YY, KEYIN_MM, KEYIN_DD, KEYIN_MAN, 
                                                   LINE_ID, JOB_PROJ_ATTR, JOB_PROJ_KIND, REMARK)
                                    VALUES(:PROJ_ID, :PROJ_NAME, :PROJ_NAME_E, :REQU_UNIT, :REQU_MAN, :REQU_TEL, :REQU_CAUS,
                                                   :KEYIN_UNIT, :KEYIN_YY, :KEYIN_MM, :KEYIN_DD, :KEYIN_MAN, 
                                                   :LINE_ID, :JOB_PROJ_ATTR, :JOB_PROJ_KIND, :REMARK)";
            var result = cn.Execute(sql, value);
            return result;
        }

        /// <summary>
        /// 修改工作代號資料(包含工作代號\施工標號\施工標號)
        /// </summary>
        /// <param name="value">JOB_NO_LIST</param>
        /// <returns>成功交易筆數</returns>
        // PUT: api/PC_JobListProj
        public int Put([FromBody] JOB_NO_LIST value)
        {
            var cn = _conn.CreateConnection("PC");
            string sql = @"UPDATE  PC.JOB_NO_LIST SET
                                                    PROJ_NAME=:PROJ_NAME, PROJ_NAME_E=:PROJ_NAME_E, 
                                                    REQU_UNIT=:REQU_UNIT, REQU_MAN=:REQU_MAN, REQU_TEL=:REQU_TEL, REQU_CAUS=:REQU_CAUS,
                                                    KEYIN_UNIT=:KEYIN_UNIT, KEYIN_YY=:KEYIN_YY, KEYIN_MM=:KEYIN_MM, KEYIN_DD=:KEYIN_DD, 
                                                    KEYIN_MAN=:KEYIN_MAN, DISU_YY=:DISU_YY, DISU_MM=:DISU_MM, DISU_DD=:DISU_DD,
                                                    DISU_CAUS=:DISU_CAUS, LINE_ID=:LINE_ID, JOB_PROJ_ATTR=:JOB_PROJ_ATTR, 
                                                    JOB_PROJ_KIND=:JOB_PROJ_KIND, REMARK=:REMARK, PROJ_NAME_A=:PROJ_NAME_A, PROJ_NAME_C=:PROJ_NAME_C
                                   WHERE PROJ_ID=:PROJ_ID";
            var result = cn.Execute(sql, value);
            return result;
        }

        /// <summary>
        /// 刪除工作代號資料
        /// </summary>
        /// <param name="id">工作代號</param>
        /// <returns>成功交易筆數</returns>
        // DELETE: api/PC_JobListProj
        public int Delete(string id)
        {
            var cn = _conn.CreateConnection("PC");
            string sql = @"DELETE PC.JOB_NO_LIST SET
                                    WHERE PROJ_ID=:PROJ_ID";
            var result = cn.Execute(sql, new { PROJ_ID = id });
            return result;
        }
    }
}
