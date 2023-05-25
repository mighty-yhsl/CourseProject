using System;
using System.Collections.Generic;

namespace CourseProject.DAL.Models.EF;

public partial class Transport
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Speed { get; set; }

    public int Weightt { get; set; }

    public int EnginePower { get; set; }

    public int Amount { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public int ManufacturerId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
