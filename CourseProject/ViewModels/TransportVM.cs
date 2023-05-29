
using CourseProject.DAL.Models.EF;

namespace CourseProject.ViewModels
{
    public class TransportVM
    {
        public string Name { get; set; } = null!;

        public int Speed { get; set; }

        public int Weightt { get; set; }

        public int EnginePower { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

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
                Price = Price
            };
        }
    }
}
