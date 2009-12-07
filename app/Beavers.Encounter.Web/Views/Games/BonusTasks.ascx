<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<BonusTask>>" %>

<div>
    <table>
        <thead>
            <tr>
			    <th>��������</th>
			    <th colspan="2">��������</th>
            </tr>
        </thead>

		<%
		foreach (BonusTask task in ViewData.Model) { %>
			<tr>
				<td><%=Html.ActionLink<BonusTasksController>(c => c.Show(task.Id), task.Name)%></td>
				<td><%=Html.ActionLink<BonusTasksController>(c => c.Edit(task.Id), "��������")%></td>
				<td>
    				<% using (Html.BeginForm<BonusTasksController>(c => c.Delete(task.Id))) { %>
                        <%= Html.AntiForgeryToken() %>
                        <% string msg = String.Format("return confirm('�� �������, ��� ������ ������� �������� ������� {0}?');", task.Name); %>
    				    <input type="submit" value="�������" onclick="<%= msg %>" />
                    <% } %>
				</td>
			</tr>
		<%} %>
    </table>

    <p><%= Html.ActionLink<BonusTasksController>(c => c.Create(), "������� ����� �������� �������")%></p>

</div>