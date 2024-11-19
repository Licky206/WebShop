using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Authorization.Models;

namespace Authorization.Repositories
{
    public class RacunRepository : IRacunRepository
    {
        private readonly string _connectionString;

        public RacunRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum, DataTable stavkeTable)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StatusRacuna", statusRacuna);
                parameters.Add("@Datum", datum);
                parameters.Add("@Stavke", stavkeTable.AsTableValuedParameter("StavkeRacunaTableType"));

                return await connection.ExecuteAsync(
                    "BulkInsertRacunAndStavke",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
