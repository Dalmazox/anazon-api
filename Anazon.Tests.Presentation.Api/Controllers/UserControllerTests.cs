using Anazon.Domain.Entities;
using Anazon.Domain.Interfaces.Services;
using Anazon.Domain.Models.Create;
using Anazon.Domain.Models.Update;
using Anazon.Domain.ValueObjects;
using Anazon.Presentation.Api.Controllers;
using Anazon.Tests.Common;
using Anazon.Tests.Common.FakeModel;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Anazon.Tests.Presentation.Api.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IValidator<CreateUserModel>> _createUserModelValidatorMock;
        private readonly Mock<IValidator<UpdateUserModel>> _updateUserModelValidatorMock;
        private readonly Mock<IUserService> _userServiceMock;

        public UserControllerTests()
        {
            _createUserModelValidatorMock = new Mock<IValidator<CreateUserModel>>();
            _updateUserModelValidatorMock = new Mock<IValidator<UpdateUserModel>>();
            _userServiceMock = new Mock<IUserService>();
        }

        [Fact(DisplayName = "Should return success with users list")]
        public void ShouldReturnSuccessWithList()
        {
            var fakeList = UserFakeList.Get();
            _userServiceMock.Setup(x => x.List()).Returns(fakeList);

            var result = (new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Index() as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(fakeList.Count(), (result.Data as IEnumerable<User>).Count());
            _userServiceMock.Verify(x => x.List(), Times.Once);
        }

        [Fact(DisplayName = "Should handle any error on listing users")]
        public void ShouldHandleErrorsOnListing()
        {
            var customMessage = "Custom exception";
            var customException = new Exception(customMessage);

            _userServiceMock.Setup(x => x.List()).Throws(customException);

            var result = (new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Index() as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(customMessage, result.Message);
            Assert.Equal(customException.InnerException, result.Data);
            _userServiceMock.Verify(x => x.List(), Times.Once);
        }

        [Fact(DisplayName = "Should return success on storing user")]
        public void ShouldReturnSuccessOnStore()
        {
            var fakeUser = CreateUserFakeModel.Get();

            _userServiceMock.Setup(x => x.Store(It.IsAny<User>())).Returns(true);

            _createUserModelValidatorMock.Setup(x => x.Validate(It.IsAny<CreateUserModel>())).Returns(new ValidationResult());

            var result = (new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Store(fakeUser) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            _createUserModelValidatorMock.Verify(x => x.Validate(It.IsAny<CreateUserModel>()), Times.Once);
            _userServiceMock.Verify(x => x.Store(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Should return store validation error on required props empty")]
        public void ShouldReturnValidationErrorOnStore()
        {
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Nome inválido") });

            _createUserModelValidatorMock.Setup(x => x.Validate(It.IsAny<CreateUserModel>())).Returns(validationResult);

            var controllerResult = (new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Store(It.IsAny<CreateUserModel>()) as ObjectResult);
            var result = controllerResult.Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Corpo da requisição inválido", result.Message);
            Assert.NotNull(result.Data);
            _createUserModelValidatorMock.Verify(x => x.Validate(It.IsAny<CreateUserModel>()), Times.Once);
            _userServiceMock.Verify(x => x.Store(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Should handle any error on storing user")]
        public void ShouldHandleErrorsOnStore()
        {
            var customMessage = "Custom exception";
            var customException = new Exception(customMessage);
            var fakeUser = CreateUserFakeModel.Get();

            _createUserModelValidatorMock.Setup(x => x.Validate(It.IsAny<CreateUserModel>())).Returns(new ValidationResult());
            _userServiceMock.Setup(x => x.Store(It.IsAny<User>())).Throws(customException);

            var result = (new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Store(fakeUser) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(customMessage, result.Message);
            Assert.Equal(customException.InnerException, result.Data);
            _createUserModelValidatorMock.Verify(x => x.Validate(It.IsAny<CreateUserModel>()), Times.Once);
            _userServiceMock.Verify(x => x.Store(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Should return success on updating user")]
        public void ShouldReturnSuccessOnUpdate()
        {
            _userServiceMock.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>())).Returns(true);

            _updateUserModelValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateUserModel>())).Returns(new ValidationResult());

            var controllerResult = new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>()) as ObjectResult;
            var result = controllerResult.Value as Result;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            _updateUserModelValidatorMock.Verify(x => x.Validate(It.IsAny<UpdateUserModel>()), Times.Once);
            _userServiceMock.Verify(x => x.Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>()), Times.Once);
        }

        [Fact(DisplayName = "Should return update validation error on required props empty")]
        public void ShouldReturnValidationErrorOnUpdate()
        {
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Nome inválido") });

            _updateUserModelValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateUserModel>())).Returns(validationResult);

            var controllerResult = new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>()) as ObjectResult;
            var result = controllerResult.Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Corpo da requisição inválido", result.Message);
            Assert.NotNull(result.Data);
            _updateUserModelValidatorMock.Verify(x => x.Validate(It.IsAny<UpdateUserModel>()), Times.Once);
            _userServiceMock.Verify(x => x.Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>()), Times.Never);
        }

        [Fact(DisplayName = "Should handle any error on updating user")]
        public void ShouldHandleErrorsOnUpdate()
        {
            var customMessage = "Custom exception";
            var customException = new Exception(customMessage);
            var fakeUser = CreateUserFakeModel.Get();

            _updateUserModelValidatorMock.Setup(x => x.Validate(It.IsAny<UpdateUserModel>())).Returns(new ValidationResult());
            _userServiceMock.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>())).Throws(customException);

            var controllerResult = new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>()) as ObjectResult;
            var result = controllerResult.Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(customMessage, result.Message);
            Assert.Equal(customException.InnerException, result.Data);
            _updateUserModelValidatorMock.Verify(x => x.Validate(It.IsAny<UpdateUserModel>()), Times.Once);
            _userServiceMock.Verify(x => x.Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>()), Times.Once);
        }

        [Fact(DisplayName = "Should return success on inactivating user")]
        public void ShouldReturnSuccessOnInactivating()
        {
            _userServiceMock.Setup(x => x.Inactivate(It.IsAny<Guid>())).Returns(true);

            var controllerResult = new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Inactivate(It.IsAny<Guid>()) as ObjectResult;
            var result = controllerResult.Value as Result;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            _userServiceMock.Verify(x => x.Inactivate(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Should handle any error on inactivating user")]
        public void ShouldHandleErrorsOnInactivating()
        {
            var customMessage = "Custom exception";
            var customException = new Exception(customMessage);

            _userServiceMock.Setup(x => x.Inactivate(It.IsAny<Guid>())).Throws(customException);

            var controllerResult = new UserController(_createUserModelValidatorMock.Object, _updateUserModelValidatorMock.Object, _userServiceMock.Object).Inactivate(It.IsAny<Guid>()) as ObjectResult;
            var result = controllerResult.Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(customMessage, result.Message);
            Assert.Equal(customException.InnerException, result.Data);
            _userServiceMock.Verify(x => x.Inactivate(It.IsAny<Guid>()), Times.Once);
        }
    }
}
