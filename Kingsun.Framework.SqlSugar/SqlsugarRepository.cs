using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.Framework.SqlSugar
{
    public class SqlsugarRepository
    {
        public string _operatorError = string.Empty;

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
                }
                return _connectionString;
            }
        }

        public SqlSugarClient GetInstance()
        {
            string connect = ConnectionString;
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connect, //必填
                DbType = DbType.SqlServer, //必填
                IsAutoCloseConnection = true

            }); //默认SystemTable

            return db;
        }

        public SqlSugarClient GetInstance(string connectionString)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString, //必填
                DbType = DbType.SqlServer, //必填
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.SystemTable
            }); //默认SystemTable
            return db;
        }


    }
}
