<%@ Page Title="Tasks" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<Task>>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h1>Tasks</h1>

    <% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
        <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
    <% } %>

    <table>
        <thead>
            <tr>
			    <th>Name</th>
			    <th colspan="3">Action</th>
            </tr>
        </thead>

		<%
		foreach (Task task in ViewData.Model) { %>
			<tr>
				<td><%= task.Name %></td>
				<td><%=Html.ActionLink<TasksController>( c => c.Show( task.Id ), "Details ") %></td>
				<td><%=Html.ActionLink<TasksController>( c => c.Edit( task.Id ), "Edit") %></td>
				<td>
    				<% using (Html.BeginForm<TasksController>(c => c.Delete(task.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
    				    <input type="submit" value="Delete" onclick="return confirm('Are you sure?');" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<TasksController>(c => c.Create(), "Create New Task") %></p>
</asp:Content>
