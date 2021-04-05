using System;
using Dietary.Models;
using Xunit;

namespace Dietary.Tests
{
    public class TaxConfigTest
    {
        [Fact]
        public void canAddLimitedRulesToCountry()
        {
            //given
            var config = NewConfigWithRuleAndMaxRules(DefaultCountryCode, 2, TaxRule.LinearRule(5, 6, "tax-code1"));

            //when
            config.Add(TaxRule.LinearRule(1, 2, "tax-code2"));

            //then
            Assert.Equal(2, config.CurrentRulesCount);
        }

        [Fact]
        public void removingRuleShouldBeTakenIntoAccount()
        {
            //given
            var config = NewConfigWithRuleAndMaxRules(DefaultCountryCode, 2, TaxRule.LinearRule(5, 6, "tax-code3"));

            //and
            var taxRule = TaxRule.LinearRule(1, 2, "tax-code2");
            config.Add(taxRule);
            //and
            config.Remove(taxRule);

            //then
            Assert.Equal(1, config.CurrentRulesCount);

            //when
            config.Add(TaxRule.LinearRule(1, 2, "tax-code5"));

            //then
            Assert.Equal(2, config.CurrentRulesCount);

        }

        [Fact]
        public void cannotAddMoreThanLimitedRulesToCountry()
        {
            //given
            var config = NewConfigWithRuleAndMaxRules(DefaultCountryCode, 2, TaxRule.LinearRule(5, 6, "tax-code3"));

            //and
            config.Add(TaxRule.LinearRule(1, 2, "tax-code2"));

            //expect
            Assert.Throws<InvalidOperationException>(() => config.Add(TaxRule.LinearRule(1, 2, "tax-code5")));
        }


        [Fact]
        public void countryConfigHasAtLeast1Rule()
        {
            //given
            var aTaxRuleWithParams = TaxRule.LinearRule(5, 6, "tax-code9");
            var config = NewConfigWithRuleAndMaxRules(DefaultCountryCode, 2, aTaxRuleWithParams);

            //expect
            Assert.Throws<InvalidOperationException>(() => config.Remove(aTaxRuleWithParams));
        }

        [Fact]
        public void countryCodeIsAlwaysValid()
        {
            //expect
            Assert.Throws<InvalidOperationException>(() => CountryCode.Of(null));
            
            Assert.Throws<InvalidOperationException>(() => CountryCode.Of(""));
            
            Assert.Throws<InvalidOperationException>(() => CountryCode.Of("1"));
        }

        private const string DefaultCountryCode = "country-code";
        
        private static TaxConfig NewConfigWithRuleAndMaxRules(string countryCode, int maxRules,
            TaxRule aTaxRuleWithParams) => new TaxConfig(countryCode, maxRules, aTaxRuleWithParams);
    }
}