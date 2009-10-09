<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<TeamsController.TeamFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Team.Id", (Model.Team != null) ? Model.Team.Id : 0)%>

    <ul>
		<li>
			<label for="Team_Name">��������:</label>
			<div>
				<%= Html.TextBox("Team.Name", 
					(Model.Team != null) ? Model.Team.Name : "")%>
			</div>
			<%= Html.ValidationMessage("Team.Name")%>
		</li>
		<li>
			<label for="Team_AccessKey">��� �������:</label>
			<div class="note">
			��������� ��� ��� ������� ����� ������� � �������. 
			���������� ��������� ��� �������� ������ �������� �������.
			������� ������ ���������� ���� ��� ������ ���������� ����� �������.
			</div>
			<div>
				<%= Html.TextBox("Team.AccessKey", 
					(Model.Team != null) ? Model.Team.AccessKey : "")%>
			</div>
			<%= Html.ValidationMessage("Team.AccessKey")%>
		</li>
		<li>
			<label for="Team_PreventTasksAfterTeams">����-����:</label>
			<div class="note">
			������ ����� �������� ������������� ����� �������, ��������� ������� ������� 
			�� �������� ��������� �� �������� ��������� ����� ������.
			</div>
			<div>
    <%
    int i = 0;
    foreach (var notAfterTeam in Model.Team.PreventTasksAfterTeams)
    {
        var empty = new Team { Name = "<�� �������>" };
        var list = new List<Team> { empty };
        var selected = new List<Team> { notAfterTeam };
        var sl = new SelectList(
            selected
            .Union(Model.Team.Game.Teams.Where(x => x.Id != Model.Team.Id).Except(Model.Team.PreventTasksAfterTeams))
            .Union(list),
            "Id", "Name", notAfterTeam);
        %>
        <%=Html.DropDownList(String.Format("Team.PreventTasksAfterTeams{0}", i), sl)%>
        <%
        i++;
    }
    var emptyTeam = new Team { Name = "<�� �������>" };
    var tasksList = new List<Team> { emptyTeam };
    var slTeam = new SelectList(
        tasksList
        .Union(Model.Team.Game.Teams.Where(x => x.Id != Model.Team.Id)),
        "Id", "Name", emptyTeam);
    %>
    <%=Html.DropDownList(String.Format("Team.PreventTasksAfterTeams{0}", i), slTeam)%>
			</div>
			<%= Html.ValidationMessage("Team.PreventTasksAfterTeams")%>
		</li>
		<li>
			<label for="Team_FinalTask">�������������� �������:</label>
			<div class="note">
			�������������� ������� ��� ������� �������� � ������ ��������� �������,
			���� � ��������� ������� ���������� ������� "�������������� �������".
			</div>
			<div>
				<%= Html.TextArea("Team.FinalTask", 
					(Model.Team != null) ? Model.Team.FinalTask : "", 10, 80, "")%>
				<div class="note">� ���� ���� ����� ������������ BBCode:</div>
				<div class="note">[b]<strong>������</strong>[/b]</div>
				<div class="note">[i]<em>������</em>[/i]</div>
				<div class="note">[u]<span style="text-decoration:underline">������������</span>[/u]</div>
				<div class="note">[del]<span style="text-decoration:line-through">�����������</span>[/del]</div>
				<div class="note">[color=Red]<span style="color:Red">�������</span>[/color]</div>
				<div class="note">[url]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/url]</div>
				<div class="note">[url=http://example.com/sample/page]<span><a href="http://example.com/sample/page">������</a></span>[/url]</div>
				<div class="note">[img]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/img]</div>
			</div>
			<%= Html.ValidationMessage("Team.FinalTask")%>
		</li>
    </ul>
    <p/>
    <%= Html.SubmitButton("btnSave", "���������") %>
    <%= Html.Button("btnCancel", "������", HtmlButtonType.Button, 
		    "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.Index()) + "';") %>
<% } %>
