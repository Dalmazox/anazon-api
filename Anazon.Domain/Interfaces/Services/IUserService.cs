using Anazon.Domain.Entities;
using Anazon.Domain.Models.Update;
using System;
using System.Collections.Generic;

namespace Anazon.Domain.Interfaces.Services
{
    public interface IUserService
    {
        IEnumerable<User> List();
        bool Store(User user);
        bool Update(Guid id, UpdateUserModel model);
        bool Inactivate(Guid id);
    }
}
