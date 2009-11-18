using System.Collections.Generic;
using NHibernate.Validator.Constraints;
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
        [Meta.Caption("��������")]
        [Meta.Description("�������� ����")]
        public virtual string Name { get; set; }

        [Meta.Caption("���� ����������")]
        [Meta.Description("������ ����� ��.��.�� ��:��.")]
        public virtual DateTime GameDate { get; set; }

		[NotNull, NotEmpty]
        [Meta.Caption("��������")]
        [Meta.TextArea(40, 3)]
        public virtual string Description { get; set; }

        [Meta.Default(540)]
        [Meta.Caption("����������������� ����")]
        [Meta.Description("�������� � �������. ��������, 540 - �.�. 9 �����.")]
        public virtual int TotalTime { get; set; }

        [Meta.Default(90)]
        [Meta.Caption("����� �� �������")]
        [Meta.Description("�������� � �������. ��������, 90 - �.�. 1.5 ����.")]
        public virtual int TimePerTask { get; set; }

        [Meta.Default(30)]
        [Meta.Caption("����� �� ���������")]
        [Meta.Description("�������� � �������. ��������, 30 - �.�. ��� ����.")]
        public virtual int TimePerTip { get; set; }

        public virtual GameStates GameState { get; set; }

        [Length(6)]
        [Meta.Caption("������� ��� ��������� ����")]
        [Meta.Description("��������, 14DR.")]
        public virtual string PrefixMainCode { get; set; }

        [Length(6)]
        [Meta.Caption("������� ��� ��������� ����")]
        [Meta.Description("��������, 14B.")]
        public virtual string PrefixBonusCode { get; set; }

        public virtual IList<Team> Teams { get; protected set; }

        public virtual IList<Task> Tasks { get; protected set; }

        public virtual IList<BonusTask> BonusTasks { get; protected set; }

        public static int BadCodesLimit = 20;
    }

    public enum GameStates
    {
        Planned,
        Startup,
        Started,
        Finished,
        Cloused
    }
}
