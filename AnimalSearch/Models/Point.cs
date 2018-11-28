using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    public class Point  //class for a point which we will search by polygon
    {
        //is used to create point which we are looking for in polygon search 
        public Animal Animal { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public Point()
        {
        }

        public void test() { }
        public Point(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
    }
}