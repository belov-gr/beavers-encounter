<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<TeamsController.TeamFormViewModel>" %>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Team.Id", (Model.Team != null) ? Model.Team.Id : 0)%>

    
    <%= Model.Team.RenderEditable<Team>(Html, x => x.Name)%>
    <%= Model.Team.RenderEditable<Team>(Html, x => x.AccessKey)%>

<% if (Model.Team != null && Model.Team.Game != null) { %>
    <%= Model.Team.RenderEditable(Html, x => x.PreventTasksAfterTeams, Model.Team.Game.Teams, new Team { Name = "<Не указано>" })%>
<% } %>

    <%= Model.Team.RenderEditable<Team>(Html, x => x.FinalTask)%>

    <div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
		        "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.Index()) + "';") %>
    </div>		    
<% } %>
