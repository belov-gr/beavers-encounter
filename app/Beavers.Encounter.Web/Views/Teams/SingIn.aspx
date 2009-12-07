<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TeamsController.TeamFormViewModel1>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Вступление в команду</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

    <% using (Html.BeginForm<TeamsController>(c => c.SingIn(0, null)))
       { %>
        <%= Html.AntiForgeryToken() %>
        <%= Html.Hidden("id", (ViewData.Model.TeamId != null) ? ViewData.Model.TeamId : 0)%>
        <div>
            <div>
                <label for="accessKey">Код доступа:</label>
                <div>
                <%= Html.TextBox("accessKey")%>
                <%= Html.ValidationMessage("accessKey")%>
                </div>
            </div>
            <div>
                <input type="submit" value="Войти" />
            </div>
        </div>
    <% } %>

</asp:Content>