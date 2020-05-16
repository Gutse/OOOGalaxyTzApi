using System.Collections;
using System.Net;
using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OOOGalaxyTzApi.Extensions;
using OOOGalaxyTzApi.Models;

namespace OOOGalaxyTzApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Базовый контроллер
    /// </summary>
    [ProducesResponseType(500)]
    [ProducesResponseType(401)]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Инстанс автомаппера
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="mapper"></param>
        public BaseController(IMapper mapper)
        {
            Mapper = mapper;
        }

        /// <summary>
        /// Конструктор стандартного ответа апи
        /// </summary>
        /// <param name="sourceObject"></param>
        protected OkObjectResult Ok<T>(T sourceObject)
        {
            return Ok<T>(sourceObject, default);
        }

        /// <summary>
        /// Конструктор стандартного ответа апи
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="dataCount"></param>
        protected OkObjectResult Ok<T>(object sourceObject, int dataCount = default)
        {
            if (sourceObject is null)
                return base.Ok(null);

            var data = sourceObject is T obj ? obj : sourceObject.MapTo<T>(Mapper);

            var apiResponse = new ApiResponse<T>
            {
                Code = HttpStatusCode.OK,
                Data = data,
                DataCount = dataCount is 0 ? sourceObject is IList list ? list.Count : default : dataCount
            };

            return base.Ok(apiResponse);
        }

        /// <summary>
        /// Отправка ошибки
        /// </summary>
        /// <param name="error"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected BadRequestObjectResult BadRequest<T>(T error, HttpStatusCode code)
        {
            var apiResponse = new ApiResponse<T>
            {
                Code = code,
                Message = error is string str ? str : JsonConvert.SerializeObject(error)
            };
            return base.BadRequest(apiResponse);
        }

        /// <inheritdoc />
        public override BadRequestObjectResult BadRequest(object error)
        {
            return BadRequest(error, HttpStatusCode.BadRequest);
        }
    }
}