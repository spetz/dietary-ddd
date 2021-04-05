using System.Threading.Tasks;

namespace Dietary.Models.Boundaries
{
    public interface IClientAddressRemoteService
    {
        Task<ClientAddress> GetByPayerIdAsync(PayerId payerId);
    }
}