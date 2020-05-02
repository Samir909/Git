using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MusicProject.GetWay
{
    public class BaseGateway
    {
        public SqlConnection Connection { get; set; }
        public SqlCommand Command { get; set; }
        public SqlDataReader Reader { get; set; }
        private string connectionString = WebConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public BaseGateway()
        {
            Connection = new SqlConnection(connectionString);
        }
    }
}