using AuthAPI.Data;
using AuthAPI.Models;
using AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(ApplicationDBContext dbContext,UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRoleAsync(string email, string roleName)
        {
            var User = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (User != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //Create the role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(User, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            var User = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
            if (User is not null)
            {
                bool IsValid = await _userManager.CheckPasswordAsync(User, loginRequestDTO.Password);
                if (IsValid)
                {
                    var _UserDto = new UserDTO() { ID = User.Id, Email = User.Email, PhoneNumber = User.PhoneNumber, Name = User.UserName };
                    var Roles = await _userManager.GetRolesAsync(User);
                    var token = _jwtTokenGenerator.GenerateToken(_UserDto, Roles);
                    return new LoginResponseDTO() {User = _UserDto, Token = token };
                }
            }
            return new LoginResponseDTO() { User = null, Token = string.Empty };
        }

        public async Task<string> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = registrationRequestDTO.Name,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                NormalizedUserName = registrationRequestDTO.Name.ToUpper(),
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };
            try
            {
                var existUser = _dbContext.Users.Where(x=>x.Email.ToLower() == registrationRequestDTO.Email.ToLower()).FirstOrDefault();
                if (existUser != null && !string.IsNullOrEmpty(existUser.Id))
                {
                    return "User already exists with the same Email address!";
                }
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    return string.Empty;
                }
                return result.Errors.FirstOrDefault().Description;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
