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
            // Kreiraj novi račun
            var racun = new Racun
            {
                StatusRacuna = "U IZDRADI",
                Datum = DateTime.Now.Date,
                Vreme = DateTime.Now.TimeOfDay,
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Umetni račun u bazu i uzmi njegov ID
                var racunId = connection.ExecuteScalar<int>(
                    "INSERT INTO Racun (StatusRacuna, Datum, Vreme) OUTPUT INSERTED.RacunId VALUES (@StatusRacuna, @Datum, @Vreme)",
                    new
                    {
                        StatusRacuna = racun.StatusRacuna,
                        Datum = racun.Datum,
                        Vreme = racun.Vreme
                    }
                );

                // Umetni stavke računa
                foreach (var proizvod in proizvodi)
                {
                    connection.Execute(
                        "INSERT INTO StavkeRacuna (RacunID, Kolicina, Popust, ProizvodID) VALUES (@RacunID, @Kolicina, @Popust, @ProizvodID)",
                        new
                        {
                            RacunID = racunId, // ID kreiranog računa
                            Kolicina = 1, // Ovde postavi količinu prema potrebi
                            Popust = 0, // Ovde postavi popust prema potrebi
                            ProizvodID = proizvod.ProizvodID
                        });
                }
            }

            return CreatedAtAction(nameof(KreirajRacun), new { id = racun.RacunId }, racun);
        }


        //BULK INSERT
        [HttpPost("Dodavanja Proizvoda - bulk insert")]
        public IActionResult BulkInsert([FromBody] List<Proizvod> proizvodi)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Proizvod";
                    bulkCopy.ColumnMappings.Add("ProizvodID", "ProizvodID");
                    bulkCopy.ColumnMappings.Add("NazivProizvoda", "NazivProizvoda");
                    bulkCopy.ColumnMappings.Add("Cena", "Cena");

                    var dataTable = new DataTable();
                    dataTable.Columns.Add("ProizvodID", typeof(int));
                    dataTable.Columns.Add("NazivProizvoda", typeof(string));
                    dataTable.Columns.Add("Cena", typeof(int));

                    foreach (var proizvod in proizvodi)
                    {
                        dataTable.Rows.Add(proizvod.ProizvodID, proizvod.NazivProizvoda, proizvod.Cena);
                    }
                    bulkCopy.WriteToServer(dataTable);
                }
            }
            return Ok("Proizvodi su uspešno dodati.");
        }

        [HttpGet("StavkeRacuna/{racunId}")]
        public IActionResult GetStavkeRacuna(int racunId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var stavke = connection.Query<StavkeRacuna, Proizvod, StavkeRacuna>(
                @"SELECT s.*, p.* 
                  FROM StavkeRacuna s
                  LEFT JOIN Proizvod p ON s.ProizvodID = p.ProizvodID
                  WHERE s.RacunID = @RacunID",
                (stavka, proizvod) =>
                {
                    stavka.Proizvod = proizvod; // Poveži proizvod sa stavkom
                    return stavka;
                },
                new { RacunID = racunId },
                splitOn: "ProizvodID"
                ).ToList();


                if (!stavke.Any())
                {
                    return NotFound("nema stavki za racunem sa id {racunId}");
                }

                return Ok(stavke);
            }
        }

    }
}