<%@ Page Title="Edit Team" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<TeamsController.TeamFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>�������������� �������</h1>

	<% Html.RenderPartial("TeamForm", ViewData); %>

</asp:Content>
