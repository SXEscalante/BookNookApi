﻿using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<FavoritesController>
        [HttpGet("myFavorites"), Authorize]
        public IActionResult GetUserReviews()
        {
            try
            { 
                string userId = User.FindFirstValue("id");
                var favorites = _context.Favorites.Where((r) => r.UserId.Equals(userId));
                return StatusCode(200, favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<FavoritesController>/bookId
        [HttpPost("{bookId}"), Authorize]
        public IActionResult Post(string bookId, [FromBody] Models.Favorite favorite)
        {
            string userId = User.FindFirstValue("id");
            var favorited = _context.Favorites.Where(f => f.BookId == bookId && f.UserId == userId).FirstOrDefault();
            if (favorited != null)
            {
                return StatusCode(201, favorite);
            }
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                favorite.UserId = userId;

                _context.Favorites.Add(favorite);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();
                return StatusCode(201, favorite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<FavoritesController>/5
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                Favorite favorite = _context.Favorites.FirstOrDefault(f => f.Id == id);
                if(favorite == null) { return NotFound(); }
                var userId = User.FindFirstValue("id");
                if(string.IsNullOrEmpty(userId) || favorite.UserId != userId)
                {
                    return Unauthorized();
                }
                _context.Favorites.Remove(favorite);
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
