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
        [HttpPost("Add Proizvod")]
        public async Task<IActionResult> AddProzivod(Proizvod proizvod)
        {
            var sql = @"INSERT INTO Proizvod (NazivProizvoda, Cena) 
                        VALUES (@NazivProizvoda, @Cena); 
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var id = await connection.QuerySingleAsync<int>(sql, proizvod);
                    return Ok(new { ProizvodID = id });
                }
                catch (Exception ex)
                {
                    return BadRequest(new {ex.Message });

                }
            }
        }

        [HttpPost("Add Racun ")]
        public async Task<IActionResult> AddRacun(Racun racun)
        {
            var sql = @"INSERT INTO Racun (StatusRacuna, Datum, Vreme) 
                VALUES (@StatusRacuna, @Datum, @Vreme); 
                SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>(sql, racun);
                return Ok(id);
            }
        }

        //BULK INSERT
        [HttpPost("bulk-insert")]
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

        //DODAVANJE U STAVKU RACUNA
        [HttpPost("dodaj-stavke/{racunId}")]
        public IActionResult DodajStavke(int racunId, [FromBody] List<StavkeRacuna> stavke)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                foreach (var stavka in stavke)
                {
                    var command = new SqlCommand("INSERT INTO StavkeRacuna (RacunID, ProizvodID, Kolicina, Popust) VALUES (@RacunID, @ProizvodID, @Kolicina, @Popust)", connection);
                    command.Parameters.AddWithValue("@RacunID", racunId);
                    command.Parameters.AddWithValue("@ProizvodID", stavka.Proizvod.ProizvodID);
                    command.Parameters.AddWithValue("@Kolicina", stavka.Kolicina);
                    command.Parameters.AddWithValue("@Popust", stavka.Popust);
                    command.ExecuteNonQuery();
                }
            }

            return Ok("Stavke su uspešno dodate.");
        }


        //Kreiranje RACUNA
        [HttpPost("KreirajRacun")]
        public async Task<IActionResult> KreirajRacun([FromBody] Racun racun)
        {
            var sql = @"
            INSERT INTO Racun (StatusRacuna, Datum, Vreme)
            VALUES (@StatusRacuna, @Datum, @Vreme);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var racunId = await connection.QuerySingleAsync<int>(sql, new
                {
                    racun.StatusRacuna,
                    racun.Datum,
                    racun.Vreme
                });

                return Ok(new { RacunID = racunId });
            }
        }



    }
}
