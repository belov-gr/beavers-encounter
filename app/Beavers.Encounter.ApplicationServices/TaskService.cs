using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.ApplicationServices
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<Task> taskRepository;
        private readonly IRepository<TeamTaskState> teamTaskStateRepository;
        private readonly IRepository<AcceptedCode> acceptedCodeRepository;
        private readonly IRepository<AcceptedBadCode> acceptedBadCodeRepository;
        private readonly IRepository<AcceptedTip> acceptedTipRepository;
        private readonly IRepository<TeamGameState> teamGameStateRepository;

        public TaskService(
            IRepository<Task> taskRepository,
            IRepository<TeamGameState> teamGameStateRepository,
            IRepository<TeamTaskState> teamTaskStateRepository,
            IRepository<AcceptedCode> acceptedCodeRepository,
            IRepository<AcceptedBadCode> acceptedBadCodeRepository,
            IRepository<AcceptedTip> acceptedTipRepository)
        {
            Check.Require(taskRepository != null, "taskRepository may not be null");
            Check.Require(teamGameStateRepository != null, "teamGameStateRepository may not be null");
            Check.Require(teamTaskStateRepository != null, "teamTaskStateRepository may not be null");
            Check.Require(acceptedCodeRepository != null, "acceptedCodeRepository may not be null");
            Check.Require(acceptedBadCodeRepository != null, "acceptedBadCodeRepository may not be null");
            Check.Require(acceptedTipRepository != null, "acceptedTipRepository may not be null");

            this.taskRepository = taskRepository;
            this.teamGameStateRepository = teamGameStateRepository;
            this.teamTaskStateRepository = teamTaskStateRepository;
            this.acceptedCodeRepository = acceptedCodeRepository;
            this.acceptedBadCodeRepository = acceptedBadCodeRepository;
            this.acceptedTipRepository = acceptedTipRepository;
        }

        public void AssignNewTaskTip(TeamTaskState teamTaskState, Tip tip)
        {
            AcceptedTip acceptedTip = new AcceptedTip
                {
                  AcceptTime = DateTime.Now,
                  Tip = tip,
                  TeamTaskState = teamTaskState
                };

            teamTaskState.AcceptedTips.Add(acceptedTip);

            acceptedTipRepository.SaveOrUpdate(acceptedTip);
            teamTaskStateRepository.SaveOrUpdate(teamTaskState);
        }

        public void AssignNewTask(TeamGameState teamGameState, Task oldTask)
        {
            Check.Require(teamGameState.ActiveTaskState == null, "Невозможно назначить команде новую задачу, т.к. коменде уже назначена задача.");

            // Пытаемся получить следующее задание для команды
            Task newTask = GetNextTaskForTeam(teamGameState, oldTask);
            
            // Если количество полученных заданий равно количеству заданий в игре и нет нового задания,
            // то считаем, что команда завершила игру
            if (newTask == null && teamGameState.AcceptedTasks.Count >= teamGameState.Game.Tasks.Count(x => !x.Locked))
            {
                TeamFinishGame(teamGameState);
                return;
            }

            TeamTaskState teamTaskState = new TeamTaskState {
                    TaskStartTime = DateTime.Now, 
                    TaskFinishTime = null,
                    State = (int) TeamTaskStateFlag.Execute,
                    TeamGameState = teamGameState,
                    Task = newTask,
                    NextTask = null
                };

            teamGameState.ActiveTaskState = teamTaskState;

            teamTaskStateRepository.SaveOrUpdate(teamTaskState);
            teamGameStateRepository.SaveOrUpdate(teamGameState);
            teamGameStateRepository.DbContext.CommitChanges();

            //Сразу же отправляем команде первую подсказку (т.е. текст задания)
            AssignNewTaskTip(teamTaskState, teamTaskState.Task.Tips.First());
        }

        public void CloseTaskForTeam(TeamTaskState teamTaskState, TeamTaskStateFlag flag)
        {
            teamTaskState.TaskFinishTime = DateTime.Now;
            teamTaskState.State = (int) flag;
            teamTaskStateRepository.SaveOrUpdate(teamTaskState);
            
            teamTaskState.TeamGameState.ActiveTaskState = null;
            teamTaskState.TeamGameState.AcceptedTasks.Add(teamTaskState);
            teamGameStateRepository.SaveOrUpdate(teamTaskState.TeamGameState);

            teamGameStateRepository.DbContext.CommitChanges();
        }

        public void TeamFinishGame(TeamGameState teamGameState)
        {
            teamGameState.GameDoneTime = DateTime.Now;
            teamGameState.ActiveTaskState = null;
            teamGameStateRepository.SaveOrUpdate(teamGameState);
            
            //teamGameState.Team.TeamGameState = null;
            //teamRepository.SaveOrUpdate(teamGameState.Team);

            teamGameStateRepository.DbContext.CommitChanges();
        }

        /// <summary>
        /// "Ускориться".
        /// </summary>
        /// <remarks>
        /// Устанавливает время ускорения в текущее и назначает вторую подсказку.
        /// </remarks>
        /// <param name="teamTaskState">Состояние команды затребовавшая ускорение.</param>
        public void AccelerateTask(TeamTaskState teamTaskState)
        {
            teamTaskState.AccelerationTaskStartTime = DateTime.Now;
            AssignNewTaskTip(teamTaskState, teamTaskState.Task.Tips.Last(tip => tip.SuspendTime > 0));
            teamTaskStateRepository.SaveOrUpdate(teamTaskState);
        }

        /// <summary>
        /// Возвращает варианты выбора подсказок, если это необходимо для задания с выбором подсказки.
        /// </summary>
        public IEnumerable<Tip> GetSuggestTips(TeamTaskState teamTaskState)
        {
            // Время от начала задания
            double taskTimeSpend = (DateTime.Now - teamTaskState.TaskStartTime).TotalMinutes;
            double lastAcceptTipTime = (teamTaskState.AcceptedTips.Last().AcceptTime - teamTaskState.TaskStartTime).TotalMinutes;

            // Подсказки, 
            // которые дожны быть выданы на данный момент, 
            // исключая уже выданные
            var tips = new List<Tip>(teamTaskState.Task.Tips
                .Where(tip => tip.SuspendTime > lastAcceptTipTime && tip.SuspendTime <= taskTimeSpend && tip.SuspendTime < teamTaskState.TeamGameState.Game.TimePerTask));

            // Если пришло время предложить коменде выбрать подсказку
            if (tips.Count() > 0)
            {
                // Все подсказки, исключая уже выданные
                return teamTaskState.Task.Tips
                    .Where(tip => tip.SuspendTime > 0)
                    .Except(teamTaskState.AcceptedTips.Tips());
            }
         
            return null;
        }

        private Task GetNextTaskForTeam(TeamGameState teamGameState, Task oldTask)
        {
            // Получаем все незаблокированные задания для текущей игры
            var gameTasks = taskRepository.GetAll()
                .Where(t => t.Game.Id == teamGameState.Game.Id && !t.Locked);

            // Получаем доступные (невыполненные) для команды задания
            List<Task> accessibleTasks = new List<Task>();
            foreach (Task task in gameTasks)
            {
                // Если задание не получено, то добавляем в список
                if (!teamGameState.AcceptedTasks.Any(x => x.Task.Id == task.Id))
                    accessibleTasks.Add(task);
            }

            // Формируем список выполняемых заданий другими командами
            Dictionary<Task, int> executingTasks = new Dictionary<Task, int>();
            foreach (Team team in teamGameState.Game.Teams)
            {
                if (team.TeamGameState != null && team.TeamGameState.ActiveTaskState != null)
                {
                    Task task = team.TeamGameState.ActiveTaskState.Task;
                    if (executingTasks.ContainsKey(task))
                    {
                        executingTasks[task] = executingTasks[task] + 1;
                    }
                    else
                    {
                        executingTasks.Add(task, 1);
                    }
                }
            }

            // Получаем задания выполненные командами, которые помечены опцией "Анти-слив"
            var excludeExecutedTasks = new List<Task>();
            foreach (Team team in teamGameState.Team.PreventTasksAfterTeams)
            {
                foreach (var task in team.TeamGameState.AcceptedTasks)
                {
                    if (!excludeExecutedTasks.Contains(task.Task))
                        excludeExecutedTasks.Add(task.Task);
                }
            }

            List<Task> tasksWithMaxPoints = new List<Task>();
            int maxPoints = 0;

            // Рассчитываем приоритет для каждого задания 
            // и отбираем задания с максимальным приоритетом
            foreach (Task task in accessibleTasks)
            {
                int taskPoints = GetTaskPoints(task, oldTask, executingTasks, excludeExecutedTasks);
                if (taskPoints > maxPoints)
                {
                    maxPoints = taskPoints;
                    tasksWithMaxPoints.Clear();
                    tasksWithMaxPoints.Add(task);
                } 
                else if (taskPoints == maxPoints)
                {
                    tasksWithMaxPoints.Add(task);
                }
            }

            // Если заданий с одинаковым приоритетом несколько, 
            // то берем произвольное
            if (tasksWithMaxPoints.Count > 1)
            {
                // Выбираем новое задание из доступных с максимальным приоритетом
                Task newTask = null;
                Random rnd = new Random();
                int indx = rnd.Next(tasksWithMaxPoints.Count);
                int i = 0;
                foreach (Task task in tasksWithMaxPoints)
                {
                    if (i == indx)
                        newTask = task;
                    i++;
                }

                return newTask;
            }
            return tasksWithMaxPoints.Count == 0 ? null : tasksWithMaxPoints.First();
        }

        /// <summary>
        /// Вычисление приоритета для задания.
        /// </summary>
        /// <param name="task">Задание для которого нужно вычислить приоритет.</param>
        /// <param name="oldTask">Предыдущее задание выполненное командой.</param>
        /// <param name="executingTasks">Задания выполняемые в данных момент другими командами.</param>
        /// <param name="excludeExecutedTasks">Задания выполненные командами, которые помечены опцией "Анти-слив".</param>
        /// <returns>Приоритет задания.</returns>
        private static int GetTaskPoints(Task task, Task oldTask, Dictionary<Task, int> executingTasks, List<Task> excludeExecutedTasks)
        {
            int taskPoints = 1000;
            
            //--------------------------------------------------------------------
            // Если задание типа Челлендж, то +500
            if (task.StreetChallendge)
            {
                taskPoints += 500;
                return taskPoints;
            }

            //--------------------------------------------------------------------
            // Если задание c агентами выполняется другой командой, то -500
            // Задание с агентами одновременно может выполняться только одной командой
            if (task.Agents && executingTasks.ContainsKey(task))
                taskPoints -= 500;

            //--------------------------------------------------------------------
            // Если задание выполнено командами, которые помечены опцией "Анти-слив", то -700
            if (excludeExecutedTasks.Contains(task))
                taskPoints -= 700;

            //--------------------------------------------------------------------
            // Если задание выполняет другая команда, то -50
            if (executingTasks.ContainsKey(task))
                taskPoints -= 50 * executingTasks[task];

            //--------------------------------------------------------------------
            // Если предыдущее задание команды входит в список блокировки по предшествованию, то -400
            if (task.NotAfterTasks.Contains(oldTask))
                taskPoints -= 400;

            //--------------------------------------------------------------------
            // Если хотя бы одно задание из списка блокировки по одновременности выполняется, то -200
            if (task.NotOneTimeTasks.Intersect(executingTasks.Keys).Count() > 0)
                taskPoints -= 200;

            //--------------------------------------------------------------------
            // Если задание содержит коды со сложностью "+500", то +30
            foreach (Code code in task.Codes)
            {
                if (code.Danger == "+500")
                {
                    taskPoints += 30;
                    break;
                }
            }

            //--------------------------------------------------------------------
            // Повышаем приоритет для заданий с бонусами
            // П = П + 10 * (число бонусных кодов в задании)
            int bonusCodes = 0;
            foreach (Code code in task.Codes)
            {
                bonusCodes += code.IsBonus ? 1 : 0;
            }
            taskPoints += bonusCodes * 10;

            //--------------------------------------------------------------------
            // Применяем собственный приоритет задачи
            taskPoints += task.Priority;

            return taskPoints;
        }

        public void SubmitCode(string codes, TeamGameState teamGameState, User user)
        {
            if (teamGameState.ActiveTaskState == null ||
                teamGameState.ActiveTaskState.AcceptedBadCodes.Count >= Game.BadCodesLimit)
                return;

            List<string> codesList = GetCodes(codes, teamGameState.Game.PrefixMainCode, teamGameState.Game.PrefixBonusCode);
            if (codesList.Count == 0)
                return;

            if (codesList.Count > teamGameState.ActiveTaskState.Task.Codes.Count)
                throw new MaxCodesCountException(String.Format("Запрещено вводить количество кодов, за один раз, большее, чем количество кодов в задании."));

            foreach (Code code in teamGameState.ActiveTaskState.Task.Codes)
            {
                if (codesList.Contains(code.Name.Trim().ToUpper()))
                {
                    codesList.Remove(code.Name.Trim().ToUpper());
                    if (!teamGameState.ActiveTaskState.AcceptedCodes.Any(x => x.Code.Id == code.Id))
                    {
                        // Добавляем правильный принятый код
                        AcceptedCode acceptedCode = new AcceptedCode
                        {
                            AcceptTime = DateTime.Now,
                            Code = code,
                            TeamTaskState = teamGameState.ActiveTaskState
                        };
                        
                        teamGameState.ActiveTaskState.AcceptedCodes.Add(acceptedCode);
                        acceptedCodeRepository.SaveOrUpdate(acceptedCode);
                    }
                }
            }

            // Добавляем некорректные принятые коды
            foreach (string badCode in codesList)
            {
                if (!teamGameState.ActiveTaskState.AcceptedBadCodes.Any(x => x.Name.Trim().ToUpper() == badCode))
                {
                    AcceptedBadCode acceptedBadCode = new AcceptedBadCode
                    {
                        AcceptTime = DateTime.Now,
                        Name = badCode,
                        TeamTaskState = teamGameState.ActiveTaskState
                    };

                    teamGameState.ActiveTaskState.AcceptedBadCodes.Add(acceptedBadCode);
                    acceptedBadCodeRepository.SaveOrUpdate(acceptedBadCode);
                }
            }

            teamTaskStateRepository.SaveOrUpdate(teamGameState.ActiveTaskState);
            teamTaskStateRepository.DbContext.CommitChanges();

            // Если приняты все основные коды, то помечаем задание выполненым и назначаем новое
            if (teamGameState.ActiveTaskState.AcceptedCodes.Count == teamGameState.ActiveTaskState.Task.Codes.Count/*(x => x.IsBonus == 0)*/ &&
                teamGameState.ActiveTaskState.AcceptedCodes.Count > 0)
            {
                Task oldTask = teamGameState.ActiveTaskState.Task;
                CloseTaskForTeam(teamGameState.ActiveTaskState, TeamTaskStateFlag.Success);
                AssignNewTask(teamGameState, oldTask);
            }

            CheckExceededBadCodes(teamGameState);
        }

        /// <summary>
        /// Проверка на превышение количества левых кодов. При превышении задание закрывается сразу перед первой подсказкой.
        /// </summary>
        public void CheckExceededBadCodes(TeamGameState teamGameState)
        {
            if (teamGameState == null || teamGameState.ActiveTaskState == null)
                return;

            if ((teamGameState.ActiveTaskState.AcceptedBadCodes.Count >= Game.BadCodesLimit)
                && (((DateTime.Now - teamGameState.ActiveTaskState.TaskStartTime).TotalMinutes + 1) //+1 - чтобы сработало до того, как покажется первая подсказка.
                     >= (teamGameState.ActiveTaskState.Task.Tips.First(x => x.SuspendTime > 0).SuspendTime)))
            {
                Task oldTask = teamGameState.ActiveTaskState.Task;
                CloseTaskForTeam(teamGameState.ActiveTaskState, TeamTaskStateFlag.Cheat);
                AssignNewTask(teamGameState, oldTask);
            }
        }

        public static List<string> GetCodes(string codes, string prefixMainCode, string prefixBonusCode)
        {
            var codesList = new List<string>();

            string[] codeParts = codes.Split(new[] { ',', ' ' });
            foreach (string codePart in codeParts)
            {
                string tmpCode = codePart.Trim().ToUpper();
                if (tmpCode.StartsWith(prefixMainCode.ToUpper()))
                {
                    tmpCode = tmpCode.Substring(prefixMainCode.Length);
                } 
                else if (tmpCode.StartsWith(prefixBonusCode.ToUpper()))
                {
                    tmpCode = tmpCode.Substring(prefixBonusCode.Length);
                }

                if (!String.IsNullOrEmpty(tmpCode))
                    codesList.Add(tmpCode);
            }
            return codesList;
        }
    }

    public class MaxCodesCountException : Exception
    {
        public MaxCodesCountException()
			: base()
		{
		}

        public MaxCodesCountException(string message)
            : base(message)
		{
		}

        protected MaxCodesCountException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

        public MaxCodesCountException(string message, Exception innerException)
            : base(message, innerException)
		{
		}        
    }
}
