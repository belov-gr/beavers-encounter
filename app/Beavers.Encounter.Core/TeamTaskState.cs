using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System.ComponentModel;

namespace Beavers.Encounter.Core
{
    public class TeamTaskState : Entity
    {
        public TeamTaskState()
        {
            InitMembers();
        }

        private void InitMembers()
        {
            AcceptedTips = new List<AcceptedTip>();
            AcceptedCodes = new List<AcceptedCode>();
            AcceptedBadCodes = new List<AcceptedBadCode>();
        }

        [NotNull]
        public virtual DateTime TaskStartTime { get; set; }

        public virtual DateTime? AccelerationTaskStartTime { get; set; }

        public virtual DateTime? TaskFinishTime { get; set; }

        [NotNull]
        public virtual int State { get; set; }

        [DomainSignature]
        [NotNull]
        public virtual TeamGameState TeamGameState { get; set; }

        [DomainSignature]
        [NotNull]
        public virtual Task Task { get; set; }

        public virtual Task NextTask { get; set; }

        public virtual IList<AcceptedTip> AcceptedTips { get; protected set; }
        public virtual IList<AcceptedCode> AcceptedCodes { get; protected set; }
        public virtual IList<AcceptedBadCode> AcceptedBadCodes { get; protected set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", TeamGameState.Team.Name, Task.Name);
        }
    }

    public enum TeamTaskStateFlag
    {
        [Description("Выполняется")]
        Execute,
        [Description("Выполнено")]
        Success,
        [Description("Не выполнено")]
        Overtime,
        [Description("Задание слито")]
        Canceled,
        [Description("Дисквалификация")]
        Cheat
    }
}
