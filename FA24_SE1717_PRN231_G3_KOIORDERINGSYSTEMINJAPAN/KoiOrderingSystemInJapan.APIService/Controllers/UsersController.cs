using KoiOrderingSystemInJapan.Data.Models;
using KoiOrderingSystemInJapan.Data.Request.Auths;
using KoiOrderingSystemInJapan.Service;
using KoiOrderingSystemInJapan.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IConfiguration configuration)
        {
            _userService ??= new UserService(configuration);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IBusinessResult> GetUsers()
        {
            return await _userService.GetAll();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetUser(Guid id)
        {
            var user = await _userService.GetById(id);

            return user;
        }

        [HttpGet("get-by-role/{roleNum}")]
        public async Task<IBusinessResult> GetUser(int roleNum)
        {
            var user = await _userService.GetByRole(roleNum);

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutUser(User user)
        {
            return await _userService.Save(user);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostUser(User user)
        {
            return await _userService.Save(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteUser(Guid id)
        {
            return await _userService.DeleteById(id);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(User request)
        {
            var msg = await _userService.AddUser(request);
            return Ok(msg);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            var msg = await _userService.Login(request.UsernameOrEmail, request.Password);

            return Ok(msg);
        }

        [HttpPost("decode-token")]

        public IActionResult DecodeToken([FromBody] TokenRequest request)
        {
            try
            {
                var br = _userService.DecodeToken(request.Token);

                return Ok(br);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
