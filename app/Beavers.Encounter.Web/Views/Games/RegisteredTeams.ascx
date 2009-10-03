<%@ Control Language="C#" Inherits="Beavers.Encounter.Web.Views.ViewUserControl<Game>" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<div class="registeredTeams">
    <h2>������������������ �������</h2>
    <ul>
    <%
    foreach (Team team in Model.Teams)
    {%>
        <li>
        <%= Html.ActionLink<Beavers.Encounter.Web.Controllers.TeamsController>(c => c.Show(team.Id), team.Name) %>           
        <% if (((User)Page.User).Role.IsAuthor)
           {%>
            <%= Html.Button("btnSingOutGame",
                "�������",
                HtmlButtonType.Button,
                "window.location.href = '" + Html.BuildUrlFromExpression<Beavers.Encounter.Web.Controllers.TeamsController>(c => c.SingOutGame(Model.Id, team.Id)) + "';")%>
        <% } %>
        </li>
<%  } %>
    </ul>
</div>

