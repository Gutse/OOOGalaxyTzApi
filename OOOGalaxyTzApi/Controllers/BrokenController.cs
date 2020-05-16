using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OOOGalaxyTzApi.Interfaces;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Контроллер аутентификации
    /// </summary>
    public class BrokenController : BaseController
    {
        private readonly IRepository<UserModel> _userRepository;

        /// <inheritdoc />
        public BrokenController(IMapper mapper, IRepository<UserModel> userRepository) : base(mapper)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 3. endpoint который ломается во время работы и возвращает unhandled exception
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [Route(nameof(Unhandled))]
        public async Task<IActionResult> Unhandled()
        {
            throw new Exception("unhandled");
        }

        /// <summary>
        /// endpoint который ломается во время работы и возвращает логическую ошибку (например запрашиваемый пользователь не активирован)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [Route(nameof(Handled))]
        public async Task<IActionResult> Handled()
        {
            return BadRequest("запрашиваемый пользователь не активирован");
        }
    }
}