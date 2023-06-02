
using CourseProject.DAL.Models.EF;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore;
using CourseProject.ViewModels;

namespace CourseProject.ViewModels
{
    public class OrderVM
    {
        public int? Id { get; set; }

        public string CustomerName { get; set; }

        public string Addres { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string? Description { get; set; }

        public int SellerId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int StatusId { get; set; }

        public IEnumerable<CartVM> Items { get; set; }


    }
}
