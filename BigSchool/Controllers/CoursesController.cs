using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.listCategory = context.Categories.ToList();

            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext context = new BigSchoolContext();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.listCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.lecturerId = user.Id;


            context.Courses.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                                                 .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course ObjCourse = temp.Course;
                ObjCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(ObjCourse.lecturerId).Name;
                courses.Add(ObjCourse);
            }
            return View(courses);
        }

        [Authorize]
        public ActionResult Mine()
        {
            ApplicationUser curentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(c => c.lecturerId == curentUser.Id && c.Datetime > DateTime.Now).ToList();
            foreach(Course i in courses)
            {
                i.LectureName = curentUser.Name;
            }
            return View(courses);
        }



        [Authorize]
        public ActionResult Edit(int sId)
        {
            BigSchoolContext context = new BigSchoolContext();
            var courses = from tt in context.Courses select tt;
            Course icourse = new Course();
            foreach(var item in courses)
            {
                if (item.Id == sId)
                {
                    icourse = item;
                    break;
                }
            }
            if (icourse == null)
            {
                return HttpNotFound();
            }
            return View(icourse);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "Id,lecturerId,Name,LectureName,Place,Datetime,CategoryId")] Course scourse)
        {
            BigSchoolContext context = new BigSchoolContext();
            if (ModelState.IsValid)
            {
                context.Entry(scourse).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Mine");
            }
            return View(scourse);
        }


        [Authorize]
        public ActionResult Delete(int sId)
        {
            BigSchoolContext context = new BigSchoolContext();
            var courses = from tt in context.Courses select tt;
            Course icourse = new Course();
            foreach (var item in courses)
            {
                if (item.Id == sId)
                {
                    icourse = item;
                    break;
                }
            }
            if (icourse == null)
            {
                return HttpNotFound();
            }
            return View(icourse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int sId, FormCollection collection)
        {
            BigSchoolContext context = new BigSchoolContext();
            var D_tin = context.Courses.Where(m => m.Id == sId).First();
            try
            {
                var D_tin2 = context.Attendances.Where(m => m.CourseId == sId).First();
                if (D_tin2 != null)
                {
                    context.Attendances.Remove(D_tin2);
                }
            }
            catch
            {

            }
            
           
            context.Courses.Remove(D_tin);

            context.SaveChanges();
            //var courses = from tt in context.Courses select tt;
            return RedirectToAction("Mine");
        }

        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =
            System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerId ==

            currentUser.Id).ToList();

            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendances.Where(p => p.Attendee ==

            currentUser.Id).ToList();

            var courses = new List<Course>();
            foreach (var course in listAttendances)

            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.lecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName =
                        System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.lecturerId).Name;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);
        }

    }
}