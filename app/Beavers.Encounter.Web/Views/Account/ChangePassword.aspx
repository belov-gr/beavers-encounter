<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Смена пароля</h2>
    <%= Html.ValidationSummary("Не удалось изменить пароль.<br />Пожалуйста, исправьте ошибки и попробуйте снова.")%>

    <% using (Html.BeginForm()) { %>
        <div>
		    <div>
                <label for="currentPassword">Текущий пароль:</label>
		        <div>
                    <%= Html.Password("currentPassword") %>
                    <%= Html.ValidationMessage("currentPassword") %>
		        </div>
            </div>
            <p>
                Новый пароль должен содержать<br />не менее <%=Html.Encode(ViewData["PasswordLength"])%> символов.<br />
            </p>
		    <div>
                <label for="newPassword">Введите новый пароль:</label>
		        <div>
                    <%= Html.Password("newPassword") %>
                    <%= Html.ValidationMessage("newPassword") %>
		        </div>
            </div>

		    <div>
                <label for="confirmPassword">Подтвердите пароль:</label>
		        <div>
                    <%= Html.Password("confirmPassword") %>
                    <%= Html.ValidationMessage("confirmPassword") %>
		        </div>
            </div>

		    <div>
                <input type="submit" value="Изменить пароль" />
            </div>
        </div>
    <% } %>
</asp:Content>
