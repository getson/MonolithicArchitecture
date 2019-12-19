using App.Application.Commands.Administrations;
using App.Application.Queries.Administrations;
using App.WebApi.IntegrationTest.Infrastructure;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace App.WebApi.IntegrationTest
{
    public class ProjectControllerTests : ControllerTestBase
    {
        private const string _apiEndpoint = "api/projects";
        private readonly TestServerFixture _fixture;

        public ProjectControllerTests(TestServerFixture fixture)
            : base(fixture)
        {
            _fixture = fixture;
            Init();
        }
        [Fact]
        public async Task Should_return_projects_with_success()
        {
            var projects = await GetTop100();
            projects.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_return_project_by_id_with_success()
        {
            var projects = await GetTop100();
            var firstProject = projects.FirstOrDefault();

            var response = await GetAsync($"{_apiEndpoint}/{firstProject.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var getProject = response.GetObject<GetProjectResult>();

            response.GetObjectAsync<GetProjectResult>().Should().NotBeNull();
            getProject.Id.Should().Be(firstProject.Id);
            getProject.Name.Should().Be(firstProject.Name);
            getProject.Description.Should().Be(firstProject.Description);
        }

        [Fact]
        public async Task Project_Should_Be_Created()
        {
            var addProject = new AddProject
            {
                Name = "title 1",
                Description = "Description 1"
            };

            var response = await PostAsync(_apiEndpoint, addProject);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var createdProject = response.GetObject<Guid>();

            createdProject.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task Project_Should_Be_Updated()
        {
            var project = (await GetTop100()).First();
            var updateProjectCommand = new UpdateProject
            {
                Id = project.Id,
                Name = "Name updated",
                Description = "Description updated"
            };
            var response = await PutAsync(_apiEndpoint, updateProjectCommand);
            response.EnsureSuccessStatusCode();

            var updatedProject = (await GetTop100()).FirstOrDefault(x => x.Id == project.Id);

            updatedProject.Description.Should().Be(updateProjectCommand.Description);
            updatedProject.Name.Should().Be(updateProjectCommand.Name);
        }

        [Fact]
        public async Task Project_Should_not_Be_Updated()
        {
            var updateProjectCommand = new UpdateProject
            {
                Id = Guid.Empty,
                Name = "Name updated",
                Description = "Description updated"
            };

            var putResponse = await PutAsync(_apiEndpoint, updateProjectCommand);
            putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Project_should_be_deleted()
        {
            var project = new AddProject
            {
                Name = "Name 3",
                Description = "Description 3"
            };

            var response = await PostAsync(_apiEndpoint, project);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var projectId = response.GetObject<Guid>();

            var deleteCommand = new DeleteProject(projectId);
            var deleteResponse = await DeleteAsync($"{_apiEndpoint}/{deleteCommand.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Project_should_not_be_deleted()
        {
            var deleteResponse = await DeleteAsync($"{_apiEndpoint}/{Guid.Empty}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        private async Task<IEnumerable<GetProjectResult>> GetTop100()
        {
            var result = await GetAsync($"{_apiEndpoint}?limit=100");
            result.EnsureSuccessStatusCode();
            return result.GetObject<GetProjectsResult>().Items;
        }
        private void Init()
        {
            var project = new AddProject
            {
                Name = "Name 3",
                Description = "Description 3"
            };

            var response = PostAsync(_apiEndpoint, project).Result;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}