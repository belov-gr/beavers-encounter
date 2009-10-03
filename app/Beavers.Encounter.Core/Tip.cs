using NHibernate.Validator.Constraints;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    public class Tip : Entity
    {
        public Tip() { }
		
        [DomainSignature]
		[NotNull, NotEmpty]
		public virtual string Name { get; set; }

		[NotNull]
		public virtual int SuspendTime { get; set; }

		[NotNull]
		public virtual Task Task { get; set; }
    }
}
