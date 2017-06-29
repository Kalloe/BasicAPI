using System;

namespace BasicAPI.Models
{
    public class CityDto
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointsOfInterest { get; set; }
    }
}
