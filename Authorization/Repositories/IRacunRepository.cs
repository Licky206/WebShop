using System.Data;

namespace Authorization.Repositories
{
    public interface IRacunRepository
    {
        public interface IInvoiceRepository
        {
            Task<int> CreateAsync(string statusRacuna, DateTime datum, TimeSpan vreme);
            Task<int> CreateWithItemsAsync(string statusRacuna, DateTime datum, TimeSpan vreme, DataTable stavkeRacuna);
        }
    }
}
