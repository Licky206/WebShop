using Authorization.Models;
using Authorization.Repositories;

namespace Authorization.Services
{
    public class ProizvodService : IProizvodService
    {
        private readonly IProizvodRepository _proizvodRepository;

        public ProizvodService(IProizvodRepository proizvodRepository)
        {
            _proizvodRepository = proizvodRepository;
        }


        public async Task AddProductAsync(string nazivProizvoda, decimal cena, int kolicina)
        {
            await _proizvodRepository.AddAsync(nazivProizvoda, cena, kolicina);
        }

        public async Task DeleteProductAsync(int proizvodId)
        {
            await _proizvodRepository.DeleteAsync(proizvodId);
        }

        public async Task<IEnumerable<Proizvod>> GetAllAsync()
        {
            return await _proizvodRepository.GetAllAsync();
        }
    }
}
