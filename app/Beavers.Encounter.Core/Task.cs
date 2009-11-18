using System.Collections.Generic;
using System.ComponentModel;
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
        [Meta.Caption("������� ��������")]
        [Meta.Description("� �������� ���� ������� �� ������ ��� ��������, ������� �������� ������� �������� ������ ������� ����. ��������, �������.")]
        public virtual string Name { get; set; }

        [NotNull]
        [Meta.Caption("Street Challenge")]
        [Meta.Description("������ ������� ���������, ��� ������� ����� ������ ���� �������� ��� ������ ������� � ������ ����.")]
        public virtual bool StreetChallendge { get; set; }

        [NotNull]
        [Meta.Caption("������� � ��������")]
        [Meta.Description("������� ������������ ��� ������������� ������� ���, ����� ������� � �������� ����������� ������������� ������ ����� ��������. ��������, ������� � +500.")]
        public virtual bool Agents { get; set; }

        [NotNull]
        [Meta.Caption("������� ��������������")]
        [Meta.Description("���� ���������� ������ �������, �� ������� �� ����� ���������� ��������. ���� ������� ����� �������������/������� � �������� ����.")]
        public virtual bool Locked { get; set; }

        [NotNull]
        [Meta.Default(0)]
        [Meta.Caption("��� �������")]
        [Meta.Description("0 - ������������ �������, 1 - ������� � ����������, 2 - ������� � ������� ���������.")]
        public virtual TaskTypes TaskType { get; set; }

        [NotNull]
        [Meta.Default(0)]
        [Meta.Caption("��������� �������")]
        [Meta.Description("��������� ����� ���� ������������� ��� �������������. ��������� ������ 100 ��������� ������� �������� �������� ��� �������, ��� ���� ������������ ������ ������� ������������ ������ ��������� 3-4 �������. ��� ���������� 150 ������������ ������� ������������ ����� ��������� 4-5 ������. ������������� ��������� ��������� ����������� ������ ������� ��������.")]
        public virtual int Priority { get; set; }

        public virtual Game Game { get; set; }

        public virtual IList<Tip> Tips { get; protected set; }

        public virtual IList<Code> Codes { get; protected set; }

        [Meta.Caption("�� �����")]
        [Meta.Description("����� �������� �������, ������� �� ����� �������������� �������� �������. ��� ���������� ��� �������������� ������� (������ -> �������) � �������� (�������� -> ��������) ��������� ������.")]
        public virtual IList<Task> NotAfterTasks { get; protected set; }

        [Meta.Caption("�� ������")]
        [Meta.Description("���� ������������� ����� ������� � ������ ������ ����������� ������-���� ������� ���������, �� ������ ���������� ������ ������� �������� �� �������� ��������. ��� ���������� ��� �������������� ����������� ������ �� ������ ������������� ������������ ���� ����� ��������. ��������, ������� \"��������\" �� ������ ���������� ��������, ���� ����� ���� ������� � ������ ������ ��������� ������� \"��������\". �.�. ��� ������� ��� ������� ��������� \"��������\" ���������� � ������ \"�� ������\" ������� ������� � ������� ��������� \"��������\".")]
        public virtual IList<Task> NotOneTimeTasks { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// ���� �������.
    /// </summary>
    /// <remarks>
    /// � ����������� �� ���� ������� ����� �� ������� ����������, ����������� � ���������� ���������. 
    /// </remarks>
    public enum TaskTypes
    {
        /// <summary>
        /// ������� �������.
        /// </summary>
        [Description("������� �������")]
        Classic,

        /// <summary>
        /// ������� � ����������.
        /// </summary>
        [Description("������� � ����������")]
        NeedForSpeed,

        /// <summary>
        /// ������� � ������� �������� ���������.
        /// </summary>
        [Description("������� � ������� �������� ���������")]
        RussianRoulette
    }
}
