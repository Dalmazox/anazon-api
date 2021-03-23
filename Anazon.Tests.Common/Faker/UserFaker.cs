using Anazon.Domain.Entities;
using Anazon.Domain.Models.Create;
using Anazon.Domain.Models.Update;
using Bogus;
using Bogus.Extensions.Brazil;
using System.Collections.Generic;
using System.Linq;

namespace Anazon.Tests.Common.Faker
{
    public static class UserFaker
    {
        private static IEnumerable<User> _users;

        public static IEnumerable<User> GetList()
        {
            if (_users is null || _users.Count() <= 0)
            {
                _users = new Faker<User>("pt_BR")
                    .RuleFor(x => x.Name, x => x.Person.FullName)
                    .RuleFor(x => x.CPF, x => x.Person.Cpf(false))
                    .RuleFor(x => x.RG, string.Join("", Enumerable.Range(1, 9)))
                    .RuleFor(x => x.Birthdate, x => x.Person.DateOfBirth)
                    .RuleFor(x => x.Sex, x => x.PickRandom('F', 'M'))
                    .Generate(5);
            }

            return _users;
        }

        public static CreateUserModel GetCreateModel()
        {
            var data = new Faker<CreateUserModel>("pt_BR")
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .RuleFor(x => x.CPF, x => x.Person.Cpf(false))
                .RuleFor(x => x.RG, string.Join("", Enumerable.Range(1, 9)))
                .RuleFor(x => x.Birthdate, x => x.Person.DateOfBirth)
                .RuleFor(x => x.Sex, x => x.PickRandom('F', 'M'))
                .Generate(1)
                .First();

            return data;
        }

        public static UpdateUserModel GetUpdateModel()
        {
            var data = new Faker<UpdateUserModel>("pt_BR")
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .RuleFor(x => x.Birthdate, x => x.Person.DateOfBirth)
                .RuleFor(x => x.Sex, x => x.PickRandom('F', 'M'))
                .Generate(1)
                .First();

            return data;
        }
    }
}
