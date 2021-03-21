using Anazon.Domain.Interfaces.Repositories;
using Anazon.Domain.Interfaces.UoW;
using Anazon.Infra.Data.Context;
using Anazon.Infra.Data.Repositories;
using System;

namespace Anazon.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnazonContext _context;
        private IUserRepository _users;

        public IUserRepository Users => _users ??= new UserRepository(_context);

        public UnitOfWork(AnazonContext context)
        {
            _context = context;
        }

        #region Dispose
        private bool _disposed = false;


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
