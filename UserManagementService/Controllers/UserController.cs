using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBl _userBl;

        public UserController(IUserBl userBl)
        {
            _userBl = userBl;
        }

        [HttpPost("SignUp/Doctor")]
      // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] UserEntity userDto)
        {
            try
            {
                userDto.UserRole = "doctor";
                await _userBl.Register(userDto);
                return Ok("Doctor registered successfully");
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while registering the doctor: {ex}");
                return StatusCode(500, "An error occurred while registering the doctor. Please try again later or contact support.");
            }
        }

        [HttpPost("SignUp/Admin")]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserEntity userDto)
        {
            try
            {
                userDto.UserRole = "admin";
                await _userBl.Register(userDto);
                return Ok("Admin registered successfully");
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while registering the admin: {ex}");
                return StatusCode(500, "An error occurred while registering the admin. Please try again later or contact support.");
            }
        }

        [HttpPost("SignUp/Patient")]
       // [Authorize(Roles = "Patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] UserEntity userDto)
        {
            try
            {
                userDto.UserRole = "patient";
                await _userBl.Register(userDto);
                return Ok("Patient registered successfully");
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while registering the patient: {ex}");
                return StatusCode(500, "An error occurred while registering the patient. Please try again later or contact support.");
            }
        }

        [HttpGet("AllUsers")]
        // [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userBl.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving all users: {ex}");
                return StatusCode(500, "An error occurred while retrieving all users. Please try again later or contact support.");
            }
        }

        //login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginEntity userLoginEntity)
        {
            try
            {
                var users = await _userBl.Login(userLoginEntity);
                if (users.Any())
                {
                    return Ok("Login successful");
                }
                else
                {
                    return BadRequest("Invalid email or password. Please check your credentials and try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging in: {ex}");
                return StatusCode(500, "An error occurred while logging in. Please try again later or contact support.");
            }
        }

        //getbyid
        [HttpGet("GetUserByUserId")]
        // [Authorize]
        public async Task<IActionResult> GetUserByUserId(int userid)
        {
            try
            {
                var user = await _userBl.GetUserByUserId(userid);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user with ID {userid}. Please try again later or contact support.");
            }
        }

        [HttpGet("AllUserDoctors")]
        //[Authorize]
        public async Task<IActionResult> GetAllUserDoctors()
        {
            try
            {
                var doctors = await _userBl.GetAllUserDoctor();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving all doctors. Please try again later or contact support.");
            }
        }


    }
}
