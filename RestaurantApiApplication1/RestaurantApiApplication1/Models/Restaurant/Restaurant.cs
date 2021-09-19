using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApiApplication1.Models.Restaurant
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Description { get; set; }
        public string Hours { get; set; }
        public List<decimal> Ratings { get; set; }
        public string AverageRating {
            get {
                return Ratings != null && Ratings.Any() ? (Ratings.Sum(x => x) / Ratings.Count()).ToString() : "";
            }
        }
    }
}
