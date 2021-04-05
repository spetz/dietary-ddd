using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Dietary.DAL;
using Dietary.Models;
using NSubstitute;
using Xunit;

namespace Dietary.Tests
{
    public class OrdersTest : IDisposable
    {
        [Fact]
        public async Task companyTest()
        {
            // caterinx
            var ordersForCompanyCaterinx = await _orderService.GetOrdersForCompanyAsync(2);
            Assert.Equal(4, ordersForCompanyCaterinx.Count);

            // zdrowo jedz
            var ordersForCompanyZdrowoJedz = await _orderService.GetOrdersForCompanyAsync(1);
            Assert.Equal(4, ordersForCompanyZdrowoJedz.Count);
        }

        [Fact]
        public async Task adminTest()
        {
            // piotr admin
            var adminOrders = await _orderService.GetOrdersForAdminAsync(3);
            Assert.Equal(8, adminOrders.Count);
        }

        [Fact]
        public async Task divisionTest()
        {
            //logistyka zdrowo jedz
            var logistyka = await _orderService.GetOrdersForCompanyAsync(5);
            Assert.Equal(2, logistyka.Count);

            //k$l zdrowo jedz
            var kl = await _orderService.GetOrdersForCompanyAsync(4);
            Assert.Equal(2, kl.Count);

            //zamowienia caterinx
            var zamowienia = await _orderService.GetOrdersForCompanyAsync(6);
            Assert.Equal(3, zamowienia.Count);
        }

        [Fact]
        public async Task personOrRepresentativeTest()
        {
            // kasia k$l
            var katarzyna = await _customerService.GetIndividualOrdersForCustomerAsync(7);
            Assert.Equal(2, katarzyna.Count);

            // kasia k$l
            var ludwik = await _customerService.GetIndividualOrdersForCustomerAsync(8);
            Assert.Empty(ludwik);

            // pawel Logistyk k$l
            var pawelLogistyk = await _customerService.GetIndividualOrdersForCustomerAsync(9);
            Assert.Empty(pawelLogistyk);

            // edward sprzedawca caterinx
            var edwardSprzedawca = await _customerService.GetIndividualOrdersForCustomerAsync(10);
            Assert.Single(edwardSprzedawca);
        }

        [Fact]
        public async Task testLogged()
        {
            var principal = new ClaimsPrincipal(new GenericIdentity("KATARZYNA"));
            _authenticationFacade.GetAuthentication().Returns(principal);
            var katarzyna = await _orderService.GetLoggedCustomerOrders(false);
            Assert.Equal(2, katarzyna.Count);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.GetLoggedCustomerOrders(true));

            principal = new ClaimsPrincipal(new GenericIdentity("ZDROWO JEDZ"));
            _authenticationFacade.GetAuthentication().Returns(principal);
            var zdrowoJedz = await _orderService.GetLoggedCustomerOrders(true);
            Assert.Equal(4, zdrowoJedz.Count);
        }

        private readonly TestDb _testDb;
        private readonly DietaryDbContext _dbContext;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerOrderGroupRepository _customerOrderGroupRepository;
        private readonly IAuthenticationFacade _authenticationFacade;
        private readonly IOrderRepository _orderRepository;
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;

        public OrdersTest()
        {
            _testDb = new TestDb();
            _dbContext = _testDb.DbContext;
            _customerRepository = new CustomerRepository(_dbContext);
            _customerOrderGroupRepository = new CustomerOrderGroupRepository(_dbContext);
            _orderRepository = new OrderRepository(_dbContext);
            _authenticationFacade = Substitute.For<IAuthenticationFacade>();
            _customerService = new CustomerService(_customerRepository);
            _orderService = new OrderService(_customerService, _customerRepository, _orderRepository,
                _customerOrderGroupRepository, _authenticationFacade);
        }

        public void Dispose()
        {
            _testDb.Dispose();
        }
    }
}
