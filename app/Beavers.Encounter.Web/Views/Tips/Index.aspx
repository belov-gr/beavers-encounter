<%@ Page Title="Tips" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<Tip>>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h1>Tips</h1>

    <% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
        <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
    <% } %>

    <table>
        <thead>
            <tr>
			    <th>Name</th>
			    <th>SuspendTime</th>
			    <th>Task</th>
			    <th colspan="3">Action</th>
            </tr>
        </thead>

		<%
		foreach (Tip tip in ViewData.Model) { %>
			<tr>
				<td><%= tip.Name %></td>
				<td><%= tip.SuspendTime %></td>
				<td><%= tip.Task %></td>
				<td><%=Html.ActionLink<TipsController>( c => c.Show( tip.Id ), "Details ") %></td>
				<td><%=Html.ActionLink<TipsController>( c => c.Edit( tip.Id ), "Edit") %></td>
				<td>
    				<% using (Html.BeginForm<TipsController>(c => c.Delete(tip.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
    				    <input type="submit" value="Delete" onclick="return confirm('Are you sure?');" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

</asp:Content>
