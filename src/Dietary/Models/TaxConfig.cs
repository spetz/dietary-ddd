using System;
using System.Collections.Generic;

namespace Dietary.Models
{
    public class TaxConfig
    {
        private readonly string _description;
        private readonly string _countryReason;
        private readonly string _modifiedBy;
        
        public long Id { get; set; }
        public CountryCode CountryCode { get; private set; }
        public DateTime LastModifiedDate { get; set; }
        public int CurrentRulesCount { get; private set; }
        public int MaxRulesCount { get; private set; }
        public List<TaxRule> TaxRules { get; set; }

        public TaxConfig()
        {
            TaxRules = new List<TaxRule>();
        }

        public TaxConfig(string countryCode, int maxRules, TaxRule aTaxRuleWithParams)
        {
            CountryCode = CountryCode.Of(countryCode);
            MaxRulesCount = maxRules;
            TaxRules = new List<TaxRule>();
            Add(aTaxRuleWithParams);
        }

        public void Remove(TaxRule taxRule)
        {
            if (TaxRules.Contains(taxRule))
            {
                if (TaxRules.Count == 1)
                {
                    throw new InvalidOperationException("Last rule in country config");
                }

                TaxRules.Remove(taxRule);
                CurrentRulesCount--;
                LastModifiedDate = DateTime.UtcNow;
            }
        }

        public void Add(TaxRule taxRule)
        {
            if (MaxRulesCount <= CurrentRulesCount)
            {
                throw new InvalidOperationException("Too many rules");
            }

            TaxRules.Add(taxRule);
            CurrentRulesCount++;
            LastModifiedDate = DateTime.UtcNow;
        }

        public string GetCountryCode() => CountryCode.AsString();
    }

    public class CountryCode : IEquatable<CountryCode>
    {
        public long Id { get; private set; } = 1; // ORM key ignore
        private readonly string _code;

        private CountryCode()
        {
        }
        
        public CountryCode(string code)
        {
            _code = code;
        }

        public static CountryCode Of(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length == 1)
            {
                throw new InvalidOperationException("Invalid country code");
            }

            return new CountryCode(code);
        }

        public string AsString() => _code;
        
        public bool Equals(CountryCode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _code == other._code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CountryCode) obj);
        }

        public override int GetHashCode() => _code.GetHashCode();
    }
}