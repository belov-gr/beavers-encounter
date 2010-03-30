<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Авторизация</h2>
    <p>
        <div>Введите свой логин и пароль.</div>
        <div>Если у Вас нет учетной записи, то <%= Html.ActionLink("зарегистрируйтесь", "Register")%>.</div>
    </p>
    <%= Html.ValidationSummary("Вход не удался. Пожалуйста, введите корректный логин и пароль.")%>

    <% using (Html.BeginForm())
       { %>
        <p></p>
        <div>
		    <div>
                <label for="username">Логин:</label>
		        <div><%= Html.TextBox("username")%>&nbsp;<%= Html.ValidationMessage("username")%></div>
            </div>
		    <div>
                <label for="password">Пароль:</label>
		        <div><%= Html.Password("password")%>&nbsp;<%= Html.ValidationMessage("password")%></div>
            </div>
            <p></p>            
		    <div><%= Html.CheckBox("rememberMe")%><label for="rememberMe">I'll be back! Запомните меня!</label></div>
            <p></p>
            <input type="submit" value="Войти" />
        </div>
    <% } %>
</asp:Content>
