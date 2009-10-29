<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<TeamGameboardController.TeamGameboardViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Common"%>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers"%>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%@ Import Namespace="Beavers.Encounter.Core.DataInterfaces"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="<%= ResolveUrl("~") %>Content/Site.css" rel="stylesheet" type="text/css" />
    <title>Game board</title>
</head>
<body>
  <div class="teamGameboardPage">
    <h1><%= Html.Encode(ViewData.Model.TeamName) %></h1>
    
    <div><%= Html.Encode(Model.Message) %></div>
    
    <% if (ViewData.Model.TeamGameState != null && ViewData.Model.ActiveTaskState != null) { %>
    
        <h2>Задание №: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count %></h2>

        <ul>
        <% 
        // Выводим текст полученного задания и подсказок
        int indx = 0;
        foreach (AcceptedTip tip in ViewData.Model.ActiveTaskState.AcceptedTips)
        { %>
            <li>
            <div style="font-weight:bold">
                <%= tip.Tip.SuspendTime == 0 ? "Задание" : String.Format("Подсказка №{0}", indx)%> 
                <span class="note">[Получено: <%= tip.AcceptTime %>]</span>
            </div>
            <div>
            <%= BBCode.ConvertToHtml(tip.Tip.Name)%>
            </div>
            </li>
            <p />
            <p />
        <%
            indx++;
        } %>
        </ul>

        <%
        // Кнопка для "ускорения" задания. Доступна только для капитана команды.
        if (((User)User).Role.IsTeamLeader && 
            Model.TeamGameState.ActiveTaskState.Task.TaskType == (int)TaskTypes.NeedForSpeed &&
            Model.TeamGameState.ActiveTaskState.AccelerationTaskStartTime == null) 
        { %>
            <p/>
            <% 
            using (Html.BeginForm<TeamGameboardController>(c => c.AccelerateTask(Model.ActiveTaskState.Id), FormMethod.Post)) 
            { %>
                <%= Html.AntiForgeryToken() %>
                <div style="color:Yellow">
                Внимание! После нажатия кнопки, на выполнение задания останется <%= Model.TeamGameState.Game.TimePerTask - Model.ActiveTaskState.Task.Tips.Last(tip => tip.SuspendTime > 0).SuspendTime%> минут. Вы уверены, что всё готово?
                </div>
                <div>
                    <%= Html.SubmitButton("btnAccelerateTask", "Всё готово!", new Dictionary<string, object> { { "onclick", "return confirm('Are you sure?');" } })%>
                </div>
            <% 
            } 
        } %>


        <%
        // Кнопки для выбора подсказки.
        if (Model.SuggestTips != null) 
        { %>
            <p/>
            <div style="color:Yellow"><%= Model.SuggestMessage %></div>
            <%
            foreach(Tip tip in Model.SuggestTips)
            { %>
                <span>
                <%
                string colorText = String.Empty;
                int tipPos = Model.ActiveTaskState.Task.Tips.TipPosition(tip);
                switch (tipPos)
                {
                    case 1: { colorText = "50x50"; break; }
                    case 2: { colorText = "Звонок другу"; break; }
                    case 3: { colorText = "Подсказка зала"; break; }
                }
                
                using (Html.BeginForm<TeamGameboardController>(c => c.SelectTip(Model.ActiveTaskState.Id, tip.Id), FormMethod.Post)) 
                { %>
                    <%= Html.AntiForgeryToken() %>
                    <%= Html.SubmitButton(String.Format("btnSelectTip{0}", tipPos), colorText)%>
                <% 
                } %> 
                </span>
            <%
            } %> 
            
        <% 
        } %>


        <p> КО: 
        <% 
        
        // Выводим коды опасности основных кодов
        foreach (Code code in ViewData.Model.ActiveTaskState.Task.Codes)
        { 
            if (code.IsBonus == 0)
            { %>
            <span class="<%= ViewData.Model.ActiveTaskState.AcceptedCodes.Any(x => x.Code.Id == code.Id) ? "acceptedCode" : "notAcceptedCode" %>">
                <%= code.Danger %>
            </span>
            <% 
            }
        }

        // Выводим коды опасности бонусных кодов, если конечно они есть в задании
        if (ViewData.Model.ActiveTaskState.Task.Codes.Any(x => x.IsBonus == 1))
        { %>
        (Б: 
            <% 
            foreach (Code code in ViewData.Model.ActiveTaskState.Task.Codes)
            { 
                if (code.IsBonus == 1)
                { %>
            <span class="<%= ViewData.Model.ActiveTaskState.AcceptedCodes.Any(x => x.Code.Id == code.Id) ? "acceptedBonusCode" : "notAcceptedCode" %>">
                <%= code.Danger %>
            </span>
                <% 
                } %>       
            <% 
            } %>       
        )
        <% 
        } %>       
        </p>

        <% 
        // Выводим принятые коды 
        if (ViewData.Model.ActiveTaskState.AcceptedCodes.Count > 0)
        { %>
            <p> Принятые коды:  
            <% 
            foreach (AcceptedCode acceptedCode in ViewData.Model.ActiveTaskState.AcceptedCodes)
            { %>
                <span class="<%= acceptedCode.Code.IsBonus == 1 ? "acceptedBonusCode" : "acceptedCode" %>">
                    <%= acceptedCode.Code.Name %>
                </span>
            <% 
            } %>       
            </p>
        <% 
        } %>       
<!--
        <% 
        // Выводим некорректные принятые коды 
        if (ViewData.Model.ActiveTaskState.AcceptedBadCodes.Count > 0)
        { %>
            <p> Некорректные принятые коды:  
            <% 
            foreach (AcceptedBadCode ccceptedBadCode in ViewData.Model.ActiveTaskState.AcceptedBadCodes)
            { %>
                <span class="acceptedBadCode">
                    <%= Html.Encode(ccceptedBadCode.Name)%>
                </span>
            <% 
            } %>       
            </p>
        <% 
        } %>       
-->
        <% 
        // Запрещаем вводить коды, если все попытки исчерпаны
        if (ViewData.Model.ActiveTaskState.AcceptedBadCodes.Count > 0)
        { %>
            <div style="color:Red">
                <%= Html.Encode(ViewData.Model.ErrorMessage) %>
            </div>
            <div style="color:Red">
                Осталось попыток: <%= Game.BadCodesLimit - ViewData.Model.ActiveTaskState.AcceptedBadCodes.Count %>
            </div>
            <div class="note">При исчерпании попыток будет заблокирован ввод кодов, а задание в момент первой подсказки будет дисквалифицировано!</div>
        <% 
        } %>
        

        <% 
        // Запрещаем вводить коды, если все попытки исчерпаны
        if (ViewData.Model.ActiveTaskState.AcceptedBadCodes.Count < Game.BadCodesLimit)
        {
            // Поле отправки кодов
            using (Html.BeginForm<TeamGameboardController>(c => c.SubmitCodes(null), FormMethod.Post))
            { %>
            <%= Html.AntiForgeryToken()%>
            <div>
                <%= Html.TextBox("Codes")%>
                <%= Html.SubmitButton("btnSubmit", "Отправить")%>
            </div>
        <% 
            }
        } %>


        <% 
        // Выводим сообщение, что все основные коды найдены и можно перейти к следующему заданию
        if (ViewData.Model.ActiveTaskState.AcceptedCodes.Count(x => x.Code.IsBonus == 0) == ViewData.Model.ActiveTaskState.Task.Codes.Count(x => x.IsBonus == 0))
        { %>
            <div style="font-weight:bold">Внимание!</div>
            <div style="color:Yellow">
            Вы нашли все основные коды! 
            Если Вы не желаете продолжить поиск бонусных кодов, то нажмите на кнопку "Следующее задание" для получения нового задания.
            </div>
            <% 
            using (Html.BeginForm<TeamGameboardController>(c => c.NextTask(ViewData.Model.ActiveTaskState.Id), FormMethod.Post)) 
            { %>
                <%= Html.AntiForgeryToken() %>
                <div>
                    <%= Html.SubmitButton("btnNextTask", "Следующее задание", new Dictionary<string, object> {{"onclick", "return confirm('Are you sure?');"}})%>
                </div>
            <% 
            }
        } %>


        <% 
        if (Model.TeamGameState.Game.BonusTasks.AvailableNowTasks().Count() > 0)
        { %>
        
        <h2>Бонусные задания</h2>
        
        <ul>
            <%
            foreach (BonusTask task in Model.TeamGameState.Game.BonusTasks.AvailableNowTasks())
            { 
                if (task.IsIndividual == 0)
                {%>
                <li>
                    <div style="font-weight:bold">
                        <b>Задание</b>
                        <span class="note">
                        [Получено: <%= task.StartTime %>, действительно до: <%= task.FinishTime %>]
                        </span>
                    </div>
                    <div>
                    <%= BBCode.ConvertToHtml(task.TaskText)%>
                    </div>
                </li>
                <%
                } %>
            <%
            } %>
        </ul>            
        <% 
        } %>

        <h2>Статистика</h2>
        <ul>
            <li>Выполнено: <%= Html.Encode(ViewData.Model.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Success)) %></li>
            <li>Невыполнено: <%= Html.Encode(ViewData.Model.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Overtime)) %></li>
            <li>Слито: <%= Html.Encode(ViewData.Model.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Canceled)) %></li>
            <li>Бонусные коды: <%= Html.Encode(ViewData.Model.TeamGameState.AcceptedTasks.BonusCodesCount())%></li>
        </ul>


        <p> </p>
        <h2>Слив задания</h2>
        <%
        // Кнопка для слива задания. Доступна только для капитана команды, после первой подсказки
        if (((User)User).Role.IsTeamLeader && ViewData.Model.TeamGameState.ActiveTaskState.AcceptedTips.Count > 1 &&
            ViewData.Model.ActiveTaskState.AcceptedCodes.Count(x => x.Code.IsBonus == 0) != ViewData.Model.ActiveTaskState.Task.Codes.Count(x => x.IsBonus == 0)) 
        { %>
            
            <% 
            using (Html.BeginForm<TeamGameboardController>(c => c.SkipTask(), FormMethod.Post)) 
            { %>
                <%= Html.AntiForgeryToken() %>
                <div>
                    <%= Html.SubmitButton("btnSkipTask", "Слить задание", new Dictionary<string, object> {{"onclick", "return confirm('Are you sure?');"}})%>
                </div>
            <% 
            } 
        }  else { %>
            <div>Слить задание можно только капитану команды после первой подсказки.</div>
        <% 
        } %>
    <% 
    } %>
  </div>
</body>
</html>
