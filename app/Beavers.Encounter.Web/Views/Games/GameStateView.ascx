<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Game>"%>

<h2>Состояние игры</h2>

<table>
    <thead>
        <tr>
		    <th>Команда</th>
		    <th>Текущее<br />задание</th>
		    <th>Время<br />выполнения</th>
		    <th>Получено\<br />Успешно\<br />Проср-но\<br />Слито\<br />Дисквал-но</th>
		    <th>Количество<br />подсказок</th>
		    <th>Количество<br />принятых кодов</th>
        </tr>
    </thead>
    
	<%
	foreach (Team team in Model.Teams) 
    {
        if (team.TeamGameState == null)
            continue;
	    %>
        
		<tr>
		    <td><%= Html.Encode(team.Name) %></td>
		    <td>
                <% if (team.TeamGameState.ActiveTaskState != null) { %>
                <%= Html.Encode(team.TeamGameState.ActiveTaskState.Task.Name) %>
                <% } %>
            </td>
		    <td align="center">
                <% if (team.TeamGameState.ActiveTaskState != null) 
                   {
                       TimeSpan time = DateTime.Now - team.TeamGameState.ActiveTaskState.TaskStartTime; 
                       %>
                       <%= time.ToString().Split(new char[] {'.'})[0] %>
                <% } %>
            </td>
		    <td align="center">
                <% if (team.TeamGameState.ActiveTaskState != null) { %>
                <%= Html.Encode(team.TeamGameState.AcceptedTasks.Count) %> \
                <%= Html.Encode(team.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Success)) %> \
                <%= Html.Encode(team.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Overtime))%> \
                <%= Html.Encode(team.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Canceled))%> \
                <%= Html.Encode(team.TeamGameState.AcceptedTasks.Count(x => x.State == (int)TeamTaskStateFlag.Cheat))%>
                <% } %>
            </td>
		    <td align="center">
                <% if (team.TeamGameState.ActiveTaskState != null) { %>
                <%= Html.Encode(team.TeamGameState.ActiveTaskState.AcceptedTips.Count - 1) %>
                <% } %>
            </td>
		    <td align="center">
                <% if (team.TeamGameState.ActiveTaskState != null) { %>
                <%= Html.Encode(team.TeamGameState.ActiveTaskState.AcceptedCodes.Count) %>
                <% } %>
            </td>
		</tr>
	<% } %>

</table>

