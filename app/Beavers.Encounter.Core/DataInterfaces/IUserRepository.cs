using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Core.DataInterfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByLogin(string login);
    }
}
