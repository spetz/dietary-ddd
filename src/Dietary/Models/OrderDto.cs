using System;

namespace Dietary.Models
{
    public class OrderDto
    {
        public long OrderId { get; set; }
        public Order.OrderState State { get; set; }
        public Order.OrderType Type { get; set; }
        public CustomerDto Customer { get; set; }
        public DateTime? ConfirmationTimestamp { get; set; }

        public OrderDto(Order order)
        {
            OrderId = order.Id;
            State = order.State;
            Type = order.Type;
            Customer = new CustomerDto(order.CustomerOrderGroup.Customer);
            ConfirmationTimestamp = order.ConfirmationTimestamp;
        }
    }
}