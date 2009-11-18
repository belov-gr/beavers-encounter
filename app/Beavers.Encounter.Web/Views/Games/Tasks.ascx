<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<Task>>" %>

<div>
    <h2>Список заданий</h2>

    <table>
        <thead>
            <tr>
			    <th>Название</th>
			    <th colspan="2">Действие</th>
            </tr>
        </thead>

		<%
		foreach (Task task in ViewData.Model) { %>
			<tr>
				<td><%=Html.ActionLink<TasksController>(c => c.Show(task.Id), task.Name)%></td>
				<td><%=Html.ActionLink<TasksController>( c => c.Edit( task.Id ), "Изменить") %></td>
				<td>
    				<% using (Html.BeginForm<TasksController>(c => c.Delete(task.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
    				    <input type="submit" value="Удалить" onclick="return confirm('Are you sure?');" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<TasksController>(c => c.Create(), "Создать новое задание") %></p>

</div>