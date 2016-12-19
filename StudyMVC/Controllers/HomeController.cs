using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyMVC.Models;
using System.Data.Entity;

namespace StudyMVC.Controllers
{
    public class HomeController : Controller
    {
        private PersonContext PContext;
        private DbSet<Person> BirthdayDB;
        //private ImageContext ImContext;
        private DbSet<Image> ImageDB;
        private Person[] Birthdays;

        public HomeController()
        {
            PContext = new Models.PersonContext();
            BirthdayDB = PContext.BirthdayDB;
            /*BirthdayDB.Add(new Person { PersonID = 0, UniqName = "Alice", Birthday = new DateTime(2016, 1, 1) });
            BirthdayDB.Add(new Person { PersonID = 1, UniqName = "Lee", Birthday = new DateTime(2018, 1, 1) });
            BirthdayDB.Add(new Person { PersonID = 2, UniqName = "Sheng", Birthday = new DateTime(2018, 1, 1) });
            PContext.SaveChanges();
            Birthdays = BirthdayDB.ToArray();*/
        }

        [Route("Home/Index")]
        [HttpGet]
        // GET: Home
        public ViewResult Index()
        {
            return View(BirthdayDB.ToArray());
            //return View(Birthdays);
        }

        [Route("Home/Details/{pid:int}")]
        public ViewResult Details(int pid)
        {
            return View(BirthdayDB.FindByNumber(pid));
            /*if (0 <= pid && pid <= Birthdays.Length)
            {
                return View(Birthdays[pid]);
            }
            else
            { //Invalid personD
                return View("Index", Birthdays);
            }*/
        }
        // This method simply display the form
        [Route("Home/Edit/{pid:int}")]
        [HttpGet]
        public ViewResult Edit(int pid)
        {
            return View(BirthdayDB.FindByNumber(pid));
            //return View(Birthdays[pid]);
        }
        // The form postds back to here
        [Route("Home/Edit/{pid:int}")]
        [HttpPost]
        public ActionResult Edit(int pid, Person p)
        {
            if ((from b in BirthdayDB
                 where b.UniqName == p.UniqName &&
                 b.PersonID != p.PersonID
                 select b).Any())
            {
                ModelState.AddModelError("UniqName","Select a number which has not been used");
            }
            if (ModelState.IsValidField("Birthday")&&
                p.Birthday > DateTime.Now)
            {
                ModelState.AddModelError("Birthday", "Select a birth date in the past");
            }
            if (ModelState.IsValid){
                //Valid data, save in the database
                Person current = BirthdayDB.FindByNumber(pid);
                current.UniqName = p.UniqName;
                current.Birthday = p.Birthday;
                PContext.SaveChanges();
                //Birthdays[pid] = p;
                return RedirectToAction("EditSucess", p);
            }
            else
            {   //Validation failed, redisplay the form
                return View();
            }

        }
        [Route("Home/EditSucess")]
        public ViewResult EditSucess(Person p)
        {
            return View(p);
        }

        [Route("Home/Create/")]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [Route("Home/Create/")]
        [HttpPost]
        public ActionResult Create(Person p)
        {
            BirthdayDB.Add(p);
            PContext.SaveChanges();
            return RedirectToAction("CreateSucess",p);
        }

        [Route("Home/CreateSucess")]
        [HttpGet]
        public ViewResult CreateSucess(Person p)
        {
            return View(p);
        }

        [Route("Home/Images/{pid:int}")]

        public ViewResult Images(int pid)
        {
            ImageContext ImContext = new Models.ImageContext();
            Image[] Images = ImContext.ImageDB.FindByPersonID(pid).ToArray();
            return View(Images);
        }

        [Route("Home/UploadImage/{pid:int}")]
        [HttpGet]
        public ViewResult UploadImage(int pid)
        {
            return View(pid);
        }

        [Route("Home/UploadImage/{pid:int}")]
        [HttpPost]
        public ActionResult UploadImage(int pid, HttpPostedFileBase SelectedImage)
        {
            if (SelectedImage != null)
            {
                Image im = new Image
                {
                    PersonID = pid,
                    ImageData = new byte[SelectedImage.ContentLength],
                    ImageMimeType = SelectedImage.ContentType
                };
                SelectedImage.InputStream.Read(im.ImageData, 0, im.ImageData.Length);
                ImageContext ImContext = new Models.ImageContext();
                ImContext.ImageDB.Add(im);
                ImContext.SaveChanges();
            }
            return RedirectToAction("Images",pid);
        }

        [Route("Home/Delete/{pid:int}")]
        [HttpGet]
        public ViewResult Delete(int pid)
        {
            return View(BirthdayDB.FindByNumber(pid));
        }

        [Route("Home/Delete/{pid:int}")]
        [HttpPost]
        public ActionResult Delete(int pid, bool confirm) {
            if (confirm){
                ImageContext ImContext = new Models.ImageContext();
                ImContext.DeleteImageByPerson(pid);
                Person p = BirthdayDB.FindByNumber(pid);
                BirthdayDB.Remove(p);
                PContext.SaveChanges();
            }
            return RedirectToAction("Index", BirthdayDB.ToArray());
        }
    }
}