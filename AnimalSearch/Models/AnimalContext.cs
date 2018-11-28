using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    public class AnimalContext: DbContext
    {
        public AnimalContext()
             : base("AnimalContext") //setting database name
        {
        }
        public DbSet<Animal> Animals { get; set; } //creating property for all collections of data
    }
}