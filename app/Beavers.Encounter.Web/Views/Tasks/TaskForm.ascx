<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TasksController.TaskFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
<%@ Import Namespace="MvcContrib.FluentHtml.Elements" %>
<%@ Import Namespace="MvcContrib.FluentHtml.Html" %>
<%@ Import Namespace="MvcContrib.FluentHtml" %>
 
	<h2>�������� �������</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Task.Id", (ViewData.Model.Task != null) ? ViewData.Model.Task.Id : 0)%>

    <ul>
		<li>
			<label for="Task_Name">������� ��������:</label>
			<div>
				<%= Html.TextBox("Task.Name", 
					(ViewData.Model.Task != null) ? ViewData.Model.Task.Name.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Task.Name")%>
			<div class="note">� �������� ���� ������� �� ������ ��� ��������, ������� �������� ������� �������� ������ ������� ����. ��������, �������.</div>
		</li>
		<li>
			<span>
				<%= Html.TextBox("Task.TaskType", 
					(ViewData.Model.Task != null) ? ViewData.Model.Task.TaskType : 0)%>
			</span>
			<%= Html.ValidationMessage("Task.TaskType")%>
			<label for="Task_TaskType">��� �������</label>
			<div class="note">0 - ������������ �������, 1 - ������� � ����������, 2 - ������� � ������� ���������.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Task.StreetChallendge", 
					(ViewData.Model.Task != null) ? ViewData.Model.Task.StreetChallendge != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Task.StreetChallendge")%>
			<label for="Task_StreetChallendge">Street Challenge</label>
			<div class="note">������ ������� ���������, ��� ������� ����� ������ ���� �������� ��� ������ ������� � ������ ����.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Task.Agents",
                                        (ViewData.Model.Task != null) ? ViewData.Model.Task.Agents != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Task.Agents")%>
			<label for="Task_Agents">������� � ��������</label>
			<div class="note">������� ������������ ��� ������������� ������� ���, ����� ������� � �������� ����������� ������������� ������ ����� ��������. ��������, ������� � +500.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Task.Locked",
                                        (ViewData.Model.Task != null) ? ViewData.Model.Task.Locked != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Task.Locked")%>
			<label for="Task_Locked">������� ��������������</label>
			<div class="note">���� ���������� ������ �������, �� ������� �� ����� ���������� ��������. ���� ������� ����� �������������/������� � �������� ����.</div>
		</li>
<!-- �� ����� -->
<%
if (ViewData.Model.Task != null)
{%>
    <li>
        <label>�� �����:</label>
    <%
    int i = 0;
    foreach (var notAfterTask in ViewData.Model.Task.NotAfterTasks)
    {
        Task empty = new Task {Name = "<�� �������>"};
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
    
    Task emptyTask = new Task {Name = "<�� �������>"};
    var tasksList = new List<Task> {emptyTask};
    SelectList slTasks = new SelectList(tasksList
            .Union(ViewData.Model.Task.Game.Tasks.Where(x => x.Id != ViewData.Model.Task.Id)),
        "Id", "Name", emptyTask);
    %>
    <%=Html.DropDownList(String.Format("Task.NotAfterTasks{0}", i), slTasks)%>
    
    <div class="note">
    ����� �������� �������, ������� �� ����� �������������� �������� �������. 
    ��� ���������� ��� �������������� ������� (������ -> �������) � �������� (�������� -> ��������) ��������� ������.
    </div>
        </li>
<%  
}%>

<!-- �� ������ -->
<%
if (ViewData.Model.Task != null)
{%>
    <li>
        <label>�� ������:</label>
    <%
    int i = 0;
    foreach (var notOneTimeTask in ViewData.Model.Task.NotOneTimeTasks)
    {
        Task empty = new Task {Name = "<�� �������>"};
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
    Task emptyTask = new Task {Name = "<�� �������>"};
    var tasksList = new List<Task> {emptyTask};
    SelectList slTasks = new SelectList(tasksList
            .Union(ViewData.Model.Task.Game.Tasks.Where(x => x.Id != ViewData.Model.Task.Id)),
        "Id", "Name", emptyTask);
    %>
    <%=Html.DropDownList(String.Format("Task.NotOneTimeTasks{0}", i), slTasks)%>

    <div class="note">
    ���� ������������� ����� ������� � ������ ������ ����������� ������-���� ������� ���������, 
    �� ������ ���������� ������ ������� �������� �� �������� ��������.
    ��� ���������� ��� �������������� ����������� ������ �� ������ ������������� ������������ ���� ����� ��������.
    ��������, ������� "��������" �� ������ ���������� ��������, ���� ����� ���� ������� � ������ ������ ��������� ������� "��������".
    �.�. ��� ������� ��� ������� ��������� "��������" ���������� � ������ "�� ������" ������� ������� � ������� ��������� "��������".
    </div>

        </li>
<%  
}%>
    </ul>
    <%= Html.SubmitButton("btnSave", "���������") %>
    <%= Html.Button("btnCancel", "������", HtmlButtonType.Button, 
		    "window.location.href = '" + Request.UrlReferrer + "';") %>
<% } %>
