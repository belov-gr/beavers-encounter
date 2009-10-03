<%@ Page Title="Create Code" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Web.Controllers.CodesController.CodeFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Новый код</h1>

	<% Html.RenderPartial("CodeForm", ViewData); %>

</asp:Content>
