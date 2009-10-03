<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GamesController.GameStateViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers"%>
<%@ Import Namespace="System.Data"%>
<%@ Import Namespace="Beavers.Encounter.Core"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <% Html.RenderPartial("GameStateControl", Model.Game); %>

    <% if (Model.GameResults == null) { %>

        <% Html.RenderPartial("GameStateView", Model.Game); %>

    <% } else { %>
    
        <% Html.RenderPartial("GameResultsView", Model.GameResults); %>

    <% } %>

</asp:Content>
