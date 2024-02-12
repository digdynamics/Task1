using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task_1.Data;
using Task_1.Models;
using System.Drawing;
using Rotativa.MVC;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;
using PagedList;
using Task_1.Services;

namespace Task_1.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly StudentService _studentService;

        public StudentsController()
        {
            
            _studentService = new StudentService();
        }

        // GET: Students
        public ActionResult Index()
        {
            var students = _studentService.GetStudents();
            return View(students);
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentService.GetStudentById((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,StudentName,BirthDay,Address,MobileNumber,CountryId,GradeId")] Student student)
        {
            if (ModelState.IsValid)
            {
                //db.Students.Add(student);
                //db.SaveChanges();
                _studentService.CreateStudent(student);
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", student.CountryId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", student.GradeId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentService.GetStudentById((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", student.CountryId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", student.GradeId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,StudentName,BirthDay,Address,MobileNumber,CountryId,GradeId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _studentService.UpdateStudent(student);
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", student.CountryId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeName", student.GradeId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _studentService.GetStudentById((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _studentService.DeleteStudent(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               _studentService.Dispose();
            }
            base.Dispose(disposing);
        }


        

        public ActionResult Student(FilterModel<Student> model, string studentName, int? studentId, int? page)
        {
            var students = db.Students
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
            else
            {
                students = students;
            }

            var result = students.ToList();

            int pageSize = 2; // Number of students per page
            int pageNumber = (page ?? 1);

            model.List = result.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_StudentTablePartialView", model);
            }

            return View(model);
        }




        public ActionResult ExportToExcel(string studentIds)
        {
            if (studentIds != null)
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var idsArray = studentIds.Split(',').Select(int.Parse).ToList();

                    var students = db.Students
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
                        worksheet.Cells[string.Format("c{0}", row)].Value = student.BirthDay.ToString("MM/dd/yyyy");
                        // Add other data as needed
                        row++;
                    }

                    byte[] fileContents = excelPackage.GetAsByteArray();
                    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
                }
                catch (Exception ex)
                {

                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("Student");
            }           
        }



        

        public ActionResult GeneratePdf(string studentIds)
        {
            if (studentIds != null)
            {
                var idsArray = studentIds.Split(',').Select(int.Parse).ToList();
                var students = db.Students
                        .Include(s => s.Country)
                        .Include(s => s.Grade)
                        .Where(s => idsArray.Contains(s.StudentId))
                        .ToList();
                return new ViewAsPdf("_PdfPartialView", students) { FileName = "StudentTable.pdf" };
            }
            else
            {
                return RedirectToAction("Student");
            }
            
        }


        public ActionResult StudentReport()
        {
            ViewBag.Students = new SelectList(db.Students, "StudentId", "StudentName");
            return View();
        }

       

        public ActionResult PrintReport(int studentId)
        {
            var student = _studentService.GetStudentById(studentId);

            var reportViewer = new ReportViewer();
            reportViewer.LocalReport.ReportPath = Server.MapPath("~/RDLC/Report.rdlc"); 
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Students", new List<Student> { student })); 

            byte[] bytes = reportViewer.LocalReport.Render("PDF"); 

            return File(bytes, "application/pdf", "StudentInfoReport.pdf"); // Return the report file to the user
        }





    }
}
