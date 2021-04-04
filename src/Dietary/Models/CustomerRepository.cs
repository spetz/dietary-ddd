using System;
using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models
{
    public interface ICustomerRepository
    {
        Task<Customer> FindByIdAsync(long id);
        Task<Customer> FindByNameAsync(string name);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly DietaryDbContext _dbContext;
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(DietaryDbContext dbContext)
        {
            _dbContext = dbContext;
            _customers = dbContext.Customers;
        }

        public Task<Customer> FindByIdAsync(long id)
            => _customers
                .Include(x => x.Group)
                .ThenInclude(x => x.Orders)
                .SingleOrDefaultAsync(x => x.Id == id);

        public Task<Customer> FindByNameAsync(string name)
            => _customers
                .Include(x => x.Group)
                .ThenInclude(x => x.Orders)
                .SingleOrDefaultAsync(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}