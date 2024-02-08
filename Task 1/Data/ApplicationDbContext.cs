using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Task_1.Models;

namespace Task_1.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext()
           : base("name=ApplicationDbContext")
        {
        }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
    }
}