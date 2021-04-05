using System.Threading.Tasks;

namespace Dietary.Models.Boundaries
{
    public interface IPayerRepository
    {
        Task<Payer> FindByIdAsync(PayerId payerId);
    }
}