using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using OOOGalaxyTzApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NHibernate.Linq;
using OOOGalaxyTzApi.Interfaces;
using OOOGalaxyTzApi.Models;
using System.Security.Claims;
using OOOGalaxyTzApi.DTO;

namespace OOOGalaxyTzApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Контроллер аутентификации
    /// </summary>
    public class AuthenticationController : BaseController
    {
        private readonly IRepository<UserModel> _userRepository;
        private readonly IPasswordHelper _passwordHelper;

        /// <inheritdoc />
        public AuthenticationController(IMapper mapper, IRepository<UserModel> userRepository, IPasswordHelper passwordHelper) : base(mapper)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <response code="200">Результат залогинивания</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login([Required] [FromHeader(Name = Constants.HeaderUserLogin)] string login,
                                               [Required] [FromHeader(Name = Constants.HeaderUserPassword)] string password)
        {
            var user = await _userRepository.Query()
                                                    .FirstOrDefaultAsync(e => e.Login == login);
            var auth = _passwordHelper.VerifyHashedPassword(user, password);
            if (!auth) return BadRequest("Неправильный логин/пароль");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = AuthOptions.GetSymmetricSecurityKey();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = AuthOptions.Lifetime,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Issuer = AuthOptions.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedJwt = tokenHandler.WriteToken(token);

            return Ok(encodedJwt);

        }

        /// <summary>
        /// создание пользователя admin/admin
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [Route(nameof(PopulateTestLogins))]
        public async Task<IActionResult> PopulateTestLogins()
        {
            var admin = await _userRepository.Query()
                .FirstOrDefaultAsync(e => e.Login == "admin");
            if (admin == null)
            {
                var newPasswordData = _passwordHelper.GetNewPassword("admin");

                admin = new UserModel
                {
                    Password = newPasswordData.hash,
                    Amount = 123,
                    BirthDate = new DateTime(1982, 5, 22),
                    Salt = newPasswordData.salt,
                    Login = "admin"
                };
                admin = await _userRepository.SaveAsync(admin);
            }

            return Ok(true);
        }

        /// <summary>
        /// просмотр информации о текущем пользователе
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), 200)]
        [Route(nameof(GetCurrentUser))]
        public async Task<IActionResult> GetCurrentUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = Convert.ToInt32(claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                return Ok<UserDto>(user);    
            }

            return BadRequest("Пользователь не найден");
        }
    }
}