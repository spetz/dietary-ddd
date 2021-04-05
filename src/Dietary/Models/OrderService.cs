using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dietary.Models
{
    public class OrderService
    {
        private readonly CustomerService _customerService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerOrderGroupRepository _customerOrderGroupRepository;
        private readonly IAuthenticationFacade _authenticationContextFacade;

        public OrderService(CustomerService customerService, ICustomerRepository customerRepository,
            IOrderRepository orderRepository, ICustomerOrderGroupRepository customerOrderGroupRepository,
            IAuthenticationFacade authenticationContextFacade)
        {
            _customerService = customerService;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _customerOrderGroupRepository = customerOrderGroupRepository;
            _authenticationContextFacade = authenticationContextFacade;
        }

        public async Task<List<OrderDto>> GetOrdersForCompanyAsync(long customerId)
        {
            var customerDto = await _customerService.GetCustomerAsync(customerId);
            var customer = await _customerRepository.FindByIdAsync(customerDto.Id);
            if (!customer.Type.Equals(Customer.CustomerType.Company) &&
                !customer.Type.Equals(Customer.CustomerType.Division))
            {
                throw new InvalidOperationException("not a company nor division");
            }

            return await GetOrdersIncludingSubordinatesAsync(customerId);
        }

        public async Task<List<OrderDto>> GetOrdersForAdminAsync(long customerId)
        {
            var customerDto = await _customerService.GetCustomerAsync(customerId);
            var customer = await _customerRepository.FindByIdAsync(customerDto.Id);
            if (!customer.Type.Equals(Customer.CustomerType.Admin))
            {
                throw new InvalidOperationException("not an admin");
            }

            return await GetOrdersIncludingSubordinatesAsync(customerId);
        }

        public async Task<List<OrderDto>> GetOrdersIncludingSubordinatesAsync(long customerId)
        {
            var customerDto = await _customerService.GetCustomerAsync(customerId);
            var customer = await _customerRepository.FindByIdAsync(customerDto.Id);
            var group = customer.Group;
            if (group is null)
            {
                throw new InvalidOperationException("group cannot be null");
            }

            var orders = group.Orders.Select(x => new OrderDto(x)).ToList();
            var children = group.Children;
            if (children is not null)
            {
                children.ForEach(async x => orders.AddRange(await GetOrdersIncludingSubordinatesAsync(x.Customer.Id)));
            }

            return orders;
        }

        private List<OrderDto> FetchChildOrders(CustomerOrderGroup group, List<OrderDto> orders)
        {
            if (group is null)
            {
                throw new InvalidOperationException("group cannot be null");
            }

            orders.AddRange(group.Orders.Select(x => new OrderDto(x)));
            var children = group.Children;
            if (children is not null)
            {
                children.ForEach(x => orders.AddRange(FetchChildOrders(x.Customer.Group, orders)));
            }

            return orders;
        }


        public async Task<OrderDto> GetOrderByIdAsync(long orderId)
        {
            var authentication = _authenticationContextFacade.GetAuthentication().Identity.Name;
            var c = await _customerRepository.FindByNameAsync(authentication);
            var getOrdersIncludingSubordinates = await GetOrdersIncludingSubordinatesAsync(c.Id);
            var requested = new OrderDto(await _orderRepository.FindByIdAsync(orderId));
            if (getOrdersIncludingSubordinates.Contains(requested))
            {
                return requested;
            }

            return null;
        }

        public async Task<List<OrderDto>> GetLoggedCustomerOrders(bool includingSubordinates)
        {
            var authentication = _authenticationContextFacade.GetAuthentication().Identity.Name;
            var c = await _customerRepository.FindByNameAsync(authentication);
            if (includingSubordinates)
            {
                if (!c.Type.Equals(Customer.CustomerType.Company) && !c.Type.Equals(Customer.CustomerType.Division))
                {
                    throw new InvalidOperationException("not a company nor division");
                }

                return await GetOrdersIncludingSubordinatesAsync(c.Id);
            }
            else
            {
                return await _customerService.GetIndividualOrdersForCustomerAsync(c.Id);
            }
        }

        public async Task<decimal> CalculateTaxForOrderAsync(long orderId)
        {
            var order = await _orderRepository.FindByIdAsync(orderId);
            var initialValue = 0M;
            foreach (var tax in order.TaxRules)
            {
                if (tax.IsLinear)
                {
                    initialValue = initialValue + initialValue * tax.AFactor + tax.BFactor;
                }

                if (tax.IsSquare)
                {
                    initialValue = initialValue + (decimal) Math.Pow((double) initialValue, 2) * tax.ASquareFactor +
                                   initialValue * tax.BSquareFactor + tax.CSquareFactor;
                }
            }

            return initialValue;
        }
    }
}