using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Task_1.Data;
using Task_1.Models;

namespace Task_1.Services
{
    public class GradeService
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public List<Grade> GetGrades()
        {
            return _context.Grades.ToList();
        }

        public Grade GetGradeById(int id)
        {
            return _context.Grades.Find(id);
        }

        public void AddGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            _context.SaveChanges();
        }

        public void UpdateGrade(Grade grade)
        {
            _context.Entry(grade).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteGrade(int id)
        {
            Grade grade = _context.Grades.Find(id);
            _context.Grades.Remove(grade);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}