<%@ Control Language="C#" Inherits="Beavers.Encounter.Web.Views.ViewUserControl<Task>" %>

	<h2>Список кодов</h2>

<div>
    <ul>
    <%
    foreach (Code code in Model.Codes)
    {%>
        <li>
        <%= Html.Encode(code.IsBonus ? Model.Game.PrefixBonusCode : Model.Game.PrefixMainCode) %>
        <b><%= Html.Encode(code.Name)%></b>
        (КО: <%= Html.Encode(code.Danger)%>)
        <%= code.IsBonus ? "Бонус" : String.Empty %>
        <%= Html.ActionLink<CodesController>(c => c.Edit(code.Id), "Изменить")%>           
		<% using (Html.BeginForm<CodesController>(c => c.Delete(code.Id)))
           { %>
            <%= Html.AntiForgeryToken() %>
		    <input type="submit" value="Удалить код" onclick="return confirm('Вы действительно хотите удалить код?');" />
        <% } %>
        </li>
	<% } %>
    </ul>
    <p><%= Html.ActionLink<CodesController>(c => c.Create(Model.Id), "Добавить новый код")%></p>
</div>


