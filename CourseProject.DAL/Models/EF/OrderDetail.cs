using System;
using System.Collections.Generic;

namespace CourseProject.DAL.Models.EF;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int TotalAmount { get; set; }

    public decimal TotalPrice { get; set; }

    public int CustomerOrderId { get; set; }

    public int TransportId { get; set; }

    public virtual CustomerOrder CustomerOrder { get; set; } = null!;

    public virtual Transport Transport { get; set; } = null!;
}
