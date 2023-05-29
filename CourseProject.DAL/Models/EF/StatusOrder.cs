using System;
using System.Collections.Generic;

namespace CourseProject.DAL.Models.EF;

public partial class StatusOrder
{
    public int Id { get; set; }

    public string StatusOrderName { get; set; } = null!;

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
}
