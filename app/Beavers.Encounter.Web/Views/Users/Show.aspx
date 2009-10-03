<%@ Page Title="User Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Core.User>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>User Details</h1>

    <ul>
		<li>
			<label for="User_Login">Login:</label>
            <span id="User_Login"><%= Server.HtmlEncode(ViewData.Model.Login.ToString()) %></span>
		</li>
		<li>
			<label for="User_Password">Password:</label>
            <span id="User_Password"><%= Server.HtmlEncode(ViewData.Model.Password.ToString()) %></span>
		</li>
		<li>
			<label for="User_Nick">Nick:</label>
            <span id="User_Nick"><%= Server.HtmlEncode(ViewData.Model.Nick.ToString()) %></span>
		</li>
		<li>
			<label for="User_Phone">Phone:</label>
            <span id="User_Phone"><%= Server.HtmlEncode(ViewData.Model.Phone.ToString()) %></span>
		</li>
		<li>
			<label for="User_Icq">Icq:</label>
            <span id="User_Icq"><%= Server.HtmlEncode(ViewData.Model.Icq.ToString()) %></span>
		</li>
		<li>
			<label for="User_Team">Team:</label>
            <span id="User_Team"><%= Server.HtmlEncode(ViewData.Model.Team != null ? ViewData.Model.Team.ToString() : String.Empty) %></span>
		</li>
	    <li class="buttons">
            <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
                "window.location.href = '" + Html.BuildUrlFromExpression<UsersController>(c => c.Index()) + "';") %>
        </li>
	</ul>

</asp:Content>
