using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dietary.Models
{
    public class TaxRuleService
    {
        private readonly ITaxRuleRepository _taxRuleRepository;
        private readonly ITaxConfigRepository _taxConfigRepository;
        private readonly IOrderRepository _orderRepository;

        public TaxRuleService(ITaxRuleRepository taxRuleRepository, ITaxConfigRepository taxConfigRepository,
            IOrderRepository orderRepository)
        {
            _taxRuleRepository = taxRuleRepository;
            _taxConfigRepository = taxConfigRepository;
            _orderRepository = orderRepository;
        }

        public async Task AddTaxRuleToCountryAsync(string countryCode, int aFactor, int bFactor, string taxCode)
        {
            var taxRule = TaxRule.LinearRule(aFactor, bFactor, taxCode);
            var taxConfig = await _taxConfigRepository.FindByCountryCodeAsync(CountryCode.Of(countryCode));
            if (taxConfig is null) {
                taxConfig = await CreateTaxConfigWithRuleAsync(countryCode, taxRule);
                return;
            }

            taxConfig.Add(taxRule);
            var byOrderState = await _orderRepository.FindByOrderStateAsync(Order.OrderState.Initial);

            byOrderState.ForEach(async order =>
            {
                if (order.CustomerOrderGroup.Customer.Type.Equals(Customer.CustomerType.Person))
                {
                    order.TaxRules.Add(taxRule);
                    await _orderRepository.SaveAsync(order);
                    await _taxRuleRepository.SaveAsync(taxRule);
                }
            });
        }

        public async Task<TaxConfig> CreateTaxConfigWithRuleAsync(string countryCode, TaxRule taxRule)
        {
            var taxConfig = new TaxConfig(countryCode, 1, taxRule);
            await _taxConfigRepository.SaveAsync(taxConfig);
            await _taxRuleRepository.SaveAsync(taxRule);
            return taxConfig;
        }

        public async Task AddTaxRuleToCountryAsync(string countryCode, int aFactor, int bFactor, int cFactor,
            string taxCode)
        {
            var taxRule = TaxRule.SquareRule(aFactor, bFactor, cFactor, taxCode);
            var taxConfig = await _taxConfigRepository.FindByCountryCodeAsync(CountryCode.Of(countryCode));
            if (taxConfig is null) {
                await CreateTaxConfigWithRuleAsync(countryCode, taxRule);
                return;
            }

            taxConfig.Add(taxRule);
        }

        public async Task DeleteRuleAsync(long taxRuleId, long configId)
        {
            var taxRule = await _taxRuleRepository.FindByIdAsync(taxRuleId);
            var taxConfig = await _taxConfigRepository.FindByIdAsync(configId);
            taxConfig.Remove(taxRule);
        }
        
        public Task<List<TaxConfig>> FindAllConfigsAsync() => _taxConfigRepository.FindAllAsync();
    }
}