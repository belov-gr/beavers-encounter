<%@ Page Title="Edit Code" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Web.Controllers.CodesController.CodeFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Свойсва кода</h1>

	<% Html.RenderPartial("CodeForm", ViewData); %>

</asp:Content>
