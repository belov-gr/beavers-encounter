using System.Collections.Generic;
using System.Data;
using System.IO;
using Beavers.Encounter.Core;

namespace Beavers.Encounter.ApplicationServices
{
    /// <summary>
    /// Фвсад сервисов управления процессом игры.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Подготавливает игру к старту.
        /// </summary>
        void StartupGame(Game game);

        /// <summary>
        /// Запуск игры.
        /// </summary>
        void StartGame(Game game);

        /// <summary>
        /// Завершить игру. 
        /// </summary>
        void StopGame(Game game);

        /// <summary>
        /// Закрыть игру.
        /// </summary>
        void CloseGame(Game game);

        /// <summary>
        /// Сброс состояния игры.
        /// Переводит игру в начальное состояние, 
        /// удаляет состояния команд.
        /// </summary>
        /// <param name="game"></param>
        void ResetGame(Game game);

        /// <summary>
        /// Обработка принятого кода от команды.
        /// </summary>
        /// <param name="codes">Принятый код.</param>
        /// <param name="teamGameState">Команда отправившая код.</param>
        /// <param name="user">Игрок отправившый код.</param>
        void SubmitCode(string codes, TeamGameState teamGameState, User user);

        /// <summary>
        /// Помечает задание как успешно выполненное.
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
        void AssignNewTaskTip(TeamTaskState teamTaskState, Tip tip);

        /// <summary>
        /// "Ускориться".
        /// </summary>
        /// <param name="teamTaskState">Состояние команды затребовавшая ускорение.</param>
        void AccelerateTask(TeamTaskState teamTaskState);

        /// <summary>
        /// Проверка на превышение количества левых кодов. При превышении задание закрывается сразу перед первой подсказкой.
        /// </summary>
        void CheckExceededBadCodes(TeamGameState teamGameState);

        /// <summary>
        /// Возвращает варианты выбора подсказок, если это необходимо для задания с выбором подсказки.
        /// </summary>
        IEnumerable<Tip> GetSuggestTips(TeamTaskState teamTaskState);

        DataTable GetGameResults(int gameId);

        void Do(int gameId);

        Stream GetReport(User user);
    }
}
