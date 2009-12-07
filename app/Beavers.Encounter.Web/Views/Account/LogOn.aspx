<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Авторизация</h2>
    <p>
        <div>Введите свой логин и пароль.</div>
        <div>Если у Вас нет учетной записи, то <%= Html.ActionLink("зарегистрируйтесь", "Register")%>.</div>
    </p>
    <%= Html.ValidationSummary("Вход не удался. Пожалуйста введите корректный логин и пароль.")%>

    <% using (Html.BeginForm())
       { %>
        <div>
		    <div>
                <label for="username">Логин:</label>
		        <div>
                <%= Html.TextBox("username")%>
                <%= Html.ValidationMessage("username")%>
		        </div>
            </div>

		    <div>
                <label for="password">Пароль:</label>
		        <div>
                <%= Html.Password("password")%>
                <%= Html.ValidationMessage("password")%>
		        </div>
            </div>

		    <div>
            <%= Html.CheckBox("rememberMe")%> <label class="inline" for="rememberMe">Запомнить</label>
            </div>
            <input type="submit" value="Войти" />
        </div>
    <% } %>
</asp:Content>
