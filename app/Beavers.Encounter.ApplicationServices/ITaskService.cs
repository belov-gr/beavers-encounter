using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;

namespace Beavers.Encounter.ApplicationServices
{
    public interface ITaskService
    {
        /// <summary>
        /// Обработка принятого кода от команды.
        /// </summary>
        /// <param name="codes">Принятый код.</param>
        /// <param name="teamGameState">Команда отправившая код.</param>
        /// <param name="user">Игрок отправившый код.</param>
        void SubmitCode(string codes, TeamGameState teamGameState, User user);

        /// <summary>
        /// Помечает задание как выполненное, назначает причину завершения.
        /// </summary>
        /// <param name="teamTaskState"></param>
        void CloseTaskForTeam(TeamTaskState teamTaskState, TeamTaskStateFlag flag);

        /// <summary>
        /// Назначение нового задания команде.
        /// </summary>
        void AssignNewTask(TeamGameState teamGameState, Task oldTask);

        /// <summary>
        /// Отправить команде подсказку.
        /// </summary>
        /// <param name="teamTaskState"></param>
        /// <param name="tip"></param>
        void AssignNewTaskTip(TeamTaskState teamTaskState, Tip tip);

        /// <summary>
        /// "Ускориться".
        /// </summary>
        /// <param name="teamTaskState">Состояние команды затребовавшая ускорение.</param>
        void AccelerateTask(TeamTaskState teamTaskState);

        /// <summary>
        /// Возвращает варианты выбора подсказок, если это необходимо для задания с выбором подсказки.
        /// </summary>
        IEnumerable<Tip> GetSuggestTips(TeamTaskState teamTaskState);
    }
}
