<%@ Page Title="Create Team" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Web.Controllers.TeamsController.TeamFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Новая команда</h1>

	<% Html.RenderPartial("TeamForm", ViewData); %>

</asp:Content>
