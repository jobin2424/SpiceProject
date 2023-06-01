using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Extensions
{
    public static class IEnumerableExtension
    {                                                                                   // we passed another variable called selectedValuue because that is the default selected item list
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)    //basically the return type, we are getting the object of all the categories and returning a list called SelectListItem back, SelectListItem is part of ASPNetCore.Mvs Rendering package. This is called ToSelectListItem, gave it a type <T>, then second rule of extension methods that the FIRST argument should be of the extended classed preceded by the "this" keyword. We called that items
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),        // get property comes from the refelection Extension class, now in our subcategory controller, for the create view page we have proper dropdown funcinality
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())

                   };
        }
    }
}