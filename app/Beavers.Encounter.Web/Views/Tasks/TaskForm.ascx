<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TasksController.TaskFormViewModel>" %>
 
	<h2>�������� �������</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null)
   { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary()%>

<% using (Html.BeginForm())
   { %>
    <%= Html.AntiForgeryToken()%>
    <%= Html.Hidden("Task.Id", (ViewData.Model.Task != null) ? ViewData.Model.Task.Id : 0)%>

    <%= Model.Task.RenderEditable<Task>(Html, x => x.Name)%>
    <%= Model.Task.RenderEditable<Task>(Html, x => x.TaskType)%>
    <%= Model.Task.RenderEditable<Task>(Html, x => x.Priority)%>
    <%= Model.Task.RenderEditable<Task>(Html, x => x.StreetChallendge)%>
    <%= Model.Task.RenderEditable<Task>(Html, x => x.Agents)%>
    <%= Model.Task.RenderEditable<Task>(Html, x => x.Locked)%>
    <%= Model.Task.RenderEditable<Task>(Html, x => x.GroupTag)%>
<% if (Model.Task != null) { %>    
    <fieldset class="property">
        <legend>������ � ���������� ��������</legend>
        <%= Model.Task.RenderEditableSingle<Task, Task>(Html, x => x.AfterTask, Model.Task.Game.Tasks, new Task { Name = "<�� �������>" })%>
        <%= Model.Task.RenderEditable<Task>(Html, x => x.GiveTaskAfter)%>
    </fieldset>

    <%= Model.Task.RenderEditable(Html, x => x.NotAfterTasks, Model.Task.Game.Tasks, new Task { Name = "<�� �������>" })%>
    <%= Model.Task.RenderEditable(Html, x => x.NotOneTimeTasks, Model.Task.Game.Tasks, new Task { Name = "<�� �������>" })%>
    <%= Model.Task.RenderEditableMultiCombo(Html, x => x.NotForTeams, Model.Task.Game.Teams, new Team { Name = "<�� �������>" })%>
<% } %>    

    <div>
        <%= Html.SubmitButton("btnSave", "���������")%>
        <%= Html.Button("btnCancel", "������", HtmlButtonType.Button,
                "window.location.href = '" + Request.UrlReferrer + "';")%>
    </div>
<% } %>
