﻿using App.Application.Commands.ProjectBC;
using App.Application.Events;
using App.Application.Queries.ProjectBC;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.WebApi.Controllers
{

    /// [FromRoute] and [FromQuery] is added in order to generate swagger documentaion correctly until it will be fixed 
    /// in swagger configuration


    /// <summary>
    /// This controller contains methods for the projects
    /// </summary>
    [Route("api/projects")]
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
            return Ok(result);
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

            return Ok(result);
        }

        /// <summary>
        /// Add a project
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Scopes.CreateProject)]
        public async Task<IActionResult> Post(AddProject command)
        {
            var result = await _bus.ExecuteAsync(command);
            await _bus.PublishAsync(new ProjectCreated
            {
                Id = result
            });
            return Created(result);
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Scopes.UpdateProject)]
        public async Task<IActionResult> Put(UpdateProject command)
        {
            _ = await _bus.ExecuteAsync(command);
            return Updated();
        }
        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Scopes.DeleteProject)]
        public async Task<IActionResult> Delete([FromRoute]DeleteProject command)
        {
            _ = await _bus.ExecuteAsync(command);
            return Deleted();
        }
    }
}