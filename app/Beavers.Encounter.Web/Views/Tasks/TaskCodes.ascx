<%@ Control Language="C#" Inherits="Beavers.Encounter.Web.Views.ViewUserControl<Task>" %>

	<h2>������ �����</h2>

<div>
    <ul>
    <%
    foreach (Code code in Model.Codes)
    {%>
        <li>
        <%= Html.Encode(code.IsBonus ? Model.Game.PrefixBonusCode : Model.Game.PrefixMainCode) %>
        <b><%= Html.Encode(code.Name)%></b>
        (��: <%= Html.Encode(code.Danger)%>)
        <%= code.IsBonus ? "�����" : String.Empty %>
        <%= Html.ActionLink<CodesController>(c => c.Edit(code.Id), "��������")%>           
		<% using (Html.BeginForm<CodesController>(c => c.Delete(code.Id)))
           { %>
            <%= Html.AntiForgeryToken() %>
		    <input type="submit" value="������� ���" onclick="return confirm('�� ������������� ������ ������� ���?');" />
        <% } %>
        </li>
	<% } %>
    </ul>
    <p><%= Html.ActionLink<CodesController>(c => c.Create(Model.Id), "�������� ����� ���")%></p>
</div>


