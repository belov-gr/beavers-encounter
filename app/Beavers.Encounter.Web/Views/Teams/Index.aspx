<%@ Page Title="Teams" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<Team>>" %>
 

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h2>Список команд</h2>

    <% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
        <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
    <% } %>

    <table>
        <thead>
            <tr>
			    <th></th>
			    <th colspan="2"></th>
            </tr>
        </thead>

		<%
		foreach (Team team in ViewData.Model) { %>
            <% if (((User)User).Team == team) { %>
			<tr style="font-weight:bold">
            <% } else { %>
			<tr>
            <% } %>
				<td><%=Html.ActionLink<TeamsController>( c => c.Show( team.Id ), team.Name) %></td>
				
                <% if (((User)User).Role.IsAuthor) { %>
				<td><%=Html.ActionLink<TeamsController>( c => c.Edit( team.Id ), "Изменить") %></td>
				<td>
    				<% using (Html.BeginForm<TeamsController>(c => c.Delete(team.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
    				    <input type="submit" value="Удалить" onclick="return confirm('Вы уверенны, что хотите удалить команду?');" />
                    <% } %>
				</td>
                <% } %>
			</tr>
		<%} %>
    </table>

    <% if (((User)User).Role.IsAuthor)
       { %>
        <p><%= Html.ActionLink<TeamsController>(c => c.Create(), "Создать новую команду") %></p>
	<%} %>
</asp:Content>
