using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class PagedList<T> where T : class
    {
        
        public IPagedList<T> List { get; set; }
    }
}