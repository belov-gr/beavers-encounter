using System.Collections.Generic;
using Newtonsoft.Json;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;

namespace Beavers.Encounter.Core
{
    [Serializable]
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
        [JsonProperty]
        [Meta.Caption("Название")]
        [Meta.Description("Название игры")]
        public virtual string Name { get; set; }

        //[JsonProperty]
        [Meta.Caption("Дата проведения")]
        [Meta.Description("Формат ввода ДД.ММ.ГГ ЧЧ:ММ.")]
        public virtual DateTime GameDate { get; set; }

		[NotNull, NotEmpty]
        [JsonProperty]
        [Meta.Caption("Описание")]
        [Meta.TextArea(40, 3)]
        public virtual string Description { get; set; }

        [JsonProperty]
        [Meta.Default(540)]
        [Meta.Caption("Продолжительность игры")]
        [Meta.Description("Значение в минутах. Например, 540 - т.е. 9 часов.")]
        public virtual int TotalTime { get; set; }

        [JsonProperty]
        [Meta.Default(90)]
        [Meta.Caption("Время на задание")]
        [Meta.Description("Значение в минутах. Например, 90 - т.е. 1.5 часа.")]
        public virtual int TimePerTask { get; set; }

        [JsonProperty]
        [Meta.Default(30)]
        [Meta.Caption("Время на подсказку")]
        [Meta.Description("Значение в минутах. Например, 30 - т.е. пол часа.")]
        public virtual int TimePerTip { get; set; }

        //[JsonProperty]
        public virtual GameStates GameState { get; set; }

        [Length(6)]
        [JsonProperty]
        [Meta.Caption("Префикс для основного кода")]
        [Meta.Description("Например, 14DR.")]
        public virtual string PrefixMainCode { get; set; }

        [Length(6)]
        [JsonProperty]
        [Meta.Caption("Префикс для бонусного кода")]
        [Meta.Description("Например, 14B.")]
        public virtual string PrefixBonusCode { get; set; }

        public virtual IList<Team> Teams { get; protected set; }

        public virtual IList<Task> Tasks { get; protected set; }

        public virtual IList<BonusTask> BonusTasks { get; protected set; }

        public override string ToString()
        {
            return Name;
        }

        public virtual void SetId(int id)
        {
            Id = id;
        }
    }

    public static class GameConsnt
    {
        public static int BadCodesLimit = 10;
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
