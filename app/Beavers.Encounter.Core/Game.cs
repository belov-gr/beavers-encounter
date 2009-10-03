using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    public class Game : Entity
    {
        public Game()
        {
            InitMembers();
        }

        /// <summary>
        /// Since we want to leverage automatic properties, init appropriate members here.
        /// </summary>
        private void InitMembers()
        {
            Teams = new List<Team>();
            Tasks = new List<Task>();
            BonusTasks = new List<BonusTask>();
        }
        
        [DomainSignature]
		[NotNull, NotEmpty]
		public virtual string Name { get; set; }

		public virtual DateTime GameDate { get; set; }

		[NotNull, NotEmpty]
		public virtual string Description { get; set; }

		public virtual int TotalTime { get; set; }

        public virtual int TimePerTask { get; set; }

        public virtual int TimePerTip { get; set; }

        public virtual int GameState { get; set; }

        [Length(6)]
        public virtual string PrefixMainCode { get; set; }

        [Length(6)]
        public virtual string PrefixBonusCode { get; set; }

        public virtual IList<Team> Teams { get; protected set; }

        public virtual IList<Task> Tasks { get; protected set; }

        public virtual IList<BonusTask> BonusTasks { get; protected set; }
    }

    public enum GameStates : int
    {
        Planned,
        Startup,
        Started,
        Finished,
        Cloused
    }
}
