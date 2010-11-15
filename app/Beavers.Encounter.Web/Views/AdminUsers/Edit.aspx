<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AdminUsersController.UserFormViewModel>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>�������� ������������</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("User.Id", Model.User.Id)%>
    <%= Html.Hidden("User.Login", Model.User.Login)%>
    <%= Html.Hidden("User.Password", "Password")%>

    <%= Model.User.Render<User>(Html, x => x.Login)%>
    <%= Model.User.RenderEditable<User>(Html, x => x.Nick)%>
    <%= Model.User.RenderEditable<User>(Html, x => x.Icq)%>
    <%= Model.User.RenderEditable<User>(Html, x => x.Phone)%>
    <%= Model.User.RenderEditableSingle<User, Team>(Html, x => x.Team, Model.Teams, new Team { Name = "<�� �������>" })%>
    <%= Model.User.RenderEditableSingle<User, Game>(Html, x => x.Game, Model.Games, new Game { Name = "<�� �������>" })%>

    <div>
        <%= Html.SubmitButton("btnSave", "���������") %>
        <%= Html.Button("btnCancel", "������", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<AdminUsersController>(c => c.Index()) + "';")%>
    </div>                    
<% } %>

</asp:Content>
