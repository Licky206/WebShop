using Authorization.Models;

namespace Authorization.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Proizvod>> GetAllProductsAsync();
        Task AddProductAsync(string nazivProizvoda, decimal cena, int kolicina);
        Task DeleteProductAsync(int proizvodId);

    }
}
