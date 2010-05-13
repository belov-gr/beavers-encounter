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
        [Meta.Caption("Код")]
        [Meta.Description("Строковое поле. Значение кода вводится без префикса, только числовая часть. Например, 2748, а не 14DR2748. Команды в процессе игры могут вводить коды как с префиксом так и без префикса. Все коды в рамках игры должны быть уникальны.")]
        public virtual string Name { get; set; }

        [JsonProperty]
        [Meta.Caption("Бонусный код")]
        [Meta.Description("Признак указывает, что код является бонусным и не является обязательным для выполнения задания.")]
        public virtual bool IsBonus { get; set; }

		[NotNull, NotEmpty]
        [JsonProperty]
        [Meta.Caption("Код опасности")]
        [Meta.Description("Строковое поле. Определяет КО кода. Например, 2, +4 или +500.")]
        public virtual string Danger { get; set; }

		[NotNull]
		public virtual Task Task { get; set; }
    }
}
