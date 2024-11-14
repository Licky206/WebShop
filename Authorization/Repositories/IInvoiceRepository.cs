using System.Data;

namespace Authorization.Repositories
{
    public interface IInvoiceRepository
    {
        Task<int> CreateInvoiceAsync(string statusRacuna, DateTime datum, TimeSpan vreme);
        Task<int> AddInvoiceWithItemsAsync(string statusRacuna, DateTime datum, TimeSpan vreme, DataTable stavkeRacuna);

    }
}
