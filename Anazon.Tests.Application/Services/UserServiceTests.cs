using Anazon.Application.Services;
using Anazon.Domain.Entities;
using Anazon.Domain.Interfaces.Repositories;
using Anazon.Domain.Interfaces.UoW;
using Anazon.Domain.Models.Update;
using Anazon.Tests.Common;
using Anazon.Tests.Common.FakeModel;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Anazon.Tests.Application.Services
{
    public class UserServiceTests
    {
        [Fact(DisplayName = "Should list users")]
        public void ShouldListUsers()
        {
            var fakeList = UserFakeList.Get().ToList();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.List()).Returns(fakeList);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var result = new UserService(unitOfWorkMock.Object).List();

            Assert.NotNull(result);
            Assert.Equal(fakeList.Count, result.Count());
            userRepositoryMock.Verify(x => x.List(), Times.Once);
        }

        [Fact(DisplayName = "Should store an user")]
        public void ShouldStoreUser()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Store(It.IsAny<User>())).Returns(true);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var inserted = new UserService(unitOfWorkMock.Object).Store(fakeUser);

            Assert.True(inserted);
            userRepositoryMock.Verify(x => x.Store(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Should throw an error on CPF already exists on insert")]
        public void ShoudThrowErrorOnCpfExistsOnInsert()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(fakeUser);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            Assert.Throws<Exception>(() => new UserService(unitOfWorkMock.Object).Store(fakeUser));
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            userRepositoryMock.Verify(x => x.Store(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Should update an user")]
        public void ShouldUpdateUser()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(fakeUser);
            userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Returns(true);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var updated = new UserService(unitOfWorkMock.Object).Update(It.IsAny<Guid>(), UpdateUserFakeModel.Get());

            Assert.True(updated);
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Guid>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Should throw an error on update an not founded user")]
        public void ShoudThrowErrorOnUpdateNotFoundedUser()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns((User)null);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            Assert.Throws<Exception>(() => new UserService(unitOfWorkMock.Object).Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>()));
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Guid>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Should inactivate an user")]
        public void ShouldInactivateUser()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(fakeUser);
            userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Returns(true);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var updated = new UserService(unitOfWorkMock.Object).Inactivate(fakeUser.Id);

            Assert.True(updated);
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Guid>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Should throw an error on inactivate an not founded user")]
        public void ShoudThrowErrorOnInactivateNotFoundedUser()
        {
            var fakeUser = UserFakeList.Get().ToList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns((User)null);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            Assert.Throws<Exception>(() => new UserService(unitOfWorkMock.Object).Inactivate(fakeUser.Id));
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Guid>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }
    }
}
