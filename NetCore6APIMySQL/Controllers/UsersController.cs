using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore6APIMySQL.Data.Repositories;
using NetCore6APIMySQL.Model;

namespace NetCore6APIMySQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userRepository.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userRepository.GetUserById(id));
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _userRepository.InsertUser(user);

            return Created("Created", created);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userRepository.UpdateUser(user);

            return NoContent();

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUser(new User { Id = id });

            return NoContent();
        }
    }
}
