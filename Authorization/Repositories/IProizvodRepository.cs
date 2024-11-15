using Authorization.Models;

namespace Authorization.Repositories
{
    public interface IProizvodRepository
    {
        Task<IEnumerable<Proizvod>> GetAllAsync();
        Task AddAsync(string nazivProizvoda, decimal cena, int kolicina);
        Task DeleteAsync(int proizvodId);
    }
}
