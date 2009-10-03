using NHibernate.Validator.Constraints;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    public class AcceptedBadCode : Entity
    {
        public AcceptedBadCode() { }

        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string Name { get; set; }

        [NotNull]
        public virtual DateTime AcceptTime { get; set; }

        [NotNull]
        public virtual TeamTaskState TeamTaskState { get; set; }
    }
}
