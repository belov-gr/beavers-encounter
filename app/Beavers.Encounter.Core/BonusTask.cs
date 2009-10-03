using NHibernate.Validator.Constraints;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    public class BonusTask : Entity
    {
        public BonusTask() { }
		
		[DomainSignature]
		[NotNull, NotEmpty]
		public virtual string Name { get; set; }

		[NotNull, NotEmpty]
		public virtual string TaskText { get; set; }

        [NotNull]
        public virtual DateTime StartTime { get; set; }

        [NotNull]
        public virtual DateTime FinishTime { get; set; }

		public virtual Game Game { get; set; }

        [NotNull]
        public virtual int IsIndividual { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
