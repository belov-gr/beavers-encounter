<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<TeamGameboardController.TeamGameboardViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Common"%>
<%@ Import Namespace="Beavers.Encounter.Core.DataInterfaces"%>
<%@ Import Namespace="System.Data"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="<%= ResolveUrl("~") %>Content/Site.css" rel="stylesheet" type="text/css" />
    <title>Результаты команды</title>
</head>
<body>
    <div class="teamGameboardPage">
        <h1><%= Html.Encode(ViewData.Model.TeamGameState.Team.Name) %></h1>
  

        <% 
        if (Model.TeamGameState.Game.BonusTasks.AvailableNowTasks().Count() > 0)
        { %>

        <h2>Бонусные задания</h2>
        
        <ul>
            <%
            foreach (BonusTask task in Model.TeamGameState.Game.BonusTasks.AvailableNowTasks())
            { %>
            <li>
                <div style="font-weight:bold">
                    <b>Задание</b>
                    <span class="note">[Получено: <%= task.StartTime %>, действительно до: <%= task.FinishTime %>]</span>
                </div>
                <div>
                <% 
                if (!task.IsIndividual)
                { %>
                    <%= BBCode.ConvertToHtml(task.TaskText)%>
                <%
                } else { %>
                    <%= BBCode.ConvertToHtml(Model.TeamGameState.Team.FinalTask)%>
                <%
                } %>
                </div>
            </li>
            <%
            } %>
        </ul>            
        <% 
        } %>
  
        <h2>Ваши результаты</h2>
        
        <div>Получено заданий: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count%></div>
        <div>Выполнено заданий: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Success)%></div>
        <div>Провалено заданий: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Overtime)%></div>
        <div>Слито заданий: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Canceled)%></div>
        <div>Дисквалифицировано заданий: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Cheat)%></div>

        <ul>
        
        <%
        foreach (var taskState in ViewData.Model.TeamGameState.AcceptedTasks)
        { %>
            <li style="line-height: 1.1em;">
                <div style="font-weight: bold;">Задание &quot;<%= Html.Encode(taskState.Task.Name) %>&quot;</div> 
                <div>Получено&nbsp;в</div>
                <div><%= Html.Encode(taskState.TaskStartTime) %></div>
                <div> 
                    <%= taskState.State == (int) TeamTaskStateFlag.Success ? "выполнено" : String.Empty %>
                    <%= taskState.State == (int) TeamTaskStateFlag.Canceled ? "слито" : String.Empty %>
                    <%= taskState.State == (int) TeamTaskStateFlag.Overtime ? "не выполнено" : String.Empty %>
                    <%= taskState.State == (int) TeamTaskStateFlag.Cheat ? "дисквалифицировано" : String.Empty%>
                &nbsp;в</div>
                <div><%= Html.Encode(taskState.TaskFinishTime) %></div>
                <div>за<span class="attention">&nbsp;<%= Html.Encode(taskState.TaskFinishTime - taskState.TaskStartTime)%>.</span><div>
            </li>
            <p></p>
        <%
        } %>
        </ul>
    </div>
</body>
</html>
