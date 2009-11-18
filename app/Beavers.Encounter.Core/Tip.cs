using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    public class Tip : Entity
    {
        [DomainSignature]
		[NotNull, NotEmpty]
        [Meta.Caption("����� �������/���������")]
        [Meta.TextArea(80, 10)]
        public virtual string Name { get; set; }

		[NotNull]
        [Meta.Caption("�����")]
        public virtual int SuspendTime { get; set; }

		[NotNull]
		public virtual Task Task { get; set; }
    }
}
