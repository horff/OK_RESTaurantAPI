using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantApiApplication1.Models.Restaurant;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RestaurantApiApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IConfiguration _config;

        public RestaurantController(IConfiguration config)
        {
            _config = config;
        }

        // GET api/restaurant
        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> Get()
        {
            var restaurants = new List<Restaurant>();

            using (var connection = new SqlConnection(_config.GetConnectionString("RestaurantDatabase")))
            {
                var sql = $"SELECT restaurants.*, (SELECT STRING_AGG(RatingValue, ', ') as Ratings FROM OK_RestaurantRatings WHERE RestaurantId = restaurants.Id GROUP BY RestaurantId) as Ratings FROM OK_Restaurants as restaurants";
                connection.Open();
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        restaurants.Add(new Restaurant
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            AddressLine1 = reader["AddressLine1"].ToString(),
                            AddressLine2 = reader["AddressLine2"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            Zip = reader["Zip"].ToString(),
                            Description = reader["Description"].ToString(),
                            Hours = reader["Hours"].ToString(),
                            Ratings = reader["Ratings"] != System.DBNull.Value && reader["Ratings"].ToString() != null ? reader["Ratings"].ToString().Split(',').Select(Decimal.Parse).ToList() : new List<decimal>()
                        });
                    }
                }

            }

            return restaurants;
        }

        // GET api/restaurant/5
        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get(int id)
        {
            var restaurant = new Restaurant();

            using (var connection = new SqlConnection(_config.GetConnectionString("RestaurantDatabase")))
            {
                var sql = $"SELECT restaurants.*, (SELECT STRING_AGG(RatingValue, ', ') as Ratings FROM OK_RestaurantRatings WHERE RestaurantId = restaurants.Id GROUP BY RestaurantId) as Ratings FROM OK_Restaurants as restaurants WHERE Id = {id}";
                connection.Open();
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        restaurant = new Restaurant
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            AddressLine1 = reader["AddressLine1"].ToString(),
                            AddressLine2 = reader["AddressLine2"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            Zip = reader["Zip"].ToString(),
                            Description = reader["Description"].ToString(),
                            Hours = reader["Hours"].ToString(),
                            Ratings = reader["Ratings"].ToString() != null ? reader["Ratings"].ToString().Split(',').Select(Decimal.Parse).ToList() : new List<decimal>()
                        };
                    }
                }

            }

            return restaurant;
        }

        // POST api/restaurant
        [HttpPost]
        public void Post([FromBody] Restaurant restaurant)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("RestaurantDatabase")))
            {
                var sql = $"INSERT INTO OK_Restaurants(Name, AddressLine1, AddressLine2, City, State, Zip, Description, Hours) VALUES(@name, @addressLine1, @addressLine2, @city, @state, @zip, @description, @hours)";
                connection.Open();
                using (var command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = restaurant.Name;
                    command.Parameters.Add("@addressLine1", System.Data.SqlDbType.VarChar).Value = restaurant.AddressLine1;
                    command.Parameters.Add("@addressLine2", System.Data.SqlDbType.VarChar).Value = restaurant.AddressLine2;
                    command.Parameters.Add("@city", System.Data.SqlDbType.VarChar).Value = restaurant.City;
                    command.Parameters.Add("@state", System.Data.SqlDbType.VarChar).Value = restaurant.State;
                    command.Parameters.Add("@zip", System.Data.SqlDbType.VarChar).Value = restaurant.Zip;
                    command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = restaurant.Description;
                    command.Parameters.Add("@hours", System.Data.SqlDbType.VarChar).Value = restaurant.Hours;
                    command.ExecuteNonQuery();
                }
            }
        }

        // PUT api/restaurant/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Restaurant restaurant)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("RestaurantDatabase")))
            {
                var sql = $"UPDATE OK_Restaurants SET Name=@name, AddressLine1=@addressLine1, AddressLine2=@addressLine2, City=@city, State=@state, Zip=@zip, Description=@description, Hours=@hours WHERE Id=@id";
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", System.Data.SqlDbType.VarChar).Value = id;
                    command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = restaurant.Name;
                    command.Parameters.Add("@addressLine1", System.Data.SqlDbType.VarChar).Value = restaurant.AddressLine1;
                    command.Parameters.Add("@addressLine2", System.Data.SqlDbType.VarChar).Value = restaurant.AddressLine2;
                    command.Parameters.Add("@city", System.Data.SqlDbType.VarChar).Value = restaurant.City;
                    command.Parameters.Add("@state", System.Data.SqlDbType.VarChar).Value = restaurant.State;
                    command.Parameters.Add("@zip", System.Data.SqlDbType.VarChar).Value = restaurant.Zip;
                    command.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = restaurant.Description;
                    command.Parameters.Add("@hours", System.Data.SqlDbType.VarChar).Value = restaurant.Hours;
                    command.ExecuteNonQuery();
                }
            }
        }

        // DELETE api/restaurant/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("RestaurantDatabase")))
            {
                var sql = $"DELETE FROM OK_RestaurantRatings WHERE RestaurantId=@id; DELETE FROM OK_Restaurants WHERE Id=@id;";
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", System.Data.SqlDbType.VarChar).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        // POST api/restaurant/rating
        [HttpPost("rating")]
        public void PostRating([FromBody] Rating rating)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("RestaurantDatabase")))
            {
                var sql = $"INSERT INTO OK_RestaurantRatings(RestaurantId, RatingValue) VALUES(@restaurantId, @ratingValue)";
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@restaurantId", System.Data.SqlDbType.VarChar).Value = rating.RestaurantId;
                    command.Parameters.Add("@ratingValue", System.Data.SqlDbType.VarChar).Value = rating.RatingValue;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
