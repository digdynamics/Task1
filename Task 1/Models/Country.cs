using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public virtual List<Student> Students { get; set; }
    }
}