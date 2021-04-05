using System.Threading.Tasks;

namespace Dietary.Models.Boundaries
{
    public interface IClaimsRemoteService
    {
        Task<bool> ClientHasNoClaimsAsync(PayerId payerId);
    }
}