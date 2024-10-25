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
        //Kreiranje novog racuna
        [HttpPost("racun")]
        public IActionResult CreateRacun([FromBody] Racun racun)
        {
            racun.StatusRacuna = "U IZRADI";
            racun.Datum = DateTime.Now.Date;
            racun.Vreme = DateTime.Now.TimeOfDay;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var racunId = connection.ExecuteScalar<int>(
                "INSERT INTO Racun (StatusRacuna, Datum, Vreme) OUTPUT INSERTED.RacunId VALUES (@StatusRacuna, @Datum, @Vreme)",
                racun
            );

                racun.RacunId = racunId;
            }

            return CreatedAtAction(nameof(GetRacun), new { id = racun.RacunId }, racun);
        }

        //Kreiranje StavkeRacuna, odnosno prikaz racuna sa svim stavkama
        [HttpGet("{id}/detalji")]
        public IActionResult GetRacun(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var racun = connection.Query<Racun, StavkeRacuna, Proizvod, Racun>(
                     @"SELECT r.*, s.*, p.*
                      FROM Racun r
                      LEFT JOIN StavkeRacuna s ON r.RacunId = s.RacunId
                      LEFT JOIN Proizvod p ON s.ProizvodID = p.ProizvodID
                      WHERE r.RacunId = @id",
                      (rac, stavka, proizvod) =>
                      {
                          if (stavka != null)
                          {
                              stavka.Proizvod = proizvod;
                              rac.StavkeRacuna.Add(stavka);
                          }
                          return rac;
                      },
                      new { id },
                        splitOn: "StavkeRacunaID,ProizvodID"
                        ).FirstOrDefault();

                    if (racun == null)
                        return NotFound($"Račun sa ID-jem {id} ne postoji.");

                    return Ok(racun);
            }
            
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
    }
}
