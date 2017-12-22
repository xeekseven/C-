using MASTERCOM.RAMS.BGR.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ImageDe_Compress
{
    class Program
    {
        static void Main(string[] args)
        {      
            System.Data.SqlTypes.SqlBytes bytes = null;
            int imageNum = 0;
            using (SqlConnection conn = new SqlConnection("Data Source=192.168.2.81;Initial Catalog=RAMSSERVER;User ID=xianuser;Password=xianuser"))
            {
                String sql = "select * from tb_lte_stati_amr_log where ifileid = 804302556";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        bytes = dr.GetSqlBytes(imageNum+7);//3是varbinary列从零开始的列序号。bytes.Stream能返回一个用于读取的Stream对象
                    }
                }
            }
            byte[] byteSource = bytes.Buffer;
            byte[] byteDecompress = ZlibHelper.DecompressBytes(byteSource);
            string imageStr = BitConverter.ToString(byteDecompress).Replace("-","");
            string gongText = "5F2502XXX";
            bool flog = imageStr.Contains(gongText);
            if (byteDecompress != null)
            {
                 Console.WriteLine(imageStr);
            }
            
            Console.ReadKey();

        }
        
    }
}
