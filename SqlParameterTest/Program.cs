using SqlParameterTest.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SqlParameterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("ADO method test:" + AdoConnMethod());
            //Console.WriteLine("======");

            //Console.WriteLine("EntitiyFramework SqlParameter use ListCollection method test:");
            //Console.WriteLine(EfParaListMethod());
            //Console.WriteLine("======");

            //Console.WriteLine("EntitiyFramework SqlParameter Implenment method test:");
            //Console.WriteLine(EfParaImpMethod());
            //Console.WriteLine("======");

            //Console.WriteLine("EntitiyFramework SqlParameter Literal method test:");
            //Console.WriteLine(EfParaLiteralMethod());
            //Console.WriteLine("======");

            string connStr;
            string str = "msdb.dbo.sp_send_dbmail";
            using (AdventureWorks2012Entities context = new AdventureWorks2012Entities())
            {
                connStr = context.Database.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@profile_name", "SendMail"));
                        cmd.Parameters.Add(new SqlParameter("@recipients", "test@test.com;test@test.com"));
                        cmd.Parameters.Add(new SqlParameter("@subject", "ADO method 2 test"));
                        cmd.Parameters.Add(new SqlParameter("@body", @"Hello World"));
                        cmd.Parameters.Add(new SqlParameter("@importance", "NORMAL"));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            Console.ReadKey();
        }

        /// <summary>
        /// ADOs the connection method.
        /// </summary>
        /// <returns></returns>
        public static object AdoConnMethod()
        {
            int result;
            string connStr;
            string str = "msdb.dbo.sp_send_dbmail";
            using (AdventureWorks2012Entities context = new AdventureWorks2012Entities())
            {
                connStr = context.Database.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;

                        // Join Parameters have multiple method, choose following any method to use, the result is the same.
                        // join parameters method 1
                        //cmd.Parameters.Add("@profile_name", SqlDbType.VarChar).Value = "SendMail";
                        //cmd.Parameters.Add("@recipients", SqlDbType.VarChar).Value = "test@test.com";
                        //cmd.Parameters.Add("@subject", SqlDbType.VarChar).Value = "ADO method 1 test";
                        //cmd.Parameters.Add("@body", SqlDbType.VarChar).Value = "Hello World";
                        //cmd.Parameters.Add("@importance", SqlDbType.VarChar).Value = "NORMAL";

                        // join parameters method 2
                        cmd.Parameters.Add(new SqlParameter("@profile_name", "SendMail"));
                        cmd.Parameters.Add(new SqlParameter("@recipients", "test@test.com;test@test.com"));
                        cmd.Parameters.Add(new SqlParameter("@subject", "ADO method 2 test"));
                        cmd.Parameters.Add(new SqlParameter("@body", @"Hello World"));
                        cmd.Parameters.Add(new SqlParameter("@importance", "NORMAL"));

                        // join parameters method 3
                        //cmd.Parameters.AddWithValue("@profile_name", "SendMail");
                        //cmd.Parameters.AddWithValue("@recipients", "test@test.com;test@test.com;test@test.com");
                        //cmd.Parameters.AddWithValue("@subject", "ADO method 3 test");
                        //cmd.Parameters.AddWithValue("@body", @"Hello World");
                        //cmd.Parameters.AddWithValue("@importance", "NORMAL");
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Efs the para list method.
        /// </summary>
        public static int EfParaListMethod()
        {
            int result;
            
            using (AdventureWorks2012Entities context = new AdventureWorks2012Entities())
            {
                string queryString = @"EXEC msdb.dbo.sp_send_dbmail @profile_name, @recipients, @copy_recipients, @blind_copy_recipients, @subject, @body, @body_format, @importance";
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@profile_name", "SendMail"));
                parameterList.Add(new SqlParameter("@recipients", "test@test.com;test@test.com"));
                parameterList.Add(new SqlParameter("@copy_recipients", DBNull.Value));
                parameterList.Add(new SqlParameter("@blind_copy_recipients", DBNull.Value));
                parameterList.Add(new SqlParameter("@subject", "EntityFramework Parameters ListCollection Method test"));
                parameterList.Add(new SqlParameter("@body", @"Hello World"));
                parameterList.Add(new SqlParameter("@body_format", "HTML"));
                parameterList.Add(new SqlParameter("@importance", "NORMAL"));
                SqlParameter[] parameters = parameterList.ToArray();
                result = context.Database.ExecuteSqlCommand(queryString, parameters);
            }
            return result;
        }

        /// <summary>
        /// Efs the para imp method.
        /// </summary>
        public static int EfParaImpMethod()
        {
            int result;
            using (AdventureWorks2012Entities context = new AdventureWorks2012Entities())
            {
                string queryString = @"EXEC msdb.dbo.sp_send_dbmail @profile_name, @recipients, @copy_recipients, @blind_copy_recipients, @subject, @body, @body_format, @importance";
                var profile = new SqlParameter()
                {
                    ParameterName = "profile_name",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 128,
                    Value = "SendMail"
                };
                var recipients = new SqlParameter()
                {
                    ParameterName = "recipients",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "test@test.com;test@test.com"
                };
                var subject = new SqlParameter()
                {
                    ParameterName = "subject",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 255,
                    Value = "EntitiyFramework Parameter Implenment method test"
                };
                var body = new SqlParameter()
                {
                    ParameterName = "body",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = "Hello World"
                };
                var importance = new SqlParameter()
                {
                    ParameterName = "importance",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 6,
                    Value = "NORMAL"
                };
                result = context.Database.ExecuteSqlCommand(queryString, profile, recipients, null, null, subject, body, null, importance);
            }
            return result;
        }

        /// <summary>
        /// Efs the para literal method.
        /// </summary>
        public static int EfParaLiteralMethod()
        {
            int result;
            using (AdventureWorks2012Entities context = new AdventureWorks2012Entities())
            {
                string queryString = @"EXEC msdb.dbo.sp_send_dbmail @profile_name, @recipients, @subject, @body, @importance";
                result = context.Database.ExecuteSqlCommand(queryString,
                    new SqlParameter("@profile_name", "SendMail"),
                    new SqlParameter("@recipients", "test@test.com;test@test.com"),
                    new SqlParameter("@subject", "EntityFramework Parameters Literal Method test"),
                    new SqlParameter("@body", "Hello World"),
                    new SqlParameter("@importance", "NORMAL"));
            }
            return result;
        }
    }
}
