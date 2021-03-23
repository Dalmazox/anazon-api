using Anazon.Domain.Entities;
using Anazon.Domain.Models.Create;
using Anazon.Domain.Models.Update;
using Anazon.Presentation.Api;
using Anazon.Tests.Common.Faker;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Anazon.Tests.Presentation.Api.Controllers
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _serializerOptions;

        public UserControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [Fact(DisplayName = "Should return success with users list")]
        public async Task ShouldReturnSuccessWithList()
        {
            var result = await _client.GetFromJsonAsync<CustomResult<IEnumerable<User>>>("/v1/user");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Data.Should().HaveCountGreaterThan(0);
        }

        [Fact(DisplayName = "Should return success on storing user")]
        public async Task ShouldReturnSuccessOnStore()
        {
            var fakeUser = UserFaker.GetCreateModel();

            var httpResponse = await _client.PostAsJsonAsync("/v1/user", fakeUser);
            var json = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CustomResult<object>>(json, _serializerOptions);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Message.Should().Be("Criado com sucesso");
        }

        [Fact(DisplayName = "Should return store validation error on required props empty")]
        public async Task ShouldReturnValidationErrorOnStore()
        {
            var fakeUser = new CreateUserModel();

            var httpResponse = await _client.PostAsJsonAsync("/v1/user", fakeUser);
            var json = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CustomResult<IEnumerable>>(json, _serializerOptions);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Message.Should().Be("Corpo da requisição inválido");
            result.Data.Should().HaveCountGreaterThan(0);
        }

        [Fact(DisplayName = "Should return success on updating user")]
        public async Task ShouldReturnSuccessOnUpdate()
        {
            var id = UserFaker.GetList().First().Id;
            var fakeUser = UserFaker.GetCreateModel();

            var httpResponse = await _client.PutAsJsonAsync($"/v1/user/{id}", fakeUser);
            var json = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CustomResult<object>>(json, _serializerOptions);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Sucesso na requisição");
        }

        [Fact(DisplayName = "Should return update validation error on required props empty")]
        public async Task ShouldReturnValidationErrorOnUpdate()
        {
            var id = UserFaker.GetList().First().Id;
            var fakeUser = new UpdateUserModel();

            var httpResponse = await _client.PutAsJsonAsync($"/v1/user/{id}", fakeUser);
            var json = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CustomResult<IEnumerable>>(json, _serializerOptions);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Message.Should().Be("Corpo da requisição inválido");
            result.Data.Should().HaveCountGreaterThan(0);
        }

        [Fact(DisplayName = "Should return success on inactivating user")]
        public async Task ShouldReturnSuccessOnInactivating()
        {
            var id = UserFaker.GetList().First().Id;

            var httpResponse = await _client.PostAsync($"/v1/user/{id}/inactivate", null);
            var json = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CustomResult<object>>(json, _serializerOptions);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Sucesso na requisição");
        }
    }
}
