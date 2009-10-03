<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<GamesController.GameFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 
	<h2>Редактирование свойств игры</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Game.Id", (ViewData.Model.Game != null) ? ViewData.Model.Game.Id : 0)%>

	<div>
		<label for="Game_Name">Название:</label>
		<div>
			<%= Html.TextBox("Game.Name", 
				(ViewData.Model.Game != null) ? ViewData.Model.Game.Name.ToString() : "")%>
		</div>
		<%= Html.ValidationMessage("Game.Name")%>
	</div>
	<div>
		<label for="Game_GameDate">Дата проведения (ДД.ММ.ГГ ЧЧ:ММ):</label>
		<div>
			<%= Html.TextBox("Game.GameDate", 
				(ViewData.Model.Game != null) ? ViewData.Model.Game.GameDate.ToString() : "")%>
		</div>
		<%= Html.ValidationMessage("Game.GameDate")%>
	</div>
	<div>
		<label for="Game_Description">Описание:</label>
		<div>
			<%= Html.TextArea("Game.Description", 
				(ViewData.Model.Game != null) ? ViewData.Model.Game.Description : "", 5, 40, "")%>
		</div>
		<%= Html.ValidationMessage("Game.Description")%>
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
	<br />
	<div>
		<label for="Game_TotalTime">Продолжительность игры (в минутах. Например, 540 - т.е. 9 часов):</label>
		<div>
			<%= Html.TextBox("Game.TotalTime", 
				(ViewData.Model.Game != null) ? ViewData.Model.Game.TotalTime.ToString() : "")%>
		</div>
		<%= Html.ValidationMessage("Game.TotalTime")%>
	</div>
	<div>
		<label for="Game_TimePerTask">Время на задание (в минутах. Например, 90 - т.е. 1.5 часа):</label>
		<div>
			<%= Html.TextBox("Game.TimePerTask", 
				(ViewData.Model.Game != null) ? ViewData.Model.Game.TimePerTask.ToString() : "")%>
		</div>
		<%= Html.ValidationMessage("Game.TimePerTask")%>
	</div>
	<div>
		<label for="Game_TimePerTip">Время на подсказку (в минутах. Например, 30 - т.е. пол часа):</label>
		<div>
			<%= Html.TextBox("Game.TimePerTip", 
				(ViewData.Model.Game != null) ? ViewData.Model.Game.TimePerTip.ToString() : "")%>
		</div>
		<%= Html.ValidationMessage("Game.TimePerTip")%>
	</div>
	<div>
		<label for="Game_PrefixMainCode">Префикс для основного кода (например, 14DR):</label>
		<div>
			<%= Html.TextBox("Game.PrefixMainCode",
                                    (ViewData.Model.Game != null) ? ViewData.Model.Game.PrefixMainCode : "")%>
		</div>
		<%= Html.ValidationMessage("Game.PrefixMainCode")%>
	</div>
	<div>
		<label for="Game_PrefixBonusCode">Префикс для бонусного кода (например, 14B):</label>
		<div>
			<%= Html.TextBox("Game.PrefixBonusCode",
                                    (ViewData.Model.Game != null) ? ViewData.Model.Game.PrefixBonusCode : "")%>
		</div>
		<%= Html.ValidationMessage("Game.PrefixBonusCode")%>
	</div>
	<div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button, 
			    "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.Index()) + "';") %>
	</div>
<% } %>
