<%@ Page Title="Game Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Core.Game>" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Описание игры</h2>

    <ul>
		<li>
			<label for="Game_Name">Название:</label>
            <span id="Game_Name"><%= Server.HtmlEncode(ViewData.Model.Name.ToString()) %></span>
		</li>
		<li>
			<label for="Game_GameDate">Дата проведения:</label>
            <span id="Game_GameDate"><%= Server.HtmlEncode(ViewData.Model.GameDate.ToString()) %></span>
		</li>
		<li>
			<label for="Game_Description">Описание:</label>
            <div id="Game_Description"><%= Beavers.Encounter.Common.BBCode.ConvertToHtml(ViewData.Model.Description) %></div>
            <br />
		</li>
        <% if (((User)User).Team != null && ((User)User).Team.Game == null && ((User)User).Role.IsTeamLeader) { %>
	        <li class="buttons">
                <%= Html.Button("registerTeam", "Будем играть", HtmlButtonType.Button, 
                    "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingInGame(ViewData.Model.Id, ((User)User).Team.Id)) + "';") %>
            </li>
        <% } %>
	</ul>

    <p><%= Html.ActionLink<GamesController>(c => c.Edit(Model.Id), "Изменить")%></p>

    <% Html.RenderPartial("RegisteredTeams", Model); %>

</asp:Content>
