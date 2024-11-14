using Authorization.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Authorization.Models;

namespace Authorization.Controllers
{
    public class DapperController : Controller
    {

        private readonly string _connectionString;

        public DapperController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        [HttpGet("Proizvodi")]
        public async Task<ActionResult<IEnumerable<Proizvod>>> GetProizvodi()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var proizvodi = await connection.QueryAsync<Proizvod>("GetProizvodi", commandType: CommandType.StoredProcedure);
                return Ok(proizvodi);
            }
        }

        [HttpPost("Dodaj proizvode")]
        public async Task AddProizvod(string nazivProizvoda, decimal cena, int kolicina)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@NazivProizvoda", nazivProizvoda);
            parameters.Add("@Cena", cena);
            parameters.Add("Kolicina", kolicina);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("dbo.AddProizvod", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        [HttpPost("CreateRacun")]
        public async Task<IActionResult> CreateRacun()
        {
            var trenutniDatum = DateTime.Now.Date;
            var trenutnoVreme = DateTime.Now.TimeOfDay;

            var parameters = new DynamicParameters();
            parameters.Add("@StatusRacuna", "U IZRADI");
            parameters.Add("@Datum", trenutniDatum);
            parameters.Add("Vreme", trenutnoVreme);

            using (var connection = new SqlConnection(_connectionString))
            {
                var racunId = await connection.ExecuteScalarAsync<int>("dbo.CreateRacun", parameters, commandType: CommandType.StoredProcedure);
                return Ok(new { racunId = racunId });
            }
        }

        [HttpPost("KreirajRacunSaStavkama")]
        public async Task<IActionResult> KreirajRacunSaStavkama(string statusRacuna, DateTime Datum, DateTime Vreme, IEnumerable<StavkeRacunaDTO> stavkeRacuna)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var statusRacunaTable = new DataTable();
                    statusRacunaTable.Columns.Add("RacunId", typeof(int)); // This will be populated in stored procedure
                    statusRacunaTable.Columns.Add("ProizvodID", typeof(int));
                    statusRacunaTable.Columns.Add("Kolicina", typeof(int));
                    statusRacunaTable.Columns.Add("Popust", typeof(decimal));

                    // Add Stavke data to the table-valued parameter
                    foreach (var stavka in stavkeRacuna)
                    {
                        statusRacunaTable.Rows.Add(0, stavka.ProizvodID, stavka.Kolicina, stavka.Popust); // RacunId will be filled in the SP
                    }

                    var parametars = new DynamicParameters();
                    parametars.Add("@StatusRacuna", statusRacuna);
                    parametars.Add("@Datum", Datum);
                    parametars.Add("@Vreme", Vreme);
                    parametars.Add("@StavkeRacuna", statusRacunaTable.AsTableValuedParameter("StavkeRacunaTableType"));

                    // Execute the stored procedure
                    var result = await connection.ExecuteAsync("BulkInsertRacunAndStavke", parametars, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        return Ok(new { message = "Invoice and items created successfully." });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to create invoice or insert items." });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the invoice.", details = ex.Message });
            }
        }


        [HttpDelete("DeleteProizvod/{proizvodId}")]
        public async Task<IActionResult> DeleteProizvod(int proizvodId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ProizvodID", proizvodId);

                try
                {
                    await connection.ExecuteAsync("dbo.DeleteProizvod", parameters, commandType: CommandType.StoredProcedure);
                    return Ok("Proizvod uspešno obrisan.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
        public class KreirajRacunSaStavkamaRequest
        {
            public int KorisnikID { get; set; }
            public List<StavkeRacuna> Proizvodi { get; set; }
        }

        public enum StatusRacuna
        {
            U_IZDRADI,
            STORNIRAN,
            FISKLAIZIRAN
        }
    }

}
