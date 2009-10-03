<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DataRow[]>" %>
<%@ Import Namespace="System.Data"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Results</title>
</head>
<body>
    <div>
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
            foreach (DataRow row in ViewData.Model)
            { %>
                <tr>
                    <td><%= Html.Encode(row["team"]) %></td>
                    <td Align=center><%= Html.Encode(row["tasks"]) %></td>
                    <td Align=center><%= Html.Encode(row["bonus"]) %></td>
                    <td Align=center><%= Html.Encode(row["time"]) %></td>
                </tr>
            <%
            } %>
            </tbody>
        </table>
    </div>
</body>
</html>
