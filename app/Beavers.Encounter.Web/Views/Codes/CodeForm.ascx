<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<CodesController.CodeFormViewModel>" %>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Code.Id", (ViewData.Model.Code != null) ? ViewData.Model.Code.Id : 0)%>
    <%= Html.Hidden("taskId", ViewData.Model.TaskId)%>

    <%= Model.Code.RenderEditable<Code>(Html, x => x.Name) %>
    <%= Model.Code.RenderEditable<Code>(Html, x => x.Danger)%>
    <%= Model.Code.RenderEditable<Code>(Html, x => x.IsBonus)%>

    <div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
		        "window.location.href = '" + Html.BuildUrlFromExpression<TasksController>(c => c.Edit(ViewData.Model.TaskId)) + "';") %>
    </div>		    
<% } %>
