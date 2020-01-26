using Microsoft.AspNetCore.Mvc;
using System;

namespace BinaryOrigin.SeedWork.WebApi.Controllers
{
    [ApiController]
    public class AppController : ControllerBase
    {
        /// <summary>
        /// return status code 204 (NoContent)
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(204)]
        protected IActionResult Deleted()
        {
            return NoContent();
        }

        /// <summary>
        /// return status code 200 and no content
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(204)]
        protected IActionResult Updated()
        {
            return Ok();
        }

        /// <summary>
        /// return status code 200 and no content
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(201)]
        protected IActionResult Created(Guid resourceId)
        {
            if(Uri.TryCreate($"{Request.Host}{Request.Path}/{resourceId}", UriKind.RelativeOrAbsolute, out var uri))
            {
                return base.Created(uri, resourceId);
            }
            return base.Created("", resourceId);
        }
    }
}