<%@ Page Title="Games" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Security"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Error</h2>

<% if(((HandleErrorInfo)ViewData.Model).Exception != null)
 {%>

    <%=((HandleErrorInfo)ViewData.Model).Exception.Message %>

<%
 } else
 {%>

<h1>An Error has occured in the application</h1>

<p>A record of the problem has been made. Please check back soon</p>

<%
 }%>

</asp:Content>
