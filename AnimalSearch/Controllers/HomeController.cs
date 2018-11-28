using AnimalSearch.Models;
using AnimalSearch.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace AnimalSearch.Controllers
{

    public class HomeController : Controller
    {
        IEnumerable<Animal> Animals { get; set; }
        UnitOfWork unitOfWork;
        public HomeController()   //constructor without parameters where we set context
        {
            unitOfWork = new UnitOfWork(new AnimalContext());
        }

        public ActionResult GetAnimals(string query)//normal search 
        {
            var result = unitOfWork.Animals.Search(query);
            return Json(result, JsonRequestBehavior.AllowGet); //allows use get method
        }

        public ActionResult Index()
        {
            return View(new List<Animal>());
        }

        public ActionResult SearchPoly(string[][] cornes)//polygon search
        {
            List<Point> fPoints = new List<Point>(); //list of polygon points

            //check if polygon is setted on a map
            if (cornes == null)
                return Json(fPoints, JsonRequestBehavior.AllowGet);
            else
            {
                var result = unitOfWork.Animals.SearchPoly(cornes);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()//adding create view
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Animal b, HttpPostedFileBase uploadImage)
        {
            //uploading image into a folder and set its path into a db
            if (ModelState.IsValid && uploadImage != null)//check if form is correct or image choosed
            {
                // guid was used to set unique names for images
                string imgName = $"{ Guid.NewGuid().ToString() }_{ uploadImage.FileName}";
                string imgSrc = $"/Content/Images/{imgName}";  //setting path for images
                uploadImage.SaveAs(AppDomain.CurrentDomain.BaseDirectory + $"\\Content\\Images\\{imgName}"); //adding image in the folder
 
                b.Path = imgSrc;   // saving path
                unitOfWork.Animals.Create(b);
                unitOfWork.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(b);
        }

        [HttpGet]
        public ActionResult Edit(int id)//get item by id
        {
            Animal b = unitOfWork.Animals.Get(id);   //  editing by id
            if (b == null)
                return HttpNotFound();
            return View(b);
        }

        [HttpPost]
        public ActionResult Edit(Animal b, HttpPostedFileBase uploadImage)//editing item
        {
            //updating image into a folder and its path
            if (ModelState.IsValid && uploadImage != null)//check for valid form and image
            {
                // guid was used to set all images unique name
                string imgName = $"{ Guid.NewGuid().ToString() }_{ uploadImage.FileName}";
                string imgSrc = $"/Content/Images/{imgName}"; //setting path for images
                uploadImage.SaveAs(AppDomain.CurrentDomain.BaseDirectory + $"\\Content\\Images\\{imgName}"); //saving image in the folder
              
                b.Path = imgSrc;   // saving path
                unitOfWork.Animals.Update(b);
                unitOfWork.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(b);
        }

        [HttpGet]
        public ActionResult Delete(int id)//deleting item from a db by id
        {
            unitOfWork.Animals.Delete(id);
            unitOfWork.SaveChanges();
            return new HttpStatusCodeResult(200); //returns standart answer about successful request
        }

        //garbage collector call
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}




