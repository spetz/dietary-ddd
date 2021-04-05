using System;
using System.Threading.Tasks;
using Dietary.DAL;
using Dietary.Models.NewProducts;
using Xunit;

namespace Dietary.Tests.NewProducts
{
    public class OldProductServiceTest
    {
        [Fact]
        public async Task canIncrementCounterIfPriceIsPositive()
        {
            //given
            var oldProduct = await ProductWithPriceAndCounterAsync(10, 10);

            //when
            await _oldProductService.IncrementCounterAsync(oldProduct.Id);

            //then
            Assert.Equal(11, await _oldProductService.GetCounterOfAsync(oldProduct.Id));
        }

        [Fact]
        public async Task cannotIncrementCounterIfPriceIsNotPositive()
        {
            //given
            var oldProduct = await ProductWithPriceAndCounterAsync(0, 10);

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(() => _oldProductService.IncrementCounterAsync(oldProduct.Id));
        }

        [Fact]
        public async Task canDecrementCounterIfPriceIsPositive()
        {
            //given
            var oldProduct = await ProductWithPriceAndCounterAsync(10, 10);

            //when
            await _oldProductService.DecrementCounterAsync(oldProduct.Id);

            //then
            Assert.Equal(9, await _oldProductService.GetCounterOfAsync(oldProduct.Id));
        }
        
        [Fact]
        public async Task cannotDecrementCounterIfPriceIsNotPositive()
        {
            //given
            var oldProduct = await ProductWithPriceAndCounterAsync(0, 10);

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(() => _oldProductService.DecrementCounterAsync(oldProduct.Id));
        }

        [Fact]
        public async Task canChangePriceIfCounterIsPositive()
        {
            //given
            var oldProduct = await ProductWithPriceAndCounterAsync(10, 10);

            //when
            await _oldProductService.ChangePriceOfAsync(oldProduct.Id, 3);

            //then
            Assert.Equal(3, await _oldProductService.GetPriceOfAsync(oldProduct.Id));
        }
        
        [Fact]
        public async Task cannotChangePriceIfCounterIsNotPositive()
        {
            //given
            var oldProduct = await ProductWithPriceAndCounterAsync(0, 0);
            
            //when
            await _oldProductService.ChangePriceOfAsync(oldProduct.Id, 10);

            //expect
            Assert.Equal(0, await _oldProductService.GetPriceOfAsync(oldProduct.Id));
        }
        
        [Fact]
        public async Task canFormatDescription()
        {
            //given
            await ProductWithDescAsync("short", "long");

            //then
            Assert.Contains("short *** long", await _oldProductService.FindAllDescriptionsAsync());
        }

        [Fact]
        public async Task canChangeCharInDescription()
        {
            //given
            var oldProduct = await ProductWithDescAsync("short", "long");
            
            //then
            await _oldProductService.ReplaceCharInDesc(oldProduct.Id,  'o', '0');

            //expect
            Assert.Contains("sh0rt *** l0ng", await _oldProductService.FindAllDescriptionsAsync());
        }
        
        private readonly TestDb _testDb;
        private readonly DietaryDbContext _dbContext;
        private readonly OldProductService _oldProductService;
        private readonly IOldProductRepository _oldProductRepository;
        private readonly IOldProductDescriptionRepository _oldProductDescriptionRepository;

        public OldProductServiceTest()
        {
            _testDb = new TestDb();
            _dbContext = _testDb.DbContext;
            _oldProductRepository = new OldProductRepository(_dbContext);
            _oldProductDescriptionRepository = new OldProductDescriptionRepository(_dbContext, _oldProductRepository);
            _oldProductService = new OldProductService(_oldProductRepository, _oldProductDescriptionRepository);
        }

        private async Task<OldProduct> ProductWithPriceAndCounterAsync(decimal? price, int counter)
        {
            var oldProduct = new OldProduct(price, "desc", "longDesc", counter);
            await _oldProductRepository.SaveAsync(oldProduct);
            await _oldProductDescriptionRepository.SaveAsync(new OldProductDescription(oldProduct));
            return oldProduct;
        }

        private async Task<OldProduct> ProductWithDescAsync(string desc, string longDesc)
        {
            var oldProduct = new OldProduct(10, desc, longDesc, 10);
            await _oldProductRepository.SaveAsync(oldProduct);
            await _oldProductDescriptionRepository.SaveAsync(new OldProductDescription(oldProduct));
            return oldProduct;
        }
    }
}