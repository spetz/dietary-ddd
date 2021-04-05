using System;

namespace Dietary.Models
{
    public class TaxRule
    {
        public long Id { get; set; }
        public string TaxCode { get; set; }
        public bool IsLinear { get; set; }
        public int AFactor { get; set; }
        public int BFactor { get; set; }
        public bool IsSquare { get; set; }
        public int ASquareFactor { get; set; }
        public int BSquareFactor { get; set; }
        public int CSquareFactor { get; set; }
        public long TaxConfigId { get; set; }
        public TaxConfig TaxConfig { get; set; }

        public static TaxRule LinearRule(int a, int b, string taxCode)
        {
            if (a == 0)
            {
                throw new InvalidOperationException("Invalid aFactor");
            }

            var taxRule = new TaxRule
            {
                IsLinear = true,
                TaxCode = $"A. 899. {DateTime.UtcNow.Year}{taxCode}",
                AFactor = a,
                BFactor = b
            };

            return taxRule;
        }
        
        public static TaxRule SquareRule(int a, int b, int c, string taxCode)
        {
            if (a == 0)
            {
                throw new InvalidOperationException("Invalid aFactor");
            }

            var taxRule = new TaxRule
            {
                IsSquare = true,
                TaxCode = $"A. 899. {DateTime.UtcNow.Year}{taxCode}",
                ASquareFactor = a,
                BSquareFactor = b,
                CSquareFactor = c
            };

            return taxRule;
        }
    }
    
        public class TaxRuleBuilder
        {
            private string _taxCode;
            private bool _isLinear;
            private int _aFactor;
            private int _bFactor;
            private bool _isSquare;
            private int _aSquareFactor;
            private int _bSquareFactor;
            private int _cSquareFactor;
            private TaxConfig _taxConfig;

            private TaxRuleBuilder()
            {
            }

            public static TaxRuleBuilder ATaxRule()
            {
                return new TaxRuleBuilder();
            }

            public TaxRuleBuilder WithTaxCode(string taxCode)
            {
                _taxCode = taxCode;
                return this;
            }

            public TaxRuleBuilder WithIsLinear(bool isLinear)
            {
                _isLinear = isLinear;
                return this;
            }

            public TaxRuleBuilder WithAFactor(int aFactor)
            {
                _aFactor = aFactor;
                return this;
            }

            public TaxRuleBuilder WithBFactor(int bFactor)
            {
                _bFactor = bFactor;
                return this;
            }

            public TaxRuleBuilder WithIsSquare(bool isSquare)
            {
                _isSquare = isSquare;
                return this;
            }

            public TaxRuleBuilder WithASquareFactor(int aSquareFactor)
            {
                _aSquareFactor = aSquareFactor;
                return this;
            }

            public TaxRuleBuilder WithBSquareFactor(int bSquareFactor)
            {
                _bSquareFactor = bSquareFactor;
                return this;
            }

            public TaxRuleBuilder WithCSquareFactor(int cSuqreFactor)
            {
                _cSquareFactor = cSuqreFactor;
                return this;
            }

            public TaxRuleBuilder WithTaxConfig(TaxConfig taxConfig)
            {
                _taxConfig = taxConfig;
                return this;
            }

            public TaxRule Build()
            {
                var taxRule = new TaxRule
                {
                    TaxCode = _taxCode,
                    IsSquare = _isSquare,
                    IsLinear = _isLinear,
                    ASquareFactor = _aSquareFactor,
                    TaxConfig = _taxConfig,
                    AFactor = _aFactor,
                    BFactor = _bFactor,
                    BSquareFactor = _bSquareFactor,
                    CSquareFactor = _cSquareFactor,
                };

                return taxRule;
            }
        }
}