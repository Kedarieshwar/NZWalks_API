using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName
            };
            var identitytResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            if(identitytResult.Succeeded)
            {
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);
                    if (!identitytResult.Succeeded)
                    {
                        return BadRequest(identitytResult.Errors);
                    }
                }
            }
            return Ok("User registered successfully.");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginRequestDTO.UserName);
            if (identityUser == null)
            {
                return BadRequest("Invalid username or password.");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(identityUser, loginRequestDTO.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Invalid username or password.");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            if (roles != null)
            {
                var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                var response = new LoginResponseDTO
                {
                    JwtToken= jwtToken
                };
                return Ok(response);
            }
            return BadRequest("User has no roles assigned.");

        }
    }

     
}
