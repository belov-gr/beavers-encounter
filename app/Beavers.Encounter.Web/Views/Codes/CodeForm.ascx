<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<Beavers.Encounter.Web.Controllers.CodesController.CodeFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Code.Id", (ViewData.Model.Code != null) ? ViewData.Model.Code.Id : 0)%>
    <%= Html.Hidden("taskId", ViewData.Model.TaskId)%>

    <ul>
		<li>
			<label for="Code_Name">Код:</label>
			<div>
				<%= Html.TextBox("Code.Name", 
					(ViewData.Model.Code != null) ? ViewData.Model.Code.Name.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Code.Name")%>
			<div class="note">
			Строковое поле. Значение кода вводится без префикса, только числовая часть. Например, 2748, а не 14DR2748.
			Команды в процессе игры могут вводить коды как с префиксом так и без префикса.
			Все коды в рамках игры должны быть уникальны.
			</div>
		</li>
		<li>
			<label for="Code_Danger">Код опасности:</label>
			<div>
				<%= Html.TextBox("Code.Danger", 
					(ViewData.Model.Code != null) ? ViewData.Model.Code.Danger.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Code.Danger")%>
			<div class="note">Строковое поле. Определяет КО кода. Например, 2, +4 или +500.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Code.IsBonus",
                                        (ViewData.Model.Code != null) ? ViewData.Model.Code.IsBonus != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Code.IsBonus")%>
			<label for="Code_IsBonus">Бонусный код</label>
			<div class="note">Признак указывает, что код является бонусным и не является обязательным для выполнения задания.</div>
		</li>
    </ul>
    <%= Html.SubmitButton("btnSave", "Сохранить") %>
    <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
		    "window.location.href = '" + Html.BuildUrlFromExpression<TasksController>(c => c.Edit(ViewData.Model.TaskId)) + "';") %>
<% } %>
