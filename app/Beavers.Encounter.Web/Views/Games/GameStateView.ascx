<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Game>"%>

<h2>��������� ����</h2>

<table>
    <thead>
        <tr>
		    <th>�������</th>
		    <th>�������<br />�������</th>
		    <th>�����<br />����������</th>
		    <th>��������\<br />�������\<br />�����-��\<br />�����\<br />�������-��</th>
		    <th>����������<br />���������</th>
		    <th>����������<br />�������� �����</th>
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

