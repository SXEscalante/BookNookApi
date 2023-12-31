﻿using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<ReviewsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ReviewsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReviewsController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Models.Review review)
        {
            try
            {
                string userId = User.FindFirstValue("id");

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                review.UserId = userId;

                _context.Reviews.Add(review);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();
                return StatusCode(201, review);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ReviewsController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] Review data)
        {
            try
            {
                Review review = _context.Reviews.Include(r => r.User).FirstOrDefault(r => r.Id == id);

                if (review == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || review.UserId != userId)
                {
                    return Unauthorized();
                }

                // Update the car properties
                review.Text = data.Text;
                review.Rating = data.Rating;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();

                return StatusCode(201, review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ReviewsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Review review = _context.Reviews.FirstOrDefault(r => r.Id == id);
                if (review == null) { return NotFound(); }
                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || review.UserId != userId)
                {
                    return Unauthorized();
                }
                _context.Reviews.Remove(review);
                _context.SaveChanges();
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
