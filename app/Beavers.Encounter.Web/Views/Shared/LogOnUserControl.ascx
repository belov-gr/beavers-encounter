<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%
    if (Request.IsAuthenticated) {
%>
        <div>Здравствуйте, <b><%= Html.Encode(Page.User.Identity.Name) %></b>!</div>
        <div>Вы - <b>
        <%= Html.Encode(Page.User.IsInRole("Administrator") ? "админ" : String.Empty)%>
        <%= Html.Encode(Page.User.IsInRole("Author") ? "автор" : String.Empty) %>
        <%= Html.Encode(Page.User.IsInRole("Player") ? "игрок" : String.Empty)%>
        <%= Html.Encode(Page.User.IsInRole("Guest") ? "гость" : String.Empty)%>
        <%= Html.Encode(Page.User.IsInRole("TeamLeader") ? "капитан" : String.Empty)%></b>.
        </div>
<%
    }
    else {
%> 
        <div>Мы знакомы? <%= Html.ActionLink("Войдите", "LogOn", "Account") %></div>
<%
    }
%>
