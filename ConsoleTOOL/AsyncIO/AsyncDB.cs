using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncIO
{
    public class AsyncDB
    {
        public async static Task ProcessAsynchronousIO()
        {
            const string connectionString = @"Data Source=XEEKS-LUOTAO\SQLEXPRESS;Initial Catalog=hangfire;Persist Security Info=True;User ID=dtauser;Password=dtauser";
            string sqlText = "select count(*) from tb_cfg_cell_lte";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using(SqlCommand command = new SqlCommand(sqlText,conn))
                {
                    var res = await command.ExecuteScalarAsync();
                    Console.WriteLine(res);
                }
            }
        }
        public async static Task InvodeAsyncDB()
        {
            await ProcessAsynchronousIO();
        }

    }
}
