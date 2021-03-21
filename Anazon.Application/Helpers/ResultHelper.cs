using Anazon.Domain.ValueObjects;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Anazon.Application.Helpers
{
    public static class ResultHelper
    {
        public static IActionResult Success(object data = null, string message = null)
            => Build(data, message ?? "Sucesso na requisição", StatusCodes.Status200OK);

        public static IActionResult Error(Exception ex)
            => Build(ex.InnerException, ex.Message, StatusCodes.Status400BadRequest);

        public static IActionResult Created(object data = null, string message = null)
            => Build(data, message ?? "Criado com sucesso", StatusCodes.Status201Created);

        public static IActionResult ValidationError(ValidationResult result)
        {
            var parsedErrors = result.Errors?.Select(x => new { Property = x.PropertyName, Message = x.ErrorMessage });

            return Build(parsedErrors, "Corpo da requisição inválido", StatusCodes.Status400BadRequest);
        }

        private static IActionResult Build(object data, string message, int statusCode)
            => new ObjectResult(new Result(data, message, statusCode)) { StatusCode = statusCode };
    }
}
