<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%
    if (Request.IsAuthenticated) {
%>
        Здравствуйте, <b><%= Html.Encode(Page.User.Identity.Name) %></b>!
        <%= Html.Encode(Page.User.IsInRole("Administrator") ? "[Админ]" : String.Empty)%>
        <%= Html.Encode(Page.User.IsInRole("Author") ? "[Автор]" : String.Empty) %>
        <%= Html.Encode(Page.User.IsInRole("Player") ? "[Игрок]" : String.Empty)%>
        <%= Html.Encode(Page.User.IsInRole("Guest") ? "[Гость]" : String.Empty)%>
        <%= Html.Encode(Page.User.IsInRole("TeamLeader") ? "[Капитан]" : String.Empty)%>
        <%= Html.ActionLink("Выйти", "LogOff", "Account") %>
<%
    }
    else {
%> 
        <%= Html.ActionLink("Войти", "LogOn", "Account") %>
<%
    }
%>
