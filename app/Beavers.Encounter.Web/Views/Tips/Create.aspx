<%@ Page Title="Create Tip" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Web.Controllers.TipsController.TipFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h2>Новое задание/подсказка</h2>

	<% Html.RenderPartial("TipForm", ViewData); %>

</asp:Content>
