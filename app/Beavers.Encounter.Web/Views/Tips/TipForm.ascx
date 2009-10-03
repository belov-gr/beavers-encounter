<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<Beavers.Encounter.Web.Controllers.TipsController.TipFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Tip.Id", (ViewData.Model.Tip != null) ? ViewData.Model.Tip.Id : 0)%>
    <%= Html.Hidden("taskId", ViewData.Model.TaskId)%>

    <ul>
		<li>
			<label for="Tip_Name">Текст задания/подсказки:</label>
			<div>
				<%= Html.TextArea("Tip.Name", 
					(ViewData.Model.Tip != null) ? ViewData.Model.Tip.Name.ToString() : "", 10, 80, "")%>
			</div>
			<%= Html.ValidationMessage("Tip.Name")%>
				<div class="note">В этом поле можно использовать BBCode:</div>
				<div class="note">[b]<strong>Жирный</strong>[/b]</div>
				<div class="note">[i]<em>Курсив</em>[/i]</div>
				<div class="note">[u]<span style="text-decoration:underline">Подчеркнутый</span>[/u]</div>
				<div class="note">[del]<span style="text-decoration:line-through">Зачеркнутый</span>[/del]</div>
				<div class="note">[color=Red]<span style="color:Red">Красный</span>[/color]</div>
				<div class="note">[url]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/url]</div>
				<div class="note">[url=http://example.com/sample/page]<span><a href="http://example.com/sample/page">Пример</a></span>[/url]</div>
				<div class="note">[img]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/img]</div>
		</li>
		<li>
			<label for="Tip_SuspendTime">Время:</label>
			<div>
				<%= Html.TextBox("Tip.SuspendTime", 
					(ViewData.Model.Tip != null) ? ViewData.Model.Tip.SuspendTime.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Tip.SuspendTime")%>
		</li>
    </ul>
    <br />
    <%= Html.SubmitButton("btnSave", "Сохранить") %>
    <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button,
                    "window.location.href = '" + Html.BuildUrlFromExpression<TasksController>(c => c.Edit(ViewData.Model.TaskId)) + "';")%>
<% } %>
