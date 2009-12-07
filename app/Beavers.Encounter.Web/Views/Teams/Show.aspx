<%@ Page Title="Team Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<Team>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <ul>
		<li>
			<label for="Team_Name">��������:</label>
            <% if (((User)User).Team == ViewData.Model)
               { %>
                    <span id="My_Team_Name"><%= "<b>" + Server.HtmlEncode(ViewData.Model.Name) + "</b>"%></span>
            <% } else { %>
                    <span id="Team_Name"><%= Server.HtmlEncode(ViewData.Model.Name) %></span>
            <% } %>
		</li>
        <% if (((User)User).Team == null && ((User)User).Role.IsPlayer && ViewData.Model.Users.Count < 6)
           { %>
	    <li class="buttons">
            <%= Html.Button("btnSignIn", "����� � �������", HtmlButtonType.Button,
                                "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingIn(ViewData.Model.Id)) + "';")%>
        </li>
        <% } %>
        <% if (((User)User).Team == ViewData.Model && (((User)User).Role.IsPlayer || ((User)User).Role.IsTeamLeader))
           { %>
	    <li class="buttons">
            <%= Html.Button("btnSignOut", "�������� �������", HtmlButtonType.Button,
                                "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingOut()) + "';")%>
        </li>
        <% } %>
	</ul>

    <% if (((User)User).Role.IsAuthor && ViewData.Model.Game == null) 
       { 
    %>
        <%= Html.Button("btnSingInGame", 
            String.Format("���������������� ������� � ����:<br/>\"{0}\"", ((User)User).Game.Name), 
            HtmlButtonType.Button,
            "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingInGame(((User)User).Game.Id, ViewData.Model.Id)) + "';")%>
    <% } %>

    <% if (ViewData.Model.Game != null)
       {
    %>
        <p>
        ������� ���������������� �� ������� � ����:
        <%= Html.ActionLink<GamesController>(c => c.Show(ViewData.Model.Game.Id), ViewData.Model.Game.Name)%>
        <% if (ViewData.Model == ((User)User).Team && ((User)User).Role.IsAuthor) { %>
            <%= Html.Button("btnSingOutGame", 
                "���������� �� �������", 
                HtmlButtonType.Button,
                "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingOutGame(ViewData.Model.Game.Id, ViewData.Model.Id)) + "';")%>
        <%} %>
        </p>
    <%} %>
    
    <h2>������ ����������</h2>
    
    <table>
        <%
        foreach (User user in ViewData.Model.Users)
        { %>
        <tr>
            <td><%= Html.Encode(user.Login) %> <%= user.Role.IsTeamLeader ? "(�������)" : String.Empty%></td>
            <td>
            <% if (ViewData.Model == ((User)User).Team && ((User)User).Role.IsTeamLeader && ((User)User).Id != user.Id)
               { %>
                <%= Html.Button(
                    "btnRemove", "�������", HtmlButtonType.Button,
                    "window.location.href = '" + Html.BuildUrlFromExpression<TeamsController>(c => c.SingOutUser(ViewData.Model.Id, user.Id)) + "';")%>
            <%} %>
            </td>
        </tr>
        <%
        } %>
    </table>
    
    <% 
    if (((User)User).Role.IsAuthor)
    { %>
    <h2>�������������� �������</h2>

    <%= Beavers.Encounter.Common.BBCode.ConvertToHtml(Model.FinalTask) %>
    
    <br/>

    <p><%= Html.ActionLink<TeamsController>(c => c.Edit(Model.Id), "��������")%></p>
    
    <%
    } %>
    
    
</asp:Content>
