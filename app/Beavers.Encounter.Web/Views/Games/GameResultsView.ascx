<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Data.DataRow[]>" %>

<h2>Итоговая таблица</h2>

<table>
    <thead>
        <th>Команда</th>
        <th>Задания</th>
        <th>Бонусы</th>
        <th>Время</th>
    </thead>
    <tbody>
    <%
    foreach (System.Data.DataRow row in Model)
    { %>
        <tr>
            <td><%= Html.Encode(row["team"]) %></td>
            <td align="center"><%= Html.Encode(row["tasks"]) %></td>
            <td align="center"><%= Html.Encode(row["bonus"]) %></td>
            <td align="center"><%= Html.Encode(row["time"]) %></td>
        </tr>
    <%
    } %>
    </tbody>
</table>
