using System;
using System.Data;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Beavers.Encounter.Core;
using Beavers.Encounter.ApplicationServices.Utils;

namespace Beavers.Encounter.ApplicationServices
{
    public class GameReportService : IGameReportService
    {
        private static DataTable GetReportDataTable(User user)
        {
            if (user.Role.IsAuthor)
            {
                DataTable dt = new DataTable("Report");

                dt.Columns.Add(new DataColumn("Команда", typeof(String)));
                foreach (Task task in user.Game.Tasks)
                {
                    dt.Columns.Add(new DataColumn(task.Name, typeof(String)));
                }

                foreach (Team team in user.Game.Teams)
                {
                    //                               
                    DataRow[] rows = new [] { 
                        dt.NewRow(),    //Получено - <время>
                        dt.NewRow(),    //Подсказки - <время><пробел><время>...<время>
                        dt.NewRow(),    //Коды - список <код>(<время>)<пробел><код>(<время>)...<код>(<время>)
                        dt.NewRow(),    //Потрачено - <всего_минут>
                        dt.NewRow(),    //Итог задания - <итог><пробел>в<пробел><время>
                        dt.NewRow()     //Разделитель - пустая строка.
                    };

                    //[Cтрока(число)][Столбец(0|ИмяЗадания)]
                    rows[0][0] = team.Name;
                    rows[1][0] = ""; //"подсказки";
                    rows[2][0] = ""; //"коды";
                    rows[3][0] = ""; //"длилось";
                    rows[4][0] = ""; //"итог";
                    rows[5][0] = "";

                    if (team.TeamGameState != null)
                    {
                        foreach (TeamTaskState taskState in team.TeamGameState.AcceptedTasks)
                        {
                            //Указываем время начала задания
                            rows[0][taskState.Task.Name] = String.Format("{0}", taskState.TaskStartTime);

                            //Выписываем все подсказки
                            foreach (AcceptedTip tip in taskState.AcceptedTips)
                            {
                                //Стартовая подсказка (SuspendTime = 0) не нужна.
                                if (tip.Tip.SuspendTime > 0)
                                {
                                    rows[1][taskState.Task.Name] = String.Format("{0} {1}", rows[1][taskState.Task.Name], tip.AcceptTime.TimeOfDay);
                                }
                            }

                            //Выписываем все коды
                            foreach (AcceptedCode code in taskState.AcceptedCodes)
                            {
                                rows[2][taskState.Task.Name] = String.Format("{0} {1}({2})", rows[2][taskState.Task.Name], code.Code.Name, code.AcceptTime.TimeOfDay);
                            }

                            //Указываем количество потраченных минут.
                            if (taskState.TaskFinishTime != null)
                            {
                                rows[3][taskState.Task.Name] =
                                    Math.Round(((DateTime)taskState.TaskFinishTime -
                                         taskState.TaskStartTime).TotalMinutes);
                            }

                            //Указываем итог задания.
                            if (taskState.TaskFinishTime != null)
                            {
                                string state = String.Empty;
                                state = taskState.State == (int)TeamTaskStateFlag.Success ? "ОК!" : state;
                                state = taskState.State == (int)TeamTaskStateFlag.Overtime ? "ПРОСР." : state;
                                state = taskState.State == (int)TeamTaskStateFlag.Canceled ? "СЛИТО" : state;
                                state = taskState.State == (int)TeamTaskStateFlag.Cheat ? "БАН!" : state;
                                rows[4][taskState.Task.Name] = String.Format("{0} в {1}", state, ((DateTime)taskState.TaskFinishTime).TimeOfDay);
                            }
                        }
                    }

                    dt.Rows.Add(rows[0]);
                    dt.Rows.Add(rows[1]);
                    dt.Rows.Add(rows[2]);
                    dt.Rows.Add(rows[3]);
                    dt.Rows.Add(rows[4]);
                    dt.Rows.Add(rows[5]);
                }
                dt.AcceptChanges();
                return dt;
            }
            return null;
        }

        public Stream GetReport(User user)
        {
            DataTable dt = GetReportDataTable(user);
            if (dt != null)
            {
                MemoryStream ms = new MemoryStream();
                ZipOutputStream zipOutputStream = new ZipOutputStream(ms);
                ZipEntry zipEntry = new ZipEntry("report.csv");
                zipOutputStream.PutNextEntry(zipEntry);
                zipOutputStream.SetLevel(5);
                string st = CsvWriter.WriteToString(dt, true, false);
                zipOutputStream.Write(Encoding.Default.GetBytes(st), 0, st.Length);
                zipOutputStream.Finish();
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                return ms;
            }
            return null;
        }
    }
}
