using Anazon.Domain.Interfaces.Repositories;
using System;

namespace Anazon.Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
    }
}
