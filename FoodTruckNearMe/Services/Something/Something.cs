using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckNearMe.Services.Something
{
    public class Something : ISomething
    {
        public string GetName()
        {
            return "Hi";
        }
    }
}
