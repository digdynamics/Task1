using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_1.Data;
using Task_1.Models;

namespace Task_1.Services
{
    public class TeacherService
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public List<Teacher> GetTeachers()
        {
            return _context.Teachers.Include(t => t.Department).ToList();
        }

        public Teacher GetTeacherById(int id)
        {
            return _context.Teachers.Find(id);
        }

        public void AddTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
        }

        public void UpdateTeacher(Teacher teacher)
        {
            _context.Entry(teacher).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteTeacher(int id)
        {
            Teacher teacher = _context.Teachers.Find(id);
            _context.Teachers.Remove(teacher);
            _context.SaveChanges();
        }

        public List<Teacher_View> GetTeacherViews()
        {
            return _context.Database.SqlQuery<Teacher_View>("EXEC GetAllTeachers").ToList();
        }

        public Teacher_View GetTeacherDetailsById(int teacherId)
        {
            return _context.Database.SqlQuery<Teacher_View>("exec GetTeacherDetailsById @teacherId", new SqlParameter("teacherId", teacherId)).SingleOrDefault();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public SelectList GetDepartmentSelectList(int selectedDepartmentId)
        {
            var departments = _context.Departments.ToList();
            return new SelectList(departments, "DepartmentId", "DepartmentName", selectedDepartmentId);
        }
        public SelectList GetDepartmentSelectListForCreate()
        {
            var departments = _context.Departments.ToList();
            return new SelectList(departments, "DepartmentId", "DepartmentName");
        }
    }
}