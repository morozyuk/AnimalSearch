using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimalSearch.Helper
{
    public static class Base64
    {
        public static string ToBase64(this HtmlHelper html, byte[] img)
        {
            return Convert.ToBase64String(img);
        }
    }
}