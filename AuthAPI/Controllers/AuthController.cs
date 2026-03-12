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
        private readonly ResponseDto _ResponseDto;
        public AuthController(IAuthService iAuthService)
        {
            _IAuthService = iAuthService;
            _ResponseDto = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            string Message = await _IAuthService.RegisterAsync(registrationRequestDTO);
            if (!string.IsNullOrEmpty(Message))
            {
                _ResponseDto.IsSuccess = false;
                _ResponseDto.Message = Message;
                return BadRequest(_ResponseDto);
            }
            _ResponseDto.IsSuccess = true;
            return Ok(_ResponseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
