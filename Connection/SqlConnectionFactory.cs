using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace B0002.Connection
{
    public class SqlConnectionFactory
    {
        public IDbConnection CreateConnection(string name)
        {
            switch (name.ToUpper())
            {
                case "WWWDB2":
                    {
                        var ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[name].ConnectionString;
                        return new SqlConnection(ConnectionString);
                    }
                case "DVDBSV01":
                    {
                        //測試SQL Server PP人事資料庫

                        string db_source = name;
                        string db_userid = "parkdba";
                        string db_password = "Abcd1234";
                        string database = "PARK_T";
                        string ConnectionString = $@"Server={db_source};
                                                     Database={database};
                                                     MultipleActiveResultSets=true;
                                                     User ID={db_userid};
                                                     Password={db_password}";
                        return new SqlConnection(ConnectionString);
                    }
                default:
                    {
                        throw new Exception("name 不存在。");
                    }
            }
        }
    }
}