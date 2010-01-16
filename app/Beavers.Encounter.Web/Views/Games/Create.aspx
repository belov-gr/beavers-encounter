<%@ Page Title="Create Game" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<GamesController.GameFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Новая игра</h1>

	<% Html.RenderPartial("GameForm", ViewData); %>

</asp:Content>
