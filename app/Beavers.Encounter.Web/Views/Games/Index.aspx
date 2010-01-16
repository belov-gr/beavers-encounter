<%@ Page Title="Games" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<Game>>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Список игр</h2>

    <% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
        <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
    <% } %>

    <table>
        <thead>
            <tr>
			    <th>Название</th>
			    <th>Дата проведения</th>
			    <th colspan="3" style="width:150px"></th>
            </tr>
        </thead>

		<%
		foreach (Game game in ViewData.Model) { %>
        <% if (((User)User).Game == game) { %>
			<tr style="font-weight: bold">
        <% } else { %>
			<tr>
        <% } %>
				<td><%= Html.ActionLink<GamesController>(c => c.Show(game.Id), game.Name)%></td>
				<td><%= game.GameDate %></td>
				<% if (((User)User).Game == game) { %>
				    <td align=right><%=Html.ActionLink<GamesController>(c => c.Edit(game.Id), "Изменить")%></td>
				    <td>
    				    <% using (Html.BeginForm<GamesController>(c => c.Delete(game.Id)))
                           { %>
                            <%= Html.AntiForgeryToken()%>
    				        <input type="submit" value="Удалить" onclick="return confirm('Вы уверены, что хотите удалить игру ''<%= game.Name%>''?');" />
                        <% } %>
				    </td>
				<% } %>
			</tr>
		<%} %>
    </table>

    <% if (((User)User).Game == null && ((User)User).Team == null && ((User)User).Role.IsPlayer)
       { %>
    <p><%= Html.ActionLink<GamesController>(c => c.Create(), "Создать новую игру") %></p>
    <% } %>

</asp:Content>
