<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<TeamGameboardController.TeamGameboardViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Common"%>
<%@ Import Namespace="Beavers.Encounter.Core.DataInterfaces"%>
<%@ Import Namespace="System.Data"%>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers"%>
<%@ Import Namespace="Beavers.Encounter.Core"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="<%= ResolveUrl("~") %>Content/Site.css" rel="stylesheet" type="text/css" />
    <title>���������� �������</title>
</head>
<body>
    <div class="teamGameboardPage">
        <h1><%= Html.Encode(ViewData.Model.TeamGameState.Team.Name) %></h1>
  

        <% 
        if (Model.TeamGameState.Game.BonusTasks.AvailableNowTasks().Count() > 0)
        { %>

        <h2>�������� �������</h2>
        
        <ul>
            <%
            foreach (BonusTask task in Model.TeamGameState.Game.BonusTasks.AvailableNowTasks())
            { %>
            <li>
                <div style="font-weight:bold">
                    <b>�������</b>
                    <span class="note">[��������: <%= task.StartTime %>, ������������� ��: <%= task.FinishTime %>]</span>
                </div>
                <div>
                <% 
                if (task.IsIndividual == 0)
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
  
        <h2>���� ����������</h2>
        
        <div>�������� �������: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count%></div>
        <div>��������� �������: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Success)%></div>
        <div>�� ��������� �������: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Overtime)%></div>
        <div>����� �������: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Canceled)%></div>
        <div>������������������ �������: <%= ViewData.Model.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Cheat)%></div>

        <ul>
        
        <%
        foreach (var taskState in ViewData.Model.TeamGameState.AcceptedTasks)
        { %>
            <li>
            ������� "<%= Html.Encode(taskState.Task.Name) %>" 
            <div><%= Html.Encode(taskState.TaskStartTime) %> ��������</div>
            <div> 
            <%= Html.Encode(taskState.TaskFinishTime) %>
            <%= taskState.State == (int) TeamTaskStateFlag.Success ? "���������" : String.Empty %>
            <%= taskState.State == (int) TeamTaskStateFlag.Canceled ? "�����" : String.Empty %>
            <%= taskState.State == (int) TeamTaskStateFlag.Overtime ? "�� ���������" : String.Empty %>
            <div>
            </li>
        <%
        } %>
        </ul>
    </div>
</body>
</html>
