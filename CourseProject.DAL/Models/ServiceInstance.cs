using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DAL.Models
{
    public class ServiceInstance
    {
        public int ServiceInstanceId { get; set; }
        public int ServiceId { get; set; }
        public int ServiceStatusId { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
