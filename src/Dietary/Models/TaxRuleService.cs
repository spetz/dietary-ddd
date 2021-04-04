using System;
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
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length == 1)
            {
                throw new InvalidOperationException("Invalid country code");
            }

            if (aFactor == 0)
            {
                throw new InvalidOperationException("Invalid aFactor");
            }

            var taxRule = new TaxRule
            {
                AFactor = aFactor,
                BFactor = bFactor,
                IsLinear = true,
                TaxCode = $"A. 899. {DateTime.UtcNow.Year}{taxCode}"
            };
            
            var taxConfig = await _taxConfigRepository.FindByCountryCodeAsync(countryCode);
            if (taxConfig is null)
            {
                taxConfig = await CreateTaxConfigWithRuleAsync(countryCode, taxRule);
            }

            var byOrderState = await _orderRepository.FindByOrderStateAsync(Order.OrderState.Initial);

            byOrderState.ForEach(async order =>
            {
                if (order.CustomerOrderGroup.Customer.Type.Equals(Customer.CustomerType.Person))
                {
                    order.TaxRules.Add(taxRule);
                    await _orderRepository.SaveAsync(order);
                }
            });
        }

        public async Task<TaxConfig> CreateTaxConfigWithRuleAsync(string countryCode, TaxRule taxRule)
        {
            var taxConfig = new TaxConfig
            {
                CountryCode = countryCode,
            };
            
            taxConfig.TaxRules.Add(taxRule);
            taxConfig.CurrentRulesCount = taxConfig.TaxRules.Count;
            
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length == 1)
            {
                throw new InvalidOperationException("Invalid country code");
            }

            await _taxConfigRepository.SaveAsync(taxConfig);
            return taxConfig;
        }

        public async Task AddTaxRuleToCountryAsync(string countryCode, int aFactor, int bFactor, int cFactor,
            String taxCode)
        {
            if (aFactor == 0)
            {
                throw new InvalidOperationException("Invalid aFactor");
            }

            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length == 1)
            {
                throw new InvalidOperationException("Invalid country code");
            }

            var taxRule = new TaxRule
            {
                ASquareFactor = aFactor,
                BSquareFactor = bFactor,
                CSquareFactor = cFactor,
                IsSquare = true,
                TaxCode = $"A. 899. {DateTime.UtcNow.Year}{taxCode}"
            };
            
            var taxConfig = await _taxConfigRepository.FindByCountryCodeAsync(countryCode);
            if (taxConfig is null)
            {
                await CreateTaxConfigWithRuleAsync(countryCode, taxRule);
            }
        }

        public async Task DeleteRuleAsync(long taxRuleId)
        {
            var taxRule = await _taxRuleRepository.FindByIdAsync(taxRuleId);
            var config = taxRule.TaxConfig;
            if (config.TaxRules.Count == 1)
            {
                throw new InvalidOperationException("Last rule in country config");
            }

            await _taxRuleRepository.DeleteAsync(taxRule);
        }


        public Task<List<TaxConfig>> FindAllConfigsAsync() => _taxConfigRepository.FindAllAsync();
    }
}