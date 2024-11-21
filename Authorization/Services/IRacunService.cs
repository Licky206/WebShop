using Authorization.Models;

namespace Authorization.Services
{
    public interface IRacunService
    {
        Task<bool> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum, IEnumerable<StavkeRacunaDTO> stavke);
        Task<IEnumerable<Racun>> GetAllRacuniAsync();
        Task<IEnumerable<StavkeRacuna>> GetStavkeByRacunIdAsync(int racunId);
        Task<Racun> GetRacunByIdAsync(int racunId); 
        Task<bool> DeleteRacunAsync(int racunId);
        Task<bool> UpdateRacunStatusAsync(int racunId, string statusRacuna);
    }
}
