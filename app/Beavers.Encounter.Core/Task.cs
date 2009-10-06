using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    public class Task : Entity
    {
        public Task()
        {
            InitMembers();
        }

        /// <summary>
        /// Since we want to leverage automatic properties, init appropriate members here.
        /// </summary>
        private void InitMembers()
        {
            Tips = new List<Tip>();
            Codes = new List<Code>();
            NotAfterTasks = new List<Task>();
            NotOneTimeTasks = new List<Task>();
        }

        [DomainSignature]
		[NotNull, NotEmpty]
		public virtual string Name { get; set; }

        [NotNull]
        public virtual int StreetChallendge { get; set; }

        [NotNull]
        public virtual int Agents { get; set; }

        [NotNull]
        public virtual int Locked { get; set; }

        public virtual Game Game { get; set; }

        public virtual IList<Tip> Tips { get; protected set; }

        public virtual IList<Code> Codes { get; protected set; }

        public virtual IList<Task> NotAfterTasks { get; protected set; }

        public virtual IList<Task> NotOneTimeTasks { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
