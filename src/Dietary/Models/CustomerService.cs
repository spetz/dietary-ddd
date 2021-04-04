using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dietary.Models
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> GetCustomerAsync(long id)
        {
            var customer = await _customerRepository.FindByIdAsync(id).OrElseThrow(() => new ArgumentNullException());
            return new CustomerDto(customer);
        }

        public async Task<List<OrderDto>> GetIndividualOrdersForCustomerAsync(long customerId)
        {
            var customerDto = await GetCustomerAsync(customerId);
            var customer = await _customerRepository.FindByIdAsync(customerDto.Id);
            var group = customer.Group;
            if (!customer.Type.Equals(Customer.CustomerType.Representative) &&
                !customer.Type.Equals(Customer.CustomerType.Person))
            {
                throw new InvalidOperationException("not a person nor representative");
            }

            if (group is null)
            {
                throw new InvalidOperationException("group cannot be null");
            }

            var orders = group.Orders;
            return orders.Select(x => new OrderDto(x)).ToList();
        }
    }
}