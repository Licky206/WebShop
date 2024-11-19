using System.Data;

namespace Authorization.Repositories
{
    public interface IRacunRepository
    {
        Task<int> KreirajRacunSaStavkama(string statusRacuna, DateTime? datum, DataTable stavkeTable);
    }
}
