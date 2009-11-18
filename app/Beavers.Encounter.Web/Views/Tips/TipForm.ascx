<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<TipsController.TipFormViewModel>" %>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Tip.Id", (ViewData.Model.Tip != null) ? ViewData.Model.Tip.Id : 0)%>
    <%= Html.Hidden("taskId", ViewData.Model.TaskId)%>

    <%= Model.Tip.RenderEditable<Tip>(Html, x => x.Name)%>
    <%= Model.Tip.RenderEditable<Tip>(Html, x => x.SuspendTime)%>

    <div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<TasksController>(c => c.Edit(ViewData.Model.TaskId)) + "';")%>
    </div>                    
<% } %>
