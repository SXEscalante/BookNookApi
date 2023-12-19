﻿using FullStackAuth_WebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // DELETE api/<ReviewsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
