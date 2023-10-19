using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cars
        [HttpGet]
        public IActionResult GetAllCars()
        {
            try
            {
                //Includes entire Owner object--insecure!
                //var cars = _context.Cars.Include(c => c.Owner).ToList();

                //Retrieve all cars from the database, using Dtos
                var cars = _context.Cars.Select(c => new CarWithUserDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    Owner = new UserForDisplayDto
                    {
                        Id = c.Owner.Id,
                        FirstName = c.Owner.FirstName,
                        LastName = c.Owner.LastName,
                        UserName = c.Owner.UserName,
                    }
                }).ToList();

                // Return the list of cars as a 200 OK response
                return StatusCode(200, cars);
            }
            catch (Exception ex)
            {
                // If an error occurs, return a 500 internal server error with the error message
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/cars/myCars
        [HttpGet("myCars"), Authorize]
        public IActionResult GetUsersCars()
        {
            try
            {
                // Retrieve the authenticated user's ID from the JWT token
                string userId = User.FindFirstValue("id");

                // Retrieve all cars that belong to the authenticated user, including the owner object
                var cars = _context.Cars.Where(c => c.OwnerId.Equals(userId));

                // Return the list of cars as a 200 OK response
                return StatusCode(200, cars);
            }
            catch (Exception ex)
            {
                // If an error occurs, return a 500 internal server error with the error message
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/cars/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                // Retrieve the car with the specified ID, including the owner object
                var car = _context.Cars.Include(c => c.Owner).FirstOrDefault(c => c.Id == id);

                // If the car does not exist, return a 404 not found response
                if (car == null)
                {
                    return NotFound();
                }

                // Return the car as a 200 OK response
                return StatusCode(200, car);
            }
            catch (Exception ex)
            {
                // If an error occurs, return a 500 internal server error with the error message
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/cars
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Car data)
        {
            try
            {
                // Retrieve the authenticated user's ID from the JWT token
                string userId = User.FindFirstValue("id");

                // If the user ID is null or empty, the user is not authenticated, so return a 401 unauthorized response
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Set the car's owner ID  the authenticated user's ID we found earlier
                data.OwnerId = userId;

                // Add the car to the database and save changes
                _context.Cars.Add(data);
                if (!ModelState.IsValid)
                {
                    // If the car model state is invalid, return a 400 bad request response with the model state errors
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();

                // Return the newly created car as a 201 created response
                return StatusCode(201, data);
            }
            catch (Exception ex)
            {
                // If an error occurs, return a 500 internal server error with the error message
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/cars/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] Car data)
        {
            try
            {
                // Find the car to be updated
                Car car = _context.Cars.Include(c => c.Owner).FirstOrDefault(c => c.Id == id);

                if (car == null)
                {
                    // Return a 404 Not Found error if the car with the specified ID does not exist
                    return NotFound();
                }

                // Check if the authenticated user is the owner of the car
                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || car.OwnerId != userId)
                {
                    // Return a 401 Unauthorized error if the authenticated user is not the owner of the car
                    return Unauthorized();
                }

                // Update the car properties
                car.OwnerId = userId;
                car.Owner = _context.Users.Find(userId);
                car.Make = data.Make;
                car.Model = data.Model;
                car.Year = data.Year;
                if (!ModelState.IsValid)
                {
                    // Return a 400 Bad Request error if the request data is invalid
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();

                // Return a 201 Created status code and the updated car object
                return StatusCode(201, car);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the error message if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/cars/5
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                // Find the car to be deleted
                Car car = _context.Cars.FirstOrDefault(c => c.Id == id);
                if (car == null)
                {
                    // Return a 404 Not Found error if the car with the specified ID does not exist
                    return NotFound();
                }

                // Check if the authenticated user is the owner of the car
                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || car.OwnerId != userId)
                {
                    // Return a 401 Unauthorized error if the authenticated user is not the owner of the car
                    return Unauthorized();
                }

                // Remove the car from the database
                _context.Cars.Remove(car);
                _context.SaveChanges();

                // Return a 204 No Content status code
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the error message if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }
    }
}
