using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using Rhino.Mocks;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Testing;

namespace Tests.Repositories
{
    public static class MockRepositoryBuilder
    {
        public static IUserRepository CreateUserRepository()
        {
            var userRepositoryMock = MockRepository.GenerateStub<IUserRepository>();

            var users = new List<User>
            {
                new User { Login = "Henry@suteki.co.uk", 
                    Password = "6C80B78681161C8349552872CFA0739CF823E87B", IsEnabled = true }, // henry1

                new User { Login = "George@suteki.co.uk", 
                    Password = "DC25F9DC0DF2BE9E6A83E6F0B26F4B41F57ADF6D", IsEnabled = true }, // george1

                new User { Login = "Sky@suteki.co.uk", 
                    Password = "980BC222DA7FDD0D37BE816D60084894124509A1", IsEnabled = true } // sky1
            };

            userRepositoryMock.Expect(ur => ur.GetAll()).Return(users);

            return userRepositoryMock;
        }

        public static IRepository<Role> CreateRoleRepository()
        {
            var roleRepositoryMock = MockRepository.GenerateStub<IRepository<Role>>();

            var roles = new List<Role>
            {
                new Role { Name = "Administrator" },
                new Role { Name = "Autor" },
                new Role { Name = "Player" },
                new Role { Name = "Guest" }
            };

            int id = 1;
            foreach (Role role in roles)
            {
                EntityIdSetter.SetIdOf<int>(role, id++);
            }

            roleRepositoryMock.Expect(r => r.GetAll()).Return(roles);

            return roleRepositoryMock;
        }

    }
}
