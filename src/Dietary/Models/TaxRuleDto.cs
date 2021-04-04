namespace Dietary.Models
{
    public class TaxRuleDto
    {
        public string FormattedTaxCode { get; set; }
        public bool IsLinear { get; set; }
        public int AFactor { get; set; }
        public int BFactor { get; set; }
        public bool IsSquare { get; set; }
        public int ASquareFactor { get; set; }
        public int BSquareFactor { get; set; }
        public int CSquareFactor { get; set; }

        public TaxRuleDto(TaxRule taxRule)
        {
            FormattedTaxCode = $" informal 671 {taxRule.TaxCode}  *** ";
            IsLinear = taxRule.IsLinear;
            AFactor = taxRule.AFactor;
            BFactor = taxRule.BFactor;
            IsSquare = taxRule.IsSquare;
            ASquareFactor = taxRule.ASquareFactor;
            BSquareFactor = taxRule.BSquareFactor;
            CSquareFactor = taxRule.CSquareFactor;
        }
    }
}