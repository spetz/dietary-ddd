using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models
{
    public interface ITaxConfigRepository
    {
        Task<TaxConfig> FindByCountryCodeAsync(string countryCode);
        Task<List<TaxConfig>> FindAllAsync();
        Task SaveAsync(TaxConfig taxConfig);
        Task DeleteAsync(TaxConfig taxConfig);
    }

    public class TaxConfigRepository : ITaxConfigRepository
    {
        private readonly DietaryDbContext _dbContext;
        private readonly DbSet<TaxConfig> _taxConfigs;

        public TaxConfigRepository(DietaryDbContext dbContext)
        {
            _dbContext = dbContext;
            _taxConfigs = dbContext.TaxConfigs;
        }

        public Task<TaxConfig> FindByCountryCodeAsync(string countryCode)
            => Query()
                .SingleOrDefaultAsync(x =>
                    x.CountryCode.Equals(countryCode, StringComparison.InvariantCultureIgnoreCase));

        public Task<List<TaxConfig>> FindAllAsync()
            => Query().ToListAsync();

        private IQueryable<TaxConfig> Query()
            => _taxConfigs
                .Include(x => x.TaxRules)
                .ThenInclude(x => x.TaxConfig);

        public Task SaveAsync(TaxConfig taxConfig) => _dbContext.UpsertAsync(taxConfig);

        public Task DeleteAsync(TaxConfig taxConfig) => _dbContext.DeleteAsync(taxConfig);
    }
}