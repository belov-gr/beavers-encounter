<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Регистрация на сайте</h2>
    <%= Html.ValidationSummary("Не удалось зарегистрироваться. Пожалуйста, исправьте ошибки и повторите регистрацию.")%>

    <% using (Html.BeginForm())
       { %>
        <div>
            <div>
                <label for="username">Имя:</label>
                <div>
                <%= Html.TextBox("username")%>
                <%= Html.ValidationMessage("username")%>
                </div>
            </div>
            <!--<div>
                <label for="email">Никнейм:</label>
                <div>
                <%= Html.TextBox("email")%>
                <%= Html.ValidationMessage("email")%>
                </div>
            </div>-->
            <div>
                <label for="password">Введите пароль:</label>
                <div>
                <%= Html.Password("password")%>
                <%= Html.ValidationMessage("password")%>
                </div>
            </div>
            <div>
                <label for="confirmPassword">Подтвердите пароль:</label>
                <div>
                <%= Html.Password("confirmPassword")%>
                <%= Html.ValidationMessage("confirmPassword")%>
                </div>
                <div>Внимание!</div>
                <div>Во второе поле нужно еще раз ввести пароль, а не код вступления в команду.</div>
            </div>
            <!--<div>
                <label for="antiSpamCode">Введите код:</label>
                <div>
                <%= Html.TextBox("antiSpamCode")%>
                <%= Html.ValidationMessage("antiSpamCode")%>
                </div>
            </div>-->
            <div>
                <input type="submit" value="Зарегистрироваться" />
            </div>
        </div>
    <% } %>
</asp:Content>
