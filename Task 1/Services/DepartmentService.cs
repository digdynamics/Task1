using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Task_1.Data;
using Task_1.Models;

namespace Task_1.Services
{
    public class DepartmentService
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public List<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }

        public Department GetDepartmentById(int id)
        {
            return _context.Departments.Find(id);
        }

        public void AddDepartment(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public void UpdateDepartment(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteDepartment(int id)
        {
            Department department = _context.Departments.Find(id);
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}