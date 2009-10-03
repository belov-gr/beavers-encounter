<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Смена пароля</h2>
    <p>
        New passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
    </p>
    <%= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) { %>
        <div>
		    <div>
                <label for="currentPassword">Текущий пароль:</label>
		        <div>
                    <%= Html.Password("currentPassword") %>
                    <%= Html.ValidationMessage("currentPassword") %>
		        </div>
            </div>

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
                <input type="submit" value="Change Password" />
            </div>
        </div>
    <% } %>
</asp:Content>
