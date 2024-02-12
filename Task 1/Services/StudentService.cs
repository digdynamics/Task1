using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Task_1.Data;
using Task_1.Models;

namespace Task_1.Services
{
    public class StudentService
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        

        public List<Student> GetStudents()
        {
            return _context.Students.Include(s => s.Country).Include(s => s.Grade).ToList();
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.Include(s => s.Country).Include(s => s.Grade).FirstOrDefault(s => s.StudentId == id);
        }

        public void CreateStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }

}