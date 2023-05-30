
using CourseProject.DAL.Models.EF;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class TransportVM
    {

        [MinLength(1, ErrorMessage = "Name length must be at least 2 characters.")]
        [MaxLength(64, ErrorMessage = "Name length must not exceed 50 characters.")]
        public string Name { get; set; } = null!;

        [Range(1, 70)]
        public int Speed { get; set; }

        [Range(1, 50)]
        public int Weightt { get; set; }

        [Range(1, 1000)]
        public int EnginePower { get; set; }

        [Range(1, 50)]
        public int Amount { get; set; }

        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Range(1, 5)]
        public int CategoryId { get; set; }

        [Range(1, 5)]
        public int ManufacturerId { get; set; }

        public Transport ConvertToTransport() 
        {
            return new Transport
            {
                Name = Name,
                Speed = Speed,
                Weightt = Weightt,
                EnginePower = EnginePower,
                Amount = Amount,
                Price = Price,
                ManufacturerId = ManufacturerId,
                CategoryId = CategoryId 
            };
        }
    }
}
