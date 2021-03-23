using Anazon.Application.Helpers;
using Anazon.Domain.Interfaces.Services;
using Anazon.Domain.Models.Create;
using Anazon.Domain.Models.Update;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Anazon.Presentation.Api.Controllers
{
    [Route("v1/[controller]"), ApiController]
    public class UserController : Controller
    {
        private readonly IValidator<CreateUserModel> _createUserModelValidator;
        private readonly IValidator<UpdateUserModel> _updateUserModelValidator;
        private readonly IUserService _usuarioService;

        public UserController(
            IValidator<CreateUserModel> createUserModelValidator,
            IValidator<UpdateUserModel> updateUserModelValidator,
            IUserService usuarioService)
        {
            _createUserModelValidator = createUserModelValidator;
            _updateUserModelValidator = updateUserModelValidator;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var usersList = _usuarioService.List();

            return ResultHelper.Success(usersList);
        }

        [HttpPost]
        public IActionResult Store([FromBody] CreateUserModel model)
        {
            var validation = _createUserModelValidator.Validate(model);
            if (!validation.IsValid)
                return ResultHelper.ValidationError(validation);

            var user = CreateUserModel.Map(model);

            _usuarioService.Store(user);

            return ResultHelper.Created();
        }

        [HttpPut, Route("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateUserModel model)
        {
            var validation = _updateUserModelValidator.Validate(model);
            if (!validation.IsValid)
                return ResultHelper.ValidationError(validation);

            _usuarioService.Update(id, model);

            return ResultHelper.Success();
        }

        [HttpPost, Route("{id}/inactivate")]
        public IActionResult Inactivate([FromRoute] Guid id)
        {
            _usuarioService.Inactivate(id);

            return ResultHelper.Success();
        }
    }
}
