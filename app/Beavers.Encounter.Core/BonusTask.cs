using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    public class BonusTask : Entity
    {
		[DomainSignature]
		[NotNull, NotEmpty]
        [Meta.Caption("������� ��������")]
        [Meta.Description("� �������� ���� ������� �� ������ ��� ��������, ������� �������� ������� �������� ������ ������� ����. ��������, �����.")]
        public virtual string Name { get; set; }

		[NotNull, NotEmpty]
        [Meta.Caption("������������ �������")]
        [Meta.TextArea(80,10)]
        public virtual string TaskText { get; set; }

        [NotNull]
        [Meta.Caption("����� ������")]
        [Meta.Description("������ ����� ��.��.�� ��:��. ����� �������� �����, � ������� ��� ������� ������� ������ �������.")]
        public virtual DateTime StartTime { get; set; }

        [NotNull]
        [Meta.Caption("����� ���������")]
        [Meta.Description("������ ����� ��.��.�� ��:��. ����� �������� �����, �� �������� ������� ����� ��������������.")]
        public virtual DateTime FinishTime { get; set; }

		public virtual Game Game { get; set; }

        [NotNull]
        [Meta.Caption("�������������� �������")]
        [Meta.Description("���� ���������� ������ �������, �� ������� ����� �������������� ��� ������ �������. ����� ������� ��� ������ ������� �������� � �������� \"�������������� �������\" �� �������� �������.")]
        public virtual bool IsIndividual { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
