<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h2>Администрирование</h2>
	
	<ul>
	    <li>
	        <%= Html.ActionLink<AdminAppConfigController>(c => c.Edit(), "Настрийки сайта")%>
	    </li>
	    <li>
	        <%= Html.ActionLink<AdminUsersController>(c => c.Index(), "Список пользователей")%>
	    </li>
	    <li>
	        <%= Html.ActionLink<AdminGamesController>(c => c.Index(), "Список игр")%>
	    </li>
	</ul>

</asp:Content>
