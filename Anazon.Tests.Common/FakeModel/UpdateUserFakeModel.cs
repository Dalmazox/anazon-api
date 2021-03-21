using Anazon.Domain.Models.Update;
using Bogus;

namespace Anazon.Tests.Common.FakeModel
{
    public class UpdateUserFakeModel
    {
        public static UpdateUserModel Get(char sex = 'M')
        {
            var faker = new Faker("pt_BR");

            return new UpdateUserModel()
            {
                Name = faker.Person.FullName,
                Birthdate = faker.Person.DateOfBirth,
                Sex = sex
            };
        }
    }
}
