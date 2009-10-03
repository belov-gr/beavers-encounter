using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Core.DomainModel;
using NHibernate.Validator.Constraints;

namespace Beavers.Encounter.Core
{
    public class TeamGameState : Entity
    {
        public TeamGameState()
        {
            InitMembers();
        }

        private void InitMembers()
        {
            AcceptedTasks = new List<TeamTaskState>();
        }

        /// <summary>
        /// Время, когда команда (возможно досрочно) закончила игру.
        /// Если null, то команда еще принимает участие в игре.
        /// </summary>
        public virtual DateTime? GameDoneTime { get; set; }
        
        [DomainSignature]
        [NotNull]
        public virtual Team Team { get; set; }

        [DomainSignature]
        [NotNull]
        public virtual Game Game { get; set; }

        /// <summary>
        /// Текущее выполняемое задание.
        /// </summary>
        public virtual TeamTaskState ActiveTaskState { get; set; }

        /// <summary>
        /// Полученные командой задания, 
        /// не включая текущего выполняемого задания.
        /// </summary>
        public virtual IList<TeamTaskState> AcceptedTasks { get; protected set; }

        /// <summary>
        /// Полученные командой сквозные бонусные задания, 
        /// не включая текущих выполняемых сквозных бонусных заданий.
        /// </summary>
        //public virtual IList<TeamTaskState> AcceptedBonusTasks { get; protected set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Team.Name, Game.Name) ;
        }
    }
}
