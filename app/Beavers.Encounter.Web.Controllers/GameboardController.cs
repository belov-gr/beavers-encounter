using System.Web.Mvc;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Web.Controllers.Binders;
using Beavers.Encounter.Web.Controllers.Filters;
using Coolite.Ext.Web;
using SharpArch.Core.PersistenceSupport;
using System.Collections.Generic;
using SharpArch.Testing;
using SharpArch.Web.NHibernate;
using SharpArch.Core;
using Beavers.Encounter.ApplicationServices;
using System;
using Task = Beavers.Encounter.Core.Task;
using Tip = Beavers.Encounter.Core.Tip;
using Newtonsoft.Json;
using System.Linq;


namespace Beavers.Encounter.Web.Controllers
{
    [GameState]
    [LockIfGameStart]
    [HandleError]
    public class GameboardController : BaseController
    {
        private readonly IRepository<Game> gameRepository;
        private readonly IRepository<Task> taskRepository;
        private readonly IRepository<Tip> tipRepository;
        private readonly IRepository<Code> codeRepository;
        private readonly IRepository<BonusTask> bonusTaskRepository;
        private readonly IGameService gameService;

        public GameboardController(
            IRepository<Game> gameRepository, 
            IRepository<Task> taskRepository, 
            IRepository<Tip> tipRepository, 
            IRepository<Code> codeRepository, 
            IRepository<BonusTask> bonusTaskRepository,
            IUserRepository userRepository, 
            IGameService gameService)
            : base(userRepository)
        {
            Check.Require(gameRepository != null, "gameRepository may not be null");
            Check.Require(taskRepository != null, "taskRepository may not be null");
            Check.Require(tipRepository != null, "tipRepository may not be null");
            Check.Require(codeRepository != null, "codeRepository may not be null");
            Check.Require(bonusTaskRepository != null, "bonusTaskRepository may not be null");
            Check.Require(gameService != null, "gameService may not be null");

            this.gameRepository = gameRepository;
            this.taskRepository = taskRepository;
            this.tipRepository = tipRepository;
            this.codeRepository = codeRepository;
            this.bonusTaskRepository = bonusTaskRepository;
            this.gameService = gameService;
        }

        [GameboardOwner]
        public ActionResult Show(int id)
        {
            Game game = gameRepository.Get(id);

            return View(game);
        }

