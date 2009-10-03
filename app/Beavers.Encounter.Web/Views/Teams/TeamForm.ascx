<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<Beavers.Encounter.Web.Controllers.TeamsController.TeamFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Team.Id", (ViewData.Model.Team != null) ? ViewData.Model.Team.Id : 0)%>

    <ul>
		<li>
			<label for="Team_Name">Название:</label>
			<div>
				<%= Html.TextBox("Team.Name", 
					(ViewData.Model.Team != null) ? ViewData.Model.Team.Name.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Team.Name")%>
		</li>
		<li>
			<label for="Team_AccessKey">Код доступа:</label>
			<div class="note">
			Секретный код для доступа новых игроков в команду. 
			Изначально секретный код известен только капитану команды.
			Капитан должен передавать этот код только участникам своей команды.
			</div>
			<div>
				<%= Html.TextBox("Team.AccessKey", 
					(ViewData.Model.Team != null) ? ViewData.Model.Team.AccessKey.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Team.AccessKey")%>
		</li>
		<li>
			<label for="Team_FinalTask">Индивидуальное задание:</label>
			<div class="note">
			Индивидуальное задание для команды выдается в рамках бонусного задания,
			если у бунусного задания установлен признак "Индивидуальное задание".
			</div>
			<div>
				<%= Html.TextArea("Team.FinalTask", 
					(Model.Team != null) ? Model.Team.FinalTask : "", 10, 80, "")%>
				<div class="note">В этом поле можно использовать BBCode:</div>
				<div class="note">[b]<strong>Жирный</strong>[/b]</div>
				<div class="note">[i]<em>Курсив</em>[/i]</div>
				<div class="note">[u]<span style="text-decoration:underline">Подчеркнутый</span>[/u]</div>
				<div class="note">[del]<span style="text-decoration:line-through">Зачеркнутый</span>[/del]</div>
				<div class="note">[color=Red]<span style="color:Red">Красный</span>[/color]</div>
				<div class="note">[url]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/url]</div>
				<div class="note">[url=http://example.com/sample/page]<span><a href="http://example.com/sample/page">Пример</a></span>[/url]</div>
				<div class="note">[img]<span><a href="http://example.com/sample/page">http://example.com/sample/page</a></span>[/img]</div>
			</div>
			<%= Html.ValidationMessage("Team.FinalTask")%>
		</li>
    </ul>
    <p/>
    <%= Html.SubmitButton("btnSave", "Сохранить") %>
    <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
		    "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.Index()) + "';") %>
<% } %>
