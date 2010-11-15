<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Game>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h2>Свойства игры</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Game.Id", Model.Id)%>

    <%= Model.RenderEditable<Game>(Html, x => x.Name)%>
    <%= Model.RenderEditable<Game>(Html, x => x.GameDate)%>
    <%= Model.RenderEditable<Game>(Html, x => x.Description)%>
    <%= Model.RenderEditable<Game>(Html, x => x.TotalTime)%>

    <div>
        <%= Html.SubmitButton("btnSave", "Сохранить") %>
        <%= Html.Button("btnCancel", "Отмена", HtmlButtonType.Button,
                        "window.location.href = '" + Html.BuildUrlFromExpression<AdminGamesController>(c => c.Index()) + "';")%>
    </div>                    
<% } %>

</asp:Content>

