using Authorization.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Authorization.Controllers
{
    public class ProizvodControllercs : Controller
    {

        private readonly string _connectionString;

        public ProizvodControllercs(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet ("Get Proizvodi")]
        public async Task<ActionResult<IEnumerable<Proizvod>>> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var proizvodi = await connection.QueryAsync<Proizvod>("SELECT * FROM Proizvod");
                return Ok(proizvodi);

            }

        }
        [HttpGet("Prozvod from ID{id}")]
        public async Task<ActionResult<Proizvod>> GetProizvod(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var proizvod = await connection.QuerySingleOrDefaultAsync<Proizvod>("SELECT * FROM Proizvodi WHERE Id = @Id", new { Id = id });

                if (proizvod == null)
                {
                    return NotFound();
                }
                return Ok(proizvod);
            }
        }
        [HttpPost ("Dodaj jedan proizvod")]
        public async Task<ActionResult<Proizvod>> CreateProizvod([FromBody] Proizvod proizvod)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var id = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO Proizvodi (Naziv, Cena, Kolicina) VALUES (@Naziv, @Cena, @Kolicina); SELECT CAST(SCOPE_IDENTITY() as int);", proizvod);
                proizvod.ProizvodID = id;
                return CreatedAtAction(nameof(GetProizvod), new { id = proizvod.ProizvodID }, proizvod);
            }
        }



        //BULK INSERT
        [HttpPost("Dodavanja vise Proizvoda od jednog - BULK INSERT")]
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


        [HttpPut(" UPDATE Proizvod po ID{id}")]
        public async Task<ActionResult> UpdateProizvod(int id, [FromBody] Proizvod proizvod)
        {
            if (id != proizvod.ProizvodID)
            {
                return BadRequest();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(
                    "UPDATE Proizvodi SET Naziv = @Naziv, Cena = @Cena, Kolicina = @Kolicina WHERE Id = @Id", proizvod);
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
        }
        [HttpDelete("DELETE Proizvod po ID {id}")]
        public async Task<ActionResult> DeleteProizvod(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync("DELETE FROM Proizvodi WHERE Id = @Id", new { Id = id });
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
        }
    }
}
