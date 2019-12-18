//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;
//using FluentAssertions;
//using System.Net;
//using App.WebApi.IntegrationTest.Infrastructure;
//using System;
//using App.Application.Commands.Administrations;
//using App.Application.Queries.Administrations;

//namespace App.WebApi.IntegrationTest
//{
//    public class AdministrationControllerTests : ControllerTestBase
//    {
//        private readonly TestServerFixture _fixture;

//        public AdministrationControllerTests(TestServerFixture fixture)
//            : base(fixture)
//        {
//            _fixture = fixture;
//        }

//        #region Test for Projects and sub projects

//        [Fact]
//        public async Task Should_return_projects_with_success()
//        {
//            var response = await GetAsync("api/Administration/GetAllProjects");

//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            response.GetObjectAsync<GetProjectsResult>().Should().NotBeNull();
//        }

//        [Fact]
//        public async Task Should_return_project_by_id_with_success()
//        {
//            var project = GetProject().Result;

//            var response = await GetAsync("api/Administration/GetProject?Id=" + project.Id);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var getProject = response.GetObject<GetProjectResult>();
//            response.GetObjectAsync<GetProjectResult>().Should().NotBeNull();
//            getProject.Id.Should().NotBe(Guid.Empty);
//        }

//        [Fact]
//        public async Task Project_Should_Be_Created()
//        {
//            var addProject = new AddProject("title 1", "Description 1");

//            var response = await PostAsync("api/Administration/AddProject", addProject);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var createdProject = response.GetObject<AddProjectResult>();

//            createdProject.Id.Should().NotBe(Guid.Empty);
//        }

//        [Fact]
//        public async Task Project_Should_Be_Updated()
//        {
//            var response = await GetAsync("api/Administration/GetAllProjects");

//            var firstProject = response.GetCollection<GetProjectsResult>().FirstOrDefault();
//            firstProject.Name = "Title updated";
//            firstProject.Description = "Description updated";

//            var putResponse = await PutAsync($"api/Administration/UpdateProject", firstProject);

//            response = await GetAsync("api/Administration/GetAllProjects");

//            var projects = response.GetCollection<GetProjectsResult>();

//            var updatedProject = projects.FirstOrDefault(ct => ct.Id == firstProject.Id);

//            updatedProject.Description.Should().Be(firstProject.Description);
//            updatedProject.Name.Should().Be(firstProject.Name);
//        }

//        [Fact]
//        public async Task Project_Should_not_Be_Updated()
//        {
//            var response = await GetAsync("api/Administration/GetAllProjects");

//            var firstProject = response.GetCollection<GetProjectsResult>().FirstOrDefault();
//            firstProject.Name = "Title updated";
//            firstProject.Description = "Description updated";
//            firstProject.Id = Guid.Empty;
//            var putResponse = await PutAsync($"api/Administration/UpdateProject", firstProject);

//            putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task Project_should_be_deleted()
//        {
//            var project = new AddProject("Title 3", "Description 3");

//            var response = await PostAsync("api/Administration/AddProject", project);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var projectDetail = response.GetObject<AddProjectResult>();
//            var deleteCommand = new DeleteProject(projectDetail.Id);
//            var deleteResponse = await DeleteAsync("api/Administration/DeleteProject?Id=" + deleteCommand.Id.ToString());
//            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }

//        [Fact]
//        public async Task Project_should_not_be_deleted()
//        {
//            var deleteResponse = await DeleteAsync("api/Administration/DeleteProject", new DeleteProject(Guid.Empty));
//            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }






    

      


//        private async Task<GetProjectsResult> GetProject()
//        {
//            var response = await GetAsync("api/Administration/GetAllProjects");
//            return response.GetCollection<GetProjectsResult>().FirstOrDefault();
//        }

//    }
//}