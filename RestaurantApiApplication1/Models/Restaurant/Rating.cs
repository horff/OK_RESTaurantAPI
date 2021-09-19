using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApiApplication1.Models.Restaurant
{
    public class Rating
    {
        public int RestaurantId { get; set; }
        public decimal RatingValue { get; set; }
    }
}
