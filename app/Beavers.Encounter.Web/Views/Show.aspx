<%@ Page Title="BonusTask Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Core.BonusTask>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>BonusTask Details</h1>

    <ul>
		<li>
			<label for="BonusTask_Name">Name:</label>
            <span id="BonusTask_Name"><%= Server.HtmlEncode(ViewData.Model.Name.ToString()) %></span>
		</li>
		<li>
			<label for="BonusTask_TaskText">TaskText:</label>
            <span id="BonusTask_TaskText"><%= Server.HtmlEncode(ViewData.Model.TaskText.ToString()) %></span>
		</li>
		<li>
			<label for="BonusTask_StartTime">StartTime:</label>
            <span id="BonusTask_StartTime"><%= Server.HtmlEncode(ViewData.Model.StartTime.ToString()) %></span>
		</li>
		<li>
			<label for="BonusTask_FinishTime">FinishTime:</label>
            <span id="BonusTask_FinishTime"><%= Server.HtmlEncode(ViewData.Model.FinishTime.ToString()) %></span>
		</li>
		<li>
			<label for="BonusTask_Game">Game:</label>
            <span id="BonusTask_Game"><%= Server.HtmlEncode(ViewData.Model.Game.ToString()) %></span>
		</li>
	    <li class="buttons">
            <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
                "window.location.href = '" + Html.BuildUrlFromExpression<BonusTasksController>(c => c.Index()) + "';") %>
        </li>
	</ul>

</asp:Content>
