<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TasksController.TaskFormViewModel>" %>
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
    <%= Html.Hidden("Task.Id", (ViewData.Model.Task != null) ? ViewData.Model.Task.Id : 0)%>

    <ul>
		<li>
			<label for="Task_Name">Кодовое название:</label>
			<div>
				<%= Html.TextBox("Task.Name", 
					(ViewData.Model.Task != null) ? ViewData.Model.Task.Name.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Task.Name")%>
			<div class="note">В процессе игры команды не увидят это название, кодовое название задания доступно только авторам игры. Например, Якуники.</div>
		</li>
		<li>
			<span>
				<%= Html.TextBox("Task.TaskType", 
					(ViewData.Model.Task != null) ? ViewData.Model.Task.TaskType : 0)%>
			</span>
			<%= Html.ValidationMessage("Task.TaskType")%>
			<label for="Task_TaskType">Тип задания</label>
			<div class="note">0 - классическое задание, 1 - задание с ускорением, 2 - задание с выбором подсказки.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Task.StreetChallendge", 
					(ViewData.Model.Task != null) ? ViewData.Model.Task.StreetChallendge != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Task.StreetChallendge")%>
			<label for="Task_StreetChallendge">Street Challenge</label>
			<div class="note">Данный признак указывает, что задание будет выдано всем командам как первое задание в начале игры.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Task.Agents",
                                        (ViewData.Model.Task != null) ? ViewData.Model.Task.Agents != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Task.Agents")%>
			<label for="Task_Agents">Задание с агентами</label>
			<div class="note">Признак используется при распределении заданий так, чтобы задание с агентами выполнялось единовременно только одной командой. Например, задание с +500.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Task.Locked",
                                        (ViewData.Model.Task != null) ? ViewData.Model.Task.Locked != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Task.Locked")%>
			<label for="Task_Locked">Задание заблокированно</label>
			<div class="note">Если установлен данный признак, то задание не будет выдаваться командам. Этот признак можно устанавливать/снимать в процессе игры.</div>
		</li>
<!-- Не после -->
<%
if (ViewData.Model.Task != null)
{%>
    <li>
        <label>Не после:</label>
    <%
    int i = 0;
    foreach (var notAfterTask in ViewData.Model.Task.NotAfterTasks)
    {
        Task empty = new Task {Name = "<Не указано>"};
        var list = new List<Task> {empty};
        var selected = new List<Task> {notAfterTask};
        SelectList sl = new SelectList(
                selected
                .Union(ViewData.Model.Task.Game.Tasks.Where(x => x.Id != ViewData.Model.Task.Id).Except(ViewData.Model.Task.NotAfterTasks))
                .Union(list),
            "Id", "Name", notAfterTask);
        %>
        <%=Html.DropDownList(String.Format("Task.NotAfterTasks{0}", i), sl)%>
    <%
        i++;
    }
    
    Task emptyTask = new Task {Name = "<Не указано>"};
    var tasksList = new List<Task> {emptyTask};
    SelectList slTasks = new SelectList(tasksList
            .Union(ViewData.Model.Task.Game.Tasks.Where(x => x.Id != ViewData.Model.Task.Id)),
        "Id", "Name", emptyTask);
    %>
    <%=Html.DropDownList(String.Format("Task.NotAfterTasks{0}", i), slTasks)%>
    
    <div class="note">
    Здесь задаются задания, которые не могут предшествовать текущему заданию. 
    Это необходимо для предотвращения дальних (Механа -> Копаево) и коротких (Мельница -> Эпицентр) перегонов команд.
    </div>
        </li>
<%  
}%>

<!-- Не вместе -->
<%
if (ViewData.Model.Task != null)
{%>
    <li>
        <label>Не вместе:</label>
    <%
    int i = 0;
    foreach (var notOneTimeTask in ViewData.Model.Task.NotOneTimeTasks)
    {
        Task empty = new Task {Name = "<Не указано>"};
        var list = new List<Task> {empty};
        var selected = new List<Task> { notOneTimeTask };
        SelectList sl = new SelectList(
                selected
                .Union(ViewData.Model.Task.Game.Tasks.Where(x => x.Id != ViewData.Model.Task.Id).Except(ViewData.Model.Task.NotOneTimeTasks))
                .Union(list),
            "Id", "Name", notOneTimeTask);
        %>
        <%=Html.DropDownList(String.Format("Task.NotOneTimeTasks{0}", i), sl)%>
    <%
        i++;
    } 
    Task emptyTask = new Task {Name = "<Не указано>"};
    var tasksList = new List<Task> {emptyTask};
    SelectList slTasks = new SelectList(tasksList
            .Union(ViewData.Model.Task.Game.Tasks.Where(x => x.Id != ViewData.Model.Task.Id)),
        "Id", "Name", emptyTask);
    %>
    <%=Html.DropDownList(String.Format("Task.NotOneTimeTasks{0}", i), slTasks)%>

    <div class="note">
    Если перечисленные здесь задания в данный момент выполняются какими-либо другими командами, 
    то движок предпочтет данное задание временно не выдавать командам.
    Это необходимо для предотвращения пересечения команд на близко расположенных относительно друг друга локациях.
    Например, задание "Мельница" не должно выдаваться командам, если какая либо команда в данный момент выполняет задание "Эпицентр".
    Т.е. для задания под кодовым названием "Мельница" необходимо в списке "Не вместе" выбрать задание с кодовым названием "Эпицентр".
    </div>

        </li>
<%  
}%>
    </ul>
    <%= Html.SubmitButton("btnSave", "Сохранить") %>
    <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
		    "window.location.href = '" + Request.UrlReferrer + "';") %>
<% } %>
