using Anazon.Application.Services;
using Anazon.Domain.Entities;
using Anazon.Domain.Interfaces.Repositories;
using Anazon.Domain.Interfaces.UoW;
using Anazon.Domain.Models.Update;
using Anazon.Tests.Common.Faker;
using FluentAssertions;
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
            var fakeList = UserFaker.GetList().ToList();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.List()).Returns(fakeList);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var result = new UserService(unitOfWorkMock.Object).List();

            result.Should().NotBeNull().And.HaveCount(fakeList.Count);
            userRepositoryMock.Verify(x => x.List(), Times.Once);
        }

        [Fact(DisplayName = "Should store an user")]
        public void ShouldStoreUser()
        {
            var fakeUser = UserFaker.GetList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Store(It.IsAny<User>())).Returns(true);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var inserted = new UserService(unitOfWorkMock.Object).Store(fakeUser);

            inserted.Should().BeTrue();
            userRepositoryMock.Verify(x => x.Store(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Should throw an error on CPF already exists on insert")]
        public void ShoudThrowErrorOnCpfExistsOnInsert()
        {
            var fakeUser = UserFaker.GetList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(fakeUser);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var userService = new UserService(unitOfWorkMock.Object);

            userService.Invoking(x => x.Store(fakeUser)).Should().Throw<Exception>().WithMessage("CPF em uso");
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            userRepositoryMock.Verify(x => x.Store(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Should update an user")]
        public void ShouldUpdateUser()
        {
            var fakeUser = UserFaker.GetList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(fakeUser);
            userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Returns(true);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var updated = new UserService(unitOfWorkMock.Object).Update(It.IsAny<Guid>(), UserFaker.GetUpdateModel());

            updated.Should().BeTrue();
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

            var userService = new UserService(unitOfWorkMock.Object);

            userService.Invoking(x => x.Update(It.IsAny<Guid>(), It.IsAny<UpdateUserModel>())).Should().Throw<Exception>().WithMessage("Usuário não encontrado");
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Guid>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Should inactivate an user")]
        public void ShouldInactivateUser()
        {
            var fakeUser = UserFaker.GetList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(fakeUser);
            userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Returns(true);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var updated = new UserService(unitOfWorkMock.Object).Inactivate(fakeUser.Id);

            updated.Should().BeTrue();
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Guid>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Should throw an error on inactivate an not founded user")]
        public void ShoudThrowErrorOnInactivateNotFoundedUser()
        {
            var fakeUser = UserFaker.GetList().First();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns((User)null);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Users).Returns(userRepositoryMock.Object);

            var userService = new UserService(unitOfWorkMock.Object);

            userService.Invoking(x => x.Inactivate(fakeUser.Id)).Should().Throw<Exception>().WithMessage("Usuário não encontrado");
            userRepositoryMock.Verify(x => x.Find(It.IsAny<Guid>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }
    }
}
