using System;
using SharpArch.Core.DomainModel;
using NHibernate.Validator.Constraints;

namespace Beavers.Encounter.Core
{
    public class AcceptedCode : Entity
    {
        public AcceptedCode() { }

        [DomainSignature]
        [NotNull]
        public virtual DateTime AcceptTime { get; set; }

        [DomainSignature]
        [NotNull]
        public virtual Code Code { get; set; }

        [NotNull]
        public virtual TeamTaskState TeamTaskState { get; set; }
    }
}
