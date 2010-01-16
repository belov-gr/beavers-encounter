<%@ Control Language="C#" Inherits="Beavers.Encounter.Web.Views.ViewUserControl<Game>" %>

<div class="registeredTeams">

    <h2>Зарегистрированные команды</h2>
	
    <ul>
    <%
    foreach (Team team in Model.Teams)
    {%>
        <li>
        <%= Html.ActionLink<TeamsController>(c => c.Show(team.Id), team.Name) %>           
        <% if (((User)Page.User).Role.IsAuthor)
           {%>
            <%= Html.Button("btnSingOutGame",
                "Удалить",
                HtmlButtonType.Button,
                "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingOutGame(Model.Id, team.Id)) + "';")%>
        <% } %>
        </li>
	<% } %>
    </ul>
</div>

