using CourseProject.DAL.Models.EF;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class SellerVM
    {
        [Range(1, 100000)]
        public string Id { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string SellerName { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string SellerSurname { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string Phone { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Name length must be at least 1 characters.")]
        [MaxLength(40, ErrorMessage = "Name length must not exceed 40 characters.")]
        public string Email { get; set; } = null!;

        public Seller ConvertToSeller()
        {
            return new Seller
            {
                SellerName = SellerName,
                SellerSurname = SellerSurname,
                Phone = Phone,
                Email = Email,
            };
        }
    }
}
