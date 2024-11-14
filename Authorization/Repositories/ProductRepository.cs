using Authorization.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Authorization.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Proizvod>> GetAllProductsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Proizvod>("GetProizvodi", commandType: CommandType.StoredProcedure);
        }

        public async Task AddProductAsync(string nazivProizvoda, decimal cena, int kolicina)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@NazivProizvoda", nazivProizvoda);
            parameters.Add("@Cena", cena);
            parameters.Add("@Kolicina", kolicina);
            await connection.ExecuteAsync("dbo.AddProizvod", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteProductAsync(int proizvodId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@ProizvodID", proizvodId);
            await connection.ExecuteAsync("dbo.DeleteProizvod", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
