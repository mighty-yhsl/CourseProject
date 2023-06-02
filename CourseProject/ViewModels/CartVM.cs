using CourseProject.DAL.Models.EF;
using System.ComponentModel.DataAnnotations;
using CourseProject.DAL.Models;
using CourseProject.ViewModels;
namespace CourseProject.ViewModels
{
    public class CartVM
    {
        public Transport Transport { get; set; }
        public int Count { get; set; }
    }
}
