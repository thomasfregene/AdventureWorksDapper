using AdventureWorksDapper.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksDapper.Controllers
{
    [ApiController]
    [Route("api/dapper")]
    public class DapperController : Controller
    {
        private const string CONNECTION_STRING = "Server=HICAD-TOM\\SQLEXPRESS;Database=AdventureWorks;Trusted_Connection=True;";

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]bool getKelvins)
        {
            var sql = @"INSERT INTO [dbo].[TestTable]
                           ([Foobar])
                            VALUES
                           (@foobar)";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                for (int x = 0; x < 1000; x++)
                {
                    await connection.ExecuteAsync(sql, new { foobar = $"testing {DateTime.UtcNow.Ticks}" });
                }
                return Ok();
            }
            //return Ok();
        }

      

        private static async Task ExecuteAsyncWithInsert(string sqlConnection)
        {
            await using var connection = new SqlConnection(sqlConnection);
            var sql = @"INSERT INTO [Person].[Person]
                         ([Title]
                          ,[FirstName]
                          ,[MiddleName]
                          ,[LastName])
                          VALUES
                           (@Title, @FirstName,@MiddleName,@LastName)";
            var person = new Person
            {
                Title = "Mr.",
                FirstName = "Jude",
                LastName = "Akhimien",
                MiddleName = "Odianose",
            };
            var result = connection.ExecuteAsync(sql, person);

        }

        private static async Task StoredProcedure(string connectionString)
        {
            await using var connection = new SqlConnection(connectionString);
            var sqlProc = "dbo.uspGetBillOfMaterials";
            var results = await connection.QueryAsync<string>(sqlProc, commandType: System.Data.CommandType.StoredProcedure);
        }

        private static async Task ExecuteAsyncWithUpdate(string connectionString)
        {
            await using var connection = new SqlConnection(connectionString);
            var sql = @"Update [Person].[Person]
                         SET Title = @Title
                          WHERE FirstName=@firstName";

            var result = await connection.ExecuteAsync(sql, new { title = "Mr.", firstName = "Jude" });
        }

        private static async Task ExecuteAsyncWithDelete(string connectionString)
        {
            await using var connection = new SqlConnection(connectionString);

            var sql = @"DELETE FROM [Person].[Person]
                          WHERE FirstName=@firstName";
            var result = await connection.ExecuteAsync(sql, new { firstName = "Jude" });
        }
    }
}
