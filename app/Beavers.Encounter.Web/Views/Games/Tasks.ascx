<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<Task>>" %>

<div>
    <h2>������ �������</h2>

    <table>
        <thead>
            <tr>
			    <th>��������</th>
			    <th colspan="2">��������</th>
            </tr>
        </thead>

		<%
		foreach (Task task in ViewData.Model) { %>
			<tr>
				<td><%=Html.ActionLink<TasksController>(c => c.Show(task.Id), task.Name)%></td>
				<td><%=Html.ActionLink<TasksController>( c => c.Edit( task.Id ), "��������") %></td>
				<td>
    				<% using (Html.BeginForm<TasksController>(c => c.Delete(task.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
    				    <input type="submit" value="�������" onclick="return confirm('Are you sure?');" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<TasksController>(c => c.Create(), "������� ����� �������") %></p>

</div>