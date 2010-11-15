<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AppConfig>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h2>Настройки сайта</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("AppConfig.Id", Model.Id)%>

    <%= Model.RenderEditable<AppConfig>(Html, x => x.Title)%>

    <div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<AdminAppConfigController>(c => c.Edit()) + "';")%>
    </div>                    
<% } %>

</asp:Content>

