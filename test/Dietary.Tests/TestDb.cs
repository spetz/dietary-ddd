using System;
using Dietary.DAL;
using Dietary.Models;
using Microsoft.EntityFrameworkCore;

namespace Dietary.Tests
{
    public class TestDb : IDisposable
    {
        public DietaryDbContext DbContext { get; }

        public TestDb()
        {
            DbContext = new DietaryDbContext(new DbContextOptionsBuilder<DietaryDbContext>()
                .UseInMemoryDatabase($"dietary-{Guid.NewGuid()}").Options);

            InitData();
        }
        
        public void Dispose()
        {
            DbContext?.Database.EnsureDeleted();
            DbContext?.Dispose();
        }

        private void InitData()
        {
            DbContext.Database.EnsureCreated();
            AddCustomers();
            AddCustomerOrderGroups();
            AddOrders();
            DbContext.SaveChanges();
        }

        private void AddCustomers()
        {
            AddCustomer(1, "ZDROWO JEDZ", Customer.CustomerType.Company);
            AddCustomer(2, "CATERINX", Customer.CustomerType.Company);
            AddCustomer(3, "PIOTR ADMINOWSKI", Customer.CustomerType.Admin);
            AddCustomer(4, "K$L", Customer.CustomerType.Division);
            AddCustomer(5, "LOGISTYKA", Customer.CustomerType.Division);
            AddCustomer(6, "ZAMÓWIENIA", Customer.CustomerType.Division);
            AddCustomer(7, "KATARZYNA", Customer.CustomerType.Person);
            AddCustomer(8, "LUDWIK", Customer.CustomerType.Representative);
            AddCustomer(9, "PAWEŁ LOGISTYK", Customer.CustomerType.Person);
            AddCustomer(10, "EDWARD SPRZEDAWCA", Customer.CustomerType.Person);

            void AddCustomer(long id, string name, Customer.CustomerType type)
                => DbContext.Customers.Add(new Customer
                {
                    Id = id,
                    Name = name,
                    Type = type
                });
        }

        private void AddCustomerOrderGroups()
        {
            AddCustomerOrderGroup(3, "PIOTR ADMIN", 3, null);
            AddCustomerOrderGroup(1, "ZDROWO JEDZ MAIN", 1, 3);
            AddCustomerOrderGroup(2, "CATERINX MAIN", 2, 3);
            AddCustomerOrderGroup(4, "ZDROWO JEDZ K$L", 4, 1);
            AddCustomerOrderGroup(5, "ZDROWO JEDZ LOGISTYKA", 5, 1);
            AddCustomerOrderGroup(6, "CATERINX ZAMÓWIENIA", 6, 2);
            AddCustomerOrderGroup(7, "KATARZYNA K$L ZDROWO JEDZ", 7, 4);
            AddCustomerOrderGroup(8, "LUDWIK K$L ZDROWO JEDZ", 8, 4);
            AddCustomerOrderGroup(9, "PAWEŁ LOGISTYKA CATERINX", 9, 5);
            AddCustomerOrderGroup(10, "EDWARD SPRZEDAWCA", 10, 6);
            
            void AddCustomerOrderGroup(long id, string description, long customerId, long? parentId)
                => DbContext.CustomerOrderGroups.Add(new CustomerOrderGroup
                {
                    Id = id,
                    Description = description,
                    CustomerId = customerId,
                    ParentId = parentId
                });
        }

        private void AddOrders()
        {
            // main Caterinx
            AddOrder(5, Order.OrderState.Paid, Order.OrderType.Phone, 2);
            
            // logistyka Zdrowo Jedz
            AddOrder(1, Order.OrderState.Initial, Order.OrderType.Phone, 5);
            AddOrder(2, Order.OrderState.Paid, Order.OrderType.Phone, 5);
            
            // zamówienia Caterinx
            AddOrder(3, Order.OrderState.Paid, Order.OrderType.Phone, 6);
            AddOrder(4, Order.OrderState.Paid, Order.OrderType.Wire, 6);
            
            // kasia k$l
            AddOrder(6, Order.OrderState.Paid, Order.OrderType.Phone, 7);
            AddOrder(7, Order.OrderState.Paid, Order.OrderType.Phone, 7);
            
            // edward sprzedawca Caterinx
            AddOrder(8, Order.OrderState.Paid, Order.OrderType.Phone, 10);
            
            void AddOrder(long id, Order.OrderState state, Order.OrderType type, long groupId)
                => DbContext.Orders.Add(new Order
                {
                    Id = id,
                    State = state,
                    Type = type,
                    CustomerOrderGroupId = groupId
                });
        }
    }
}