﻿using Anazon.Application.Helpers;
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
            try
            {
                var usersList = _usuarioService.List();

                return ResultHelper.Success(usersList);
            }
            catch (Exception ex)
            {
                return ResultHelper.Error(ex);
            }
        }

        [HttpPost]
        public IActionResult Store([FromBody] CreateUserModel model)
        {
            try
            {
                var validation = _createUserModelValidator.Validate(model);
                if (!validation.IsValid)
                    return ResultHelper.ValidationError(validation);

                var user = CreateUserModel.Map(model);

                _usuarioService.Store(user);

                return ResultHelper.Created();
            }
            catch (Exception ex)
            {
                return ResultHelper.Error(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateUserModel model)
        {
            try
            {
                var validation = _updateUserModelValidator.Validate(model);
                if (!validation.IsValid)
                    return ResultHelper.ValidationError(validation);

                _usuarioService.Update(id, model);

                return ResultHelper.Success();
            }
            catch (Exception ex)
            {
                return ResultHelper.Error(ex);
            }
        }

        [HttpPost, Route("{id}/inactivate")]
        public IActionResult Inactivate([FromRoute] Guid id)
        {
            try
            {
                _usuarioService.Inactivate(id);

                return ResultHelper.Success();
            }
            catch (Exception ex)
            {
                return ResultHelper.Error(ex);
            }
        }
    }
}
