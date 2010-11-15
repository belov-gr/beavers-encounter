<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<User>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>����� ������������</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>

    <%= Model.RenderEditable<User>(Html, x => x.Login)%>
    <div>
        <label for="User_Password">������:</label>
        <div>
        <%= Html.Password("User.Password")%>
        <%= Html.ValidationMessage("User.Password")%>
        </div>
    </div>
    <div>
        <label for="confirmPassword">����������� ������:</label>
        <div>
        <%= Html.Password("confirmPassword")%>
        <%= Html.ValidationMessage("confirmPassword")%>
        </div>
    </div>

    <%= Model.RenderEditable<User>(Html, x => x.Nick)%>
    <%= Model.RenderEditable<User>(Html, x => x.Icq)%>
    <%= Model.RenderEditable<User>(Html, x => x.Phone)%>

    <div>
        <%= Html.SubmitButton("btnCreate", "�������") %>
        <%= Html.Button("btnCancel", "������", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<AdminUsersController>(c => c.Index()) + "';")%>
    </div>                    
<% } %>

</asp:Content>
