<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DataRow[]>" %>
<%@ Import Namespace="System.Data"%>

<h2>�������� �������</h2>
<table>
    <thead>
        <th>�������</th>
        <th>�������</th>
        <th>������</th>
        <th>�����</th>
    </thead>
    <tbody>
    <%
    foreach (DataRow row in Model)
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
