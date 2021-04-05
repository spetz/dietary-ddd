using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models.NewProducts
{
    public interface IOldProductRepository
    {
        Task<OldProduct> FindByIdAsync(Guid id);
        Task<List<OldProduct>> FindAllAsync();
        Task SaveAsync(OldProduct oldProduct);
        Task DeleteAsync(OldProduct oldProduct);
    }

    public class OldProductRepository : IOldProductRepository
    {
        private readonly DietaryDbContext _dbContext;
        private readonly DbSet<OldProduct> _oldProducts;

        public OldProductRepository(DietaryDbContext dbContext)
        {
            _dbContext = dbContext;
            _oldProducts = dbContext.OldProducts;
        }

        public Task<OldProduct> FindByIdAsync(Guid id)
            => _oldProducts
                .SingleOrDefaultAsync(x => x.Id == id);

        public Task<List<OldProduct>> FindAllAsync() => _oldProducts.ToListAsync();

        public Task SaveAsync(OldProduct oldProduct) => _dbContext.UpsertAsync(oldProduct);

        public Task DeleteAsync(OldProduct oldProduct) => _dbContext.DeleteAsync(oldProduct);
    }
}