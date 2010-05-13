using System;
using Newtonsoft.Json;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    [Serializable]
    public class Code : Entity
    {
		[DomainSignature]
		[NotNull, NotEmpty]
        [JsonProperty]
        [Meta.Caption("���")]
        [Meta.Description("��������� ����. �������� ���� �������� ��� ��������, ������ �������� �����. ��������, 2748, � �� 14DR2748. ������� � �������� ���� ����� ������� ���� ��� � ��������� ��� � ��� ��������. ��� ���� � ������ ���� ������ ���� ���������.")]
        public virtual string Name { get; set; }

        [JsonProperty]
        [Meta.Caption("�������� ���")]
        [Meta.Description("������� ���������, ��� ��� �������� �������� � �� �������� ������������ ��� ���������� �������.")]
        public virtual bool IsBonus { get; set; }

		[NotNull, NotEmpty]
        [JsonProperty]
        [Meta.Caption("��� ���������")]
        [Meta.Description("��������� ����. ���������� �� ����. ��������, 2, +4 ��� +500.")]
        public virtual string Danger { get; set; }

		[NotNull]
		public virtual Task Task { get; set; }
    }
}
