using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dietary.Models.Boundaries
{
    public interface IOrderRemoteService
    {
        Task<List<ClientOrder>> GetByPayerIdAsync(PayerId payerId);
        Task InformAboutNewOrderWithPaymentAsync(decimal? amount);
    }
}