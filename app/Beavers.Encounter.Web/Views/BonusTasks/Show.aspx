<%@ Page Title="BonusTask Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Core.BonusTask>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>BonusTask Details</h1>

    <ul>
		<li>
			<label for="BonusTask_Name">Кодовое название:</label>
            <span id="BonusTask_Name"><%= Server.HtmlEncode(ViewData.Model.Name.ToString()) %></span>
		</li>
		<li>
			<label for="BonusTask_TaskText">Текст задания:</label>
            <div id="BonusTask_TaskText"><%= Beavers.Encounter.Common.BBCode.ConvertToHtml(ViewData.Model.TaskText.ToString()) %></div>
            <br />
		</li>
		<li>
			<label for="BonusTask_StartTime">Время начала:</label>
            <span id="BonusTask_StartTime"><%= Server.HtmlEncode(ViewData.Model.StartTime.ToString()) %></span>
		</li>
		<li>
			<label for="BonusTask_FinishTime">Время окончания:</label>
            <span id="BonusTask_FinishTime"><%= Server.HtmlEncode(ViewData.Model.FinishTime.ToString()) %></span>
		</li>
	</ul>
            <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
                "window.location.href = '" + Html.BuildUrlFromExpression<GamesController>(c => c.Edit(ViewData.Model.Game.Id)) + "';") %>

</asp:Content>
