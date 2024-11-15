using Authorization.Models;

namespace Authorization.Services
{
    public interface IProizvodService
    {

        Task<IEnumerable<Proizvod>> GetProizvodAsync();
        Task AddProductAsync(string nazivProizvoda, decimal cena, int kolicina);
        Task DeleteProductAsync(int proizvodId);
    }
}
