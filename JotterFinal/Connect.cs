using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JotterFinal
{
    internal class Connect
    {
        SqlConnection conn;
        public SqlConnection GetCon()
        {
            conn = new SqlConnection("Data Source=LAPTOP-40R7JHFO\\SQLEXPRESS;Initial Catalog=myNewDb;Integrated Security=True");
            return conn;

        }
    }
}
