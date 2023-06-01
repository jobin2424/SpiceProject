using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceProject.Extensions
{
    public static class ReflectionExtension
    {
     public static string GetPropertyValue<T>(this T item, string propertyName) // this method is used in the ienumeral class 
        {
            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();    // this gets the value of whatever property we pass. used in the ienumeral class, we get the  Name, Id, and the value for the selected value. The selected value is whatever we select in our dropdown on the create page in the create view

        }
            
            }
}
