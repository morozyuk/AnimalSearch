using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalSearch.Models
{
    /// <summary>
    /// is used for point which we are looking for by polygon
    /// </summary>
    public class Point
    {

        public decimal X { get; set; }
        public decimal Y { get; set; }

        public Point() {}

        public Point(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
    }
}