using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class FilterModel<T> where T : class
    {
        //public string studentName { get; set; }
        //public int? studentId { get; set; }
        public IPagedList<T> List { get; set; }
    }
}