using Authorization.Models;

namespace Authorization.Services
{
    public interface IRacunService
    {
        Task<bool> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum,  IEnumerable<StavkeRacunaDTO> stavke);
    }
}
