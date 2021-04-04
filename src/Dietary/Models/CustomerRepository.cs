using System;
using System.Linq;
using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models
{
    public interface ICustomerRepository
    {
        Task<Customer> FindByIdAsync(long id);
        Task<Customer> FindByNameAsync(string name);
        Task SaveAsync(Customer customer);
        Task DeleteAsync(Customer customer);
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
            => Query()
                .SingleOrDefaultAsync(x => x.Id == id);

        public Task<Customer> FindByNameAsync(string name)
            => Query()
                .SingleOrDefaultAsync(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        private IQueryable<Customer> Query()
            => _customers
                .Include(x => x.Group)
                .ThenInclude(x => x.Orders)
                .ThenInclude(x => x.TaxRules)
                .ThenInclude(x => x.TaxConfig);
        
        public Task SaveAsync(Customer customer) => _dbContext.UpsertAsync(customer);
        
        public Task DeleteAsync(Customer customer) => _dbContext.DeleteAsync(customer);
    }
}