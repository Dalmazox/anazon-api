using Anazon.Domain.Entities;
using Anazon.Domain.Interfaces.Services;
using Anazon.Domain.Interfaces.UoW;
using Anazon.Domain.Models.Update;
using System;
using System.Collections.Generic;

namespace Anazon.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<User> List()
        {
            return _uow.Users.List();
        }

        public bool Inactivate(Guid id)
        {
            var user = _uow.Users.Find(id);
            if (user is null)
                throw new Exception("Usuário não encontrado");

            user.IsActive = false;

            return _uow.Users.Update(user);
        }

        public bool Update(Guid id, UpdateUserModel model)
        {
            var user = _uow.Users.Find(id);
            if (user is null)
                throw new Exception("Usuário não encontrado");

            user.Name = model.Name;
            user.Birthdate = model.Birthdate;
            user.Sex = model.Sex;

            return _uow.Users.Update(user);
        }

        public bool Store(User user)
        {
            var cpf = user.CPF;
            var exists = _uow.Users.Find(x => x.CPF == user.CPF) is not null;
            if (exists)
                throw new Exception("CPF em uso");

            return _uow.Users.Store(user);
        }
    }
}
