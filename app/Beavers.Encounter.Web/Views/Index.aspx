<%@ Page Title="BonusTasks" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<Beavers.Encounter.Core.BonusTask>>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h1>BonusTasks</h1>

    <% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
        <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
    <% } %>

    <table>
        <thead>
            <tr>
			    <th>Name</th>
			    <th>TaskText</th>
			    <th>StartTime</th>
			    <th>FinishTime</th>
			    <th>Game</th>
			    <th colspan="3">Action</th>
            </tr>
        </thead>

		<%
		foreach (BonusTask bonusTask in ViewData.Model) { %>
			<tr>
				<td><%= bonusTask.Name %></td>
				<td><%= bonusTask.TaskText %></td>
				<td><%= bonusTask.StartTime %></td>
				<td><%= bonusTask.FinishTime %></td>
				<td><%= bonusTask.Game %></td>
				<td><%=Html.ActionLink<BonusTasksController>( c => c.Show( bonusTask.Id ), "Details ") %></td>
				<td><%=Html.ActionLink<BonusTasksController>( c => c.Edit( bonusTask.Id ), "Edit") %></td>
				<td>
    				<% using (Html.BeginForm<BonusTasksController>(c => c.Delete(bonusTask.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
    				    <input type="submit" value="Delete" onclick="return confirm('Are you sure?');" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<BonusTasksController>(c => c.Create(), "Create New BonusTask") %></p>
</asp:Content>
