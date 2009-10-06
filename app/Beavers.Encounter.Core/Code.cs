using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    public class Code : Entity
    {
        public Code() { }
		
		[DomainSignature]
		[NotNull, NotEmpty]
		public virtual string Name { get; set; }

        public virtual int IsBonus { get; set; }

		[NotNull, NotEmpty]
		public virtual string Danger { get; set; }

		[NotNull]
		public virtual Task Task { get; set; }
    }
}
