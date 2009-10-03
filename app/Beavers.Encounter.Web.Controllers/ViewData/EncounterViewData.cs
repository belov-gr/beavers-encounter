using Beavers.Encounter.Common.ViewData;
using Beavers.Encounter.Core;
using System.Collections.Generic;

namespace Beavers.Encounter.Web.Controllers.ViewData
{
    public class EncounterViewData : ViewDataBase
    {
        public Role Role { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }

        public Team Team { get; set; }
        public IEnumerable<Team> Teams { get; set; }

        public Game Game { get; set; }
        public IEnumerable<Game> Games { get; set; }

        public Task Task { get; set; }
        public IEnumerable<Task> Tasks { get; set; }

        // attempt at a fluent interface

        public EncounterViewData WithRole(Role role)
        {
            Role = role;
            return this;
        }

        public EncounterViewData WithRoles(IEnumerable<Role> roles)
        {
            Roles = roles;
            return this;
        }

        public EncounterViewData WithUser(User user)
        {
            User = user;
            return this;
        }

        public EncounterViewData WithUsers(IEnumerable<User> users)
        {
            Users = users;
            return this;
        }

        public EncounterViewData WithTeam(Team team)
        {
            Team = team;
            return this;
        }

        public EncounterViewData WithTeams(IEnumerable<Team> teams)
        {
            Teams = teams;
            return this;
        }

        public EncounterViewData WithGame(Game game)
        {
            Game = game;
            return this;
        }

        public EncounterViewData WithGames(IEnumerable<Game> games)
        {
            Games = games;
            return this;
        }

        public EncounterViewData WithTask(Task task)
        {
            Task = task;
            return this;
        }

        public EncounterViewData WithTasks(IEnumerable<Task> tasks)
        {
            Tasks = tasks;
            return this;
        }
    }

    /// <summary>
    /// So you can write 
    /// ShopView.Data.WithProducts(myProducts);
    /// </summary>
    public class EncounterView
    {
        public static EncounterViewData Data { get { return new EncounterViewData(); } }
    }
}
