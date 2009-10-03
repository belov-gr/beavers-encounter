<%@ Page Title="Edit Tip" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Web.Controllers.TipsController.TipFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Edit Tip</h1>

	<% Html.RenderPartial("TipForm", ViewData); %>

</asp:Content>
