<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<Beavers.Encounter.Web.Controllers.BonusTasksController.BonusTaskFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("BonusTask.Id", (ViewData.Model.BonusTask != null) ? ViewData.Model.BonusTask.Id : 0)%>

    <ul>
		<li>
			<label for="BonusTask_Name">Name:</label>
			<div>
				<%= Html.TextBox("BonusTask.Name", 
					(ViewData.Model.BonusTask != null) ? ViewData.Model.BonusTask.Name.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.Name")%>
		</li>
		<li>
			<label for="BonusTask_TaskText">TaskText:</label>
			<div>
				<%= Html.TextBox("BonusTask.TaskText", 
					(ViewData.Model.BonusTask != null) ? ViewData.Model.BonusTask.TaskText.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.TaskText")%>
		</li>
		<li>
			<label for="BonusTask_StartTime">StartTime:</label>
			<div>
				<%= Html.TextBox("BonusTask.StartTime", 
					(ViewData.Model.BonusTask != null) ? ViewData.Model.BonusTask.StartTime.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.StartTime")%>
		</li>
		<li>
			<label for="BonusTask_FinishTime">FinishTime:</label>
			<div>
				<%= Html.TextBox("BonusTask.FinishTime", 
					(ViewData.Model.BonusTask != null) ? ViewData.Model.BonusTask.FinishTime.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.FinishTime")%>
		</li>
		<li>
			<label for="BonusTask_Game">Game:</label>
			<div>
				<%= Html.TextBox("BonusTask.Game", 
					(ViewData.Model.BonusTask != null) ? ViewData.Model.BonusTask.Game.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.Game")%>
		</li>
	    <li>
            <%= Html.SubmitButton("btnSave", "Save BonusTask") %>
	        <%= Html.Button("btnCancel", "Cancel", HtmlButtonType.Button, 
				    "window.location.href = '" + Html.BuildUrlFromExpression<BonusTasksController>(c => c.Index()) + "';") %>
        </li>
    </ul>
<% } %>
