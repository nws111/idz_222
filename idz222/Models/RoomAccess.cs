using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idz222.Models
{
    public class RoomAccess
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool IsActive { get; set; }
    }
}
