using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    public class Animal //class for entity framework to set fileds in databse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Kind { get; set; }
        public string Breed { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public string Path { get; set; }
        public string Latitude { get; set; } //lat and lng added for polygon search
        public string Longtitude { get; set; }
    }
}