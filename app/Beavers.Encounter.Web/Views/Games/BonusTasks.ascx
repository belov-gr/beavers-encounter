<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<BonusTask>>" %>

<div>
    <table>
        <thead>
            <tr>
			    <th>Название</th>
			    <th colspan="2">Действие</th>
            </tr>
        </thead>

		<%
		foreach (BonusTask task in ViewData.Model) { %>
			<tr>
				<td><%=Html.ActionLink<BonusTasksController>(c => c.Show(task.Id), task.Name)%></td>
				<td><%=Html.ActionLink<BonusTasksController>(c => c.Edit(task.Id), "Изменить")%></td>
				<td>
    				<% using (Html.BeginForm<BonusTasksController>(c => c.Delete(task.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
                        <% string msg = String.Format("return confirm('Вы уверены, что хотите удалить бонусное задание {0}?');", task.Name); %>
    				    <input type="submit" value="Удалить" onclick="<%= msg %>" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<BonusTasksController>(c => c.Create(), "Создать новое бонусное задание")%></p>

</div>