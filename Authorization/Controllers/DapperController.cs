using Authorization.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Authorization.Controllers
{
    public class DapperController : Controller
    {
        private readonly string _connectionString;

        public DapperController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IActionResult Index()
        {
            return View();
        }

        // Prikaz računa
        [HttpPost("KreirajRacun")]
        public IActionResult KreirajRacun([FromBody] List<Proizvod> proizvodi)
        {
            var racun = new Racun
            {
                StatusRacuna = "U IZDRADI",
                Datum = DateTime.Now.Date,
                Vreme = DateTime.Now.TimeOfDay,
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                 
                var racunId = connection.ExecuteScalar<int>(
                    "INSERT INTO Racun (StatusRacuna, Datum, Vreme) OUTPUT INSERTED.RacunId VALUES (@StatusRacuna, @Datum, @Vreme)",
                    new
                    {
                        StatusRacuna = racun.StatusRacuna,
                        Datum = racun.Datum,
                        Vreme = racun.Vreme
                    }
                );

                foreach (var proizvod in proizvodi)
                {
                    connection.Execute(
                        "INSERT INTO StavkeRacuna (RacunID, Kolicina, Popust, ProizvodID) VALUES (@RacunID, @Kolicina, @Popust, @ProizvodID)",
                        new
                        {
                            RacunID = racunId, 
                            Kolicina = 1,  
                            Popust = 0, 
                            ProizvodID = proizvod.ProizvodID
                        });
                }
            }

            return CreatedAtAction(nameof(KreirajRacun), new { id = racun.RacunId }, racun);
        }
        [HttpPost("Dodavanja-vise-Proizvoda")]
        public IActionResult BulkInsert([FromBody] List<Proizvod> proizvodi)
        {
            // Check if proizvodi is null
            if (proizvodi == null)
            {
                return BadRequest("Proizvodi cannot be null.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("BulkIns_Proizvod", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Create a DataTable to hold the products
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("ProizvodID", typeof(int));
                    dataTable.Columns.Add("NazivProizvoda", typeof(string));
                    dataTable.Columns.Add("Cena", typeof(decimal));

                    // Populate the DataTable
                    foreach (var proizvod in proizvodi)
                    {
                        dataTable.Rows.Add(proizvod.NazivProizvoda, proizvod.Cena);
                    }

                    // Add the DataTable as a parameter
                    var parameter = new SqlParameter("@Proizvodi", SqlDbType.Structured)
                    {
                        TypeName = "dbo.ProizvodTableType", // Make sure this matches your SQL Server Table Type
                        Value = dataTable
                    };
                    command.Parameters.Add(parameter);

                    // Execute the command
                    command.ExecuteNonQuery();
                }
            }

            return Ok("Proizvodi su uspešno dodati.");
        }

    }
}