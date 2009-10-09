using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Beavers.Encounter.Core;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Binders
{
    public class TeamBinderAttribute : CustomModelBinderAttribute
    {
        public TeamBinderAttribute()
        {
            Fetch = true;
        }

        public override IModelBinder GetBinder()
        {
            return new TeamBinder(
                ServiceLocator.Current.GetInstance(typeof(IRepository<Team>)) as IRepository<Team>,
                Fetch);
        }

        public bool Fetch { get; set; }
    }

    public class TeamBinder : IModelBinder
    {
        private readonly bool fetch;
        private readonly IRepository<Team> teamRepository;

        public TeamBinder(IRepository<Team> teamRepository, bool fetch)
        {
            Check.Require(teamRepository != null, "teamRepository may not be null");

            this.teamRepository = teamRepository;
            this.fetch = fetch;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection values = controllerContext.HttpContext.Request.Form;
            Team team = fetch ? teamRepository.Get(Convert.ToInt32(values["Team.Id"])) : new Team();

            team.Name = values["Team.Name"];
            team.AccessKey = values["Team.AccessKey"];

            // ----------------------------------
            // Не после команды
            var forRemove = new List<Team>();

            for (int i = 0; i < 5; i++)
            {
                if (values.AllKeys.Contains("Team.PreventTasksAfterTeams" + i))
                {
                    int notAfterTaskId = Convert.ToInt32(values["Team.PreventTasksAfterTeams" + i]);
                    if (notAfterTaskId != 0)
                    {
                        var tm = teamRepository.Get(notAfterTaskId);
                        if (!team.PreventTasksAfterTeams.Contains(tm))
                            team.PreventTasksAfterTeams.Add(tm);
                    }
                    else
                    {
                        try
                        {
                            forRemove.Add(team.PreventTasksAfterTeams[i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                }
            }

            foreach (var item in forRemove)
            {
                team.PreventTasksAfterTeams.Remove(item);
            }

            if (!fetch)
            {
                //task.Game = taskRepository.Get(Convert.ToInt32(values["taskId"]));
            }
            return team;
        }
    }
}
