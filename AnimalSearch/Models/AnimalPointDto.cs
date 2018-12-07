using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    //is used to transfer animal and point data in a one object
    public class AnimalPointDto
    {
        public Animal Animal { get; set; }
        public Point Point { get; set; }
    }
}