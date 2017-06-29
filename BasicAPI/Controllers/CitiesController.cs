using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BasicAPI.Controllers
{
    public class CitiesController : Controller
    {
        [HttpGet("api/cities")]
        public JsonResult GetCities()
        {
            return new JsonResult(new List<object>()
            {
                new{id = 1, Name = "New Yourk City"},
                new{id = 2, Name = "Antwerp"}
            });
        }
    }
}
