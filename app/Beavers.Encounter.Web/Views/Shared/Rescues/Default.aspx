<%@ Page Title="Games" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="log4net"%>
<%@ Import Namespace="System.Security"%>
<%@ Import Namespace="Beavers.Encounter.Common"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Error</h2>

<% if(((HandleErrorInfo)ViewData.Model).Exception != null)
 {
     ILog log = LogManager.GetLogger("ErrorReporter");
     log.Error(((HandleErrorInfo)ViewData.Model).Exception.Expand());
    
    %>

    <div style="color:Red"><%= ((HandleErrorInfo)ViewData.Model).Exception.Message %></div>

    <%= ((HandleErrorInfo)ViewData.Model).Exception.Expand().Replace("\t", "    ")
                .Replace("\r\n", "<br />")
    %>

<%
 } else
 {%>

<h1>An Error has occured in the application</h1>

<p>A record of the problem has been made. Please check back soon</p>

<%
 }%>

</asp:Content>