        [AjaxMethod]
        [AcceptVerbs(HttpVerbs.Post)]
        [GameboardOwner]
        public string NodeLoad(string gameId, string nodeId)
        {
            TreeNodeCollection nodes = new TreeNodeCollection();
            if (!string.IsNullOrEmpty(nodeId))
            {
                if (nodeId == "tasks")
                {
                    Game game = gameRepository.Get(Convert.ToInt32(gameId));
                    foreach(Task task in game.Tasks)
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = task.Name;
                        treeNode.NodeID = task.Id.ToString();
                        treeNode.Leaf = true;
                        nodes.Add(treeNode);
                    }
                }
                else if (nodeId == "bonuses")
                {
                    Game game = gameRepository.Get(Convert.ToInt32(gameId));
                    foreach (BonusTask task in game.BonusTasks)
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = task.Name;
                        treeNode.NodeID = task.Id.ToString();
                        treeNode.Leaf = true;
                        nodes.Add(treeNode);
                    }
                }
                else if (nodeId == "teams") 
                {
                    Game game = gameRepository.Get(Convert.ToInt32(gameId));
                    foreach (Team team in game.Teams)
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = team.Name;
                        treeNode.NodeID = team.Id.ToString();
                        treeNode.Leaf = true;
                        nodes.Add(treeNode);
                    }
                }
            }
            string result = nodes.ToJson();
            return result;
        }

        #region Игра

        [GameboardOwner]
        public AjaxStoreResult GetGame(int id)
        {
            Game game = gameRepository.Get(id);
            return new AjaxStoreResult(new List<Game> { FixGameNHProxyForJson(game) }, 1);
        }

        /// <summary>
        /// Создает новый класс Game, взамен созданного прокси класса hnibernat-ом.
        /// Пока по невыясненным причинам Newtonsoft.Json отказывается сериализовать 
        /// прокси класс созданный hnibernatе. Причем клас Task сериализуется без проблем.
        /// </summary>
        private Game FixGameNHProxyForJson(Game game)
        {
            Game bagGame = new Game
            {
               Name = game.Name,
               Description = game.Description,
               GameDate = game.GameDate,
               TotalTime = game.TotalTime,
               TimePerTask = game.TimePerTask,
               TimePerTip = game.TimePerTip,
               PrefixMainCode = game.PrefixMainCode,
               PrefixBonusCode = game.PrefixBonusCode,
               GameState = game.GameState

            };

            bagGame.SetIdTo(game.Id);
            return bagGame;
        }

        //[ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        [GameboardOwner]
        public AjaxFormResult SaveGame(Game game)
        {
            var response = new AjaxFormResult();
            try
            {
                Game gameToUpdate = gameRepository.Get(game.Id);
                TransferGameFormValuesTo(gameToUpdate, game);

                if (ViewData.ModelState.IsValid && game.IsValid())
                {
                    gameRepository.SaveOrUpdate(gameToUpdate);
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Errors.Add(new FieldError("Game_ID", "The ID field is required"));
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ExtraParams["msg"] = e.ToString();
            }
            return response;
        }

        private void TransferGameFormValuesTo(Game gameToUpdate, Game gameFromForm)
        {
            gameToUpdate.Name = gameFromForm.Name;
            gameToUpdate.GameDate = gameFromForm.GameDate;
            gameToUpdate.Description = gameFromForm.Description;
            gameToUpdate.TotalTime = gameFromForm.TotalTime;
            gameToUpdate.TimePerTask = gameFromForm.TimePerTask;
            gameToUpdate.TimePerTip = gameFromForm.TimePerTip;
            gameToUpdate.PrefixMainCode = gameFromForm.PrefixMainCode;
            gameToUpdate.PrefixBonusCode = gameFromForm.PrefixBonusCode;
        }

        #endregion Игра

        #region Задания

        [GameboardOwner]
        public AjaxStoreResult GetTask(int gameId, int taskId)
        {
            Task task = taskRepository.Get(taskId);
            return new AjaxStoreResult(new List<Task> { task }, 1);
        }

        public AjaxStoreResult CreateTask()
        {
            Task task = new Task();
            Tip tip = new Tip { Name = "Здесь должен быть текст задания...", SuspendTime = 0, Task = task };
            task.Tips.Add(tip);
            return new AjaxStoreResult(new List<Task> { task }, 1);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public AjaxFormResult SaveTask([TaskBinder(Fetch = true)] Task task)
        {
            var response = new AjaxFormResult();
            try
            {
                if (ViewData.ModelState.IsValid && task.IsValid())
                {
                    if (task.Id == 0)
                    {
                        task.Game = User.Game;
                        Tip tip = new Tip { Name = "Здесь должен быть текст задания...", SuspendTime = 0, Task = task };
                        task.Tips.Add(tip);
                        taskRepository.SaveOrUpdate(task);
                        tipRepository.SaveOrUpdate(tip);

                        response.ExtraParams["newID"] = task.Id.ToString();
                        response.ExtraParams["name"] = task.Name;
                    }
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Errors.Add(new FieldError("Task_ID", "The CustomerID field is required"));

                    taskRepository.DbContext.RollbackTransaction();
                }
            }
            catch(Exception e)
            {
                response.Success = false;
                response.ExtraParams["msg"] = e.ToString();
            }
            return response;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public AjaxResult DeleteTask(int id)
        {
            var response = new AjaxResult();
            try
            {
                Task task = taskRepository.Get(id);
                response.ExtraParamsResponse.Add(new Parameter("id", task.Id.ToString()));
                taskRepository.Delete(task);
            }
            catch(Exception e)
            {
                response.ErrorMessage = e.Message;
            }
            return response;
        }

        #endregion Задания
        
        #region Подсказки

        public AjaxStoreResult CreateTip(int taskId)
        {
            Tip tip = new Tip() { Name = "Новая подсказка" };
            Task task = taskRepository.Get(taskId);
            tip.SuspendTime = task.Tips.Count * task.Game.TimePerTip;
            return new AjaxStoreResult(new List<Tip> { tip }, 1);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public AjaxFormResult SaveTip([TipBinder(Fetch = true)] Tip tip)
        {
            var response = new AjaxFormResult();
            try
            {
                if (ViewData.ModelState.IsValid && tip.IsValid())
                {
                    if (tip.Id == 0)
                    {
                        tipRepository.SaveOrUpdate(tip);

                        response.ExtraParams["newID"] = tip.Id.ToString();
                        response.ExtraParams["name"] = tip.Name;
                    }
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Errors.Add(new FieldError("Tip_Id", "The TipID field is required"));

                    tipRepository.DbContext.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ExtraParams["msg"] = e.ToString();
            }
            return response;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public AjaxResult DeleteTip(int id)
        {
            try
            {
                Tip tip = tipRepository.Get(id);
                tipRepository.Delete(tip);
                return new AjaxResult();
            }
            catch (Exception e)
            {
                return new AjaxResult { ErrorMessage = e.Message };
            }
        }

        #endregion Подсказки

        #region Коды

        //[ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveCode(int taskId)
        {
            AjaxStoreResult ajaxStoreResult = new AjaxStoreResult(StoreResponseFormat.Save);
            try
            {
                StoreDataHandler dataHandler = new StoreDataHandler(HttpContext.Request["data"]);
                ChangeRecords<Code> obj = dataHandler.ObjectData<Code>();

                Task task = taskRepository.Get(taskId);
                foreach (Code code in obj.Updated)
                {
                    code.Task = task;
                    if (code.IsValid())
                    {
                        codeRepository.SaveOrUpdate(code);
                    }
                }

                foreach (Code code in obj.Created)
                {
                    code.Task = task;
                    if (code.IsValid())
                    {
                        codeRepository.SaveOrUpdate(code);
                    }
                }

                foreach (Code code in obj.Deleted)
                {
                    code.Task = task;
                    if (code.IsValid())
                    {
                        codeRepository.Delete(code);
                    }
                }
                codeRepository.DbContext.CommitChanges();
                ajaxStoreResult.SaveResponse.Success = true;
            }
            catch (Exception e)
            {
                ajaxStoreResult.SaveResponse.Success = false;
                ajaxStoreResult.SaveResponse.ErrorMessage = e.Message;
                codeRepository.DbContext.RollbackTransaction();
            }
            return ajaxStoreResult;
        }

        #endregion Коды

        #region Бонусные задания

        [GameboardOwner]
        public AjaxStoreResult GetBonus(int gameId, int bonusId)
        {
            BonusTask bonusTask = bonusTaskRepository.Get(bonusId);
            return new AjaxStoreResult(new List<BonusTask> { bonusTask }, 1);
        }

        public AjaxStoreResult CreateBonus()
        {
            BonusTask task = new BonusTask();
            if (User != null)
            {
                task.StartTime = User.Game.GameDate.AddMinutes(60);
                task.FinishTime = User.Game.GameDate.AddMinutes(User.Game.TotalTime);
            }
            return new AjaxStoreResult(new List<BonusTask> { task }, 1);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public AjaxFormResult SaveBonus([BonusTaskBinder(Fetch = true)] BonusTask bonusTask)
        {
            var response = new AjaxFormResult();
            try
            {
                if (ViewData.ModelState.IsValid && bonusTask.IsValid())
                {
                    if (bonusTask.Id == 0)
                    {
                        bonusTask.Game = User.Game;
                        bonusTaskRepository.SaveOrUpdate(bonusTask);

                        response.ExtraParams["newID"] = bonusTask.Id.ToString();
                        response.ExtraParams["name"] = bonusTask.Name;
                    }
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Errors.Add(new FieldError("BonusTask_ID", "The ID field is required"));

                    taskRepository.DbContext.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ExtraParams["msg"] = e.ToString();
            }
            return response;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public AjaxResult DeleteBonus(int id)
        {
            var response = new AjaxResult();
            try
            {
                BonusTask bonusTask = bonusTaskRepository.Get(id);
                response.ExtraParamsResponse.Add(new Parameter("id", bonusTask.Id.ToString()));
                bonusTaskRepository.Delete(bonusTask);
            }
            catch (Exception e)
            {
                response.ErrorMessage = e.Message;
            }
            return response;
        }

        #endregion Бонусные задания

        #region Управление игрой

        [GameboardOwner]
        public AjaxStoreResult GetTeamsState(int id)
        {
            var list = new List<TeamsState>();

            var game = gameRepository.Get(id);
            foreach (Team team in game.Teams)
            {
                if (team.TeamGameState == null)
                    continue;

                var teamState = new TeamsState();
                teamState.Id = team.Id;
                teamState.Name = team.Name;
                if (team.TeamGameState.ActiveTaskState != null)
                {
                    teamState.Task = team.TeamGameState.ActiveTaskState.Task.Name;
                    teamState.Time = DateTime.Now - team.TeamGameState.ActiveTaskState.TaskStartTime;

                    var acceptedTasks = team.TeamGameState.AcceptedTasks;
                    teamState.Accpt = acceptedTasks.Count;
                    teamState.Success = acceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Success);
                    teamState.Overtime = acceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Overtime);
                    teamState.Canceled = acceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Canceled);
                    teamState.Cheat = acceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Cheat);

                    teamState.Tips = team.TeamGameState.ActiveTaskState.AcceptedTips.Count - 1;
                    teamState.CodesMainCount = team.TeamGameState.ActiveTaskState.Task.Codes.Count(c => c.IsBonus == false);
                    teamState.CodesBonusCount = team.TeamGameState.ActiveTaskState.Task.Codes.Count(c => c.IsBonus == true);
                    teamState.CodesAccpt = team.TeamGameState.ActiveTaskState.AcceptedCodes.Count(c => c.Code.IsBonus == false);
                    teamState.CodesBonusAccpt = team.TeamGameState.ActiveTaskState.AcceptedCodes.Count(c => c.Code.IsBonus == true);
                }
                list.Add(teamState);
            }

            return new AjaxStoreResult(list, list.Count);    
        }

        [AjaxMethod]
        [GameboardOwner]
        [AcceptVerbs(HttpVerbs.Post)]
        public AjaxResult GetGameState(int id)
        {
            return new AjaxResult(gameRepository.Get(id).GameState);
        }

        [AjaxMethod]
        [AcceptVerbs(HttpVerbs.Post)]
        [GameboardOwner]
        [Transaction]
        public AjaxResult SetGameState(int id, string state)
        {
            try
            {
                var game = gameRepository.Get(id);
                switch (state)
                {
                    case "Planned":
                        gameService.ResetGame(game);
                        break;
                    case "Startup":
                        gameService.StartupGame(game);
                        break;
                    case "Started":
                        gameService.StartGame(game);
                        break;
                    case "Finished":
                        gameService.StopGame(game);
                        break;
                    case "Cloused":
                        gameService.CloseGame(game);
                        break;
                }
                return new AjaxResult { Result = "OK" };
            }
            catch(Exception e)
            {
                return new AjaxResult { ErrorMessage = e.Message };
            }
        }

        #endregion Управление игрой
    }

    [Serializable]
    public class TeamsState
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }
        
        [JsonProperty]
        public string Task { get; set; }
        
        [JsonProperty]
        public TimeSpan Time { get; set; }
        
        [JsonProperty]
        public int Accpt { get; set; }

        [JsonProperty]
        public int Success { get; set; }

        [JsonProperty]
        public int Overtime { get; set; }

        [JsonProperty]
        public int Canceled { get; set; }

        [JsonProperty]
        public int Cheat { get; set; }

        [JsonProperty]
        public int Tips { get; set; }

        [JsonProperty]
        public int CodesMainCount { get; set; }

        [JsonProperty]
        public int CodesBonusCount { get; set; }

        [JsonProperty]
        public int CodesAccpt { get; set; }

        [JsonProperty]
        public int CodesBonusAccpt { get; set; }
    }
}
