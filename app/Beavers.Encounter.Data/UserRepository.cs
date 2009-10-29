using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;
using SharpArch.Data.NHibernate;
using NHibernate;
using Beavers.Encounter.Core.DataInterfaces;
using NHibernate.Criterion;

namespace Beavers.Encounter.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public User GetByLogin(string login)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(User))
                .Add(Expression.Eq("Login", login));
                        
            List<User> lu = criteria.List<User>() as List<User>;
            User user = lu.Find(x => x.Login.ToUpper() == login.ToUpper());
            return user;
        }
    }
}
