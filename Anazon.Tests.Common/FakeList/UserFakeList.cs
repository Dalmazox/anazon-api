using Anazon.Domain.Entities;
using Bogus;
using Bogus.Extensions.Brazil;
using System.Collections.Generic;
using System.Linq;

namespace Anazon.Tests.Common
{
    public static class UserFakeList
    {
        public static IEnumerable<User> Get()
        {
            var faker = new Faker("pt_BR");

            for (int i = 0; i < 2; i++)
            {
                yield return new User
                {
                    Name = faker.Person.FullName,
                    CPF = faker.Person.Cpf(),
                    RG = string.Join("", Enumerable.Range(1, 9)),
                    Birthdate = faker.Person.DateOfBirth,
                    Sex = 'I'
                };
            }
        }
    }
}
