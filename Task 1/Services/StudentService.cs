using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public SelectList GetCountrySelectListForCreate()
        {
            var countries = _context.Countries.ToList();
            return new SelectList(countries, "CountryId", "CountryName");
        }

        public SelectList GetGradeSelectListForCreate()
        {
            var grades = _context.Grades.ToList();
            return new SelectList(grades, "GradeId", "GradeName");
        }

        public SelectList GetCountrySelectList(int countryId)
        {
            var countries = _context.Countries.ToList();
            return new SelectList(countries, "CountryId", "CountryName", countryId);
        }
        public SelectList GetGradeSelectList(int gradeId)
        {
            var grades = _context.Grades.ToList();
            return new SelectList(grades, "GradeId", "GradeName" , gradeId);
        }

        public List<Student> GetFilteredStudents(string studentName, int? studentId)
        {
            var students = _context.Students
                .Include(s => s.Country)
                .Include(s => s.Grade);

            if (studentName != null && studentName != "" && studentId != null)
            {
                students = students.Where(x => (x.StudentId == studentId) && (x.StudentName == studentName));
            }
            else if (studentName != null && studentId == null)
            {
                students = students.Where(x => x.StudentName == studentName);
            }
            else if (studentId != null)
            {
                students = students.Where(x => x.StudentId == studentId);
            }

            return students.ToList();
        }

        public byte[] ExportStudentsToExcel(string studentIds)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var idsArray = studentIds.Split(',').Select(int.Parse).ToList();

            var students = _context.Students
                .Include(s => s.Country)
                .Include(s => s.Grade)
                .Where(s => idsArray.Contains(s.StudentId))
                .ToList();

            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Students");

            worksheet.Cells["A1"].Value = "Student ID";
            worksheet.Cells["B1"].Value = "Student Name";
            worksheet.Cells["C1"].Value = "Birthday";
            // Add other headers as needed

            int row = 2;
            foreach (var student in students)
            {
                worksheet.Cells[string.Format("A{0}", row)].Value = student.StudentId;
                worksheet.Cells[string.Format("B{0}", row)].Value = student.StudentName;
                worksheet.Cells[string.Format("C{0}", row)].Value = student.BirthDay.ToString("MM/dd/yyyy");
                // Add other data as needed
                row++;
            }

            return excelPackage.GetAsByteArray();
        }


        public List<Student> GetStudentsByIds(string studentIds)
        {
            var idsArray = studentIds.Split(',').Select(int.Parse).ToList();
            var students = _context.Students
                .Include(s => s.Country)
                .Include(s => s.Grade)
                .Where(s => idsArray.Contains(s.StudentId))
                .ToList();

            return students;
        }

        public SelectList GetStudentsSelectList()
        {
            return new SelectList(_context.Students, "StudentId", "StudentName");
        }

    }

}