using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;

namespace Beavers.Encounter.Core
{
    [Serializable]
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
        [JsonProperty]
        [Meta.Caption("Кодовое название")]
        [Meta.Description("В процессе игры команды не увидят это название, кодовое название задания доступно только авторам игры. Например, Якуники.")]
        public virtual string Name { get; set; }

        [NotNull]
        [JsonProperty]
        [Meta.Caption("Street Challenge")]
        [Meta.Description("Данный признак указывает, что задание будет выдано всем командам как первое задание в начале игры.")]
        public virtual bool StreetChallendge { get; set; }

        [NotNull]
        [JsonProperty]
        [Meta.Caption("Задание с агентами")]
        [Meta.Description("Признак используется при распределении заданий так, чтобы задание с агентами выполнялось единовременно только одной командой. Например, задание с +500.")]
        public virtual bool Agents { get; set; }

        [NotNull]
        [JsonProperty]
        [Meta.Caption("Задание заблокированно")]
        [Meta.Description("Если установлен данный признак, то задание не будет выдаваться командам. Этот признак можно устанавливать/снимать в процессе игры.")]
        public virtual bool Locked { get; set; }

        [NotNull]
        [JsonProperty]
        [Meta.Default(0)]
        [Meta.Caption("Тип задания")]
        [Meta.Description("0 - классическое задание, 1 - задание с ускорением, 2 - задание с выбором подсказки.")]
        public virtual TaskTypes TaskType { get; set; }

        [NotNull]
        [JsonProperty]
        [Meta.Default(0)]
        [Meta.Caption("Приоритет задания")]
        [Meta.Description("Приоритет может быть положительным или отрицательным. Приоритет равный 100 позволяет быстрее получить командам это задание, при этом одновременно данное задание потенциально смогут выполнять 3-4 команды. При приоритете 150 одновременно задание потенциально могут выполнять 4-5 команд. Отрицательный приоритет уменьшает вероятность выдачи задания командам.")]
        public virtual int Priority { get; set; }

        [NotNull]
        [Meta.Default(0)]
        [Meta.Caption("Индекс группы задания")]
        [Meta.Description("Если отличен от нуля, то за всю игру команде будет выдано не более одного задания из всех заданий с таким-же индексом.")]
        public virtual int GroupTag { get; set; }

        public virtual Game Game { get; set; }

        [JsonProperty]
        public virtual IList<Tip> Tips { get; protected set; }

        [JsonProperty]
        public virtual IList<Code> Codes { get; protected set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        [Meta.Caption("Не после")]
        [Meta.Description("Здесь задаются задания, которые не могут предшествовать текущему заданию. Это необходимо для предотвращения дальних (Механа -> Копаево) и коротких (Мельница -> Эпицентр) перегонов команд.")]
        public virtual IList<Task> NotAfterTasks { get; protected set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        [Meta.Caption("Не вместе")]
        [Meta.Description("Если перечисленные здесь задания в данный момент выполняются какими-либо другими командами, то движок предпочтет данное задание временно не выдавать командам. Это необходимо для предотвращения пересечения команд на близко расположенных относительно друг друга локациях. Например, задание \"Мельница\" не должно выдаваться командам, если какая либо команда в данный момент выполняет задание \"Эпицентр\". Т.е. для задания под кодовым названием \"Мельница\" необходимо в списке \"Не вместе\" выбрать задание с кодовым названием \"Эпицентр\".")]
        public virtual IList<Task> NotOneTimeTasks { get; protected set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore, IsReference = true)]
        [Meta.Caption("Не выдавать командам")]
        [Meta.Description("Здесь задаются команды, которые не должны получить данное задание.")]
        public virtual IList<Team> NotForTeams { get; protected set; }

        #region Связка с предшествующим заданием

        [JsonProperty]
        [Meta.Caption("Выдать после задания")]
        [Meta.Description("Текущее задание будет выдано после успешного выполнения указанного задания.")]
        public virtual Task AfterTask { get; set; }

        [JsonProperty]
        [Meta.Caption("Выдать")]
        public virtual GiveTaskAfter GiveTaskAfter { get; set; }

        #endregion Связка с предшествующим заданием

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Виды заданий.
    /// </summary>
    /// <remarks>
    /// В зависимости от типа задания будет по разному задаваться, отображатся и выдаваться подсказки. 
    /// </remarks>
    public enum TaskTypes
    {
        /// <summary>
        /// Обычное задание.
        /// </summary>
        [Description("Обычное задание")]
        Classic,

        /// <summary>
        /// Задание с ускорением.
        /// </summary>
        [Description("Задание с ускорением")]
        NeedForSpeed,

        /// <summary>
        /// Задание с выбором варианта подсказки.
        /// </summary>
        [Description("Задание с выбором варианта подсказки")]
        RussianRoulette
    }

    /// <summary>
    /// Условия выдачи задания после предшествующего, если задана связка с предшествующим заданием.
    /// </summary>
    public enum GiveTaskAfter
    {
        /// <summary>
        /// Если успешно выполнено предшествующее.
        /// </summary>
        [Description("Если успешно выполнено предшествующее")]
        Strictly,

        /// <summary>
        /// В конце игры, если предшествующее не выполнено.
        /// </summary>
        [Description("В конце игры, если предшествующее не выполнено")]
        StrictlyOrFinaly,

        /// <summary>
        /// В любом случае.
        /// </summary>
        [Description("В любом случае")]
        InAnyCase
    }
}
