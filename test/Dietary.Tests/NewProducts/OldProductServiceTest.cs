using System.Threading.Tasks;
using Dietary.DAL;
using Dietary.Models.NewProducts;
using Xunit;

namespace Dietary.Tests.NewProducts
{
    public class OldProductServiceTest
    {
        [Fact]
        public async Task canListAllProductsDescriptions()
        {
            //given
            await _oldProductDescriptionRepository.SaveAsync(
                new OldProductDescription(ProductWithDesc("desc1", "longDesc1")));
            await _oldProductDescriptionRepository.SaveAsync(
                new OldProductDescription(ProductWithDesc("desc2", "longDesc2")));

            //when
            var allDescriptions =  await _oldProductService.FindAllDescriptionsAsync();

            //then
            Assert.Contains("desc1 *** longDesc1", allDescriptions);
            Assert.Contains("desc2 *** longDesc2", allDescriptions);
        }

        [Fact]
        public async Task canDecrementCounter()
        {
            //given
            var oldProduct = ProductWithPriceAndCounter(10, 10);
            await _oldProductRepository.SaveAsync(oldProduct);

            //when
            await _oldProductService.DecrementCounterAsync(oldProduct.SerialNumber);

            //then
            Assert.Equal(9, await _oldProductService.GetCounterOfAsync(oldProduct.SerialNumber));
        }

        [Fact]
        public async Task canIncrementCounter()
        {
            //given
            var oldProduct = ProductWithPriceAndCounter(10, 10);
            await _oldProductRepository.SaveAsync(oldProduct);

            //when
            await _oldProductService.IncrementCounterAsync(oldProduct.SerialNumber);

            //then
            Assert.Equal(11, await _oldProductService.GetCounterOfAsync(oldProduct.SerialNumber));
        }

        [Fact]
        public async Task canChangePrice()
        {
            //given
            var oldProduct = ProductWithPriceAndCounter(10, 10);
            await _oldProductRepository.SaveAsync(oldProduct);

            //when
            await _oldProductService.ChangePriceOfAsync(oldProduct.SerialNumber, 0);

            //then
            Assert.Equal(0, await _oldProductService.GetPriceOfAsync(oldProduct.SerialNumber));
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

        private static OldProduct ProductWithPriceAndCounter(decimal? price, int counter)
            => new OldProduct(price, "desc", "longDesc", counter);

        private static OldProduct ProductWithDesc(string desc, string longDesc)
            => new OldProduct(10, desc, longDesc, 10);
    }
}