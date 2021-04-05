using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dietary.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Models.NewProducts
{
    public interface IOldProductDescriptionRepository
    {
        Task<OldProductDescription> FindByIdAsync(Guid productId);
        Task<List<OldProductDescription>> FindAllAsync();
        Task SaveAsync(OldProductDescription oldProductDescription);
        Task DeleteAsync(OldProductDescription oldProductDescription);
    }
    
    public class OldProductDescriptionRepository : IOldProductDescriptionRepository
    {
        private readonly DietaryDbContext _dbContext;
        private readonly DbSet<OldProductDescription> _oldProductDescriptions;
        private readonly IOldProductRepository _oldProductRepository;

        public OldProductDescriptionRepository(DietaryDbContext dbContext, IOldProductRepository oldProductRepository)
        {
            _dbContext = dbContext;
            _oldProductRepository = oldProductRepository;
            _oldProductDescriptions = dbContext.OldProductDescriptions;
        }

        public async Task<OldProductDescription> FindByIdAsync(Guid id)
            => new OldProductDescription(await _oldProductRepository.FindByIdAsync(id));

        public Task<List<OldProductDescription>> FindAllAsync() => _oldProductDescriptions.ToListAsync();

        public Task SaveAsync(OldProductDescription oldProductDescription) => _dbContext.UpsertAsync(oldProductDescription);

        public Task DeleteAsync(OldProductDescription oldProductDescription) => _dbContext.DeleteAsync(oldProductDescription);
    }
}