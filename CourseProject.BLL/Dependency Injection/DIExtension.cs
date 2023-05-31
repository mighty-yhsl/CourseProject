using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Repositories;
using CourseProject.BLL.Services;
using CourseProject.BLL.Validators;
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
            services.AddTransient<TransportRepository>();
            services.AddTransient<OrderRepository>();
            services.AddTransient<CustomerRepository>();
            services.AddTransient<SellerRepository>();
            services.AddTransient<TransportService>();
            services.AddTransient<TransportValidator>();
            services.AddTransient<SellerService>();
            services.AddTransient<SellerValidator>();
        }
    }
}
