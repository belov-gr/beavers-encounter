using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    public class AcceptedTip : Entity
    {
        public AcceptedTip()
        {
        }

        [DomainSignature]
        [NotNull]
        public virtual DateTime AcceptTime { get; set; }

        [DomainSignature]
        [NotNull]
        public virtual Tip Tip { get; set; }

        [NotNull]
        public virtual TeamTaskState TeamTaskState { get; set; }
    }
}
