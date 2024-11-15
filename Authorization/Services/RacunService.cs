using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Authorization.Models;

namespace Authorization.Services
{
    public class RacunService : IRacunService
    {
        private readonly string _connectionString;

        public RacunService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum, TimeSpan? vreme, IEnumerable<StavkeRacunaDTO> stavke)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var stavkeTable = new DataTable();
                stavkeTable.Columns.Add("RacunId", typeof(int));
                stavkeTable.Columns.Add("ProizvodID", typeof(int));
                stavkeTable.Columns.Add("Kolicina", typeof(int));
                stavkeTable.Columns.Add("Popust", typeof(decimal));

                foreach (var stavka in stavke)
                {
                    stavkeTable.Rows.Add(0,stavka.ProizvodID, stavka.Kolicina, stavka.Popust);
                }

                var parameters = new DynamicParameters();
                parameters.Add("@StatusRacuna", statusRacuna);
                parameters.Add("@Datum", datum);
                parameters.Add("@Vreme", vreme);
                parameters.Add("@Stavke", stavkeTable.AsTableValuedParameter("StavkeRacunaTableType"));

                var result = await connection.ExecuteAsync("BulkInsertRacunAndStavke", parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }

    }
}
