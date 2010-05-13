using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    [JsonObject(IsReference = true)]
    public class Team : Entity
    {
        public Team()
        {
            InitMembers();
        }

        /// <summary>
        /// Since we want to leverage automatic properties, init appropriate members here.
        /// </summary>
        private void InitMembers()
        {
            TeamGameStates = new List<TeamGameState>();
            Users = new List<User>();
            PreventTasksAfterTeams = new List<Team>();
        }

        [DomainSignature]
		[NotNull, NotEmpty]
        [Length(100)]
        [JsonProperty]
        [Meta.Caption("��������")]
		public virtual string Name { get; set; }

        [JsonProperty]
        [Meta.Caption("��� �������")]
        [Meta.Description("��������� ��� ��� ������� ����� ������� � �������. ���������� ��������� ��� �������� ������ �������� �������. ������� ������ ���������� ���� ��� ������ ���������� ����� �������.")]
        public virtual string AccessKey { get; set; }

        [Meta.Caption("�������������� �������")]
        [Meta.Description("�������������� ������� ��� ������� �������� � ������ ��������� �������, ���� � ��������� ������� ���������� ������� \"�������������� �������\".")]
        [Meta.TextArea(80, 10)]
        public virtual string FinalTask { get; set; }

        public virtual Game Game { get; set; }

        public virtual TeamGameState TeamGameState { get; set; }

        [JsonProperty(IsReference = true, ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
        public virtual User TeamLeader { get; set; }

        public virtual IList<TeamGameState> TeamGameStates { get; protected set; }

        public virtual IList<User> Users { get; protected set; }

        [Meta.Caption("����-����")]
        [Meta.Description("������ ����� �������� ������������� ����� �������, ��������� ������� ������� �� �������� ��������� �� �������� ��������� ����� ������.")]
        public virtual IList<Team> PreventTasksAfterTeams { get; protected set; }

        public virtual void SetId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
