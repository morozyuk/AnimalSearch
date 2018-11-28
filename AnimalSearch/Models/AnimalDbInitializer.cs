using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    public class AnimalDbInitializer : DropCreateDatabaseAlways<AnimalContext>  //was used to create new database when changed something in a database
    {
        protected override void Seed(AnimalContext db)
        {
          
           
        }
    }
}