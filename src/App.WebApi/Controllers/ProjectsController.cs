using App.Application.Commands.ProjectBC;
using App.Application.Queries.ProjectBC;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.WebApi.Controllers
{
    /// <summary>
    /// This controller contains methods for the admin
    /// </summary>
    [Route("api/[controller]")]
    public class ProjectsController : AppController
    {
        private readonly IBus _bus;

        /// <summary>
        /// the controller cunstructor
        /// </summary>
        /// <param name="bus"></param>
        public ProjectsController(IBus bus)
        {
            _bus = bus;
        }

        /// <summary>
        /// Get All projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]GetProjects queryModel)
        {
            var result = await _bus.QueryAsync(queryModel);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// get project by Id
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]GetProject queryModel)
        {
            var result = await _bus.QueryAsync(queryModel);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Add a project
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(AddProject command)
        {
            var result = await _bus.ExecuteAsync(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProject command)
        {
            var result = await _bus.ExecuteAsync(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Updated();
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteProject command)
        {
            var result = await _bus.ExecuteAsync(command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Deleted();
        }
    }
}