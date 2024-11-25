using Authorization.Models;
using System.Data;

namespace Authorization.Repositories
{
    public interface IRacunRepository
    {
        Task<int> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum, DataTable stavkeTable);
        Task<IEnumerable<Racun>> GetAllRacuniAsync();
        Task<Racun> GetRacunByIdAsync(int racunId);
        Task<IEnumerable<StavkeRacuna>> GetStavkeByRacunIdAsync(int racunId);
        Task<bool> UpdateRacunStatusAsync(int racunId, string noviStatus);
        Task<bool> DeleteRacunAsync(int racunId);
        Task<decimal> GetUkupnaCenaRacunaAsync(int racunId);
        Task<bool> UpdateStavkeRacunaAsync(int stavkaId, int newKolicina, decimal newPopust);
        Task<bool> UpdateStavkaAsync(int stavkaId, int newKolicina, decimal newPopust);
    }
}
