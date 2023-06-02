using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DAL.Enums
{
    public enum StatusOrderEnum
    {
        WAITING_FOR_PROCESSING = 1,
        RENT,
        AWAITING_CONFIRMATION,
        CONFIRMED,
        CANCELLED,
        FOR_RENT
    }
}
