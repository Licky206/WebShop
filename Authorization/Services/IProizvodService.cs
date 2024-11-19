using Authorization.Models;

namespace Authorization.Services
{
    public interface IProizvodService
    {

        Task<IEnumerable<Proizvod>> GetAllAsync();
        Task AddProductAsync(string nazivProizvoda, decimal cena, int kolicina);
        Task DeleteProductAsync(int proizvodId);
    }
}
