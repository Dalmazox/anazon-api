using Anazon.Domain.Entities;
using Anazon.Domain.Interfaces.Repositories;
using Anazon.Infra.Data.Context;

namespace Anazon.Infra.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AnazonContext context) : base(context) { }
    }
}
