using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dietary.Models.NewProducts
{
    public class OldProductService
    {
        private readonly IOldProductRepository _oldProductRepository;
        private readonly IOldProductDescriptionRepository _oldProductDescriptionRepository;

        public OldProductService(IOldProductRepository oldProductRepository,
            IOldProductDescriptionRepository oldProductDescriptionRepository)
        {
            _oldProductRepository = oldProductRepository;
            _oldProductDescriptionRepository = oldProductDescriptionRepository;
        }

        public async Task<List<string>> FindAllDescriptionsAsync()
            => (await _oldProductDescriptionRepository.FindAllAsync()).Select(x => x.FormatDesc()).ToList();

        public async Task ReplaceCharInDesc(Guid productId, char oldChar, char newChar)
        {
            var oldProductDescription = await _oldProductDescriptionRepository.FindByIdAsync(productId);
            oldProductDescription.ReplaceCharFromDesc(oldChar, newChar);
        }

        public async Task IncrementCounterAsync(Guid productId)
        {
            var product = await _oldProductRepository.FindByIdAsync(productId);
            product.IncrementCounter();
        }

        public async Task DecrementCounterAsync(Guid productId)
        {
            var product = await _oldProductRepository.FindByIdAsync(productId);
            product.DecrementCounter();
        }

        public async Task ChangePriceOfAsync(Guid productId, decimal? newPrice)
        {
            var product = await _oldProductRepository.FindByIdAsync(productId);
            product.ChangePriceTo(newPrice);
        }

        public async Task<int> GetCounterOfAsync(Guid serialNumber)
            => (await _oldProductRepository.FindByIdAsync(serialNumber)).GetCounter();

        public async Task<decimal> GetPriceOfAsync(Guid serialNumber)
            => (await _oldProductRepository.FindByIdAsync(serialNumber)).GetPrice();
    }
}