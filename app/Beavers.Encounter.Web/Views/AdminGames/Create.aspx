<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Game>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Новая игра</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>

    <%= Model.RenderEditable<Game>(Html, x => x.Name)%>
    <%= Model.RenderEditable<Game>(Html, x => x.GameDate)%>
    <%= Model.RenderEditable<Game>(Html, x => x.Description)%>
    <%= Model.RenderEditable<Game>(Html, x => x.TotalTime)%>

    <div>
        <%= Html.SubmitButton("btnCreate", "Создать") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<AdminGamesController>(c => c.Index()) + "';")%>
    </div>                    
<% } %>

</asp:Content>
