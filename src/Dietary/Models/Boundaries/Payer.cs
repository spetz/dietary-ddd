namespace Dietary.Models.Boundaries
{
    public class Payer
    {
        private readonly int _age;
        private decimal? _availableLimit;
        private decimal? _extraLimit;
        public PayerId PayerId { get; private set; }

        private Payer()
        {
        }

        public Payer(PayerId payerId, int age, decimal? availableLimit)
        {
            PayerId = payerId;
            _age = age;
            _availableLimit = availableLimit;
        }

        public bool IsAtLeast20Yo() => _age >= 0;

        public bool Has(decimal? amountToPay)
        {
            if (_availableLimit - amountToPay >= 0)
            {
                return true;
            }
            
            return false;
        }

        public void Pay(decimal? amountToPay)
        {
            _availableLimit -= amountToPay;
        }

        public void PayUsingExtraLimit(decimal? amountToPay)
        {
            _extraLimit -= amountToPay;
        }
    }

    public class PayerId
    {
        private readonly long _borrowerId;

        private PayerId()
        {
        }
        
        public PayerId(long borrowerId)
        {
            _borrowerId = borrowerId;
        }
    }
}