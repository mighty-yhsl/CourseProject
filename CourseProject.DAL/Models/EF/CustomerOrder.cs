using System;
using System.Collections.Generic;

namespace CourseProject.DAL.Models.EF;

public partial class CustomerOrder
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int SellerId { get; set; }

    public int CustomerId { get; set; }

    public int StatusId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Seller Seller { get; set; } = null!;

    public virtual StatusOrder Status { get; set; } = null!;
}
