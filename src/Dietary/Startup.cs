using Dietary.DAL;
using Dietary.Models;
using Dietary.Models.NewProducts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dietary
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<CustomerService>()
                .AddScoped<OldProductService>()
                .AddScoped<OrderService>()
                .AddScoped<TaxRuleService>()
                .AddScoped<IAuthenticationFacade, AuthenticationFacade>()
                .AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<ICustomerOrderGroupRepository, CustomerOrderGroupRepository>()
                .AddScoped<IOldProductRepository, OldProductRepository>()
                .AddScoped<IOldProductDescriptionRepository, OldProductDescriptionRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<ITaxConfigRepository, TaxConfigRepository>()
                .AddScoped<ITaxRuleRepository, TaxRuleRepository>()
                .AddDbContext<DietaryDbContext>(x => x.UseInMemoryDatabase("dietary"))
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DietaryDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Dietary API"));
            });

            dbContext.Database.EnsureCreated();
        }
    }
}
