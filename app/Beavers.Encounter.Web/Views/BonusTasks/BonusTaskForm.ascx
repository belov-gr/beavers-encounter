<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BonusTasksController.BonusTaskFormViewModel>" %>
 
	<h2>Свойства задания</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("BonusTask.Id", (Model.BonusTask != null) ? Model.BonusTask.Id : 0)%>

    <%= Model.BonusTask.RenderEditable<BonusTask>(Html, x => x.Name)%>
    <%= Model.BonusTask.RenderEditable<BonusTask>(Html, x => x.TaskText)%>
    <%= Model.BonusTask.RenderEditable<BonusTask>(Html, x => x.StartTime, Model.StartTime)%>
    <%= Model.BonusTask.RenderEditable<BonusTask>(Html, x => x.FinishTime, Model.FinishTime)%>
    <%= Model.BonusTask.RenderEditable<BonusTask>(Html, x => x.IsIndividual)%>

    <div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
		        "window.location.href = '" + Request.UrlReferrer + "';") %>
    </div>		    
<% } %>
