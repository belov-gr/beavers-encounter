<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BonusTasksController.BonusTaskFormViewModel>" %>
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
    <%= Html.Hidden("BonusTask.Id", (Model.BonusTask != null) ? Model.BonusTask.Id : 0)%>

    <ul>
		<li>
			<label for="BonusTask_Name">������� ��������:</label>
			<div>
				<%= Html.TextBox("BonusTask.Name", 
					(Model.BonusTask != null) ? Model.BonusTask.Name : "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.Name")%>
			<div class="note">� �������� ���� ������� �� ������ ��� ��������, ������� �������� ������� �������� ������ ������� ����. ��������, �������.</div>
		</li>
		<li>
			<label for="BonusTask_TaskText">������������ �������:</label>
			<div>
				<%= Html.TextArea("BonusTask.TaskText", 
					(Model.BonusTask != null) ? Model.BonusTask.TaskText : "", 10, 80, "")%>
			</div>
			<%= Html.ValidationMessage("BonusTask.TaskText")%>
				<div class="note">� ���� ���� ����� ������������ BBCode:</div>
				<div class="note">[b]<strong>������</strong>[/b]</div>
				<div class="note">[i]<em>������</em>[/i]</div>
				<div class="note">[u]<span style="text-decoration:underline">������������</span>[/u]</div>
				<div class="note">[del]<span style="text-decoration:line-through">�����������</span>[/del]</div>
				<div class="note">[color=Red]<span style="color:Red">�������</span>[/color]</div>
				<div class="note">[url]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/url]</div>
				<div class="note">[url=http://example.com/sample/page]<span><a href="http://example.com/sample/page">������</a></span>[/url]</div>
				<div class="note">[img]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/img]</div>
		</li>
		<li>
			<label for="BonusTask_StartTime">����� ������ (��.��.�� ��:��):</label>
			<div>
				<%= Html.TextBox("BonusTask.StartTime", 
					(Model.BonusTask != null) ? Model.BonusTask.StartTime.ToString() : Model.StartTime.ToString())%>
			</div>
			<%= Html.ValidationMessage("BonusTask.StartTime")%>
			<div class="note">
			����� �������� �����, � ������� ��� ������� ������� ������ �������.
			</div>
		</li>
		<li>
			<label for="BonusTask_FinishTime">����� ��������� (��.��.�� ��:��):</label>
			<div>
				<%= Html.TextBox("BonusTask.FinishTime", 
					(Model.BonusTask != null) ? Model.BonusTask.FinishTime.ToString() : Model.FinishTime.ToString())%>
			</div>
			<%= Html.ValidationMessage("BonusTask.FinishTime")%>
			<div class="note">
			����� �������� �����, �� �������� ������� ����� ��������������.
			</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("BonusTask.IsIndividual", 
					(Model.BonusTask != null) ? Model.BonusTask.IsIndividual != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("BonusTask.IsIndividual")%>
			<label for="BonusTask_IsIndividual">�������������� �������</label>
			<div class="note">
			���� ���������� ������ �������, �� ������� ����� �������������� ��� ������ �������. 
			����� ������� ��� ������ ������� �������� � �������� "�������������� �������" �� �������� �������.
			</div>
		</li>
    </ul>
    <%= Html.SubmitButton("btnSave", "���������") %>
    <%= Html.Button("btnCancel", "������", HtmlButtonType.Button, 
		    "window.location.href = '" + Request.UrlReferrer + "';") %>
<% } %>
