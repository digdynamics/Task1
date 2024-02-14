using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task_1.Data;
using Task_1.Models;
using Task_1.Services;

namespace Task_1.Controllers
{
    public class TeachersController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private readonly  TeacherService _teacherService = new TeacherService();

        
        // GET: Teachers
        public ActionResult Index()
        {
            var teachers = _teacherService.GetTeachers();
            return View(teachers);
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = _teacherService.GetTeacherById(id.Value);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = _teacherService.GetDepartmentSelectListForCreate();
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherId,TeacherName,Address,MobileNumber,Age,DepartmentId")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _teacherService.AddTeacher(teacher);
                return RedirectToAction("Index");
               
            }

            ViewBag.DepartmentId = _teacherService.GetDepartmentSelectList(teacher.DepartmentId);
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = _teacherService.GetTeacherById(id.Value);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = _teacherService.GetDepartmentSelectList(teacher.DepartmentId);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherId,TeacherName,Address,MobileNumber,Age,DepartmentId")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _teacherService.UpdateTeacher(teacher);
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = _teacherService.GetDepartmentSelectList(teacher.DepartmentId);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = _teacherService.GetTeacherById(id.Value);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _teacherService.DeleteTeacher(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _teacherService.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Teacher()
        {
            var teachers = _teacherService.GetTeacherViews();
            return View(teachers);
        }

        public ActionResult TeacherDetails(int teacherId)
        {
            var teacherDetails = _teacherService.GetTeacherDetailsById(teacherId);
            return View(teacherDetails);
        }
    }
}
