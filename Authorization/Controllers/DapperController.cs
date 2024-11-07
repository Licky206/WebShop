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
        [HttpGet]
        public async Task GetProizvod()
        {

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

            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("dbo.AddProizvod", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        [HttpPost("CreateRacun")]
        public async Task CreateRacun([FromBody] StatusRacuna statusRacuna)
        {
            var trenutniDatum = DateTime.Now.Date;
            var trenutnoVreme = DateTime.Now.TimeOfDay;

            var parameters = new DynamicParameters();
            parameters.Add("@StatusRacuna", statusRacuna.ToString().Replace('_', ' ')); 
            parameters.Add("@Datum", trenutniDatum);
            parameters.Add("Vreme", trenutnoVreme);

            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("dbo.CreateRacun", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        [HttpPost("AddStavkaRacuna")]
        public async Task AddStavkaRacuna(int racunId, int proizvodId, int kolicina, decimal popust)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RacunID", racunId);
            parameters.Add("@ProizvodID", proizvodId);
            parameters.Add("@Kolicina", kolicina);
            parameters.Add("@Popust", popust);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("dbo.AddStavkaRacuna", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        [HttpPost("ZavrsiRacun")]
        public async Task<IActionResult> ZavrsiRacun([FromBody] Korpa korpa)
        {
            // Validacija da korpa ima stavke
            if (!korpa.Stavke.Any())
            {
                return BadRequest("Korpa je prazna. Dodajte stavke pre završetka računa.");
            }

            // Uzimanje trenutnog datuma i vremena
            var trenutniDatum = DateTime.Now.Date;
            var trenutnoVreme = DateTime.Now.TimeOfDay;

            // Kreiranje računa
            var parameters = new DynamicParameters();
            parameters.Add("@StatusRacuna", "U IZRADI"); // ili drugi status
            parameters.Add("@Datum", trenutniDatum);
            parameters.Add("@Vreme", trenutnoVreme);

            using (var connection = new SqlConnection(_connectionString))
            {
                var racunId = await connection.ExecuteScalarAsync<int>("dbo.CreateRacun", parameters, commandType: CommandType.StoredProcedure);

                // Proveri da li je racunId validan
                if (racunId <= 0)
                {
                    return StatusCode(500, "Greška prilikom kreiranja računa.");
                }

                // Dodavanje stavki računa
                foreach (var stavka in korpa.Stavke)
                {
                    var stavkaParameters = new DynamicParameters();
                    stavkaParameters.Add("@RacunId", racunId);
                    stavkaParameters.Add("@ProizvodID", stavka.ProizvodID);
                    stavkaParameters.Add("@Kolicina", stavka.Kolicina);
                    stavkaParameters.Add("@Popust", stavka.Popust);

                    await connection.ExecuteAsync("dbo.AddStavkaRacuna", stavkaParameters, commandType: CommandType.StoredProcedure);
                }
            }

            // Očisti korpu nakon uspešnog dodavanja
            korpa.OcistiKorpu();

            return Ok("Račun je uspešno kreiran sa stavkama.");
        }
        public enum StatusRacuna
        {
            U_IZDRADI,
            STORNIRAN,
            FISKLAIZIRAN
        }

    }
}