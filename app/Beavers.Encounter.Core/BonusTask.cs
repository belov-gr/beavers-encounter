using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    public class BonusTask : Entity
    {
		[DomainSignature]
		[NotNull, NotEmpty]
        [Meta.Caption("Кодовое название")]
        [Meta.Description("В процессе игры команды не увидят это название, кодовое название задания доступно только авторам игры. Например, Тыква.")]
        public virtual string Name { get; set; }

		[NotNull, NotEmpty]
        [Meta.Caption("Формулировка задания")]
        [Meta.TextArea(80,10)]
        public virtual string TaskText { get; set; }

        [NotNull]
        [Meta.Caption("Время выдачи")]
        [Meta.Description("Формат ввода ДД.ММ.ГГ ЧЧ:ММ. Здесь задается время, в которое все команды получат данное задание.")]
        public virtual DateTime StartTime { get; set; }

        [NotNull]
        [Meta.Caption("Время окончания")]
        [Meta.Description("Формат ввода ДД.ММ.ГГ ЧЧ:ММ. Здесь задается время, до которого задание будет действительным.")]
        public virtual DateTime FinishTime { get; set; }

		public virtual Game Game { get; set; }

        [NotNull]
        [Meta.Caption("Индивидуальное задание")]
        [Meta.Description("Если установлен данных признак, то задание будет индивидуальным для каждой команды. Текст задания для каждой команды задается в свойстве \"Индивидуальное задание\" на странице команды.")]
        public virtual bool IsIndividual { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
