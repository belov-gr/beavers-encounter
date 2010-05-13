using System;
using Newtonsoft.Json;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    [Serializable]
    public class Tip : Entity
    {
        [DomainSignature]
		[NotNull, NotEmpty]
        [JsonProperty]
        [Meta.Caption("Текст задания/подсказки")]
        [Meta.TextArea(80, 10)]
        public virtual string Name { get; set; }

		[NotNull]
        [JsonProperty]
        [Meta.Caption("Время")]
        public virtual int SuspendTime { get; set; }

		[NotNull]
		public virtual Task Task { get; set; }
    }
}
