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
        public async Task<IActionResult> Index()
        {
            var sql = @"SELECT 
                          [PersonType]
                          ,[NameStyle]
                          ,[Title]
                          ,[FirstName]
                          ,[MiddleName]
                          ,[LastName]
                      FROM [Person].[Person]
                      WHERE FirstName=@firstName";
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var persons = await connection.QueryAsync<Person>(sql, new { firstName="Kelvin"});
                return Ok(persons);
            }
            //return Ok();
        }
    }
}
