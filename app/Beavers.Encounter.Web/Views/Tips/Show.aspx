<%@ Page Title="Tip Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Core.Tip>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Tip Details</h1>

    <ul>
		<li>
			<label for="Tip_Name">Name:</label>
            <span id="Tip_Name"><%= Server.HtmlEncode(ViewData.Model.Name.ToString()) %></span>
		</li>
		<li>
			<label for="Tip_SuspendTime">SuspendTime:</label>
            <span id="Tip_SuspendTime"><%= Server.HtmlEncode(ViewData.Model.SuspendTime.ToString()) %></span>
		</li>
		<li>
			<label for="Tip_Task">Task:</label>
            <span id="Tip_Task"><%= Server.HtmlEncode(ViewData.Model.Task.ToString()) %></span>
		</li>
	    <li class="buttons">
            <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
                "window.location.href = '" + Html.BuildUrlFromExpression<TipsController>(c => c.Index()) + "';") %>
        </li>
	</ul>

</asp:Content>
