<%@ Page Title="Create Task" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<TasksController.TaskFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Новое задание</h1>

	<% Html.RenderPartial("TaskForm", ViewData); %>

</asp:Content>
