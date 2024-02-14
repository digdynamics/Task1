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
        //private ApplicationDbContext db = new ApplicationDbContext();
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
            ViewBag.CountryId = _studentService.GetCountrySelectListForCreate();
            ViewBag.GradeId = _studentService.GetGradeSelectListForCreate();
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

            ViewBag.CountryId = _studentService.GetCountrySelectList(student.CountryId);
            ViewBag.GradeId = _studentService.GetGradeSelectList(student.GradeId);
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
            ViewBag.CountryId = _studentService.GetCountrySelectList(student.CountryId);
            ViewBag.GradeId = _studentService.GetGradeSelectList(student.GradeId);
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
            ViewBag.CountryId = _studentService.GetCountrySelectList(student.CountryId);
            ViewBag.GradeId = _studentService.GetGradeSelectList(student.GradeId);
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




        public ActionResult Student(Models.PagedList<Student> model, string studentName, int? studentId, int? page)
        {
            var result = _studentService.GetFilteredStudents(studentName, studentId);

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
                    byte[] fileContents = _studentService.ExportStudentsToExcel(studentIds);
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
                var students = _studentService.GetStudentsByIds(studentIds);
                return new ViewAsPdf("_PdfPartialView", students) { FileName = "StudentTable.pdf" };
            }
            else
            {
                return RedirectToAction("Student");
            }
            
        }


        public ActionResult StudentReport()
        {
            ViewBag.Students = _studentService.GetStudentsSelectList();
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
