using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models
{
    public interface ICustomerOrderGroupRepository
    {
        Task<CustomerOrderGroup> FindByIdAsync(long id);
    }
    
    public class CustomerOrderGroupRepository : ICustomerOrderGroupRepository
    {
        private readonly DietaryDbContext _dbContext;
        private readonly DbSet<CustomerOrderGroup> _groups;

        public CustomerOrderGroupRepository(DietaryDbContext dbContext)
        {
            _dbContext = dbContext;
            _groups = dbContext.CustomerOrderGroups;
        }

        public Task<CustomerOrderGroup> FindByIdAsync(long id)
            => _groups
                .Include(x => x.Customer)
                .Include(x => x.Orders)
                .SingleOrDefaultAsync(x => x.Id == id);
    }
}