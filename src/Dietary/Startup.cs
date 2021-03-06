using Dietary.DAL;
using Dietary.Models;
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
                .AddScoped<OrderService>()
                .AddScoped<IAuthenticationFacade, AuthenticationFacade>()
                .AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<ICustomerOrderGroupRepository, CustomerOrderGroupRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
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
