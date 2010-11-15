<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<User>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h2>Новый пользователь</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>

    <%= Model.RenderEditable<User>(Html, x => x.Login)%>
    <div>
        <label for="User_Password">Пароль:</label>
        <div>
        <%= Html.Password("User.Password")%>
        <%= Html.ValidationMessage("User.Password")%>
        </div>
    </div>
    <div>
        <label for="confirmPassword">Подтвердите пароль:</label>
        <div>
        <%= Html.Password("confirmPassword")%>
        <%= Html.ValidationMessage("confirmPassword")%>
        </div>
    </div>

    <%= Model.RenderEditable<User>(Html, x => x.Nick)%>
    <%= Model.RenderEditable<User>(Html, x => x.Icq)%>
    <%= Model.RenderEditable<User>(Html, x => x.Phone)%>

    <div>
        <%= Html.SubmitButton("btnCreate", "Создать") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<AdminUsersController>(c => c.Index()) + "';")%>
    </div>                    
<% } %>

</asp:Content>
