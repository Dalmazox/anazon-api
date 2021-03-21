using Anazon.Domain.Interfaces.Repositories;
using Anazon.Infra.Data.Context;
using Anazon.Infra.Data.UoW;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Anazon.Tests.Data.UoW
{
    public class UnitOfWorkTests
    {
        private readonly DbContextOptions<AnazonContext> ContextOptions;

        public UnitOfWorkTests()
        {
            ContextOptions = new DbContextOptionsBuilder<AnazonContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact(DisplayName = "Should get users repository")]
        public void ShouldGetUsersRepository()
        {
            using var context = new AnazonContext(ContextOptions);

            using var uow = new UnitOfWork(context);

            Assert.NotNull(uow.Users);
            Assert.IsAssignableFrom<IUserRepository>(uow.Users);
        }
    }
}
