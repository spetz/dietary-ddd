using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models
{
    public interface IOrderRepository
    {
        Task<Order> FindByIdAsync(long id);
    }
    
    public class OrderRepository : IOrderRepository
    {
        private readonly DietaryDbContext _dbContext;
        private readonly DbSet<Order> _orders;

        public OrderRepository(DietaryDbContext dbContext)
        {
            _dbContext = dbContext;
            _orders = dbContext.Orders;
        }

        public Task<Order> FindByIdAsync(long id)
            => _orders
                .Include(x => x.CustomerOrderGroup)
                .ThenInclude(x => x.Customer)
                .SingleOrDefaultAsync(x => x.Id == id);
    }
}