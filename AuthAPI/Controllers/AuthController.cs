using AuthAPI.Data;
using AuthAPI.Models;
using AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _IAuthService;
        private readonly ResponseDTO _ResponseDTO;
        public AuthController(IAuthService iAuthService, ApplicationDBContext dbContext)
        {
            _IAuthService = iAuthService;
            _ResponseDTO = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            string Message = await _IAuthService.RegisterAsync(registrationRequestDTO);
            if (!string.IsNullOrEmpty(Message))
            {
                _ResponseDTO.IsSuccess = false;
                _ResponseDTO.Message = Message;
                return BadRequest(_ResponseDTO);
            }
            _ResponseDTO.IsSuccess = true;
            return Ok(_ResponseDTO);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginReqDto)
        {
            var Response = await _IAuthService.LoginAsync(loginReqDto);
            if (Response.User is not null)
            {
                _ResponseDTO.IsSuccess = true;
                _ResponseDTO.Result = Response;
                _ResponseDTO.Message = "Login successful!";
                return Ok(_ResponseDTO);
            }
            _ResponseDTO.IsSuccess = false;
            _ResponseDTO.Message = "User name or password is incorrect!";
             return BadRequest(_ResponseDTO);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            var Response = await _IAuthService.AssignRoleAsync(registrationRequestDTO.Email, registrationRequestDTO.RoleName.ToUpper());
            if (Response)
            {
                _ResponseDTO.IsSuccess = true;
                _ResponseDTO.Result = Response;
                _ResponseDTO.Message = "Role assigned successful!";
                return Ok(_ResponseDTO);
            }
            _ResponseDTO.IsSuccess = false;
            _ResponseDTO.Message = "Error while trying to assign the role!";
            return BadRequest(_ResponseDTO);
        }
    }
}
