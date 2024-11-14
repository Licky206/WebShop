using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Authorization.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly string _connectionString;

        public InvoiceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateInvoiceAsync(string statusRacuna, DateTime datum, TimeSpan vreme)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@StatusRacuna", statusRacuna);
            parameters.Add("@Datum", datum);
            parameters.Add("@Vreme", vreme);
            return await connection.ExecuteScalarAsync<int>("dbo.CreateRacun", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddInvoiceWithItemsAsync(string statusRacuna, DateTime datum, TimeSpan vreme, DataTable stavkeRacuna)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@StatusRacuna", statusRacuna);
            parameters.Add("@Datum", datum);
            parameters.Add("@Vreme", vreme);
            parameters.Add("@StavkeRacuna", stavkeRacuna.AsTableValuedParameter("StavkeRacunaTableType"));
            return await connection.ExecuteAsync("BulkInsertRacunAndStavke", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}