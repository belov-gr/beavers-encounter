<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<BonusTask>>" %>
<%@ Import Namespace="Beavers.Encounter.Core"%>
<%@ Import Namespace="Beavers.Encounter.Web.Controllers"%>
<div>
    <h2>Сквозные бонусные задания</h2>

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
    				    <input type="submit" value="Удалить" onclick="return confirm('Are you sure?');" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<BonusTasksController>(c => c.Create(), "Создать новое задание")%></p>

</div>