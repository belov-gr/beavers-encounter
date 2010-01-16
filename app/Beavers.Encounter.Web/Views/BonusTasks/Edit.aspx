<%@ Page Title="Edit BonusTask" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<BonusTasksController.BonusTaskFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Редактирование бонусного задания</h1>

	<% Html.RenderPartial("BonusTaskForm", ViewData); %>

</asp:Content>
