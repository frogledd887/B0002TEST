using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace E0001.Connection
{
    public class OracleConnectionFactory
    {
        public IDbConnection CreateConnection(string dbName)
        {
            string db_source;
            string db_userid;
            string db_password;

            switch (dbName.ToUpper())
            {
                case "PC1":
                    {
                        db_source = "PDDBP701";
                        db_userid = "JPCUSER";
                        db_password = "D10818";
                        break;
                    }

                case "PC":
                    {
                        db_source = "DVDBP701";
                        db_userid = "PC";
                        db_password = "AAAAAA";
                        break;
                    }
                case "EM":
                    {
                        db_source = "DVDBP701";
                        db_userid = "EM";
                        db_password = "AAAAAA";
                        break;
                    }
                default:
                    {
                        throw new Exception("dbName 不存在。");
                    }
            }
            string ConnectionString = $@"DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={db_source})(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME={db_source})));Persist Security Info=True;User ID={db_userid};Password={db_password}";
            return new OracleConnection(ConnectionString);
        }
    }
}
