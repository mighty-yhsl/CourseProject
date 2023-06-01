using CourseProject.DAL.Models.EF;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class CustomerVM
    {
        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string CustomerName { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string CustomerSurname { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string Phone { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(13, ErrorMessage = "Name length must not exceed 13 characters.")]
        public string Email { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string Addres { get; set; } = null!;

        public Customer ConvertToCustomer()
        {
            return new Customer
            {
                CustomerName = CustomerName,
                CustomerSurname = CustomerSurname,
                Phone = Phone,
                Email = Email,
                Addres = Addres
            };
        }
    }
}
