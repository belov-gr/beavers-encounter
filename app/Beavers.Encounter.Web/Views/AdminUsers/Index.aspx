<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IList<User>>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h2>Список пользователей</h2>

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<p><%= Html.ActionLink<AdminUsersController>(c => c.Create(), "Создать нового пользователя")%></p>

    <table>
        <thead>
            <tr>
                <th>Id</th>
                <th>Логин</th>
                <th>Ник</th>
                <th>Icq</th>
                <th>Телефон</th>
                <th>Член команды</th>
                <th>Автор игры</th>
                <th>Роль</th>
                <th colspan="2">Действие</th>
            </tr>
        </thead>
        <tbody>
        <% foreach (User user in Model)
           {
               if (user.Role.IsAdministrator)
                   continue; 
        %>
           <tr>
            <td><%= user.Id %></td> 
            <td><%= Html.ActionLink<AdminUsersController>(c => c.Edit(user.Id), user.Login)%></td> 
            <td><%= user.Nick %></td> 
            <td><%= user.Icq %></td> 
            <td><%= user.Phone %></td> 
            <td><%= user.Team != null ? user.Team.Name : "" %></td> 
            <td><%= user.Game != null ? user.Game.Name : "" %></td> 
            <td><%= user.Role %></td> 
            <td><%= Html.ActionLink<AdminUsersController>(c => c.Edit(user.Id), "Изменить")%></td>
            <td>
            <% 
            using (Html.BeginForm<AdminUsersController>(c => c.Delete(user.Id)))
            { %>
                <%= Html.AntiForgeryToken()%>
                <input type="submit" value="Удалить" onclick="return confirm('Вы уверены, что хотите удалить пользователя?');" />
            <% } %>
            </td> 
           </tr>
        <%
           }%>
        </tbody>
    </table>
</asp:Content>
