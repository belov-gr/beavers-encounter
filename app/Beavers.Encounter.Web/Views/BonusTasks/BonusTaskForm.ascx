<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BonusTasksController.BonusTaskFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
<%@ Import Namespace="MvcContrib.FluentHtml.Elements" %>
<%@ Import Namespace="MvcContrib.FluentHtml.Html" %>
<%@ Import Namespace="MvcContrib.FluentHtml" %>
 
	<h2>Свойства задания</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("BonusTask.Id", (Model.BonusTask != null) ? Model.BonusTask.Id : 0)%>

    <ul>
		<li>
			<label for="BonusTask_Name">Кодовое название:</label>
			<div>
				<%= Html.TextBox("BonusTask.Name", 
					(Model.BonusTask != null) ? Model.BonusTask.Name : "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.Name")%>
			<div class="note">В процессе игры команды не увидят это название, кодовое название задания доступно только авторам игры. Например, Якуники.</div>
		</li>
		<li>
			<label for="BonusTask_TaskText">Формулировка задания:</label>
			<div>
				<%= Html.TextArea("BonusTask.TaskText", 
					(Model.BonusTask != null) ? Model.BonusTask.TaskText : "", 10, 80, "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.TaskText")%>
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
			<label for="BonusTask_StartTime">Время выдачи (ДД.ММ.ГГ ЧЧ:ММ):</label>
			<div>
				<%= Html.TextBox("BonusTask.StartTime", 
					(Model.BonusTask != null) ? Model.BonusTask.StartTime.ToString() : Model.StartTime.ToString())%>
			</div>
			<%= Html.ValidationMessage("BonusTask.StartTime")%>
			<div class="note">
			Здесь задается время, в которое все команды получат данное задание.
			</div>
		</li>
		<li>
			<label for="BonusTask_FinishTime">Время окончания (ДД.ММ.ГГ ЧЧ:ММ):</label>
			<div>
				<%= Html.TextBox("BonusTask.FinishTime", 
					(Model.BonusTask != null) ? Model.BonusTask.FinishTime.ToString() : Model.FinishTime.ToString())%>
			</div>
			<%= Html.ValidationMessage("BonusTask.FinishTime")%>
			<div class="note">
			Здесь задается время, до которого задание будет действительным.
			</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("BonusTask.IsIndividual", 
					(Model.BonusTask != null) ? Model.BonusTask.IsIndividual != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("BonusTask.IsIndividual")%>
			<label for="BonusTask_IsIndividual">Индивидуальное задание</label>
			<div class="note">
			Если установлен данных признак, то задание будет индивидуальным для каждой команды. 
			Текст задания для каждой команды задается в свойстве "Индивидуальное задание" на странице команды.
			</div>
		</li>
    </ul>
    <%= Html.SubmitButton("btnSave", "Сохранить") %>
    <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
		    "window.location.href = '" + Request.UrlReferrer + "';") %>
<% } %>
