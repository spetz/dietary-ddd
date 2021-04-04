using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models
{
    public interface IOrderRepository
    {
        Task<Order> FindByIdAsync(long id);
        Task<List<Order>> FindByOrderStateAsync(Order.OrderState state);
        Task SaveAsync(Order order);
        Task DeleteAsync(Order order);
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
            => Query()
                .SingleOrDefaultAsync(x => x.Id == id);

        public Task<List<Order>> FindByOrderStateAsync(Order.OrderState state)
            => Query()
                .Where(x => x.State.Equals(state))
                .ToListAsync();

        public Task SaveAsync(Order order) => _dbContext.UpsertAsync(order);

        public Task DeleteAsync(Order order) => _dbContext.DeleteAsync(order);

        private IQueryable<Order> Query()
            => _orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Order)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .Include(x => x.CustomerOrderGroup)
                .ThenInclude(x => x.Customer)
                .Include(x => x.TaxRules)
                .ThenInclude(x => x.TaxConfig);
    }
}