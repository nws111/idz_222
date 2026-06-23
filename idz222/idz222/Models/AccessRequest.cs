using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idz222.Models
{
    public class AccessRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string RequestType { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
