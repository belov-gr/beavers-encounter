<%@ Page Title="Codes" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<Beavers.Encounter.Core.Code>>" %>
<%@ Import Namespace="Beavers.Encounter.Core" %>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers" %>
 

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h1>Codes</h1>

    <% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
        <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
    <% } %>

    <table>
        <thead>
            <tr>
			    <th>Name</th>
			    <th>Danger</th>
			    <th>IsBonus</th>
			    <th>Task</th>
			    <th colspan="3">Action</th>
            </tr>
        </thead>

		<%
		foreach (Code code in ViewData.Model) { %>
			<tr>
				<td><%= code.Name %></td>
				<td><%= code.Danger %></td>
				<td><%= code.IsBonus %></td>
				<td><%= code.Task %></td>
				<td><%=Html.ActionLink<CodesController>( c => c.Show( code.Id ), "Details ") %></td>
				<td><%=Html.ActionLink<CodesController>( c => c.Edit( code.Id ), "Edit") %></td>
				<td>
    				<% using (Html.BeginForm<CodesController>(c => c.Delete(code.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
    				    <input type="submit" value="Delete" onclick="return confirm('Are you sure?');" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

</asp:Content>
