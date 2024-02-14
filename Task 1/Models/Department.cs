using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public virtual List<Teacher> Teachers { get; set; }
    }
}