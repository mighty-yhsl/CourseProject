using System;
using System.Collections.Generic;

namespace CourseProject.DAL.Models.EF;

public partial class Manufacturer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Transport> Transports { get; set; } = new List<Transport>();
}
