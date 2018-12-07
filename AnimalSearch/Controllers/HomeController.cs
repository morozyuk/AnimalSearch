using AnimalSearch.Helper;
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
        /// <summary>
        /// is used to set lng and lat for markers 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="longtitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public ActionResult UpdateGeo(int id, string longtitude, string latitude)
        {
            var animal = unitOfWork.Animals.Get(id);
            animal.Longtitude = longtitude;
            animal.Latitude = latitude;
            unitOfWork.Animals.Update(animal);
            unitOfWork.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        IEnumerable<Animal> Animals { get; set; }
        UnitOfWork unitOfWork;
        /// <summary>
        /// context set
        /// </summary>
        public HomeController()
        {
            unitOfWork = new UnitOfWork(new AnimalContext());
        }
        /// <summary>
        /// simple search
        /// </summary>
        /// <param name="query">string what we are looking for</param>
        /// <returns>json result</returns>
        public ActionResult GetAnimals(string query)
        {
            var result = unitOfWork.Animals.Search(query);
            return Json(result, JsonRequestBehavior.AllowGet); //allows use get method
        }

        public ActionResult Index()
        {
            return View(new List<Animal>());
        }

        /// <summary>
        /// polygon search
        /// </summary>
        /// <param name="cornes">array of poly markers</param>
        /// <returns>json result</returns>
        public ActionResult SearchPoly(string[][] cornes)
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
        /// <summary>
        /// create view adding
        /// </summary>
        /// <returns>create view</returns>
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Animal b, HttpPostedFileBase uploadImage)
        {
            //uploading image into a folder and set its path into a db
            if (ModelState.IsValid && uploadImage != null)//check if form is correct or image choosed
            {
                ImageAdd.Set(b, uploadImage);
                unitOfWork.Animals.Create(b);
                unitOfWork.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(b);
        }
        /// <summary>
        /// choosing edited element and view adding
        /// </summary>
        /// <returns>edit view</returns>
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

               ImageAdd.Set(b, uploadImage);//setting new info
                unitOfWork.Animals.Update(b);
                unitOfWork.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(b);
        }

        [HttpGet]
        public ActionResult Delete(int id)
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




