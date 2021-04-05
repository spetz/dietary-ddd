using System;

namespace Dietary.Models.Boundaries
{
    public class ClientOrder
    {
        private readonly decimal? _amount;
        private readonly DateTime _timestamp;

        public ClientOrder(decimal? amount, DateTime timestamp)
        {
            _amount = amount;
            _timestamp = timestamp;
        }

        public bool IsMoreThan(decimal? amount)
        {
            return _amount - amount > 0;
        }
    }
}