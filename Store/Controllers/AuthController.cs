using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Store.Models;
using Store.Service.IService;
using Store.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Store.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var responseDto = await _authService.loginAsync(loginRequestDTO);
            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDTO loginRDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(responseDto.Result));
                if (loginRDTO != null && loginRDTO.User != null && !string.IsNullOrEmpty(loginRDTO.User.ID))
                {
                    await _authService.SignInUser(loginRDTO, HttpContext);
                    _tokenProvider.SetToken(loginRDTO.Token);
                    return RedirectToAction("Index", "Home");
                }
                responseDto.Message = "Login failed!";
            }
            TempData["Error"] = responseDto.Message;
            return View(loginRequestDTO);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var RoleList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem(){ Text=SD.RoleCustomer, Value=SD.RoleCustomer}
            };
            ViewBag.RoleList = RoleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var response = await _authService.RegisterAsync(registrationRequestDTO);
            if (response != null && response.IsSuccess)            
            {
                var roleResponse= await _authService.AssignRoleAsync(registrationRequestDTO);
                if (roleResponse != null && roleResponse.IsSuccess)
                {
                    TempData["Success"] = "Registration completed succcesssfully!";
                    return RedirectToAction("Login");
                }
                if (roleResponse != null && !string.IsNullOrEmpty(roleResponse.Message))
                {
                    TempData["Error"] = roleResponse.Message;
                }
            }
            if (response != null && !string.IsNullOrEmpty(response.Message))
            {
                TempData["Error"] = response.Message;
            }
            var RoleList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem(){ Text=SD.RoleCustomer, Value=SD.RoleCustomer}
            };
            ViewBag.RoleList = RoleList;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
             await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }


    }
}
