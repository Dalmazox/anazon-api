using Anazon.Domain.Entities;
using Anazon.Infra.Data.Context;
using Anazon.Tests.Common.Faker;

namespace Anazon.Tests.Presentation.Api
{
    public static class Seed
    {
        public static void PopulateUsers(AnazonContext context)
        {
            var fakeData = UserFaker.GetList();

            context.Set<User>().AddRange(fakeData);
            context.SaveChanges();
        }
    }
}
