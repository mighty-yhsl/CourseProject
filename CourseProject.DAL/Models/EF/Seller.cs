using System;
using System.Collections.Generic;

namespace CourseProject.DAL.Models.EF;

public partial class Seller
{
    public int Id { get; set; }

    public string SellerName { get; set; } = null!;

    public string SellerSurname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
}
