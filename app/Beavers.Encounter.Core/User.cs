using System.Security.Principal;
using NHibernate.Validator.Constraints;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    public class User : Entity, IPrincipal
    {
        public User() { }
		
		[DomainSignature]
		[NotNull, NotEmpty]
		public virtual string Login { get; set; }

		[NotNull, NotEmpty]
		public virtual string Password { get; set; }

		public virtual string Nick { get; set; }

		public virtual string Phone { get; set; }

		public virtual string Icq { get; set; }

        private Team team;
        public virtual Team Team
        {
            get { return team; }
            set
            {
                if (value != null && role != null && 
                    (role.IsAdministrator || role.IsGuest))
                {
                    throw new Exception("ѕользователь не может создавать команды.");
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
                if (value != null && role != null && 
                    (role.IsAdministrator || role.IsGuest))
                {
                    throw new Exception("ѕользователь не может создавать игры.");
                }
                game = value;
            }
        }

        public virtual bool IsEnabled { get; set; }

        private Role role;
        public virtual Role Role
        {
            get
            {
                if (role.IsAdministrator || role.IsGuest)
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
                return new User() { Login = "Guest", Role = Role.Guest };
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
                return new Identity(isAuthenticated, this.Login);
            }
        }

        public virtual bool IsInRole(string role)
        {
            return this.Role.Name.Equals(role);
        }
    }

    /// <summary>
    /// Simple implementation of IIdentity
    /// </summary>
    public class Identity : IIdentity
    {
        private bool isAuthenticated;
        private string name;

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
