using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Repositories;
using CourseProject.DAL.Models.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Dependency_Injection
{
    public static class DIExtension
    {
        public static void AddDependencies(this IServiceCollection services) 
        {
            services.AddTransient<ITransportRepository, TransportRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ISellerRepository, SellerRepository>();
            services.AddDbContext<ShopContext>();
        }
    }
}
