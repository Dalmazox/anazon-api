using Anazon.Domain.Models.Create;
using Bogus;
using Bogus.Extensions.Brazil;
using System.Linq;

namespace Anazon.Tests.Common.FakeModel
{
    public class CreateUserFakeModel
    {
        public static CreateUserModel Get(char sex = 'M')
        {
            var faker = new Faker("pt_BR");

            return new CreateUserModel()
            {
                Name = faker.Person.FullName,
                CPF = faker.Person.Cpf(false),
                RG = string.Join("", Enumerable.Range(1, 9)),
                Birthdate = faker.Person.DateOfBirth,
                Sex = sex
            };
        }
    }
}
