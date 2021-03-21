using Anazon.Domain.Entities;
using Anazon.Infra.Data.Context;
using Anazon.Infra.Data.Repositories;
using Anazon.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Anazon.Tests.Data.Repositories
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<AnazonContext> ContextOptions;

        public RepositoryTests()
        {
            ContextOptions = new DbContextOptionsBuilder<AnazonContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact(DisplayName = "Should list entities")]
        public void ShouldListEntity()
        {
            var fakeList = UserFakeList.Get().ToList();

            using var context = new AnazonContext(ContextOptions);
            context.Set<User>().AddRange(fakeList);
            context.SaveChanges();

            var result = new BaseRepository<User>(context).List();

            Assert.NotEmpty(result);
            Assert.Equal(fakeList.Count, result.Count());
        }

        [Fact(DisplayName = "Should store an entity")]
        public void ShouldStoreEntity()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            using var context = new AnazonContext(ContextOptions);

            var inserted = new BaseRepository<User>(context).Store(fakeUser);

            var user = context.Set<User>().Find(fakeUser.Id);

            Assert.True(inserted);
            Assert.NotNull(user);
        }

        [Fact(DisplayName = "Should update an entity")]
        public void ShouldUpdateEntity()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            using var context = new AnazonContext(ContextOptions);
            var dbSet = context.Set<User>();
            dbSet.Add(fakeUser);
            context.SaveChanges();

            var findedUser = dbSet.Find(fakeUser.Id);
            var updatedValue = "Altered field";
            findedUser.Name = updatedValue;

            var updated = new BaseRepository<User>(context).Update(findedUser);

            var user = dbSet.Find(fakeUser.Id);

            Assert.True(updated);
            Assert.Equal(updatedValue, user.Name);
            Assert.NotEqual(user.CreatedAt, user.UpdatedAt);
        }

        [Fact(DisplayName = "Should delete an entity")]
        public void ShouldDeleteEntity()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            using var context = new AnazonContext(ContextOptions);
            var dbSet = context.Set<User>();
            dbSet.Add(fakeUser);
            context.SaveChanges();

            var findedUser = dbSet.Find(fakeUser.Id);

            var deleted = new BaseRepository<User>(context).Remove(findedUser);

            var user = dbSet.Find(fakeUser.Id);

            Assert.True(deleted);
            Assert.Null(user);
        }

        [Fact(DisplayName = "Should find entity by id")]
        public void ShouldFindEntityById()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            using var context = new AnazonContext(ContextOptions);
            var dbSet = context.Set<User>();
            dbSet.Add(fakeUser);
            context.SaveChanges();

            var user = new BaseRepository<User>(context).Find(fakeUser.Id);

            Assert.NotNull(user);
            Assert.Equal(fakeUser.CPF, user.CPF);
        }

        [Fact(DisplayName = "Should find entity by filter")]
        public void ShouldFundEntityByFilter()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            using var context = new AnazonContext(ContextOptions);
            var dbSet = context.Set<User>();
            dbSet.Add(fakeUser);
            context.SaveChanges();

            var user = new BaseRepository<User>(context).Find(x => x.CPF == fakeUser.CPF);

            Assert.NotNull(user);
            Assert.Equal(fakeUser.CPF, user.CPF);
        }
    }
}
