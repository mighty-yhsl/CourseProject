using CourseProject.BLL.Repositories;
using CourseProject.BLL.Services;
using CourseProject.DAL.Models.EF;
using CourseProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace CourseProject.Controllers
{
    public class CustomerController:Controller
    {
        public CustomerController()
        {

        }

        public ActionResult Get([FromServices] CustomerRepository customerRepository)
        {
            return View(customerRepository.Get());
        }

        public ActionResult GetAllCustomer([FromServices] CustomerRepository customerRepository)
        {
            return View(customerRepository.Get());
        }

        public ActionResult GetCustomerName([FromServices] CustomerService customerService, CustomerVM model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.CustomerName))
                {
                    Customer customer = customerService.GetCustomerName(model.CustomerName);
                    if (customer != null)
                    {
                        var customerVM = new CustomerVM
                        {
                            CustomerName = customer.CustomerName,
                            CustomerSurname = customer.CustomerSurname,
                            Phone = customer.Phone,
                            Email = customer.Email,
                            Addres = customer.Addres
                        };

                        var customerList = new List<CustomerVM> { customerVM };
                        return View(customerList);
                    }
                    else
                    {
                        Console.WriteLine("Customer not found !");
                    }
                }
                else
                {
                    Console.WriteLine("CustomerName is null or empty!");
                }

                return Redirect("/customer/GetAllCustomer");
            }
            catch (ArgumentException ex)
            {
                ViewData["Exception"] = ex.Message;
                return View("/customer/GetAllCustomer");
            }
        }
    }
}
