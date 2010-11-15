<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IList<Game>>" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h2>������ ���</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<p><%= Html.ActionLink<AdminGamesController>(c => c.Create(), "������� ����� ����")%></p>

    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>��������</th>
                <th>���� ����������</th>
                <th>���������</th>
                <th colspan="2">��������</th>
            </tr>
        </thead>
        <tbody>
        <% foreach (Game game in Model)
           {
        %>
           <tr>
            <td><%= game.Id %></td> 
            <td><%= Html.ActionLink<AdminGamesController>(c => c.Edit(game.Id), game.Name)%></td> 
            <td><%= game.GameDate %></td> 
            <td><%= game.GameState %></td> 
            <td><%= Html.ActionLink<AdminGamesController>(c => c.Edit(game.Id), "��������")%></td> 
            <td>
            <% 
            using (Html.BeginForm<AdminGamesController>(c => c.Delete(game.Id)))
            { %>
                <%= Html.AntiForgeryToken()%>
                <input type="submit" value="�������" onclick="return confirm('�� �������, ��� ������ ������� ����?');" />
            <% } %>
            </td> 
           </tr>
        <%
           }%>
        </tbody>
    </table>
</asp:Content>
