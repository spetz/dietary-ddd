using System.Collections.Generic;

namespace Dietary.Models
{
    public class CustomerOrderGroup
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long? ParentId { get; set; }
        public CustomerOrderGroup Parent { get; private set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; private set; }
        public List<Order> Orders { get; private set; }
        public List<CustomerOrderGroup> Children { get; private set; }
    }
}