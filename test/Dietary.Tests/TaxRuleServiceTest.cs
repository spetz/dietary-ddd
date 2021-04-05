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
        public async Task canAddLimitedRulesToCountry()
        {
            //given
            await NewConfigWithRuleAndMaxRulesAsync(_countryCode, 2, TaxRule.LinearRule(5, 6, "tax-code1"));

            //when
            await _taxRuleService.AddTaxRuleToCountryAsync(_countryCode, 1, 2, "tax-code2");

            //then
            Assert.Equal(2, (await ConfigByAsync(_countryCode)).CurrentRulesCount);
        }
        
        [Fact]
        public async Task removingRuleShouldBeTakenIntoAccount()
        {
            //given
            var config = await NewConfigWithRuleAndMaxRulesAsync(_countryCode, 2, TaxRule.LinearRule(5, 6, "tax-code3"));

            //and
            await _taxRuleService.AddTaxRuleToCountryAsync(_countryCode, 1, 2, "tax-code4");
            //and
            await _taxRuleService.DeleteRuleAsync(await RuleByTaxCodeAsync("tax-code3"), config.Id);

            //then
            Assert.Equal(1, (await ConfigByAsync(_countryCode)).CurrentRulesCount);

            //when
            await _taxRuleService.AddTaxRuleToCountryAsync(_countryCode, 1, 2, "tax-code5");

            //then
            Assert.Equal(2, (await ConfigByAsync(_countryCode)).CurrentRulesCount);
        }
        
        private async Task<long> RuleByTaxCodeAsync(string taxCode)
            => (await _taxRuleRepository.FindByTaxCodeContainingAsync(taxCode)).Id;

        [Fact]
        public async Task cannotAddMoreThanLimitedRulesToCountry()
        {
            //given
            await NewConfigWithRuleAndMaxRulesAsync(_countryCode, 2, TaxRule.LinearRule(5, 6, "tax-code6"));

            //and
            await _taxRuleService.AddTaxRuleToCountryAsync(_countryCode, 1, 2, "tax-code7");

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _taxRuleService.AddTaxRuleToCountryAsync(_countryCode, 4, 99, "tax-code8"));
        }

        [Fact]
        public async Task countryConfigHasAtLeast1Rule()
        {
            //given
            var config = await NewConfigWithRuleAndMaxRulesAsync(_countryCode, 2, TaxRule.LinearRule(5, 6, "tax-code9"));

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _taxRuleService.DeleteRuleAsync(await RuleByTaxCodeAsync("tax-code9"), config.Id));
        }
        
        [Fact]
        public void countryCodeIsAlwaysValid()
        {
            //expect
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _taxRuleService.CreateTaxConfigWithRuleAsync(null, TaxRule.LinearRule(5, 6, "tax-code10")));
            
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _taxRuleService.CreateTaxConfigWithRuleAsync("", TaxRule.LinearRule(5, 6, "tax-code11")));
            
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _taxRuleService.CreateTaxConfigWithRuleAsync("1", TaxRule.LinearRule(5, 6, "tax-code12")));
        }

        [Fact]
        public async Task aFactorIsAlwaysNotZero()
        {
            var config = await NewConfigWithRuleAndMaxRulesAsync(_countryCode, 2, TaxRule.LinearRule(5, 6, "tax-code13"));

            //expect
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _taxRuleService.AddTaxRuleToCountryAsync(_countryCode, 0, 2, "tax-code14"));
        }

        private readonly string _countryCode;
        private readonly TestDb _testDb;
        private readonly DietaryDbContext _dbContext;
        private readonly ITaxRuleRepository _taxRuleRepository;
        private readonly ITaxConfigRepository _taxConfigRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly TaxRuleService _taxRuleService;

        public TaxRuleServiceTest()
        {
            _countryCode = Guid.NewGuid().ToString();
            _testDb = new TestDb();
            _dbContext = _testDb.DbContext;
            _taxRuleRepository = new TaxRuleRepository(_dbContext);
            _taxConfigRepository = new TaxConfigRepository(_dbContext);
            _orderRepository = new OrderRepository(_dbContext);
            _taxRuleService = new TaxRuleService(_taxRuleRepository, _taxConfigRepository, _orderRepository);
        }

        private async Task<TaxConfig> ConfigByAsync(string countryCode)
            => await _taxConfigRepository.FindByCountryCodeAsync(CountryCode.Of(countryCode));

        private async Task<TaxConfig> NewConfigWithRuleAndMaxRulesAsync(string countryCode, int maxRules,
            TaxRule aTaxRuleWithParams)
        {
            var taxConfig = new TaxConfig(countryCode, maxRules, aTaxRuleWithParams);
            await _taxConfigRepository.SaveAsync(taxConfig);
            return taxConfig;
        }
    }
}