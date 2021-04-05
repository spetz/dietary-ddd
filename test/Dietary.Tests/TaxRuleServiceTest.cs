using System;
using System.Threading.Tasks;
using Dietary.DAL;
using Dietary.Models;
using Xunit;

namespace Dietary.Tests
{
    public class TaxRuleServiceTest
    {
        [Fact]
        public async Task countryCodeIsAlwaysValid()
        {
            var rule = TaxRule.LinearRule(10, 10, "taxCode");
            await Assert.ThrowsAsync<InvalidOperationException>(() => CreateConfigWithInitialRuleAsync("", 2, rule));
            await Assert.ThrowsAsync<InvalidOperationException>(() => CreateConfigWithInitialRuleAsync(null, 2, rule));
            await Assert.ThrowsAsync<InvalidOperationException>(() => CreateConfigWithInitialRuleAsync("1", 2, rule));
        }

        [Fact]
        public async Task aFactorIsNotZero()
        {
            //given
            await CreateConfigWithInitialRuleAsync("HUN", 2, TaxRule.LinearRule(10, 10, "taxCode"));

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddTaxRuleToCountryAsync("HUN", 0, 4, "taxRule2"));
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddTaxRuleToCountryAsync("HUN", 0, 4, 5, "taxRule3"));
        }

        [Fact]
        public async Task shouldNotHaveMoreThanMaximumNumberOfRules()
        {
            //given
            var rule = TaxRule.LinearRule(10, 10, "taxCode");
            var config = CreateConfigWithInitialRuleAsync("PL1", 2, rule);
            
            //and
            await _service.AddTaxRuleToCountryAsync("PL1", 2, 4, "taxRule2");

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddTaxRuleToCountryAsync("PL1", 2, 4, "taxRule3"));
        }

        [Fact]
        public async Task canAddARule()
        {
            //given
            var rule = TaxRule.LinearRule(10, 10, "taxCode");
            var config = CreateConfigWithInitialRuleAsync("PL2", 2, rule);

            //when
            await _service.AddTaxRuleToCountryAsync("PL2", 2, 4, "taxRule2");

            //then
            Assert.Equal(2, await _service.RulesCountAsync("PL2"));
        }

        [Fact]
        public async Task canDeleteARule()
        {
            //given
            var rule = TaxRule.LinearRule(10, 10, "taxCode");
            var config = CreateConfigWithInitialRuleAsync("PL3", 2, rule);
            //and
            await _service.AddTaxRuleToCountryAsync("PL3", 2, 4, "taxRule2");

            //when
            await _service.DeleteRuleAsync(rule.Id, config.Id);

            //expect
            Assert.Equal(1, await _service.RulesCountAsync("PL3"));
        }

        [Fact]
        public async Task cannotDeleteARuleIfThatIsTheLastOne()
        {
            //given
            var rule = TaxRule.LinearRule(10, 10, "taxCode");
            var config = await CreateConfigWithInitialRuleAsync("PL4", 2, rule);

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteRuleAsync(rule.Id, config.Id));
        }

        private readonly TestDb _testDb;
        private readonly DietaryDbContext _dbContext;
        private readonly ITaxRuleRepository _taxRuleRepository;
        private readonly ITaxConfigRepository _taxConfigRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly TaxRuleService _service;

        public TaxRuleServiceTest()
        {
            _testDb = new TestDb();
            _dbContext = _testDb.DbContext;
            _taxRuleRepository = new TaxRuleRepository(_dbContext);
            _taxConfigRepository = new TaxConfigRepository(_dbContext);
            _orderRepository = new OrderRepository(_dbContext);
            _service = new TaxRuleService(_taxRuleRepository, _taxConfigRepository, _orderRepository);
        }

        private async Task<TaxConfig> CreateConfigWithInitialRuleAsync(String countryCode, int maxRules, TaxRule rule)
            => await _service.CreateTaxConfigWithRuleAsync(countryCode, maxRules, rule);
    }
}