<%@ Page Title="Create BonusTask" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<BonusTasksController.BonusTaskFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Новое бонусное задание</h1>

	<% Html.RenderPartial("BonusTaskForm", ViewData); %>

</asp:Content>
