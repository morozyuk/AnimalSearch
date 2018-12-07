using AnimalSearch.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

/// <summary>
/// gives us an opportunity to abstract from connections and is an intermediate section between classes and rest part of a program
/// </summary>
namespace AnimalSearch.Repository
{
     /// <summary>
     /// interface implementing repository methods
     /// </summary>
     /// <typeparam name="T"></typeparam>
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(); //get all data
        T Get(int id);//getting only one item from a db
        void Create(T item);//creating new
        void Update(T item);//updating items info
        void Delete(int id);//deleting something
        List<AnimalDto> Search(string query);//normal search
        List<AnimalDto> SearchPoly(string[][] corners);//polygon search

    }

    public class AnimalRepository : IRepository<Animal>
    {
        /// <summary>
        /// set context
        /// </summary>
        /// <param name="context">animal's context instance</param>
        public AnimalRepository(AnimalContext context)
        {
            db = context;
        }
        private AnimalContext db;

        /// <summary>
        /// get all data from db
        /// </summary>
        /// <returns>data</returns>
        public IEnumerable<Animal> GetAll()
        {
            return db.Animals;
        }

        /// <summary>
        ///  gets db object
        /// </summary>
        /// <param name="id">animal's id</param>
        /// <returns>object whis we was looking for</returns>
        public Animal Get(int id)
        {
            return db.Animals.Find(id);
        }

        public void Create(Animal animal)
        {
            db.Animals.Add(animal);
        }

        internal IEnumerable<object> ToList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// updating data
        /// </summary>
        /// <param name="animal">instance of animal class</param>
        public void Update(Animal animal)
        {
            db.Entry(animal).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Animal animal = db.Animals.Find(id);//find by id
            if (animal != null)
                db.Animals.Remove(animal);
        }

        /// <summary>
        /// search by name (animalDto used to transfer data)
        /// </summary>
        /// <param name="query">search string which we are looking for</param>
        /// <returns>animalDTO instance of searched animal</returns>
        public List<AnimalDto> Search(string query)
        {
            var animals = db.Animals.ToList();
            if (!string.IsNullOrEmpty(query))//if search fill is not empty
                animals = animals.Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();  // linq to search


            //class AnimalDTO is used to transfer all data into one  object
            var result = animals.Select(x =>
                new AnimalDto()
                {
                    Id = x.Id,
                    Address = x.Adress,
                    Name = x.Name,
                    Breed = x.Breed,
                    Kind = x.Kind,
                    Description = x.Description,
                    ImageSrc = x.Path
                }).ToList();
            return result;
        }

        /// <summary>
        /// polygon search
        /// </summary>
        /// <param name="corners">mass of polygon's markers</param>
        /// <returns>animalDTO instance of searched animal by polygon</returns>
        public List<AnimalDto> SearchPoly(string[][] corners)
        {
            List<AnimalPointDto> PloyPoints = new List<AnimalPointDto>();//list of polygon points


            var animals = db.Animals
                .Where(x => !string.IsNullOrEmpty(x.Latitude) && !string.IsNullOrEmpty(x.Longtitude)).ToList();

            List<AnimalPointDto> points = new List<AnimalPointDto>();//new list of points for markers in polygon
            foreach (var x in animals)//setting data into a point class
            {
                AnimalPointDto p = new AnimalPointDto();
                p.Animal = new Animal()
                {
                    Id = x.Id,
                    Adress = x.Adress,
                    Name = x.Name,
                    Kind = x.Kind,
                    Breed = x.Breed,
                    Description = x.Description,
                    Path = x.Path,

                    Latitude = x.Latitude,
                    Longtitude = x.Longtitude
                };

                p.Point = new Point();
                //making invariant for all languages of computer system for points in polygon
                p.Point.X = decimal.Parse(p.Animal.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                p.Point.Y = decimal.Parse(p.Animal.Longtitude, System.Globalization.CultureInfo.InvariantCulture);
                points.Add(p);
            }

            List<Point> list = new List<Point>();
            foreach (string[] corner in corners)//making invariant for all languages of computer system for markers of polygon
            {
                decimal lat = decimal.Parse(corner[0], System.Globalization.CultureInfo.InvariantCulture);
                decimal lng = decimal.Parse(corner[1], System.Globalization.CultureInfo.InvariantCulture);
                list.Add(new Point(lat, lng));
            }

            var Xmin = list.Select(x => x.X).Min(x => x);//setting corners of our polygon
            var Xmax = list.Select(x => x.X).Max(x => x);
            var Ymin = list.Select(x => x.Y).Min(x => x);
            var Ymax = list.Select(x => x.Y).Max(x => x);


            foreach (var p in points)
            {
                // p is your point, p.x is the x coord, p.y is the y coord
                if (!(p.Point.X < Xmin || p.Point.X > Xmax || p.Point.Y < Ymin || p.Point.Y > Ymax))//check what surrounds our point
                {
                    PloyPoints.Add(p);

                }
                else
                {
                    // Definitely not within the polygon!
                }
            }
            //class AnimalDTO is used to transfer all data into one  object
            var result = PloyPoints.Select(x =>
                new AnimalDto()
                {
                    Id = x.Animal.Id,
                    Address = x.Animal.Adress,
                    Name = x.Animal.Name,
                    Breed = x.Animal.Breed,
                    Kind = x.Animal.Kind,
                    Description = x.Animal.Description,
                    ImageSrc = x.Animal.Path
                }).ToList();
            return result;
        }
    

    }
}