<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Регистрация</h2>
    <%= Html.ValidationSummary("Не удалось зарегистрироваться. Пожалуйста, исправьте ошибки и повторите регистрацию.")%>

    <% using (Html.BeginForm())
       { %>
        <div>
            <div>
                <label for="username">Имя:</label>
                <div><%= Html.TextBox("username")%>&nbsp;<%= Html.ValidationMessage("username")%></div>
                </div>
            <div>
                <label for="password">Введите пароль:</label>
                <div><%= Html.Password("password")%>&nbsp;<%= Html.ValidationMessage("password")%></div>
            </div>
            <div>
                <label for="confirmPassword">Подтвердите пароль:</label>
                <div><%= Html.Password("confirmPassword")%>&nbsp;<%= Html.ValidationMessage("confirmPassword")%></div>
                <div class="attention">Внимание!</div>
                <div>Во второе поле нужно еще раз ввести пароль, а не код вступления в команду.</div>
            </div>
            <p></p>
            <div><input type="submit" value="Зарегистрироваться" /></div>
        </div>
    <% } %>
</asp:Content>
