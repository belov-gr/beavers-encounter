<%@ Page Title="Create Task" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Beavers.Encounter.Web.Controllers.TasksController.TaskFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>����� �������</h1>

	<% Html.RenderPartial("TaskForm", ViewData); %>

</asp:Content>
