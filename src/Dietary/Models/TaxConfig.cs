using System;
using System.Collections.Generic;

namespace Dietary.Models
{
    public class TaxConfig
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string CountryReason { get; set; }
        public string CountryCode { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int CurrentRulesCount { get; set; }
        public int MaxRulesCount { get; set; }
        public List<TaxRule> TaxRules { get; set; }
    }
}