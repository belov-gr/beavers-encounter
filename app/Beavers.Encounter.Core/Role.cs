using System;
using Newtonsoft.Json;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    [Serializable]
    public class Role : Entity
    {
        public Role()
            : base()
        {}

        public Role(int Id)
            : base()
        {
            this.Id = Id;
        }

        public override string ToString()
        {
            return Name;
        }

        [DomainSignature]
        [NotNullNotEmpty]
        [JsonProperty]
        public virtual string Name { get; set; }

        public const int AdministratorId = 1;
        public const int AuthorId = 2;
        public const int PlayerId = 3;
        public const int GuestId = 4;
        public const int TeamLeaderId = 5;

        // allowed roles. These must match the data in table Role
        public static Role Administrator { get { return new Role { Id = AdministratorId, Name = "Administrator" }; } }
        public static Role Author { get { return new Role { Id = AuthorId, Name = "Author" }; } }
        public static Role Player { get { return new Role { Id = PlayerId, Name = "Player" }; } }
        public static Role Guest { get { return new Role { Id = GuestId, Name = "Guest" }; } }
        public static Role TeamLeader { get { return new Role { Id = TeamLeaderId, Name = "TeamLeader" }; } }

        public virtual bool IsAdministrator { get { return Name == Administrator.Name; } }
        public virtual bool IsAuthor { get { return Name == Author.Name; } }
        public virtual bool IsPlayer { get { return Name == Player.Name; } }
        public virtual bool IsGuest { get { return Name == Guest.Name; } }
        public virtual bool IsTeamLeader { get { return Name == TeamLeader.Name; } }
    }
}
