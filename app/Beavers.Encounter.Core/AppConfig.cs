using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Core.DomainModel;
using NHibernate.Validator.Constraints;

namespace Beavers.Encounter.Core
{
    public class AppConfig : Entity
    {
        [NotNull, NotEmpty]
        [Meta.Caption("Название сайта")]
        [Meta.Description("Строковое поле. Название сайта выводится в заголовке браузера и на главной странице.")]
        public virtual string Title { get; set; }
    }
}
