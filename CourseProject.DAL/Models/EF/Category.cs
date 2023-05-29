using System;
using System.Collections.Generic;

namespace CourseProject.DAL.Models.EF;

public partial class Category
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Transport> Transports { get; set; } = new List<Transport>();
}
