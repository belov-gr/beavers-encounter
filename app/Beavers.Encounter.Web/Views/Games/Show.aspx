<%@ Page Title="Game Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Game>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Описание игры</h2>

    <ul>
		<li>
		    <%= Model.Render<Game>(Html, x => x.Name) %>
		</li>
		<li>
		    <%= Model.Render<Game>(Html, x => x.GameDate) %>
		</li>
		<li>
		    <%= Model.Render<Game>(Html, x => x.Description) %>
		</li>
		<li>
		    <%= Model.Render<Game>(Html, x => x.PrefixMainCode) %>
		</li>
		<li>
		    <%= Model.Render<Game>(Html, x => x.PrefixBonusCode) %>
		</li>
	</ul>

    <% if (((User)User).Team != null && ((User)User).Team.Game == null && ((User)User).Role.IsTeamLeader) { %>
        <%= Html.Button("registerTeam", "Будем играть", HtmlButtonType.Button, 
            "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingInGame(ViewData.Model.Id, ((User)User).Team.Id)) + "';") %>
    <% } %>

    <p><%= Html.ActionLink<GamesController>(c => c.Edit(Model.Id), "Изменить")%></p>

    <% Html.RenderPartial("RegisteredTeams", Model); %>

</asp:Content>
