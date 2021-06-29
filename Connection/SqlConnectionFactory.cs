using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace E0001.Connection
{
    public class SqlConnectionFactory
    {
        public IDbConnection CreateConnection(string name)
        {
            switch (name.ToUpper())
            {
                case "name":
                    {
                        // 此方法須於 Web.config 定義
                        var ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[name].ConnectionString;
                        return new SqlConnection(ConnectionString);
                    }

                case "PDCGSV03":
                    {
                        string db_source = "PDCGSV03";
                        string db_userid = "pmuser";
                        string db_password = "Pm#8021mp";
                        string database = "Dorts_TCGPSN";
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