<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<GamesController.GameFormViewModel>" %>
 
	<h2>Редактирование свойств игры</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Game.Id", (ViewData.Model.Game != null) ? ViewData.Model.Game.Id : 0)%>

    <%= Model.Game.RenderEditable<Game>(Html, x => x.Name)%>
    <%= Model.Game.RenderEditable<Game>(Html, x => x.GameDate)%>
    <%= Model.Game.RenderEditable<Game>(Html, x => x.Description)%>
    <%= Model.Game.RenderEditable<Game>(Html, x => x.TotalTime)%>
    <%= Model.Game.RenderEditable<Game>(Html, x => x.TimePerTask)%>
    <%= Model.Game.RenderEditable<Game>(Html, x => x.TimePerTip)%>
    <%= Model.Game.RenderEditable<Game>(Html, x => x.PrefixMainCode)%>
    <%= Model.Game.RenderEditable<Game>(Html, x => x.PrefixBonusCode)%>
	
	<div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
			    "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.Index()) + "';") %>
	</div>
<% } %>
