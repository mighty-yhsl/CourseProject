
using CourseProject.DAL.Models.EF;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore;
using CourseProject.ViewModels;

namespace CourseProject.ViewModels
{
    public class OrderVM
    {
        [Range(1, 100000)]
        public int Id { get; set; }

        [MinLength(0, ErrorMessage = "Description length must be at least 1 characters.")]
        [MaxLength(256, ErrorMessage = "Description length must not exceed 40 characters.")]
        public string? Description { get; set; } = null;

        public string CustomerName { get; set; }

        public string CustomerSurname { get; set; }


        public string Phone { get; set; }

        public string Email { get; set; }

        public string Addres { get; set; }

        [Range(1, 100000)]
        public int SellerId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Range(1, 100000)]
        public int StatusId { get; set; }

        public IEnumerable<CartVM> Items { get; set; }


    }
}
