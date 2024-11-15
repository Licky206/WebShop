using Authorization.Models;

namespace Authorization.Services
{
    public interface IRacunService
    {
        Task<bool> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum, TimeSpan? vreme, IEnumerable<StavkeRacunaDTO> stavke);
    }
}
