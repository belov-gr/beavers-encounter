using System.Collections.Generic;
using System.Linq;

namespace Beavers.Encounter.Core.DataInterfaces
{
    public static class UserRepositoryExtensions
    {
        public static User WhereLoginIs(this IList<User> users, string login)
        {
            return users.Where(user => user.Login.ToUpper() == login.ToUpper()).SingleOrDefault();
        }

        public static bool ContainsUser(this IList<User> users, string login, string password)
        {
            return users.GetUser(login, password) != null;
        }

        public static User GetUser(this IList<User> users, string login, string password)
        {
            return users.SingleOrDefault(
                user =>
                    user.Login.ToUpper() == login.ToUpper() &&
                    user.Password == password /*&&
                    user.IsEnabled*/ //TODO: Блокировка пользователей
                );
        }
    }
}
