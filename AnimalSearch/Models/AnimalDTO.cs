using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    //is used to transfer all data into one  object
    public class AnimalDTO
    {
        public AnimalDTO()
        {
        }
        public int id { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        public string breed { get; set; }
        public string kind { get; set; }
        public string description { get; set; }
        public string base64Img { get; set; }
        public string imageSrc { get; set; }

    }
}