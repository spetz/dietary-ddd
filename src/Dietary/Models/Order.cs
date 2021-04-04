using System;

namespace Dietary.Models
{
    public class Order
    {
        public long Id { get; set; }
        public OrderState State { get; set; }
        public OrderType Type { get; set; }
        public long CustomerOrderGroupId { get; set; }
        public CustomerOrderGroup CustomerOrderGroup { get; set; }
        public DateTime? ConfirmationTimestamp { get; set; }
        
        public enum OrderState
        {
            Initial,
            Paid,
            Delivered,
            Returned
        }

        public enum OrderType
        {
            Phone,
            Wire,
            WireOneItem,
            SpecialDiscount,
            RegularBatch
        }
    }
}