using Authorization.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Authorization.Repositories
{
    public class ProizvodRepository : IProizvodRepository
    {
        private readonly string _connectionString;

        public ProizvodRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Proizvod>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = " SELECT proizvodID, NazivProizvoda, Cena FROM Proizvod;";
            return await connection.QueryAsync<Proizvod>(query);
        }

        public async Task AddAsync(string nazivProizvoda, decimal cena, int kolicina)
        {
            using var connection = new SqlConnection(_connectionString);
            var paramteras = new DynamicParameters();
            paramteras.Add("@NazivProizvoda", nazivProizvoda);
            paramteras.Add("@Cena", cena);
            paramteras.Add("@Kolicina", kolicina);
            await connection.ExecuteAsync("dbo.AddProizvod", paramteras, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task DeleteAsync(int proizvodID)
        {
            using var connection = new SqlConnection(_connectionString);
            var paramteras = new DynamicParameters();
            paramteras.Add("@ProizvodID", proizvodID);
            await connection.ExecuteAsync("dbo.DeleteProizvod", paramteras, commandType: System.Data.CommandType.StoredProcedure);
        }


    }
}
