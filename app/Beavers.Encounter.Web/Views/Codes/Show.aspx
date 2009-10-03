<%@ Page Title="Code Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Core.Code>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Code Details</h1>

    <ul>
		<li>
			<label for="Code_Name">Name:</label>
            <span id="Code_Name"><%= Server.HtmlEncode(ViewData.Model.Name.ToString()) %></span>
		</li>
		<li>
			<label for="Code_Danger">Danger:</label>
            <span id="Code_Danger"><%= Server.HtmlEncode(ViewData.Model.Danger.ToString()) %></span>
		</li>
		<li>
			<label for="Code_IsBonus">IsBonus:</label>
            <span id="Span1"><%= Server.HtmlEncode(ViewData.Model.IsBonus.ToString()) %></span>
		</li>
		<li>
			<label for="Code_Task">Task:</label>
            <span id="Code_Task"><%= Server.HtmlEncode(ViewData.Model.Task.ToString()) %></span>
		</li>
	    <li class="buttons">
            <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
                "window.location.href = '" + Html.BuildUrlFromExpression<CodesController>(c => c.Index()) + "';") %>
        </li>
	</ul>

</asp:Content>
