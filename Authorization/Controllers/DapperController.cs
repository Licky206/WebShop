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
                    stavka.Proizvod = proizvod;  
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