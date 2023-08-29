using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kota_Palace_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<AppUsers> manager;
        private SignInManager<AppUsers> signInManager;
        private readonly AppDBContext _context;


        public AccountController(UserManager<AppUsers> manager, SignInManager<AppUsers> signInManager, AppDBContext context)
        {
            this.manager = manager;
            this.signInManager = signInManager;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUsers>> Login(UserLogin login)
        {
            var results = await signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (results.Succeeded)
            {
                var user = await manager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            else
            {
                return NotFound("Incorrect usrname or password");
            }
        }

        //update order status to in-progress
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutUpdateUser(string id, AppUsers users)
        {
            var u = await _context.AppUsers.FindAsync(id);

            if (u == null)
            {
                return NotFound("User not found!");
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
            }

            return Ok($"Updated successfully!!!");
        }

        [HttpPost("signup")]
        public async Task<ActionResult> SignUp(UserSignUp signUp)
        {
            AppUsers users = new AppUsers()
            {
                Email = signUp.Email,
                Firstname = signUp.Firstname,
                Lastname = signUp.Lastname,
                PhoneNumber = signUp.PhoneNumber,
                UserType = "CUSTOMER",
                UserName = signUp.Email
            };

            var results = await manager.CreateAsync(users, signUp.Password);

            if (results.Succeeded)
            {
                return Ok(users.Id);
            }
            else
            {
                string error = "";

                foreach (var res in results.Errors)
                {
                    error = $"{error} {res.Description}\n";
                }

                return BadRequest(error);
            }
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(UserLogin passwordReset)
        {
            var results = await manager.FindByEmailAsync(passwordReset.Email);

            if (results != null)
            {
                string token = await manager.GeneratePasswordResetTokenAsync(results);

                if (token != null)
                {
                    var token_results = await manager.ResetPasswordAsync(results, token, passwordReset.Password);

                    if (token_results.Succeeded)
                    {
                        return Ok("Password reset was succesful");
                    }
                    else
                    {
                        string error = "";

                        foreach (var err in token_results.Errors)
                        {
                            error = $"{error}{err.Description}\n";
                        }

                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(string id)
        {
            if (id == null)
            {
                return BadRequest("Id is required");

            }
            else
            {
                var user = await manager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound($"User associted with Id: {id} does not exists");
                }
                else
                {
                    return Ok(user);
                }
            }
        }

        [HttpGet("drivers")]
        public ActionResult<IEnumerable<AppUsers>> GetDriver()
        {
            var drivers = _context.AppUsers.Where(x => x.UserType == "Customer");

            if (drivers == null)
            {
                return NotFound("There are no drivers yet");
            }
            else
            {
                return Ok(drivers);
            }
        }

        //[HttpDelete("delete/{id}")]

        private bool UserExists(string id)
        {
            return _context.AppUsers.Any(e => e.Id == id);
        }

    }

}

public class UserLogin
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UpdateUser
{
    public string Id { get; set; }
    public string Url { get; set; }
}
[NotMapped]
public class UserSignUp
{
    [Key]
    public string Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string PhoneNumber { get; set; }
    public string UserType { get; set; }
    public string Url { get; set; }
}
