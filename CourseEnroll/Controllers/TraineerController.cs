using CourseEnroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CourseEnroll.Models.VM;
using System.IO;
using System.Web.UI.WebControls;

namespace CourseEnroll.Controllers
{
    public class TraineerController : Controller
    {
        TrainingEntities1 db = new TrainingEntities1();
        // GET: Traineer
        public ActionResult Index()
        {
            var traineer = db.Traineers.Include(c => c.Enrollments.Select(b => b.Course)).OrderByDescending(x => x.TraineeId).ToList();
            return View(traineer);
        }
        public ActionResult AddCourse(int? id)
        {
            ViewBag.Course = new SelectList(db.Courses.ToList(),"CourseId","CourseName",(id !=null)? id.ToString():"");
            return PartialView("addCourse");
        }
        public ActionResult Create() 
        {
        return View();
        }
        [HttpPost]
        public ActionResult Create(TraineerVM traineerVM, int[] CourseId)
        {
            if (ModelState.IsValid) 
            {
                Traineer traineer = new Traineer()
                {
                    TraineeName = traineerVM.TraineeName,
                    Age = traineerVM.Age,
                    DOB =  traineerVM.DOB,
                    MorningShift = traineerVM.MorningShift,
                };
                //For image
                HttpPostedFileBase file = traineerVM.PictureFile;
                if (file != null) 
                {
                string filePath = Path.Combine("/Images/",DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    traineer.Picture = filePath;
                }
                //Save all Post
              foreach(var item in CourseId)
                {
                    Enrollment enrollment = new Enrollment()
                    {
                        Traineer = traineer,
                        TraineeId = traineer.TraineeId,
                        CourseId = item
                    };
                    db.Enrollments.Add(enrollment);
                }
              db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int? id)
        {
            var traineer = db.Traineers.Include(c => c.Enrollments).
                   FirstOrDefault(x => x.TraineeId == id);
            var traineerVM = new TraineerVM()
            {
                TraineeId=traineer.TraineeId,
                TraineeName=traineer.TraineeName,  
                Picture = traineer.Picture
            };
            return View(traineerVM);
        }
        [HttpPost]
        public ActionResult Edit(TraineerVM traineerVM, int[] CourseId)
        {
            if(ModelState.IsValid)
            {
                Traineer traineer = new Traineer()
                {
                    TraineeId = traineerVM.TraineeId,
                    TraineeName = traineerVM.TraineeName,
                    Age = traineerVM.Age,
                    DOB = traineerVM.DOB
                };
                HttpPostedFileBase file = traineerVM.PictureFile;
                if(file != null)
                {
                    string filePath = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    traineer.Picture = filePath;
                }
                else
                {
                    traineer.Picture = traineerVM.Picture;
                }
                var existCourseEntry = db.Enrollments.Where(x => x.TraineeId == traineer.TraineeId).ToList();
                foreach(var item in existCourseEntry)
                {
                    db.Enrollments.Remove(item);
                }
                foreach(var item in CourseId)
                {
                    Enrollment enrollment = new Enrollment()
                    {
                        Traineer = traineer,
                        TraineeId = traineer.TraineeId,
                        CourseId = item
                    };
                    db.Enrollments.Add(enrollment);
                }
                db.Entry(traineer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();  
        }
        public ActionResult Delete(int? id)
        {
    if(id != null)
            {
                var trainer = db.Traineers.FirstOrDefault(x => x.TraineeId == id);
                var enrollmentinfo = db.Enrollments.Where(x => x.TraineeId == id).ToList();

                db.Enrollments.RemoveRange(enrollmentinfo);
                db.Traineers.Remove(trainer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}