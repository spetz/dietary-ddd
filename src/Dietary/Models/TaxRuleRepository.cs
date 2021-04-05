using System.Linq;
using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models
{
    public interface ITaxRuleRepository
    {
        Task<TaxRule> FindByIdAsync(long id);
        Task<TaxRule> FindByTaxCodeContainingAsync(string taxCode);
        Task SaveAsync(TaxRule taxRule);
        Task DeleteAsync(TaxRule taxRule);
    }

    public class TaxRuleRepository : ITaxRuleRepository
    {
        private readonly DietaryDbContext _dbContext;
        private readonly DbSet<TaxRule> _taxRules;

        public TaxRuleRepository(DietaryDbContext dbContext)
        {
            _dbContext = dbContext;
            _taxRules = dbContext.TaxRules;
        }

        public Task<TaxRule> FindByIdAsync(long id)
            => Query()
                .SingleOrDefaultAsync(x => x.Id == id);

        public Task<TaxRule> FindByTaxCodeContainingAsync(string taxCode)
            => Query()
                .SingleOrDefaultAsync(x => x.TaxCode.Contains(taxCode));

        private IQueryable<TaxRule> Query()
            => _taxRules
                .Include(x => x.TaxConfig)
                .ThenInclude(x => x.TaxRules);

        public Task SaveAsync(TaxRule taxRule) => _dbContext.UpsertAsync(taxRule);

        public Task DeleteAsync(TaxRule taxRule) => _dbContext.DeleteAsync(taxRule);
    }
}