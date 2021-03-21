using Anazon.Application.Helpers;
using Anazon.Domain.ValueObjects;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace Anazon.Tests.Application.Helpers
{
    public class ResultHelperTests
    {
        [Fact(DisplayName = "Should generate success return")]
        public void ShouldGenerateSuccessReturn()
        {
            var result = (ResultHelper.Success() as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Sucesso na requisição", result.Message);
        }

        [Fact(DisplayName = "Should generate success return with data")]
        public void ShouldGenerateSuccessReturnWithData()
        {
            var customData = new { Date = DateTime.Now };
            var result = (ResultHelper.Success(customData) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Sucesso na requisição", result.Message);
            Assert.Equal(customData, result.Data);
        }

        [Fact(DisplayName = "Should generate success return with custom message")]
        public void ShouldGenerateSuccessReturnWithCustomMessage()
        {
            var customData = new { Date = DateTime.Now };
            var customMessage = "Custom success return message";
            var result = (ResultHelper.Success(customData, customMessage) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(customMessage, result.Message);
            Assert.Equal(customData, result.Data);
        }

        [Fact(DisplayName = "Should generate error return")]
        public void ShouldGenerateErrorReturn()
        {
            var customException = new Exception("Any exception");
            var result = (ResultHelper.Error(customException) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(customException.Message, result.Message);
            Assert.Equal(customException.InnerException, result.Data);
        }

        [Fact(DisplayName = "Should generate validation error return")]
        public void ShouldGenerateValidationErrorReturn()
        {
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "Error message") });
            var result = (ResultHelper.ValidationError(validationResult) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Corpo da requisição inválido", result.Message);
            Assert.NotNull(result.Data);
        }

        [Fact(DisplayName = "Should generate created status return")]
        public void ShouldGenerateCreatedStatusReturn()
        {
            var result = (ResultHelper.Created() as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal("Criado com sucesso", result.Message);
        }

        [Fact(DisplayName = "Should generate created status return with data")]
        public void ShouldGenerateCreatedStatusReturnWithData()
        {
            var customData = new { Date = DateTime.Now };
            var result = (ResultHelper.Created(customData) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal("Criado com sucesso", result.Message);
            Assert.Equal(customData, result.Data);
        }

        [Fact(DisplayName = "Should generate created status return with custom message")]
        public void ShouldGenerateCreatedStatusReturnWithCustomMessage()
        {
            var customData = new { Date = DateTime.Now };
            var customMessage = "Custom created status return message";
            var result = (ResultHelper.Created(customData, customMessage) as ObjectResult).Value as Result;

            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal(customMessage, result.Message);
            Assert.Equal(customData, result.Data);
        }
    }
}
