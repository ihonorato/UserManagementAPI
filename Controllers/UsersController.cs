using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(UserRepository repo) : ControllerBase
    {
        private readonly UserRepository _repo = repo;

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _repo.GetAll();

            if (users.Count == 0)
                return Ok(new { message = "No users found", data = users });

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _repo.GetById(id);
            if (user == null) 
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
       
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
                return BadRequest("Name and Email are required");

            _repo.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
                return NotFound(new { message = $"Name and Email are required" });

            var updated = _repo.Update(user);
            if (!updated) 
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            };

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _repo.Delete(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
