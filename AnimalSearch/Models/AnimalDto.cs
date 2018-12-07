using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    /// <summary>
    /// is used to transfer all data into one  object
    /// </summary>
    public class AnimalDto
    {
        public AnimalDto()
        {
        }
        public int Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Kind { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }

    }
}