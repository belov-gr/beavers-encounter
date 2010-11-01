using System;
using System.Security.Principal;
using Newtonsoft.Json;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    public class User : Entity, IPrincipal
    {
        public User() { }

        [DomainSignature]
        [NotNull, NotEmpty]
        [JsonProperty]
        public virtual string Login { get; set; }

        [NotNull, NotEmpty]
        public virtual string Password { get; set; }

        [JsonProperty]
        public virtual string Nick { get; set; }

        [JsonProperty]
        public virtual string Phone { get; set; }

        [JsonProperty]
        public virtual string Icq { get; set; }

        private Team team;
        [JsonProperty(IsReference = true, ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
        public virtual Team Team
        {
            get { return team; }
            set
            {
                if (value != null && (IsAdministrator || IsAuthor || Role.IsGuest))
                {
                    throw new Exception("Пользователь не может быть членом команды.");
                }
                team = value;
            }
        }


        private Game game;
        public virtual Game Game
        {
            get { return game; }
            set
            {
                if (value != null && (IsAdministrator || Role.IsGuest))
                {
                    throw new Exception("Пользователь не может быть автором игры.");
                }
                if (value != null && Team != null)
                {
                    throw new Exception("Член команды не может быть автором игры.");
                }
                game = value;
            }
        }

        [JsonProperty]
        public virtual bool IsEnabled { get; set; }

        private Role role;
        [JsonProperty]
        public virtual Role Role
        {
            get
            {
                if (role != null && (role.IsAdministrator || role.IsGuest))
                    return role;
                if (Game != null)
                    return Role.Author;
                if (Team != null && Team.TeamLeader != null && Team.TeamLeader.Id == Id)
                    return Role.TeamLeader;
                if (Team != null)
                    return Role.Player;
                if (Team == null)
                    return Role.Player;
                return role;
            }
            set { role = value; }
        }

        public static User DefaultUser
        {
            get { return new User { Login = "", Password = "", Role = Role.Player, IsEnabled = true }; }
        }

        public static User Guest
        {
            get
            {
                return new User { Login = "Guest", Role = Role.Guest };
            }
        }

        public virtual bool IsAdministrator { get { return Role.Id == Role.AdministratorId; } }
        public virtual bool IsAuthor { get { return Role.Id == Role.AuthorId; } }
        public virtual bool IsPlayer { get { return Role.Id == Role.PlayerId; } }

        public virtual IIdentity Identity
        {
            get
            {
                bool isAuthenticated = !(Role.Name == Role.Guest.Name);
                return new Identity(isAuthenticated, Login);
            }
        }

        public virtual bool IsInRole(string role)
        {
            return Role.Name.Equals(role);
        }

        public virtual void SetId(int id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Simple implementation of IIdentity
    /// </summary>
    public class Identity : IIdentity
    {
        private readonly bool isAuthenticated;
        private readonly string name;

        public Identity(bool isAuthenticated, string name)
        {
            this.isAuthenticated = isAuthenticated;
            this.name = name;
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}
