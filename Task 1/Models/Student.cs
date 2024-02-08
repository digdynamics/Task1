using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        [ForeignKey("Grade")]
        public int GradeId { get; set; }
        public virtual Grade Grade { get; set; }

    }
}