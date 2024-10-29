using Authorization.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace Authorization.Controllers
{
    public class RacunController : Controller
    {

        private readonly string _connectionString;
        public RacunController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("dodaj-racun")]
        public async Task<IActionResult> DodajRacun([FromBody] List<Proizvod> izabraniProizvodi)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var racunId = await connection.ExecuteScalarAsync<int>(

                    "INSERT INTO Racun (Datum) OUTPUT INSERTED.RacunID VALUES (GETDATE())"
                );

                //datatable od stavke
                var stavkeDataTable = new DataTable();
                stavkeDataTable.Columns.Add("RacunId", typeof(int));
                stavkeDataTable.Columns.Add("ProizvodID", typeof(int));
                stavkeDataTable.Columns.Add("Kolicina", typeof(int));
                stavkeDataTable.Columns.Add("Popust", typeof(int));

                foreach (var proizvod in izabraniProizvodi)
                {
                    stavkeDataTable.Rows.Add(racunId, proizvod.ProizvodID, proizvod.Kolicina, 0);
                }

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "StavkeRacuna";
                    bulkCopy.ColumnMappings.Add("RacunId", "RacunId");
                    bulkCopy.ColumnMappings.Add("ProizvodID", "ProizvodID");
                    bulkCopy.ColumnMappings.Add("Kolicina", "Kolicina");
                    bulkCopy.ColumnMappings.Add("Popust", "Popust");
                    

                    await bulkCopy.WriteToServerAsync(stavkeDataTable);

                }
            }
            return Ok("Račun je kreiran i stavke su dodate.");

        }

        //Uzimanje Racuna
        [HttpGet("racun/ {racunId}")]
        public async Task<ActionResult<Racun>> GetRacun(int racunId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var racun = await connection.QuerySingleOrDefaultAsync<Racun>("SELECT * FROM Racun WHERE RacunID = @Id", new { Id = racunId });
                if (racun == null)
                {
                    return NotFound();
                }

                return Ok(racun);
            }
        }
        //// Prikaz stavki
        //[HttpGet("stavke-racuna/{racunId}")]
        //public async Task<ActionResult<IEnumerable<StavkeRacuna>>> GetStavkeRacuna(int racunId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var stavke = await connection.QueryAsync<StavkeRacuna>("SELECT * FROM StavkeRacuna WHERE RacunID = @Id", new { Id = racunId });
        //        return Ok(stavke);
        //    }
        //}



        //   Stavke racuna po RACUN ID
        [HttpGet(" Pokazi StavkeRacuna za radunID :StavkeRacuna/{racunId}")]
        public async Task<ActionResult<IEnumerable<StavkeRacuna>>> GetStavkeRacuna(int racunId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var stavke = await connection.QueryAsync<StavkeRacuna, Proizvod, StavkeRacuna>(
                    @"SELECT sr.*, p.ProizvodID, p.NazivProizvoda, p.Cena
                      FROM StavkeRacuna sr
                      LEFT JOIN Proizvod p ON sr.ProizvodID = p.ProizvodID
                      WHERE sr.RacunID = @RacunID",
                    (stavka, proizvod) =>
                    {
                        stavka.Proizvod = proizvod;
                        return stavka;
                    },
                   new { RacunID = racunId },
                    splitOn: "ProizvodID"
                );

                if (stavke == null || !stavke.Any())
                {
                    return NotFound("Nema stavki za dati račun.");
                }

                return Ok(stavke);
            }
        }
    }
}
