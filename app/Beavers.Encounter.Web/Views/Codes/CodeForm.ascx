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
			<label for="Code_Name">���:</label>
			<div>
				<%= Html.TextBox("Code.Name", 
					(ViewData.Model.Code != null) ? ViewData.Model.Code.Name.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Code.Name")%>
			<div class="note">
			��������� ����. �������� ���� �������� ��� ��������, ������ �������� �����. ��������, 2748, � �� 14DR2748.
			������� � �������� ���� ����� ������� ���� ��� � ��������� ��� � ��� ��������.
			��� ���� � ������ ���� ������ ���� ���������.
			</div>
		</li>
		<li>
			<label for="Code_Danger">��� ���������:</label>
			<div>
				<%= Html.TextBox("Code.Danger", 
					(ViewData.Model.Code != null) ? ViewData.Model.Code.Danger.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Code.Danger")%>
			<div class="note">��������� ����. ���������� �� ����. ��������, 2, +4 ��� +500.</div>
		</li>
		<li>
			<span>
				<%= Html.CheckBox("Code.IsBonus",
                                        (ViewData.Model.Code != null) ? ViewData.Model.Code.IsBonus != 0 : false)%>
			</span>
			<%= Html.ValidationMessage("Code.IsBonus")%>
			<label for="Code_IsBonus">�������� ���</label>
			<div class="note">������� ���������, ��� ��� �������� �������� � �� �������� ������������ ��� ���������� �������.</div>
		</li>
    </ul>
    <%= Html.SubmitButton("btnSave", "���������") %>
    <%= Html.Button("btnCancel", "������", HtmlButtonType.Button, 
		    "window.location.href = '" + Html.BuildUrlFromExpression<TasksController>(c => c.Edit(ViewData.Model.TaskId)) + "';") %>
<% } %>
