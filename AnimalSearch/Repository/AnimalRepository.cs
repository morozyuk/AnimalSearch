using AnimalSearch.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AnimalSearch.Repository
{
    /* gives us an opportunity to abstract from connections and is an intermediate section between classes and rest part of a program
     */
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(); //get all data
        T Get(int id);//getting only one item from a db
        void Create(T item);//creating new
        void Update(T item);//updating items info
        void Delete(int id);//deleting something
        List<AnimalDTO> Search(string query);//normal search
        List<AnimalDTO> SearchPoly(string[][] corners);//polygon search

    }

    public class AnimalRepository : IRepository<Animal>
    {
        //constructor to set context
        public AnimalRepository(AnimalContext context)
        {
            db = context;
        }
        private AnimalContext db;
        // get all data from db
        public IEnumerable<Animal> GetAll()
        {
            return db.Animals;
        }
        // get one db object
        public Animal Get(int id)
        {
            return db.Animals.Find(id);
        }


        //adding new element in db
        public void Create(Animal animal)
        {
            db.Animals.Add(animal);
        }

        internal IEnumerable<object> ToList()
        {
            throw new NotImplementedException();
        }
        //updating data
        public void Update(Animal animal)
        {
            db.Entry(animal).State = EntityState.Modified;
        }
        //deleting
        public void Delete(int id)
        {
            Animal animal = db.Animals.Find(id);//find by id
            if (animal != null)
                db.Animals.Remove(animal);
        }
        //search by name (a class was used to make work with data easier)
        public List<AnimalDTO> Search(string query)
        {
            var animals = db.Animals.ToList();
            if (!string.IsNullOrEmpty(query))//if search fill is not empty
                animals = animals.Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();  // linq to search


            //class AnimalDTO is used to transfer all data into one  object
            var result = animals.Select(x =>
                new AnimalDTO()
                {
                    id = x.Id,
                    address = x.Adress,
                    name = x.Name,
                    breed = x.Breed,
                    kind = x.Kind,
                    description = x.Description,
                    imageSrc = x.Path
                }).ToList();
            return result;
        }
        //polygon search
        public List<AnimalDTO> SearchPoly(string[][] corners)
        {
            List<Point> fPoints = new List<Point>();//list of polygon points


            var animals = db.Animals
                .Where(x => !string.IsNullOrEmpty(x.Latitude) && !string.IsNullOrEmpty(x.Longtitude)).ToList();

            List<Point> points = new List<Point>();//new list of points for markers in polygon
            foreach (var x in animals)//setting data into a point class
            {
                var p = new Point();
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
                //making invariant for all languages of computer system for points in polygon
                p.X = decimal.Parse(p.Animal.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                p.Y = decimal.Parse(p.Animal.Longtitude, System.Globalization.CultureInfo.InvariantCulture);
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
                if (!(p.X < Xmin || p.X > Xmax || p.Y < Ymin || p.Y > Ymax))//check what surrounds our point
                {
                    fPoints.Add(p);

                }
                else
                {
                    // Definitely not within the polygon!
                }
            }
            //class AnimalDTO is used to transfer all data into one  object
            var result = fPoints.Select(x =>
                new AnimalDTO()
                {
                    id = x.Animal.Id,
                    address = x.Animal.Adress,
                    name = x.Animal.Name,
                    breed = x.Animal.Breed,
                    kind = x.Animal.Kind,
                    description = x.Animal.Description,
                    imageSrc = x.Animal.Path
                }).ToList();
            return result;
        }
    

    }
}