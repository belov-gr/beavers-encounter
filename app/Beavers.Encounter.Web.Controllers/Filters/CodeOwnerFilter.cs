using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class CodeOwnerAttribute : FilterUsingAttribute
    {
        public CodeOwnerAttribute()
            : base(typeof(CodeOwnerFilter))
        {
            Order = 1;
        }
    }

    public class CodeOwnerFilter : EntityOwnerFilter<Code, CodesController>
    {
        public CodeOwnerFilter(IRepository<Code> repository)
            : base(repository)
        {
        }

        protected override string GetEntityIdSpecificName()
        {
            return "codeId";
        }

        protected override int GetGameId(Code entity)
        {
            return entity.Task.Game.Id;
        }
    }
}
