using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public virtual List<Student> Students { get; set; }
    }
}