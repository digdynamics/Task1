using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public int Age { get; set; }
        
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}