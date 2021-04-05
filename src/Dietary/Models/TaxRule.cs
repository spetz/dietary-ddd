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
        public TaxConfig TaxConfig { get;set; }

        public static TaxRule LinearRule(int a, int b, string taxCode)
        {
            var taxRule = new TaxRule
            {
                IsLinear = true,
                TaxCode = taxCode,
                AFactor = a,
                BFactor = b
            };

            return taxRule;
        }
    }
}